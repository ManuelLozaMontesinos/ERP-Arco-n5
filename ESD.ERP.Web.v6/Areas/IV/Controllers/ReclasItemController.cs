using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service.DTO;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.IV.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using NLog.Targets;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.IV.Controllers
{
   [Area("IV")]
   [Route("[controller]")]
   public class ReclasItemController : AuthorizedController
   {
      private readonly IAlmacenProxy almacenProxy;
      private readonly IMovimientoReclasificacionProxy movimientoReclasificacionProxy;
      private readonly IMesContableProxy mesContableProxy;
      private readonly IEstadoMovimientoReclasificacionProxy estadoMovimientoReclasificacionProxy;
      private readonly IMovimientoAlmacenProductoProxy movimientoAlmacenProductoProxy;
      private readonly AppHelper appHelper;
      public ReclasItemController(
         IAlmacenProxy almacenProxy,
         IMovimientoReclasificacionProxy movimientoReclasificacionProxy,
         IMesContableProxy mesContableProxy,
         IEstadoMovimientoReclasificacionProxy estadoMovimientoReclasificacionProxy,
         IMovimientoAlmacenProductoProxy movimientoAlmacenProductoProxy,
         AppHelper appHelper
         )
      {
         this.almacenProxy = almacenProxy;
         this.movimientoReclasificacionProxy = movimientoReclasificacionProxy;
         this.mesContableProxy = mesContableProxy;
         this.estadoMovimientoReclasificacionProxy = estadoMovimientoReclasificacionProxy;
         this.movimientoAlmacenProductoProxy = movimientoAlmacenProductoProxy;
         this.appHelper = appHelper;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         ReclasItemViewModel reclasItemViewModel = new();

         cargarListas(reclasItemViewModel);

         return PartialView(reclasItemViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         ReclasItemViewModel model = JsonSerializer.Deserialize<ReclasItemViewModel>(jsonModel);

         model.MovimientoReclasificacionDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.MovimientoReclasificacionDTO.OrderBy = "mr.fecha desc,mr.mov_reclasi";

         model.MovimientoReclasificacionDTO.PeriodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(model.ViewModelState.CID);

         try
         {
            List<MovimientoReclasificacionDTO> movimientoReclasificacionDTOList = movimientoReclasificacionProxy.Search(model.MovimientoReclasificacionDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = movimientoReclasificacionDTOList.Count,
               rows = from f in movimientoReclasificacionDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.MovimientoReclasificacionId,
                            f.Codigo,
                            Fecha = f.Fecha.ToShortDateString(),
                            f.MovimientoAlmacenId,
                            MovimientoAlmacen = f.MovimientoAlmacenDTO.Codigo,
                            TransaccionAlmacenNombre = f.MovimientoAlmacenDTO.TransaccionAlmacenDTO.Nombre,
                            TipoMovimientoAlmacen = f.MovimientoAlmacenDTO.TipoMovimientoAlmacenDTO.Codigo,
                            AlmacenNombre = f.MovimientoAlmacenDTO.AlmacenDTO.Nombre,
                            f.MovimientoAlmacenDTO.DocumentoSerieNumeroReferencial,
                            f.MovimientoAlmacenDTO.ProveedorDTO?.RazonSocial,
                            EstadoMovimientoReclasificacion = f.EstadoMovimientoReclasificacionDTO.Codigo,
                            EstadoMovimientoReclasificacionNombre = f.EstadoMovimientoReclasificacionDTO.Nombre,
                            f.Estado,
                            UsuarioAutoriza = f.UsuarioAutorizaDTO?.NombreCompleto
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
      [HttpPost("Crear")]
      public ActionResult Crear([FromBody] ReclasItemViewModel model)
      {
         Logger.InfoStart();

         ReclasItemViewModel reclasItemViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true,
         };

         try
         {
            cargarDataParaEditar(reclasItemViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, reclasItemViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] ReclasItemViewModel model)
      {

         Logger.InfoStart();

         ReclasItemViewModel reclasItemViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(reclasItemViewModel, model.MovimientoReclasificacionDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, reclasItemViewModel);
      }
      private void cargarDataParaEditar(ReclasItemViewModel model, int id)
      {

         MovimientoReclasificacionDTO movimientoReclasificacionDTO;
         if (model.EditAction == ViewModelEditAction.New)
         {
            movimientoReclasificacionDTO = new MovimientoReclasificacionDTO() {
               Fecha = DateTime.Today,
               MovimientoAlmacenDTO = new MovimientoAlmacenDTO(),
               EstadoMovimientoReclasificacionDTO = new EstadoMovimientoReclasificacionDTO()
            };
         }
         else
         {
            movimientoReclasificacionDTO = movimientoReclasificacionProxy.GetById(new MovimientoReclasificacionDTO() { MovimientoReclasificacionId = id });
         }

         model.MovimientoReclasificacionDTO = movimientoReclasificacionDTO;

         cargarListas(model);

         WebHelper.SelectedListItemSetValue(model.AlmacenList, movimientoReclasificacionDTO.MovimientoAlmacenDTO.AlmacenId);

         model.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { MenuItemType = MenuItemType.Save },
            new MenuItem() { Key = "Solicitar", Content = "Solicitar Aprobacion" },
            new MenuItem() { MenuItemType = MenuItemType.Exit }
         };

      }
      private void cargarListas(ReclasItemViewModel model)
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
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody] ReclasItemViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            MovimientoReclasificacionDTO movimientoReclasificacionDTO = cargarMovimientoReclasificacionDTO(model);

            int newId = model.MovimientoReclasificacionDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = movimientoReclasificacionProxy.Insert(movimientoReclasificacionDTO);
                  messageResourceKey = "IV_movimiento_reclasificacion_agregado";
                  break;

               case ViewModelEditAction.Edit:
                  movimientoReclasificacionProxy.Update(movimientoReclasificacionDTO);
                  messageResourceKey = "IV_movimiento_reclasificacion_actualizado";
                  break;
            }

            movimientoReclasificacionDTO = movimientoReclasificacionProxy.GetById(new MovimientoReclasificacionDTO() { MovimientoReclasificacionId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, movimientoReclasificacionDTO.Codigo);
            resultResponse.Data = new
            {
               movimientoReclasificacionDTO.Id,
               movimientoReclasificacionDTO.Codigo,
               movimientoReclasificacionDTO.MesContableId
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

         private MovimientoReclasificacionDTO cargarMovimientoReclasificacionDTO(ReclasItemViewModel model)
      {

         MovimientoReclasificacionDTO movimientoReclasificacionDTO;
         try
         {

            movimientoReclasificacionDTO = model.MovimientoReclasificacionDTO;

            if (model.EditAction == ViewModelEditAction.New)
            {
               PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

               movimientoReclasificacionDTO.PeriodoEmpresaSedeDTO = periodoEmpresaSedeDTO;
               movimientoReclasificacionDTO.PeriodoEmpresaSedeId = periodoEmpresaSedeDTO.PeriodoEmpresaSedeId;

               string mesContable = string.Format(StringFormatConstant.DATE_MONTH, movimientoReclasificacionDTO.Fecha);

               MesContableDTO mesContableDTO = mesContableProxy.ObtenerPorCodigo(new MesContableDTO() { Codigo = mesContable });
               movimientoReclasificacionDTO.MesContableId = mesContableDTO.Id;

               EstadoMovimientoReclasificacionDTO estadoMovimientoReclasificacionDTO = estadoMovimientoReclasificacionProxy.ObtenerPorCodigo(new EstadoMovimientoReclasificacionDTO() { Codigo = EstadoMovimientoReclasificacionConstante.MOVIMIENTO });
               movimientoReclasificacionDTO.EstadoMovimientoReclasificacionId = estadoMovimientoReclasificacionDTO.Id;

            }
            else
            {
               movimientoReclasificacionDTO = movimientoReclasificacionProxy.GetById(new MovimientoReclasificacionDTO() { MovimientoReclasificacionId 
                  = movimientoReclasificacionDTO.Id });

               model.MovimientoReclasificacionDTO.PeriodoEmpresaSedeId = movimientoReclasificacionDTO.PeriodoEmpresaSedeId;
               movimientoReclasificacionDTO = model.MovimientoReclasificacionDTO;
            }

            if (string.IsNullOrEmpty(movimientoReclasificacionDTO.Observacion))
               movimientoReclasificacionDTO.Observacion = string.Empty;

            movimientoReclasificacionDTO.Estado = EstadoConstante.ACTIVO;
            AuditDTO(movimientoReclasificacionDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return movimientoReclasificacionDTO;
      }
      [HttpPost("Eliminar")]
      public ActionResult Eliminar([FromBody] ReclasItemViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.MovimientoReclasificacionDTO.DTOState = DTOState.Modified;

            AuditDTO(model.MovimientoReclasificacionDTO);

            movimientoReclasificacionProxy.Delete(model.MovimientoReclasificacionDTO);

            resultResponse.Message = SR.GetString("IV_movimiento_reclasificacion_anulado", model.MovimientoReclasificacionDTO.Codigo);
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
      [HttpPost("ListarMovimientoAlmacenProducto")]
      public ActionResult ListarMovimientoAlmacenProducto(string jsonModel)
      {

         Logger.InfoStart();

         ReclasItemViewModel model = JsonSerializer.Deserialize<ReclasItemViewModel>(jsonModel);

         model.MovimientoAlmacenProductoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.MovimientoAlmacenProductoDTO.OrderBy = "p.producto";

         try
         {
            List<MovimientoAlmacenProductoDTO> movimientoAlmacenProductoDTOList = movimientoAlmacenProductoProxy.Search(model.MovimientoAlmacenProductoDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = movimientoAlmacenProductoDTOList.Count,
               rows = from f in movimientoAlmacenProductoDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new 
                         {
                            f.MovimientoAlmacenProductoId,
                            f.ProductoDTO.Codigo,
                            ProductoNombre = f.ProductoDTO.Descripcion,
                            UnidadMedidaNombre = f.ProductoDTO.UnidadMedidaDTO.Abreviatura,
                            f.Descripcion,
                            f.Cantidad,
                            f.ValorUnitario,
                            Total = f.ValorUnitario * f.Cantidad,
                            ProductoReclasificacionId = f?.ProductoReclasificacionDTO?.ProductoId,
                            ProductoReclasificacion = f?.ProductoReclasificacionDTO?.Codigo,
                            ProductoReclasificacionNombre = f?.ProductoReclasificacionDTO?.Descripcion,
                            f.CantidadReclasificacion,
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
      [HttpPost("GrabarMovimientoAlmacenProducto")]
      public ActionResult GrabarMovimientoAlmacenProducto([FromBody] ReclasItemViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            MovimientoAlmacenProductoDTO movimientoAlmacenProductoDTO = cargarMovimientoAlmacenProductoDTO(model);

            int newId = model.MovimientoAlmacenProductoDTO.Id;
            string messageResourceKey = string.Empty;
            switch (movimientoAlmacenProductoDTO.DTOState)
            {
               case DTOState.Modified:
                  movimientoAlmacenProductoProxy.ActualizarProductoReclasificacion(movimientoAlmacenProductoDTO);
                  messageResourceKey = "IV_producto_actualizado";
                  break;
            }

            resultResponse.Message = SR.GetString(messageResourceKey);
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
      private MovimientoAlmacenProductoDTO cargarMovimientoAlmacenProductoDTO(ReclasItemViewModel model)
      {

         MovimientoAlmacenProductoDTO movimientoAlmacenProductoDTO;
         try
         {

            movimientoAlmacenProductoDTO = model.MovimientoAlmacenProductoDTO;

            AuditDTO(movimientoAlmacenProductoDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return movimientoAlmacenProductoDTO;
      }

        [HttpPost("Solicitar")]
        public ActionResult Solicitar([FromBody] ReclasItemViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();
            try
            {
                MovimientoReclasificacionDTO movimientoReclasificacionDTO = cargarMovimientoReclasificacionDTO(model);
                int newId = model.MovimientoReclasificacionDTO.Id;
                string messageResourceKey = "IV_movimiento_reclasificacion_solicitar";
                movimientoReclasificacionProxy.Solicitar(movimientoReclasificacionDTO);
                movimientoReclasificacionDTO = movimientoReclasificacionProxy.GetById(new MovimientoReclasificacionDTO() { MovimientoReclasificacionId = newId });

                resultResponse.Message = SR.GetString(messageResourceKey, movimientoReclasificacionDTO.Codigo);
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

        [HttpGet("ReportTo")]
        public FileContentResult ReportTo(string format, string cid, string movimientoReclasificacionId)
        {
         byte[] reportData = obtenerReporteData(movimientoReclasificacionId);

         return appHelper.ReportToPdf(reportData, "IV_Reclasificacion");
        }
        private byte[] obtenerReporteData(string movimientoReclasificacionId)
        {
        Dictionary<string, string> parameters = new()
        {
        { "movimientoReclasificacionId", movimientoReclasificacionId }
        };

        byte[] reportData = appHelper.ExecuteReport("IV_Reclasificacion", parameters, ReportFormatConstant.PDF);

        return reportData;
        }
   }
}