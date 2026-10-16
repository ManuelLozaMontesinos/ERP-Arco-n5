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
   public class AlmacenController : AuthorizedController
   {

      private readonly IAlmacenProxy almacenProxy;
      private readonly IEmpresaSedeProxy empresaSedeProxy;
      private readonly ITipoViaProxy tipoViaProxy;
      private readonly ITipoZonaProxy tipoZonaProxy;
      private readonly IDepartamentoProxy departamentoProxy;
      private readonly IProvinciaProxy provinciaProxy;
      private readonly IDistritoProxy distritoProxy;
      private readonly IOrigenRegistroProxy origenRegistroProxy;
      private readonly ITransaccionAlmacenProxy transaccionAlmacenProxy;
      private readonly IAlmacenTransaccionProxy almacenTransaccionProxy;
      private readonly IAlmacenTransaccionConfiguracionProxy almacenTransaccionConfiguracionProxy;
      private readonly IAlmacenSerieProxy almacenSerieProxy;

      public AlmacenController(
         IAlmacenProxy almacenProxy,
         IEmpresaSedeProxy empresaSedeProxy,
         ITipoViaProxy tipoViaProxy,
         ITipoZonaProxy tipoZonaProxy,
         IDepartamentoProxy departamentoProxy,
         IProvinciaProxy provinciaProxy,
         IDistritoProxy distritoProxy,
         IOrigenRegistroProxy origenRegistroProxy,
         ITransaccionAlmacenProxy transaccionAlmacenProxy,
         IAlmacenTransaccionProxy almacenTransaccionProxy,
         IAlmacenTransaccionConfiguracionProxy almacenTransaccionConfiguracionProxy,
         IAlmacenSerieProxy almacenSerieProxy
         )
      {
         this.almacenProxy = almacenProxy;
         this.empresaSedeProxy = empresaSedeProxy;
         this.tipoViaProxy = tipoViaProxy;
         this.tipoZonaProxy = tipoZonaProxy;
         this.departamentoProxy = departamentoProxy;
         this.provinciaProxy = provinciaProxy;
         this.distritoProxy = distritoProxy;
         this.origenRegistroProxy = origenRegistroProxy;
         this.transaccionAlmacenProxy = transaccionAlmacenProxy;
         this.almacenTransaccionProxy = almacenTransaccionProxy;
         this.almacenTransaccionConfiguracionProxy = almacenTransaccionConfiguracionProxy;
         this.almacenSerieProxy = almacenSerieProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         AlmacenViewModel almacenViewModel = new();

         cargarListas(almacenViewModel);

         return PartialView(almacenViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         AlmacenViewModel model = JsonSerializer.Deserialize<AlmacenViewModel>(jsonModel);

         model.AlmacenDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.AlmacenDTO.OrderBy = "a.nombre";

         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

         if (model.AlmacenDTO.EmpresaSedeDTO == null)
            model.AlmacenDTO.EmpresaSedeDTO = new EmpresaSedeDTO();

         model.AlmacenDTO.EmpresaSedeDTO.EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId;

         try
         {
            List<AlmacenDTO> almacenDTOList = almacenProxy.Search(model.AlmacenDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = almacenDTOList.Count,
               rows = from f in almacenDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.AlmacenId,
                            EmpresaSedeNombre = f.EmpresaSedeDTO.Nombre,
                            f.Nombre,
                            f.Direccion,
                            f.EsVenta,
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
      public ActionResult Crear([FromBody] AlmacenViewModel model)
      {
         Logger.InfoStart();

         AlmacenViewModel almacenViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(almacenViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, almacenViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] AlmacenViewModel model)
      {

         Logger.InfoStart();

         AlmacenViewModel almacenViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(almacenViewModel, model.AlmacenDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, almacenViewModel);
      }
      private void cargarDataParaEditar(AlmacenViewModel model, int id)
      {

         AlmacenDTO almacenDTO;
         if (model.EditAction == ViewModelEditAction.New)
         {
            almacenDTO = new AlmacenDTO();
            DistritoDTO distritoDTO = new()
            {
               Codigo = DistritoConstante.DEFAULT,
               Estado = EstadoConstante.ACTIVO,
               ProvinciaDTO = new ProvinciaDTO()
               {
                  Codigo = ProvinciaConstante.DEFAULT,
                  DepartamentoDTO = new DepartamentoDTO() { Codigo = DepartamentoConstante.DEFAULT }
               }
            };

            almacenDTO.DistritoDTO = distritoProxy.SingleSearch(distritoDTO);
            almacenDTO.DistritoId = almacenDTO.DistritoDTO.Id;

            TipoViaDTO tipoViaDTO = tipoViaProxy.ObtenerPorCodigo(new TipoViaDTO() { Codigo = TipoViaConstante.DEFAULT });
            almacenDTO.TipoViaId = tipoViaDTO.Id;

            TipoZonaDTO tipoZonaDTO = tipoZonaProxy.ObtenerPorCodigo(new TipoZonaDTO() { Codigo = TipoZonaConstante.DEFAULT });
            almacenDTO.TipoZonaId = tipoZonaDTO.Id;
         }
         else
         {
            almacenDTO = almacenProxy.GetById(new AlmacenDTO() { AlmacenId = id });
         }

         model.AlmacenDTO = almacenDTO;

         cargarListas(model);
         cargarUbigeo(model, model.AlmacenDTO.DistritoDTO.ProvinciaDTO.DepartamentoId,
            model.AlmacenDTO.DistritoDTO.ProvinciaId);

         almacenDTO.EsVenta = almacenDTO.EsVenta == SiNoConstante.S ? EstadoConstante.ACTIVO : EstadoConstante.INACTIVO;

         WebHelper.SelectedListItemSetValue(model.EmpresaSedeList, almacenDTO.EmpresaSedeId);
         WebHelper.SelectedListItemSetValue(model.TipoViaList, almacenDTO.TipoViaId);
         WebHelper.SelectedListItemSetValue(model.TipoZonaList, almacenDTO.TipoZonaId);
         WebHelper.SelectedListItemSetValue(model.DistritoList, almacenDTO.DistritoId);
         WebHelper.SelectedListItemSetValue(model.ProvinciaList, almacenDTO.DistritoDTO.ProvinciaId);
         WebHelper.SelectedListItemSetValue(model.DepartamentoList, almacenDTO.DistritoDTO.ProvinciaDTO.DepartamentoId);

      }
      private void cargarListas(AlmacenViewModel model)
      {
         List<EmpresaSedeDTO> empresaSedeDTOList = empresaSedeProxy.Search(new EmpresaSedeDTO() { 
            EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID),
            Estado = EstadoConstante.ACTIVO 
         });
         model.EmpresaSedeList = WebHelper.ToSelectListItem(empresaSedeDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<TipoViaDTO> tipoViaDTOList = tipoViaProxy.Search(new TipoViaDTO() { Estado = EstadoConstante.ACTIVO });
         model.TipoViaList = WebHelper.ToSelectListItem(tipoViaDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<TipoZonaDTO> tipoZonaDTOList = tipoZonaProxy.Search(new TipoZonaDTO() { Estado = EstadoConstante.ACTIVO });
         model.TipoZonaList = WebHelper.ToSelectListItem(tipoZonaDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<TransaccionAlmacenDTO> transaccionAlmacenDTOList = transaccionAlmacenProxy.Search(new TransaccionAlmacenDTO() { 
            EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID),
            Estado = EstadoConstante.ACTIVO 
         });
         model.TransaccionAlmacenString = string.Join(SeparadorConstante.PUNTO_COMA, transaccionAlmacenDTOList.Select(x => x.Id.ToString() 
         + SeparadorConstante.DOS_PUNTOS + x.Nombre));
      }
      private void cargarUbigeo(AlmacenViewModel model, int departamentoId, int provinciaId)
      {

         List<DepartamentoDTO> departamentoDTOList = departamentoProxy.Search(new DepartamentoDTO() { Estado = EstadoConstante.ACTIVO });
         model.DepartamentoList = WebHelper.ToSelectListItem(departamentoDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<ProvinciaDTO> provinciaDTOList = obtenerListaProvincia(departamentoId);
         model.ProvinciaList = WebHelper.ToSelectListItem(provinciaDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<DistritoDTO> distritoDTOList = obtenerListaDistrito(provinciaId);
         model.DistritoList = WebHelper.ToSelectListItem(distritoDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);
      }
      private List<ProvinciaDTO> obtenerListaProvincia(int departamentoId)
      {
         List<ProvinciaDTO> provinciaDTOList = provinciaProxy.Search(new ProvinciaDTO()
         {
            DepartamentoId = departamentoId,
            Estado = EstadoConstante.ACTIVO
         });

         return provinciaDTOList;
      }
      private List<DistritoDTO> obtenerListaDistrito(int provinciaId)
      {
         List<DistritoDTO> distritoDTOList = distritoProxy.Search(new DistritoDTO()
         {
            ProvinciaId = provinciaId,
            Estado = EstadoConstante.ACTIVO
         });

         return distritoDTOList;
      }
      [HttpGet("ListarProvincia")]
      public JsonResult ListarProvincia(int id)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {

            List<ProvinciaDTO> provinciaDTOList = obtenerListaProvincia(id);
            resultResponse.Data = WebHelper.ToSelectListItem(provinciaDTOList, x => x.Id.ToString(), x => x.Nombre,
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
      [HttpGet("ListarDistrito")]
      public JsonResult ListarDistrito(int id)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {
            List<DistritoDTO> provinciaDTOList = obtenerListaDistrito(id);
            resultResponse.Data = WebHelper.ToSelectListItem(provinciaDTOList, x => x.Id.ToString(), x => x.Nombre,
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
      public ActionResult Grabar([FromBody] AlmacenViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            AlmacenDTO almacenDTO = cargarAlmacenDTO(model);

            int newId = model.AlmacenDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = almacenProxy.Insert(almacenDTO);
                  messageResourceKey = "IV_almacen_agregado";
                  break;

               case ViewModelEditAction.Edit:
                  almacenProxy.Update(almacenDTO);
                  messageResourceKey = "IV_almacen_actualizado";
                  break;
            }

            almacenDTO = almacenProxy.GetById(new AlmacenDTO() { AlmacenId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, almacenDTO.Nombre);
            resultResponse.Data = new
            {
               almacenDTO.Id
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
      private AlmacenDTO cargarAlmacenDTO(AlmacenViewModel model)
      {

         AlmacenDTO almacenDTO;
         try
         {

            almacenDTO = model.AlmacenDTO;

            if (model.EditAction == ViewModelEditAction.New)
            {
               OrigenRegistroDTO origenRegistroDTO = origenRegistroProxy
                  .ObtenerPorCodigo(new OrigenRegistroDTO() { Codigo = OrigenRegistroConstante.USUARIO });
               almacenDTO.OrigenRegistroId = origenRegistroDTO.Id;
            }

            almacenDTO.Estado = EstadoConstante.ACTIVO;
            AuditDTO(almacenDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return almacenDTO;
      }
      [HttpPost("Eliminar")]
      public ActionResult Eliminar([FromBody] AlmacenViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.AlmacenDTO.DTOState = DTOState.Modified;

            AuditDTO(model.AlmacenDTO);

            almacenProxy.Delete(model.AlmacenDTO);

            resultResponse.Message = SR.GetString("IV_almacen_anulado", model.AlmacenDTO.Nombre);
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
      [HttpPost("ListarAlmacenTransaccion")]
      public ActionResult ListarAlmacenTransaccion(string jsonModel)
      {

         Logger.InfoStart();

         AlmacenViewModel model = JsonSerializer.Deserialize<AlmacenViewModel>(jsonModel);

         model.AlmacenTransaccionDTO.PageSize = ObtenerMaximoRegistrosBusqueda();

         try
         {
            List<AlmacenTransaccionDTO> almacenTransaccionDTOList = almacenTransaccionProxy.Search(model.AlmacenTransaccionDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = almacenTransaccionDTOList.Count,
               rows = from f in almacenTransaccionDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.AlmacenTransaccionId,
                            f.AlmacenId,
                            f.TransaccionAlmacenId,
                            f.TransaccionAlmacenDTO.Nombre,
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
      [HttpPost("GrabarAlmacenTransaccion")]
      public ActionResult GrabarAlmacenTransaccion([FromBody] AlmacenViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            AlmacenTransaccionDTO almacenTransaccionDTO = model.AlmacenTransaccionDTO;
            almacenTransaccionDTO.Estado = EstadoConstante.ACTIVO;
            AuditDTO(almacenTransaccionDTO);

            switch (almacenTransaccionDTO.DTOState)
            {
               case DTOState.Added:
                  almacenTransaccionProxy.Insert(almacenTransaccionDTO);
                  break;

               case DTOState.Modified:
                  almacenTransaccionProxy.Update(almacenTransaccionDTO);
                  break;
            }

            resultResponse.Message = SR.GetString("IV_almacen_trasaccion_agregada");
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
      [HttpPost("EliminarAlmacenTransaccion")]
      public ActionResult EliminarAlmacenTransaccion([FromBody] AlmacenViewModel model)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.AlmacenTransaccionDTO.DTOState = DTOState.Deleted;

            almacenTransaccionProxy.Delete(model.AlmacenTransaccionDTO);

            resultResponse.Message = SR.GetString("IV_almacen_trasaccion_anulada");

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
      [HttpPost("ListarAlmacenTransaccionConfiguracion")]
      public ActionResult ListarAlmacenTransaccionConfiguracion(string jsonModel)
      {

         Logger.InfoStart();

         AlmacenViewModel model = JsonSerializer.Deserialize<AlmacenViewModel>(jsonModel);

         model.AlmacenTransaccionConfiguracionDTO.PageSize = ObtenerMaximoRegistrosBusqueda();

         try
         {
            List<AlmacenTransaccionConfiguracionDTO> almacenTransaccionConfigurationDTOList = 
               almacenTransaccionConfiguracionProxy.ListarPorPorAlmacenTransaccionId(model.AlmacenTransaccionConfiguracionDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = almacenTransaccionConfigurationDTOList.Count,
               rows = from f in almacenTransaccionConfigurationDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.AlmacenTransaccionConfiguracionId,
                            f.AlmacenTransaccionId,
                            f.AtributoConfiguracionId,
                            f.AtributoConfiguracionDTO.Nombre,
                            Visible = f.Visible == SiNoConstante.S ? DigitoConstante.UNO : DigitoConstante.CERO,
                            Requerido = f.Requerido == SiNoConstante.S ? DigitoConstante.UNO : DigitoConstante.CERO,
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
      [HttpPost("GrabarAlmacenTransaccionConfiguracion")]
      public ActionResult GrabarAlmacenTransaccionConfiguracion([FromBody] AlmacenViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            AlmacenTransaccionConfiguracionDTO almacenTransaccionConfiguracionDTO = model.AlmacenTransaccionConfiguracionDTO;
            almacenTransaccionConfiguracionDTO.Estado = EstadoConstante.ACTIVO;
            AuditDTO(almacenTransaccionConfiguracionDTO);

            if(almacenTransaccionConfiguracionDTO.AlmacenTransaccionConfiguracionId == 0)
               almacenTransaccionConfiguracionProxy.Insert(almacenTransaccionConfiguracionDTO);
            else
               almacenTransaccionConfiguracionProxy.Update(almacenTransaccionConfiguracionDTO);
 
            resultResponse.Message = SR.GetString("IV_almacen_trasaccion_config_actualizada");
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
      [HttpPost("ListarAlmacenSerie")]
      public ActionResult ListarAlmacenSerie(string jsonModel)
      {

         Logger.InfoStart();

         AlmacenViewModel model = JsonSerializer.Deserialize<AlmacenViewModel>(jsonModel);

         model.AlmacenSerieDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.AlmacenSerieDTO.OrderBy = "tdc.tipo_doc_com,dcs.serie";

         try
         {
            List<AlmacenSerieDTO> almacenSerieDTOList = almacenSerieProxy.Search(model.AlmacenSerieDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = almacenSerieDTOList.Count,
               rows = from f in almacenSerieDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.AlmacenSerieId,
                            f.AlmacenId,
                            f.DocumentoComercialSerieId,
                            TipoDocumentoComercialNombre = f.DocumentoComercialSerieDTO.TipoDocumentoComercialDTO.Nombre,
                            f.DocumentoComercialSerieDTO.Serie,
                            f.DocumentoComercialSerieDTO.Correlativo,
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
      [HttpPost("GrabarAlmacenSerie")]
      public ActionResult GrabarAlmacenSerie([FromBody] AlmacenViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            AlmacenSerieDTO almacenSerieDTO = cargarAlmacenSerieDTO(model);

            string messageResourceKey = "VT_documento_comercial_serie_agregado";

            almacenSerieProxy.ActualizarListaDocumentoComercialSerieId(almacenSerieDTO);

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
      private AlmacenSerieDTO cargarAlmacenSerieDTO(AlmacenViewModel model)
      {

         AlmacenSerieDTO almacenSerieDTO;
         try
         {

            almacenSerieDTO = model.AlmacenSerieDTO;
            almacenSerieDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(almacenSerieDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return almacenSerieDTO;
      }
      [HttpPost("EliminarAlmacenSerie")]
      public ActionResult EliminarAlmacenSerie([FromBody] AlmacenViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.AlmacenSerieDTO.DTOState = DTOState.Modified;
            AuditDTO(model.AlmacenSerieDTO);

            almacenSerieProxy.Delete(model.AlmacenSerieDTO);

            resultResponse.Message = SR.GetString("VT_documento_comercial_serie_anulado");
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
      [HttpPost("ListPopup")]
      public ActionResult ListPopup([FromBody] AlmacenViewModel model)
      {
         if (model == null)
            model = new AlmacenViewModel();

         return PartialView(model);
      }
   }
}