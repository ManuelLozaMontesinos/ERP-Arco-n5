using ESD.Core.Logging;
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
   public class MovAlmacenSaldo : AuthorizedController
   {
      private readonly IMovimientoAlmacenSaldoProxy movimientoAlmacenSaldoProxy;

      public MovAlmacenSaldo(
         IMovimientoAlmacenSaldoProxy movimientoAlmacenSaldoProxy
         )
      {
         this.movimientoAlmacenSaldoProxy = movimientoAlmacenSaldoProxy;
      }
      [Route("ListPopup")]
      public IActionResult ListPopup([FromBody] MovAlmacenViewSaldoModel model)
      {

         ViewBag.ModalEditId = GetModalEditPanelId();
         return PartialView(model);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         MovAlmacenViewSaldoModel model = JsonSerializer.Deserialize<MovAlmacenViewSaldoModel>(jsonModel);

         model.MovimientoAlmacenSaldoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.MovimientoAlmacenSaldoDTO.OrderBy = "p.descripcion";

         model.MovimientoAlmacenSaldoDTO.PeriodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(model.ViewModelState.CID);

         try
         {
            List<MovimientoAlmacenSaldoDTO> movimientoAlmacenSaldoDTOList = movimientoAlmacenSaldoProxy.Search(model.MovimientoAlmacenSaldoDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = movimientoAlmacenSaldoDTOList.Count,
               rows = from f in movimientoAlmacenSaldoDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.MovimientoAlmacenSaldoId,
                            f.ProductoId,
                            f.ProductoDTO.Codigo,
                            f.ProductoDTO.Descripcion,
                            f.ProductoDTO.CodigoReferencial,
                            UnidadMedidaNombre = f.ProductoDTO.UnidadMedidaDTO.Nombre,
                            f.Saldo
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