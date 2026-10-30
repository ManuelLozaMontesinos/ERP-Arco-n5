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
   public class TransfeIngresoController : AuthorizedController
   {
      private readonly IAlmacenProxy almacenProxy;
      private readonly IMovimientoAlmacenProxy movimientoAlmacenProxy;
      private readonly ITipoMovimientoAlmacenProxy tipoMovimientoAlmacenProxy;
      private readonly IMesContableProxy mesContableProxy;
      private readonly IEstadoMovimientoAlmacenProxy estadoMovimientoAlmacenProxy;
      private readonly IMovimientoAlmacenProductoProxy movimientoAlmacenProductoProxy;
      public TransfeIngresoController(
         IAlmacenProxy almacenProxy,
         IMovimientoAlmacenProxy movimientoAlmacenProxy,
         ITipoMovimientoAlmacenProxy tipoMovimientoAlmacenProxy,
         IMesContableProxy mesContableProxy,
         IEstadoMovimientoAlmacenProxy estadoMovimientoAlmacenProxy,
         IMovimientoAlmacenProductoProxy movimientoAlmacenProductoProxy
         )
      {
         this.almacenProxy = almacenProxy;
         this.movimientoAlmacenProxy = movimientoAlmacenProxy;
         this.tipoMovimientoAlmacenProxy = tipoMovimientoAlmacenProxy;
         this.mesContableProxy = mesContableProxy;
         this.estadoMovimientoAlmacenProxy = estadoMovimientoAlmacenProxy;
         this.movimientoAlmacenProductoProxy = movimientoAlmacenProductoProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         TransfeIngresoViewModel transfeSalidaViewModel = new();

         cargarListasCompartidas(transfeSalidaViewModel);

         transfeSalidaViewModel.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { Key = "recibir", Content = "Recibir" }
         };

         return PartialView(transfeSalidaViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         TransfeIngresoViewModel model = JsonSerializer.Deserialize<TransfeIngresoViewModel>(jsonModel);

         model.MovimientoAlmacenDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.MovimientoAlmacenDTO.OrderBy = "ma.fecha desc,ma.mov_almacen desc";

         model.MovimientoAlmacenDTO.PeriodoEmpresaSedeDTO= ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);
         model.MovimientoAlmacenDTO.TransaccionAlmacenDTO = new TransaccionAlmacenDTO() { Codigo = TransaccionAlmacenConstante.TRANSFERENCIA_ENTRE_ALMACENES };
         model.MovimientoAlmacenDTO.TipoMovimientoAlmacenDTO = new TipoMovimientoAlmacenDTO() { Codigo = TipoMovimientoAlmacenConstante.SALIDA };
         model.MovimientoAlmacenDTO.EstadoMovimientoAlmacenList = new List<string>() { EstadoMovimientoAlmacenConstante.TRANSITO,
            EstadoMovimientoAlmacenConstante.ANULADO
         };

         try
         {
            List<MovimientoAlmacenDTO> movimientoAlmacenDTOList = movimientoAlmacenProxy.BuscarPorEmpresa(model.MovimientoAlmacenDTO);

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
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] TransfeIngresoViewModel model)
      {

         Logger.InfoStart();

         TransfeIngresoViewModel transfeSalidaViewModel = new()
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
      private void cargarDataParaEditar(TransfeIngresoViewModel model, int id)
      {

         MovimientoAlmacenDTO movimientoAlmacenDTO = movimientoAlmacenProxy.GetById(new MovimientoAlmacenDTO() { MovimientoAlmacenId = id });

         movimientoAlmacenDTO.Codigo = string.Empty;
         //movimientoAlmacenDTO.Fecha = DateTime.Today;

         model.MovimientoAlmacenDTO = movimientoAlmacenDTO;

         cargarListas(model);

         WebHelper.SelectedListItemSetValue(model.AlmacenList, movimientoAlmacenDTO.AlmacenId);
         WebHelper.SelectedListItemSetValue(model.AlmacenDestinoList, movimientoAlmacenDTO.AlmacenDestinoId);
      }
      private void cargarListas(TransfeIngresoViewModel model)
      {
         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO();

         List<AlmacenDTO> almacenDTOList = almacenProxy.Search(new AlmacenDTO()
         {
            EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId },
            Estado = EstadoConstante.ACTIVO
         });

         model.AlmacenList = WebHelper.ToSelectListItem(almacenDTOList, x => x.AlmacenId.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         model.AlmacenDestinoList = WebHelper.ToSelectListItem(almacenDTOList, x => x.AlmacenId.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);
      }
      private void cargarListasCompartidas(TransfeIngresoViewModel model)
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
      public ActionResult Grabar([FromBody] TransfeIngresoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            MovimientoAlmacenDTO movimientoAlmacenDTO = cargarMovimientoAlmacenDTO(model);

            int newId = movimientoAlmacenProxy.RecepcionarTransferencia(movimientoAlmacenDTO);

            movimientoAlmacenDTO = movimientoAlmacenProxy.GetById(new MovimientoAlmacenDTO() { MovimientoAlmacenId = newId });

            resultResponse.Message = SR.GetString("IV_almacen_transferencia_ingreso_recepcionada", movimientoAlmacenDTO.Codigo);
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
      private MovimientoAlmacenDTO cargarMovimientoAlmacenDTO(TransfeIngresoViewModel model)
      {

         MovimientoAlmacenDTO movimientoAlmacenDTO;
         try
         {

            movimientoAlmacenDTO = movimientoAlmacenProxy.GetById(new MovimientoAlmacenDTO() { MovimientoAlmacenId = model.MovimientoAlmacenDTO.Id });

            PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

            movimientoAlmacenDTO.PeriodoEmpresaSedeId = periodoEmpresaSedeDTO.PeriodoEmpresaSedeId;
            movimientoAlmacenDTO.AlmacenId = model.MovimientoAlmacenDTO.AlmacenId;

            TipoMovimientoAlmacenDTO tipoMovimientoAlmacenDTO = tipoMovimientoAlmacenProxy.ObtenerPorCodigo(new TipoMovimientoAlmacenDTO()
            {
               Codigo = TipoMovimientoAlmacenConstante.INGRESO
            });
            movimientoAlmacenDTO.TipoMovimientoAlmacenId = tipoMovimientoAlmacenDTO.Id;
            movimientoAlmacenDTO.AlmacenDestinoId = null;
            movimientoAlmacenDTO.MovimientoAlmacenReferencialId = model.MovimientoAlmacenDTO.Id;
            movimientoAlmacenDTO.Fecha = model.MovimientoAlmacenDTO.Fecha;

            string mesContable = string.Format(StringFormatConstant.DATE_MONTH, movimientoAlmacenDTO.Fecha);
            MesContableDTO mesContableDTO = mesContableProxy.ObtenerPorCodigo(new MesContableDTO() { Codigo = mesContable });
            movimientoAlmacenDTO.MesContableId = mesContableDTO.Id;

            movimientoAlmacenDTO.AfectaSaldo = SiNoConstante.S;

            EstadoMovimientoAlmacenDTO estadoMovimientoAlmacenDTO = estadoMovimientoAlmacenProxy.ObtenerPorCodigo(
               new EstadoMovimientoAlmacenDTO() { Codigo = EstadoMovimientoAlmacenConstante.MOVIMIENTO });
            movimientoAlmacenDTO.EstadoMovimientoAlmacenId = estadoMovimientoAlmacenDTO.Id;

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
      public ActionResult Eliminar([FromBody] TransfeIngresoViewModel model)
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

         TransfeIngresoViewModel model = JsonSerializer.Deserialize<TransfeIngresoViewModel>(jsonModel);

         model.MovimientoAlmacenProductoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.MovimientoAlmacenProductoDTO.OrderBy = "p.producto";
         model.MovimientoAlmacenProductoDTO.Estado = EstadoConstante.ACTIVO;

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
   }
}