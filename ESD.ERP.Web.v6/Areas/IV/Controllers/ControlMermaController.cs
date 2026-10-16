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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.IV.Controllers
{
   [Area("IV")]
   [Route("[controller]")]
   public class ControlMermaController : AuthorizedController
   {
      private readonly IAlmacenProxy almacenProxy;
      private readonly ITransaccionAlmacenProxy transaccionAlmacenProxy;
      private readonly IMovimientoAlmacenProxy movimientoAlmacenProxy;
      private readonly ITipoMovimientoAlmacenProxy tipoMovimientoAlmacenProxy;
      private readonly IMesContableProxy mesContableProxy;
      private readonly IEstadoMovimientoAlmacenProxy estadoMovimientoAlmacenProxy;
      private readonly IMovimientoAlmacenProductoProxy movimientoAlmacenProductoProxy;
      public ControlMermaController(
         IAlmacenProxy almacenProxy,
         ITransaccionAlmacenProxy transaccionAlmacenProxy,
         IMovimientoAlmacenProxy movimientoAlmacenProxy,
         ITipoMovimientoAlmacenProxy tipoMovimientoAlmacenProxy,
         IMesContableProxy mesContableProxy,
         IEstadoMovimientoAlmacenProxy estadoMovimientoAlmacenProxy,
         IMovimientoAlmacenProductoProxy movimientoAlmacenProductoProxy
         )
      {
         this.almacenProxy = almacenProxy;
         this.movimientoAlmacenProxy = movimientoAlmacenProxy;
         this.transaccionAlmacenProxy = transaccionAlmacenProxy;
         this.tipoMovimientoAlmacenProxy = tipoMovimientoAlmacenProxy;
         this.mesContableProxy = mesContableProxy;
         this.estadoMovimientoAlmacenProxy = estadoMovimientoAlmacenProxy;
         this.movimientoAlmacenProductoProxy = movimientoAlmacenProductoProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         ControlMermaViewModel controlMermaViewModel = new();

         cargarListasCompartidas(controlMermaViewModel);

         controlMermaViewModel.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { MenuItemType = MenuItemType.New },
            new MenuItem() { MenuItemType = MenuItemType.Edit },
            new MenuItem() { MenuItemType = MenuItemType.Delete },
            new MenuItem() { Key="imprimir_fmt", Content="Imprimir formato" }
         };

         return PartialView(controlMermaViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         ControlMermaViewModel model = JsonSerializer.Deserialize<ControlMermaViewModel>(jsonModel);

         model.MovimientoAlmacenDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.MovimientoAlmacenDTO.OrderBy = "ma.fecha desc,ma.mov_almacen";

         model.MovimientoAlmacenDTO.PeriodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(model.ViewModelState.CID);
         model.MovimientoAlmacenDTO.TransaccionAlmacenList = new List<string>() { TransaccionAlmacenConstante.INGRESO_MERMA, TransaccionAlmacenConstante.SALIDA_MERMA };

         try
         {
            List<MovimientoAlmacenDTO> movimientoAlmacenDTOList = movimientoAlmacenProxy.Search(model.MovimientoAlmacenDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = movimientoAlmacenDTOList.Count,
               rows = from f in movimientoAlmacenDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.MovimientoAlmacenId,
                            f.Codigo,
                            Fecha = f.Fecha.ToShortDateString(),
                            AlmacenNombre = f.AlmacenDTO.Nombre,
                            TransaccionAlmacenNombre = f.TransaccionAlmacenDTO.Nombre,
                            PersonalNombre = f.PersonalDTO == null ? string.Empty : f.PersonalDTO.ProveedorDTO.RazonSocial,
                            PersonalAutoriza = f.PersonalAutorizaDTO == null ? string.Empty : f.PersonalAutorizaDTO.ProveedorDTO.RazonSocial,
                            FechaAutoriza = f.FechaAutorizacion.HasValue ? f.FechaAutorizacion.Value.ToShortDateString() : string.Empty,
                            EstadoAprobacion = f?.EstadoAprobacionDTO?.Codigo,
                            EstadoAprobacionNombre = f?.EstadoAprobacionDTO?.Nombre,
                            EstadoMovimientoAlmacenNombre = f.EstadoMovimientoAlmacenDTO.Nombre,
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
      [HttpPost("Crear")]
      public ActionResult Crear([FromBody] ControlMermaViewModel model)
      {
         Logger.InfoStart();

         ControlMermaViewModel controlMermaViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(controlMermaViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, controlMermaViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] ControlMermaViewModel model)
      {

         Logger.InfoStart();

         ControlMermaViewModel controlMermaViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(controlMermaViewModel, model.MovimientoAlmacenDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, controlMermaViewModel);
      }
      private void cargarDataParaEditar(ControlMermaViewModel model, int id)
      {

         MovimientoAlmacenDTO movimientoAlmacenDTO;
         if (model.EditAction == ViewModelEditAction.New)
         {
            movimientoAlmacenDTO = new MovimientoAlmacenDTO()
            {
               TipoMovimientoAlmacenDTO = new TipoMovimientoAlmacenDTO(),
               TransaccionAlmacenDTO = new TransaccionAlmacenDTO() { Codigo = string.Empty },
               UsuarioDTO = new UsuarioDTO()
            };
            movimientoAlmacenDTO.Fecha = DateTime.Today;
         }
         else
         {
            movimientoAlmacenDTO = movimientoAlmacenProxy.GetById(new MovimientoAlmacenDTO() { MovimientoAlmacenId = id });
         }

         model.MovimientoAlmacenDTO = movimientoAlmacenDTO;

         cargarListas(model);

         WebHelper.SelectedListItemSetValue(model.AlmacenList, movimientoAlmacenDTO.AlmacenId);
         WebHelper.SelectedListItemSetValue(model.TipoMovimientoAlmacenList, movimientoAlmacenDTO.TipoMovimientoAlmacenId);

         model.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { MenuItemType = MenuItemType.Save },
            new MenuItem() { Key="modificar", Content="Solicitar Modificacion"},
            new MenuItem() { Key="solicitar", Content="Solicitar Aprobación" },
            new MenuItem() { MenuItemType = MenuItemType.Exit}
         };
      }
      private void cargarListas(ControlMermaViewModel model)
      {
         cargarListasCompartidas(model);

         List<TipoMovimientoAlmacenDTO> tipoMovimientoAlmacenDTOList = tipoMovimientoAlmacenProxy.Search(new TipoMovimientoAlmacenDTO()
         {
            Estado = EstadoConstante.ACTIVO
         });
         model.TipoMovimientoAlmacenList = WebHelper.ToSelectListItem(tipoMovimientoAlmacenDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);
      }
      private void cargarListasCompartidas(ControlMermaViewModel model)
      {
         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

         List<AlmacenDTO> almacenDTOList = almacenProxy.Search(new AlmacenDTO() { 
            EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId },
            EmpresaSedeId = periodoEmpresaSedeDTO.EmpresaSedeId,
            Estado = EstadoConstante.ACTIVO 
         });
         model.AlmacenList = WebHelper.ToSelectListItem(almacenDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

      }
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody] ControlMermaViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            MovimientoAlmacenDTO movimientoAlmacenDTO = cargarMovimientoAlmacenDTO(model);

            int newId = model.MovimientoAlmacenDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = movimientoAlmacenProxy.Insert(movimientoAlmacenDTO);
                  messageResourceKey = "IV_movimiento_almacen_agregado";
                  break;

               case ViewModelEditAction.Edit:
                  movimientoAlmacenProxy.Update(movimientoAlmacenDTO);
                  messageResourceKey = "IV_movimiento_almacen_actualizado";
                  break;
            }

            movimientoAlmacenDTO = movimientoAlmacenProxy.GetById(new MovimientoAlmacenDTO() { MovimientoAlmacenId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, movimientoAlmacenDTO.Codigo);
            resultResponse.Data = new
            {
               movimientoAlmacenDTO.Id,
               movimientoAlmacenDTO.Codigo,
               movimientoAlmacenDTO.MesContableId,
               movimientoAlmacenDTO.AfectaSaldo,
               TipoMovimientoAlmacen = movimientoAlmacenDTO.TipoMovimientoAlmacenDTO.Codigo
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
      private MovimientoAlmacenDTO cargarMovimientoAlmacenDTO(ControlMermaViewModel model)
      {

         MovimientoAlmacenDTO movimientoAlmacenDTO;
         try
         {

            movimientoAlmacenDTO = model.MovimientoAlmacenDTO;

            if (model.EditAction == ViewModelEditAction.New)
            {
               PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

               movimientoAlmacenDTO.PeriodoEmpresaSedeId = periodoEmpresaSedeDTO.PeriodoEmpresaSedeId;

               string mesContable = string.Format(StringFormatConstant.DATE_MONTH,movimientoAlmacenDTO.Fecha);

               MesContableDTO mesContableDTO = mesContableProxy.ObtenerPorCodigo(new MesContableDTO() { Codigo = mesContable });
               movimientoAlmacenDTO.MesContableId = mesContableDTO.Id;

               TipoMovimientoAlmacenDTO tipoMovimientoAlmacenDTO = tipoMovimientoAlmacenProxy.GetById(new TipoMovimientoAlmacenDTO()
               {
                  TipoMovimientoAlmacenId = movimientoAlmacenDTO.TipoMovimientoAlmacenId
               });

               string transaccionAlmacen = string.Empty;
               if (tipoMovimientoAlmacenDTO.Codigo == TipoMovimientoAlmacenConstante.INGRESO)
                  transaccionAlmacen = TransaccionAlmacenConstante.INGRESO_MERMA;
               else
                  transaccionAlmacen = TransaccionAlmacenConstante.SALIDA_MERMA;

               TransaccionAlmacenDTO transaccionAlmacenDTO = transaccionAlmacenProxy.SingleSearch(new TransaccionAlmacenDTO() { 
                  EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId,
                  Codigo = transaccionAlmacen
               });
               movimientoAlmacenDTO.TransaccionAlmacenId = transaccionAlmacenDTO.Id;
               movimientoAlmacenDTO.AfectaSaldo = SiNoConstante.N;

               EstadoMovimientoAlmacenDTO estadoMovimientoAlmacenDTO = estadoMovimientoAlmacenProxy.ObtenerPorCodigo(
                  new EstadoMovimientoAlmacenDTO() { Codigo = EstadoMovimientoAlmacenConstante.MOVIMIENTO });
               movimientoAlmacenDTO.EstadoMovimientoAlmacenId = estadoMovimientoAlmacenDTO.Id;
            }
            else
            {
               movimientoAlmacenDTO = movimientoAlmacenProxy.GetById(new MovimientoAlmacenDTO() { MovimientoAlmacenId = movimientoAlmacenDTO.Id });

               model.MovimientoAlmacenDTO.AlmacenDestinoId = movimientoAlmacenDTO.AlmacenDestinoId;
               model.MovimientoAlmacenDTO.MesContableId = movimientoAlmacenDTO.MesContableId;
               model.MovimientoAlmacenDTO.ComprobanteId = movimientoAlmacenDTO.ComprobanteId;
               model.MovimientoAlmacenDTO.TieneGuiaRemision = movimientoAlmacenDTO.TieneGuiaRemision;
               model.MovimientoAlmacenDTO.GuiaRemisionEnModificacion = movimientoAlmacenDTO.GuiaRemisionEnModificacion;
               model.MovimientoAlmacenDTO.PersonalId = movimientoAlmacenDTO.PersonalId;
               model.MovimientoAlmacenDTO.PersonalAutorizaId = movimientoAlmacenDTO.PersonalAutorizaId;
               model.MovimientoAlmacenDTO.AfectaSaldo = movimientoAlmacenDTO.AfectaSaldo;
               model.MovimientoAlmacenDTO.EstadoMovimientoAlmacenId = movimientoAlmacenDTO.EstadoMovimientoAlmacenId;

               movimientoAlmacenDTO = model.MovimientoAlmacenDTO;
            }

            if (string.IsNullOrEmpty(movimientoAlmacenDTO.NumeroOrdenCompra))
               movimientoAlmacenDTO.NumeroOrdenCompra = string.Empty;

            if (string.IsNullOrEmpty(movimientoAlmacenDTO.SerieDocumento))
               movimientoAlmacenDTO.SerieDocumento = string.Empty;

            if (string.IsNullOrEmpty(movimientoAlmacenDTO.NumeroDocumento))
               movimientoAlmacenDTO.NumeroDocumento = string.Empty;

            if (string.IsNullOrEmpty(movimientoAlmacenDTO.NumeroGuiaRemision))
               movimientoAlmacenDTO.NumeroGuiaRemision = string.Empty;

            if (string.IsNullOrEmpty(movimientoAlmacenDTO.TieneGuiaRemision))
               movimientoAlmacenDTO.TieneGuiaRemision = SiNoConstante.N;

            if (string.IsNullOrEmpty(movimientoAlmacenDTO.GuiaRemisionEnModificacion))
               movimientoAlmacenDTO.GuiaRemisionEnModificacion = SiNoConstante.N;

            movimientoAlmacenDTO.Estado = EstadoConstante.ACTIVO;
            AuditDTO(movimientoAlmacenDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return movimientoAlmacenDTO;
      }
      [HttpPost("Eliminar")]
      public ActionResult Eliminar([FromBody] ControlMermaViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.MovimientoAlmacenDTO.DTOState = DTOState.Modified;

            AuditDTO(model.MovimientoAlmacenDTO);

            movimientoAlmacenProxy.Delete(model.MovimientoAlmacenDTO);

            resultResponse.Message = SR.GetString("IV_movimiento_almacen_anulado", model.MovimientoAlmacenDTO.Codigo);
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

         ControlMermaViewModel model = JsonSerializer.Deserialize<ControlMermaViewModel>(jsonModel);

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
                            f.MovimientoAlmacenId,
                            f.ProductoId,
                            f.ProductoDTO.Codigo,
                            ProductoNombre = f.ProductoDTO.Descripcion,
                            UnidadMedidaNombre = f.ProductoDTO.UnidadMedidaDTO.Abreviatura,
                            f.Descripcion,
                            f.CantidadDocumento,
                            f.PesoNetoDocumento,
                            f.PesoBrutoDocumento,
                            f.Cantidad,
                            f.PesoNeto,
                            f.PesoBruto,
                            f.ValorUnitario,
                            Total = f.ValorUnitario * f.Cantidad,
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
      public ActionResult GrabarMovimientoAlmacenProducto([FromBody] ControlMermaViewModel model)
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
               case DTOState.Added:
                  newId = movimientoAlmacenProductoProxy.Insert(movimientoAlmacenProductoDTO);
                  messageResourceKey = "IV_producto_agregado";
                  break;

               case DTOState.Modified:
                  movimientoAlmacenProductoProxy.Update(movimientoAlmacenProductoDTO);
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
      private MovimientoAlmacenProductoDTO cargarMovimientoAlmacenProductoDTO(ControlMermaViewModel model)
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
      [HttpPost("EliminarMovimientoAlmacenProducto")]
      public ActionResult EliminarMovimientoAlmacenProducto([FromBody] ControlMermaViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.MovimientoAlmacenProductoDTO.DTOState = DTOState.Modified;
            AuditDTO(model.MovimientoAlmacenProductoDTO);

            movimientoAlmacenProductoProxy.Delete(model.MovimientoAlmacenProductoDTO);

            resultResponse.Message = SR.GetString("IV_producto_anulado");
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
      [HttpPost("Solicitar")]
      public ActionResult Solicitar([FromBody] ControlMermaViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            MovimientoAlmacenDTO movimientoAlmacenDTO = cargarMovimientoAlmacenDTO(model);

            int newId = model.MovimientoAlmacenDTO.Id;
            string messageResourceKey = "IV_movimiento_almacen_aprobacion_solicitada";

            movimientoAlmacenProxy.SolicitarAutorizacion(movimientoAlmacenDTO);

            movimientoAlmacenDTO = movimientoAlmacenProxy.GetById(new MovimientoAlmacenDTO() { MovimientoAlmacenId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, movimientoAlmacenDTO.Codigo);
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

      [HttpPost("Modificar")]
      public ActionResult Modificar([FromBody] ControlMermaViewModel model)
      {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                MovimientoAlmacenDTO movimientoAlmacenDTO = cargarMovimientoAlmacenDTO(model);

                int newId = model.MovimientoAlmacenDTO.Id;
                string messageResourceKey = "IV_movimiento_almacen_aprobacion_solicitada";

                movimientoAlmacenProxy.SolicitarModificacionEdicion(movimientoAlmacenDTO);

                movimientoAlmacenDTO = movimientoAlmacenProxy.GetById(new MovimientoAlmacenDTO() { MovimientoAlmacenId = newId });

                resultResponse.Message = SR.GetString(messageResourceKey, movimientoAlmacenDTO.Codigo);
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

      [HttpGet("ObtenerServicioImpresion")]
      public JsonResult ObtenerServicioImpresion(int id)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {
            MovimientoAlmacenDTO movimientoAlmacenDTO = movimientoAlmacenProxy.GetById(new MovimientoAlmacenDTO() { MovimientoAlmacenId = id });

            AlmacenDTO almacenDTO = almacenProxy.GetById(new AlmacenDTO() { AlmacenId = movimientoAlmacenDTO.AlmacenId });

            string url = ObtenerDireccionServicioGeneracionFormatoImpresion();

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(url);

            resultResponse.Data = new
            {
               almacenDTO.ServidorImpresion,
               DataUrl = Convert.ToBase64String(plainTextBytes)
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
   }
}