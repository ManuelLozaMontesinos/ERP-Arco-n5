using ESD.Core.Logging;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.IV.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.IV.Controllers
{
   [Area("IV")]
   [Route("[controller]")]
   public class ConsultaLlegadaController : AuthorizedController
   {
      private readonly IOrdenCompraEntregaProxy ordenCompraEntregaProxy;
      public ConsultaLlegadaController(
         IOrdenCompraEntregaProxy ordenCompraEntregaProxy
         )
      {
         this.ordenCompraEntregaProxy = ordenCompraEntregaProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         ConsultaLlegadaViewModel controlMermaViewModel = new();

         controlMermaViewModel.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { MenuItemType = MenuItemType.New }
         };

         controlMermaViewModel.SinProveedor = SiNoConstante.N;
         if (UsuarioEnRol(new List<string>() { "00004" }))
         {
            controlMermaViewModel.SinProveedor = SiNoConstante.S;
         }

         return PartialView(controlMermaViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         ConsultaLlegadaViewModel model = JsonSerializer.Deserialize<ConsultaLlegadaViewModel>(jsonModel);

         model.OrdenCompraEntregaDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.OrdenCompraEntregaDTO.OrderBy = "oce.fecha desc,prv.razon_social";

         model.OrdenCompraEntregaDTO.OrdenCompraProductoDTO.OrdenCompraDTO.PeriodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);
         model.OrdenCompraEntregaDTO.OrdenCompraProductoDTO.OrdenCompraDTO.EstadoAprobacionDTO = new EstadoAprobacionDTO() { Codigo = EstadoAprobacionConstante.APROBADO };
         model.OrdenCompraEntregaDTO.OrdenCompraProductoDTO.ConCantidadPendiente = SiNoConstante.S;
         model.OrdenCompraEntregaDTO.Estado = EstadoConstante.ACTIVO;
         model.OrdenCompraEntregaDTO.OrdenCompraProductoDTO.OrdenCompraDTO.EstadoOrdenCompraList =
            new List<string>() { EstadoOrdenCompraConstante.INGRESO_PARCIAL, EstadoOrdenCompraConstante.ORDEN_COMPRA };

         try
         {
            List<OrdenCompraEntregaDTO> ordenCompraEntregaDTOList = ordenCompraEntregaProxy.Search(model.OrdenCompraEntregaDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = ordenCompraEntregaDTOList.Count,
               rows = from f in ordenCompraEntregaDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.OrdenCompraEntregaId,
                            f.OrdenCompraProductoDTO.OrdenCompraDTO.ProveedorDTO.RazonSocial,
                            f.OrdenCompraProductoDTO.ProductoDTO.Codigo,
                            Producto = f.OrdenCompraProductoDTO.ProductoDTO.Descripcion,
                            UnidadMedida = f.OrdenCompraProductoDTO.ProductoDTO.UnidadMedidaDTO.Abreviatura,
                            Fecha = f.Fecha.Value.ToShortDateString(),
                            f.Cantidad,
                            f.Estado
                         }
                      }
            };

            Logger.InfoFinish();
            return Json(jsonData, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }
      }
      
   }
}