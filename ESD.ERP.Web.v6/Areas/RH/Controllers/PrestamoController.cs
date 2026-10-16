using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service.DTO;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.RH.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.RH.Controllers
{
   [Area("RH")]
   [Route("[controller]")]
   public class PrestamoController : AuthorizedController
   {
      private readonly IPrestamoPersonalProxy prestamoPersonalProxy;
      private readonly IMonedaProxy monedaProxy;
      private readonly ITipoPrestamoProxy tipoPrestamoProxy;
      private readonly IPersonalProxy personalProxy;
      private readonly IPrestamoCuotaProxy prestamoCuotaProxy;
      private readonly ITipoCancelacionCuotaProxy tipoCancelacionCuotaProxy;
      private readonly IEstadoCuotaProxy estadoCuotaProxy;
      private readonly IEstadoPrestamoProxy estadoPrestamoProxy;
      public PrestamoController(
         IPrestamoPersonalProxy prestamoPersonalProxy,
         IMonedaProxy monedaProxy,
         ITipoPrestamoProxy tipoPrestamoProxy,
         IPersonalProxy personalProxy,
         IPrestamoCuotaProxy prestamoCuotaProxy,
         ITipoCancelacionCuotaProxy tipoCancelacionCuotaProxy,
         IEstadoCuotaProxy estadoCuotaProxy,
         IEstadoPrestamoProxy estadoPrestamoProxy
         )
      {
         this.prestamoPersonalProxy = prestamoPersonalProxy;
         this.monedaProxy = monedaProxy;
         this.tipoPrestamoProxy = tipoPrestamoProxy;
         this.personalProxy = personalProxy;
         this.prestamoCuotaProxy = prestamoCuotaProxy;
         this.tipoCancelacionCuotaProxy = tipoCancelacionCuotaProxy;
         this.estadoCuotaProxy = estadoCuotaProxy;
         this.estadoPrestamoProxy = estadoPrestamoProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         PrestamoViewModel prestamoPrestamoViewModel = new();

         cargarListas(prestamoPrestamoViewModel);

         return PartialView(prestamoPrestamoViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         PrestamoViewModel model = JsonSerializer.Deserialize<PrestamoViewModel>(jsonModel);

         model.PrestamoPersonalDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.PrestamoPersonalDTO.OrderBy = "pp.fecha desc,prv.razon_social";

         try
         {
            List<PrestamoPersonalDTO> prestamoPersonalDTOList = prestamoPersonalProxy.Search(model.PrestamoPersonalDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = prestamoPersonalDTOList.Count,
               rows = from f in prestamoPersonalDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.PrestamoPersonalId,
                            f.Codigo,
                            Fecha = f.Fecha.ToShortDateString(),
                            TipoDocumentoIdentidad = f.PersonalDTO.ProveedorDTO.TipoDocumentoIdentidadDTO.Abreviatura,
                            f.PersonalDTO.ProveedorDTO.NumeroDocumentoIdentidad,
                            f.PersonalDTO.ProveedorDTO.RazonSocial,
                            MonedaNombre = f.MonedaDTO.Nombre,
                            f.Monto,
                            TipoPrestamo = f.TipoPrestamoDTO.Nombre,
                            EstadoNombre = f.EstadoPrestamoDTO.Nombre,
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
      public ActionResult Crear([FromBody] PrestamoViewModel model)
      {
         Logger.InfoStart();

         PrestamoViewModel prestamoPrestamoViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(prestamoPrestamoViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, prestamoPrestamoViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] PrestamoViewModel model)
      {

         Logger.InfoStart();

         PrestamoViewModel prestamoPrestamoViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(prestamoPrestamoViewModel, model.PrestamoPersonalDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, prestamoPrestamoViewModel);
      }
      private void cargarDataParaEditar(PrestamoViewModel model, int id)
      {

         PrestamoPersonalDTO prestamoPersonalDTO = null;
         if (model.EditAction == ViewModelEditAction.New)
         {
            MonedaDTO monedaDTO = monedaProxy.SingleSearch(new MonedaDTO() { EsLocal = SiNoConstante.S });
            prestamoPersonalDTO = new PrestamoPersonalDTO
            {
               Fecha = DateTime.Today,
               PersonalDTO = new PersonalDTO() { ProveedorDTO = new ProveedorDTO() },
               MonedaDTO = monedaDTO,
               MonedaId = monedaDTO.MonedaId,
               FechaInicio = DateTime.Today,
            };
         }
         else
         {
            prestamoPersonalDTO = prestamoPersonalProxy.GetById(new PrestamoPersonalDTO() { PrestamoPersonalId = id });
         }

         TipoCancelacionCuotaDTO tipoCancelacionCuotaDTO = tipoCancelacionCuotaProxy.ObtenerPorCodigo(new TipoCancelacionCuotaDTO()
            { Codigo = TipoCancelacionCuotaConstante.DESCUENTO });
         model.TipoCancelacionCuotaId = tipoCancelacionCuotaDTO.Id;
         model.TipoCancelacionCuotaNombre = tipoCancelacionCuotaDTO.Nombre;

         EstadoCuotaDTO estadoCuotaDTO = estadoCuotaProxy.ObtenerPorCodigo(new EstadoCuotaDTO() { Codigo = EstadoCuotaConstante.PENDIENTE });
         model.EstadoCuotaId = estadoCuotaDTO.Id;
         model.EstadoCuotaNombre = estadoCuotaDTO.Nombre;

         model.PrestamoPersonalDTO = prestamoPersonalDTO;
         cargarListas(model);

         WebHelper.SelectedListItemSetValue(model.TipoPrestamoList, prestamoPersonalDTO.TipoPrestamoId);
         WebHelper.SelectedListItemSetValue(model.MonedaList, prestamoPersonalDTO.MonedaId);

         model.MenuItemList = new List<MenuItem>() {
            new MenuItem() { Key="suspender", Content="Suspender"},
            new MenuItem() { Key="pagar", Content="Pagar"}
         };
      }
      private void cargarListas(PrestamoViewModel model)
      {

         List<TipoPrestamoDTO> tipoPrestamoDTOList = tipoPrestamoProxy.Search(new TipoPrestamoDTO()
         {
            Estado = EstadoConstante.ACTIVO
         });

         model.TipoPrestamoList = WebHelper.ToSelectListItem(tipoPrestamoDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<MonedaDTO> monedaDTOList = monedaProxy.Search(new MonedaDTO()
         {
            Estado = EstadoConstante.ACTIVO
         });
         model.MonedaList = WebHelper.ToSelectListItem(monedaDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

      }
      [HttpGet("ObtenerTipoPrestamo")]
      public JsonResult ObtenerTipoPrestamo(int id)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {
            TipoPrestamoDTO tipoPrestamoDTO = tipoPrestamoProxy.GetById(new TipoPrestamoDTO() { TipoPrestamoId = id });

            resultResponse.Data = new
            {
               tipoPrestamoDTO.Codigo
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
      [HttpGet("ObtenerMoneda")]
      public JsonResult ObtenerMoneda(int id)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {
            MonedaDTO monedaDTO = monedaProxy.GetById(new MonedaDTO() { MonedaId = id });

            resultResponse.Data = new
            {
               monedaDTO.EsLocal
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
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody] PrestamoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            PrestamoPersonalDTO prestamoPersonalDTO = cargarPrestamoPersonalDTO(model);

            int newId = model.PrestamoPersonalDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = prestamoPersonalProxy.Insert(prestamoPersonalDTO);
                  messageResourceKey = "RH_prestamo_agregado";
                  break;

               case ViewModelEditAction.Edit:
                  prestamoPersonalProxy.Update(prestamoPersonalDTO);
                  messageResourceKey = "RH_prestamo_actualizado";
                  break;
            }

            prestamoPersonalDTO = prestamoPersonalProxy.GetById(new PrestamoPersonalDTO() { PrestamoPersonalId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, prestamoPersonalDTO.Codigo);
            resultResponse.Data = new
            {
               Id = newId,
               prestamoPersonalDTO.Codigo
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
      private PrestamoPersonalDTO cargarPrestamoPersonalDTO(PrestamoViewModel model)
      {

         try
         {

            PrestamoPersonalDTO prestamoPersonalDTO = null;
            if (model.EditAction == ViewModelEditAction.New)
            {
               prestamoPersonalDTO = model.PrestamoPersonalDTO;

               PersonalDTO personalDTO = personalProxy.GetById(new PersonalDTO() { PersonalId = prestamoPersonalDTO.PersonalId });
               prestamoPersonalDTO.PersonalDTO = personalDTO;
               prestamoPersonalDTO.PeriodoId = ObtenerPeriodoId(model.ViewModelState.CID);

               EstadoPrestamoDTO estadoPrestamoDTO = estadoPrestamoProxy.ObtenerPorCodigo(new EstadoPrestamoDTO() { Codigo = EstadoPrestamoConstante.PENDIENTE });
               prestamoPersonalDTO.EstadoPrestamoId = estadoPrestamoDTO.Id;
            }
            else
            {
               prestamoPersonalDTO = prestamoPersonalProxy.GetById(new PrestamoPersonalDTO() { PrestamoPersonalId = model.PrestamoPersonalDTO.Id });

               prestamoPersonalDTO = model.PrestamoPersonalDTO;
            }

            prestamoPersonalDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(prestamoPersonalDTO);

            foreach (PrestamoCuotaDTO prestamoCuotaDTO in model.PrestamoCuotaDTOList)
            {
               prestamoCuotaDTO.Estado = EstadoConstante.ACTIVO;
               AuditDTO(prestamoCuotaDTO);
            }

            prestamoPersonalDTO.PrestamoCuotaDTOList = model.PrestamoCuotaDTOList;
         }
         catch (Exception)
         {
            throw;
         }

         return model.PrestamoPersonalDTO;
      }
      [HttpGet("Eliminar")]
      public JsonResult Eliminar(int id)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            PrestamoPersonalDTO prestamoPersonalDTO = prestamoPersonalProxy.GetById(new PrestamoPersonalDTO() { PrestamoPersonalId = id });

            AuditDTO(prestamoPersonalDTO);

            prestamoPersonalProxy.Delete(prestamoPersonalDTO);

            resultResponse.Message = SR.GetString("RH_prestamo_anulado", prestamoPersonalDTO.Codigo);
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
      [HttpPost("ListarPrestamoCuota")]
      public ActionResult ListarPrestamoCuota(string jsonModel)
      {

         Logger.InfoStart();

         PrestamoViewModel model = JsonSerializer.Deserialize<PrestamoViewModel>(jsonModel);

         model.PrestamoCuotaDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.PrestamoCuotaDTO.OrderBy = "cuota";

         try
         {
            List<PrestamoCuotaDTO> personalDependienteDTOList = prestamoCuotaProxy.Search(model.PrestamoCuotaDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = personalDependienteDTOList.Count,
               rows = from f in personalDependienteDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.PrestamoCuotaId,
                            f.PrestamoPersonalId,
                            f.Cuota,
                            f.Monto,
                            FechaVencimiento = f.FechaVencimiento.ToShortDateString(),
                            FechaCancelacion = f.FechaCancelacion == null ? string.Empty : f.FechaCancelacion.Value.ToShortDateString(),
                            f.TipoCancelacionCuotaId,
                            TipoCancelacionCuota = f.TipoCancelacionCuotaDTO.Nombre,
                            f.EstadoCuotaId,
                            EstadoCuota = f.EstadoCuotaDTO.Nombre,
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
