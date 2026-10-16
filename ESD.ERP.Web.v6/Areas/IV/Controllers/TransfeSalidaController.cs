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
using System.Text.Json;

namespace ESD.ERP.Web.Areas.IV.Controllers
{
   [Area("IV")]
   [Route("[controller]")]
   public class TransfeSalidaController : AuthorizedController
   {
      private readonly IAlmacenProxy almacenProxy;
      private readonly ITransaccionAlmacenProxy transaccionAlmacenProxy;
      private readonly IMovimientoAlmacenProxy movimientoAlmacenProxy;
      private readonly ITipoMovimientoAlmacenProxy tipoMovimientoAlmacenProxy;
      private readonly IMesContableProxy mesContableProxy;
      private readonly IEstadoMovimientoAlmacenProxy estadoMovimientoAlmacenProxy;
      private readonly IMovimientoAlmacenProductoProxy movimientoAlmacenProductoProxy;
      public TransfeSalidaController(
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
         TransfeSalidaViewModel transfeSalidaViewModel = new();

         cargarListasCompartidas(transfeSalidaViewModel);

         return PartialView(transfeSalidaViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         TransfeSalidaViewModel model = JsonSerializer.Deserialize<TransfeSalidaViewModel>(jsonModel);

         model.MovimientoAlmacenDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.MovimientoAlmacenDTO.OrderBy = "ma.fecha,ma.mov_almacen";

         model.MovimientoAlmacenDTO.PeriodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(model.ViewModelState.CID);
         model.MovimientoAlmacenDTO.TransaccionAlmacenDTO = new TransaccionAlmacenDTO() { Codigo = TransaccionAlmacenConstante.TRANSFERENCIA_ENTRE_ALMACENES };
         model.MovimientoAlmacenDTO.TipoMovimientoAlmacenDTO = new TipoMovimientoAlmacenDTO() { Codigo = TipoMovimientoAlmacenConstante.SALIDA };
         model.MovimientoAlmacenDTO.EstadoMovimientoAlmacenList = new List<string>() { EstadoMovimientoAlmacenConstante.TRANSITO,
            EstadoMovimientoAlmacenConstante.ANULADO
         };

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
                            AlmacenDestinoNombre = f.AlmacenDestinoDTO == null ? string.Empty : f.AlmacenDestinoDTO.Nombre,
                            f.NumeroGuiaRemision,
                            f.DocumentoSerieNumeroReferencial,
                            RazonSocial = f.ProveedorDTO == null ? string.Empty : f.ProveedorDTO.RazonSocial,
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
      public ActionResult Crear([FromBody] TransfeSalidaViewModel model)
      {
         Logger.InfoStart();

         TransfeSalidaViewModel transfeSalidaViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(transfeSalidaViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, transfeSalidaViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] TransfeSalidaViewModel model)
      {

         Logger.InfoStart();

         TransfeSalidaViewModel transfeSalidaViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(transfeSalidaViewModel, model.MovimientoAlmacenDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, transfeSalidaViewModel);
      }
      private void cargarDataParaEditar(TransfeSalidaViewModel model, int id)
      {

         MovimientoAlmacenDTO movimientoAlmacenDTO;
         if (model.EditAction == ViewModelEditAction.New)
         {
            movimientoAlmacenDTO = new MovimientoAlmacenDTO() {
               Fecha = DateTime.Today
            };
         }
         else
         {
            movimientoAlmacenDTO = movimientoAlmacenProxy.GetById(new MovimientoAlmacenDTO() { MovimientoAlmacenId = id });
         }

         model.MovimientoAlmacenDTO = movimientoAlmacenDTO;

         cargarListas(model);

         WebHelper.SelectedListItemSetValue(model.AlmacenList, movimientoAlmacenDTO.AlmacenId);
         WebHelper.SelectedListItemSetValue(model.AlmacenDestinoList, movimientoAlmacenDTO.AlmacenDestinoId);
      }
      private void cargarListas(TransfeSalidaViewModel model)
      {
         cargarListasCompartidas(model);

         if(model.MovimientoAlmacenDTO.AlmacenId > 0)
         {
            PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO();

            List<AlmacenDTO> almacenDTOList = almacenProxy.Search(new AlmacenDTO()
            {
               EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId },
               NotAlmacenId = model.MovimientoAlmacenDTO.AlmacenId,
               Estado = EstadoConstante.ACTIVO
            });

            model.AlmacenDestinoList = WebHelper.ToSelectListItem(almacenDTOList, x => x.AlmacenId.ToString(), x => x.Nombre,
               SelectListFirstOptionType.Select);
         }
         else
            model.AlmacenDestinoList = WebHelper.ToSelectListItem(SelectListFirstOptionType.Select);
      }
      private void cargarListasCompartidas(TransfeSalidaViewModel model)
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
      [HttpGet("ListarAlmacenDestino")]
      public JsonResult ListarAlmacenDestino(int id)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {
            PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO();

            List<AlmacenDTO> almacenDTOList = almacenProxy.Search(new AlmacenDTO()
            {
               EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId },
               NotAlmacenId = id,
               Estado = EstadoConstante.ACTIVO
            });

            resultResponse.Data = WebHelper.ToSelectListItem(almacenDTOList, x => x.AlmacenId.ToString(), x => x.Nombre,
               SelectListFirstOptionType.Select);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            resultResponse.Message = e.Message;
            resultResponse.Success = false;
         }

         Logger.InfoFinish();

         return Json(resultResponse);
      }
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody] TransfeSalidaViewModel model)
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
               movimientoAlmacenDTO.AfectaSaldo
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
      private MovimientoAlmacenDTO cargarMovimientoAlmacenDTO(TransfeSalidaViewModel model)
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

               TransaccionAlmacenDTO transaccionAlmacenDTO = transaccionAlmacenProxy.SingleSearch(new TransaccionAlmacenDTO() { 
                  EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId,
                  Codigo = TransaccionAlmacenConstante.TRANSFERENCIA_ENTRE_ALMACENES
               });
               movimientoAlmacenDTO.TransaccionAlmacenId = transaccionAlmacenDTO.Id;

               TipoMovimientoAlmacenDTO tipoMovimientoAlmacenDTO = tipoMovimientoAlmacenProxy.ObtenerPorCodigo(new TipoMovimientoAlmacenDTO() { 
                  Codigo = TipoMovimientoAlmacenConstante.SALIDA
               });
               movimientoAlmacenDTO.TipoMovimientoAlmacenId = tipoMovimientoAlmacenDTO.Id;

               AlmacenDTO almacenDestinoDTO = almacenProxy.GetById(new AlmacenDTO() { AlmacenId = movimientoAlmacenDTO.AlmacenDestinoId.Value });
               movimientoAlmacenDTO.AfectaSaldo = almacenDestinoDTO.EmpresaSedeId != periodoEmpresaSedeDTO.EmpresaSedeId ? SiNoConstante.S : SiNoConstante.N;

               EstadoMovimientoAlmacenDTO estadoMovimientoAlmacenDTO = estadoMovimientoAlmacenProxy.ObtenerPorCodigo(
                  new EstadoMovimientoAlmacenDTO() { Codigo = EstadoMovimientoAlmacenConstante.TRANSITO });
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
               model.MovimientoAlmacenDTO.CentroCostoId = movimientoAlmacenDTO.CentroCostoId;
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
      public ActionResult Eliminar([FromBody] TransfeSalidaViewModel model)
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

         TransfeSalidaViewModel model = JsonSerializer.Deserialize<TransfeSalidaViewModel>(jsonModel);

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
      public ActionResult GrabarMovimientoAlmacenProducto([FromBody] TransfeSalidaViewModel model)
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
      private MovimientoAlmacenProductoDTO cargarMovimientoAlmacenProductoDTO(TransfeSalidaViewModel model)
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
      public ActionResult EliminarMovimientoAlmacenProducto([FromBody] TransfeSalidaViewModel model)
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
   }
}