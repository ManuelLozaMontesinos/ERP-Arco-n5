using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service.DTO;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.PD.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.PD.Controllers
{
   [Area("PD")]
   [Route("[controller]")]
   public class RequeInsumoController : AuthorizedController
   {
      private readonly IOrdenProduccionProxy ordenProduccionProxy;
      private readonly IOrdenProduccionProductoProxy ordenProduccionProductoProxy;
      private readonly IEstadoDespachoProxy estadoDespachoProxy;
      private readonly IAlmacenProxy almacenProxy;

      public RequeInsumoController(
         IOrdenProduccionProxy ordenProduccionProxy,
         IOrdenProduccionProductoProxy ordenProduccionProductoProxy,
         IEstadoDespachoProxy estadoDespachoProxy,
         IAlmacenProxy almacenProxy
         )
      {
         this.ordenProduccionProxy = ordenProduccionProxy;
         this.ordenProduccionProductoProxy = ordenProduccionProductoProxy;
         this.estadoDespachoProxy = estadoDespachoProxy;
         this.almacenProxy = almacenProxy;
      }

      [Route("[action]")]
      public IActionResult Index()
      {
         RequeInsumoViewModel requeInsumoViewModel = new()
         {
            MenuItemList = new List<MenuItem>
            {
               new MenuItem() { MenuItemType = MenuItemType.Edit }
            }
         };
         cargarListaEstadoDespacho(requeInsumoViewModel);

         return PartialView(requeInsumoViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         RequeInsumoViewModel model = JsonSerializer.Deserialize<RequeInsumoViewModel>(jsonModel);

         model.OrdenProduccionDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.OrdenProduccionDTO.OrderBy = "op.fecha desc,op.orden_pro desc";

         model.OrdenProduccionDTO.PeriodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(model.ViewModelState.CID);
         model.OrdenProduccionDTO.IncluyeInsumo = SiNoConstante.S;

         try
         {
            List<OrdenProduccionDTO> ordenProduccionDTOList = ordenProduccionProxy.Search(model.OrdenProduccionDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = ordenProduccionDTOList.Count,
               rows = from f in ordenProduccionDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.OrdenProduccionId,
                            f.Codigo,
                            Fecha = f.Fecha.ToShortDateString(),
                            Cotizacion = f?.CotizacionDTO?.Codigo,
                            f.ProveedorId,
                            Proveedor = f.ProveedorDTO.Codigo,
                            f.ProveedorDTO.RazonSocial,
                            f.ProveedorDTO.NumeroDocumentoIdentidad,
                            EstadoDespacho = f?.EstadoDespachoDTO?.Codigo,
                            EstadoDespachoNombre = f?.EstadoDespachoDTO?.Nombre,
                            PersonalDespacho = f?.PersonalDTO?.ProveedorDTO?.RazonSocial,
                            FechaDespacho = f.FechaDespacho.HasValue ? string.Format(StringFormatConstant.DATE_TIME, f.FechaDespacho.Value) : string.Empty,
                            f.ObservacionDespacho,
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
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] RequeInsumoViewModel model)
      {

         Logger.InfoStart();

         RequeInsumoViewModel requeInsumoViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(requeInsumoViewModel, model.OrdenProduccionDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, requeInsumoViewModel);
      }
      private void cargarDataParaEditar(RequeInsumoViewModel model, int id)
      {

         model.OrdenProduccionDTO = ordenProduccionProxy.GetById(new OrdenProduccionDTO() { OrdenProduccionId = id });

         cargarListas(model);

         model.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { Key="cerrar", Content="Cerrar Requerimiento" },
            new MenuItem() { MenuItemType = MenuItemType.Exit }
         };

         model.InsumoMenuBar = new List<MenuItem>
         {
             new MenuItem() { Key = "restock", Content="Stockear", Disabled = true }
         };

         WebHelper.SelectedListItemSetValue(model.AlmacenList, model.OrdenProduccionDTO.AlmacenId);

      }
      private void cargarListas(RequeInsumoViewModel model)
      {
         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

         List<AlmacenDTO> almacenDTOList = almacenProxy.Search(new AlmacenDTO()
         {
            EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId },
            EmpresaSedeId = periodoEmpresaSedeDTO.EmpresaSedeId,
            Estado = EstadoConstante.ACTIVO
         });
         model.AlmacenList = WebHelper.ToSelectListItem(almacenDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);
      }
      private void cargarListaEstadoDespacho(RequeInsumoViewModel model)
      {
         List<EstadoDespachoDTO> estadoDespachoDTOList = estadoDespachoProxy.Search(new EstadoDespachoDTO()
         {
            NotEstadoDespachoList = new List<string>() { EstadoDespachoConstante.ATENDIDO_PARCIAL },
            Estado = EstadoConstante.ACTIVO
         });
         model.EstadoDespachoList = WebHelper.ToSelectListItem(estadoDespachoDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         int estadoDespachoId = estadoDespachoDTOList.Where(x => x.Codigo == EstadoDespachoConstante.PENDIENTE).FirstOrDefault().Id;

         WebHelper.SelectedListItemSetValue(model.EstadoDespachoList, estadoDespachoId);
      }
      [HttpPost("ListarOrdenProduccionProducto")]
      public ActionResult ListarOrdenProduccionProducto(string jsonModel)
      {

         Logger.InfoStart();

         RequeInsumoViewModel model = JsonSerializer.Deserialize<RequeInsumoViewModel>(jsonModel);

         model.OrdenProduccionProductoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.OrdenProduccionProductoDTO.OrderBy = "tp.tipo_producto,p.producto";
         model.OrdenProduccionProductoDTO.ProductoDTO = new ProductoDTO()
         {
            FamiliaProductoDTO = new FamiliaProductoDTO()
            {
               ClaseProductoDTO = new ClaseProductoDTO()
               {
                  GrupoProductoDTO = new GrupoProductoDTO() { EsInsumo = SiNoConstante.S}
               }
            }
         };

         try
         {
            List<OrdenProduccionProductoDTO> ordenProduccionProductoDTOList = ordenProduccionProductoProxy.Search(model.OrdenProduccionProductoDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = ordenProduccionProductoDTOList.Count,
               rows = from f in ordenProduccionProductoDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.OrdenProduccionProductoId,
                            f.OrdenProduccionId,
                            f.ProductoId,
                            f.ProductoDTO.Codigo,
                            ProductoNombre = f.ProductoDTO.Descripcion,
                            TipoProducto = f.ProductoDTO.FamiliaProductoDTO.ClaseProductoDTO.GrupoProductoDTO.TipoProductoDTO.Codigo,
                            f.ProductoDTO.UnidadMedidaDTO.UnidadMedidaId,
                            UnidadMedidaNombre = f.ProductoDTO.UnidadMedidaDTO.Abreviatura,
                            f.Descripcion,
                            f.Cantidad,
                            f.CantidadEntregada,
                            f.Estado,
                            DTOState = DTOState.Unchanged
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
      [HttpPost("CerrarPopup")]
      public ActionResult CerrarPopup([FromBody] RequeInsumoViewModel model)
      {

         try
         {
            if (model == null)
            {
               model = new RequeInsumoViewModel
               {
                  OrdenProduccionDTO = new OrdenProduccionDTO()
               };
            }
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         return PartialView(model);
      }
      [HttpPost("Cerrar")]
      public ActionResult Cerrar([FromBody] RequeInsumoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            AuditDTO(model.OrdenProduccionDTO);

            ordenProduccionProxy.CerrarDespacho(model.OrdenProduccionDTO);
            ordenProduccionProxy.CerrarInsumo(model.OrdenProduccionDTO);

            OrdenProduccionDTO ordenProduccionDTO = ordenProduccionProxy.GetById(new OrdenProduccionDTO() { OrdenProduccionId = model.OrdenProduccionDTO.Id });

            resultResponse.Message = SR.GetString("PD_orden_produccion_req_insumo_atendido", ordenProduccionDTO.Codigo);
            resultResponse.Data = new
            {
               EstadoDespacho = ordenProduccionDTO?.EstadoDespachoDTO?.Codigo,
               ordenProduccionDTO.ObservacionDespacho
            };
         }
         catch (Exception e)
         {
            Logger.Error(e);
            resultResponse.Message = e.Message;
            resultResponse.Success = false;
         }

         Logger.InfoFinish();

         return Json(resultResponse, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
      }
      [HttpPost("EditarInsumo")]
      public ActionResult EditarInsumo([FromBody] RequeInsumoViewModel model)
      {
        Logger.InfoStart();

        ResultResponse resultResponse = new();

          try
          {
                OrdenProduccionProductoDTO ordenProduccionProductoDTO = ordenProduccionProductoProxy.GetById(model.OrdenProduccionProductoDTO);

                ordenProduccionProductoProxy.actualizarInsumos(ordenProduccionProductoDTO);
                resultResponse.Message = "Insumo Restaurado";
            }
          catch (Exception e) 
          {
              Logger.Error(e);
              resultResponse.Message = e.Message;
              resultResponse.Success = false;
          }
        Logger.InfoFinish();

        return Json(resultResponse, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true});
      }

   }
}