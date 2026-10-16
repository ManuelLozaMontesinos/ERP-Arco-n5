using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service.DTO;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.AS.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.AS.Controllers
{
   [Area("AS")]
   [Route("[controller]")]
   public class EmpresaController : AuthorizedController
   {
      private readonly IEmpresaProxy empresaProxy;
      private readonly ISectorProxy sectorProxy;
      private readonly ITipoViaProxy tipoViaProxy;
      private readonly ITipoZonaProxy tipoZonaProxy;
      private readonly IBancoProxy bancoProxy;
      private readonly IDepartamentoProxy departamentoProxy;
      private readonly IProvinciaProxy provinciaProxy;
      private readonly IDistritoProxy distritoProxy;
      private readonly IEmpresaSedeProxy empresaSedeProxy;
      private readonly IPeriodoProxy periodoProxy;
      private readonly IPeriodoEmpresaSedeProxy periodoEmpresaSedeProxy;

      public EmpresaController(
         IEmpresaProxy empresaProxy,
         ISectorProxy sectorProxy,
         ITipoViaProxy tipoViaProxy,
         ITipoZonaProxy tipoZonaProxy,
         IBancoProxy bancoProxy,
         IDepartamentoProxy departamentoProxy,
         IProvinciaProxy provinciaProxy,
         IDistritoProxy distritoProxy,
         IEmpresaSedeProxy empresaSedeProxy,
         IPeriodoProxy periodoProxy,
         IPeriodoEmpresaSedeProxy periodoEmpresaSedeProxy
         )
      {
         this.empresaProxy = empresaProxy;
         this.sectorProxy = sectorProxy;
         this.tipoViaProxy = tipoViaProxy;
         this.tipoZonaProxy = tipoZonaProxy;
         this.bancoProxy = bancoProxy;
         this.departamentoProxy = departamentoProxy;
         this.provinciaProxy = provinciaProxy;
         this.distritoProxy = distritoProxy;
         this.empresaSedeProxy = empresaSedeProxy;
         this.periodoProxy = periodoProxy;
         this.periodoEmpresaSedeProxy = periodoEmpresaSedeProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         EmpresaViewModel empresaViewModel = new();

         return PartialView(empresaViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         EmpresaViewModel model = JsonSerializer.Deserialize<EmpresaViewModel>(jsonModel);

         model.EmpresaDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.EmpresaDTO.OrderBy = "e.ruc";

         try
         {
            List<EmpresaDTO> empresaDTOList = empresaProxy.Search(model.EmpresaDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = empresaDTOList.Count,
               rows = from f in empresaDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.EmpresaId,
                            f.RUC,
                            f.Nombre,
                            Sector = f.SectorDTO.Nombre,
                            f.Direccion,
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
      public ActionResult Crear([FromBody]EmpresaViewModel model)
      {
         Logger.InfoStart();

         EmpresaViewModel empresaViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(empresaViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, empresaViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody]EmpresaViewModel model)
      {

         Logger.InfoStart();

         EmpresaViewModel empresaViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(empresaViewModel, model.EmpresaDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, empresaViewModel);
      }
      private void cargarDataParaEditar(EmpresaViewModel model, int id)
      {

         EmpresaDTO empresaDTO;
         if (model.EditAction == ViewModelEditAction.New)
         {
            empresaDTO = new EmpresaDTO
            {
               Estado = EstadoConstante.ACTIVO
            };

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

            empresaDTO.DistritoDTO = distritoProxy.SingleSearch(distritoDTO);
            empresaDTO.DistritoId = empresaDTO.DistritoDTO.Id;

            TipoViaDTO tipoViaDTO = tipoViaProxy.ObtenerPorCodigo(new TipoViaDTO() { Codigo = TipoViaConstante.DEFAULT });
            empresaDTO.TipoViaId = tipoViaDTO.Id;

            TipoZonaDTO tipoZonaDTO = tipoZonaProxy.ObtenerPorCodigo(new TipoZonaDTO() { Codigo = TipoZonaConstante.DEFAULT });
            empresaDTO.TipoZonaId = tipoZonaDTO.Id;
         }
         else
         {
            empresaDTO = empresaProxy.GetById(new EmpresaDTO() { EmpresaId = id });
         }

         model.EmpresaDTO = empresaDTO;

         cargarListas(model);
         cargarUbigeo(model, model.EmpresaDTO.DistritoDTO.ProvinciaDTO.DepartamentoId,
            model.EmpresaDTO.DistritoDTO.ProvinciaId);

         WebHelper.SelectedListItemSetValue(model.SectorList, empresaDTO.SectorId);
         WebHelper.SelectedListItemSetValue(model.TipoViaList, empresaDTO.TipoViaId);
         WebHelper.SelectedListItemSetValue(model.TipoZonaList, empresaDTO.TipoZonaId);
         WebHelper.SelectedListItemSetValue(model.DistritoList, empresaDTO.DistritoId);
         WebHelper.SelectedListItemSetValue(model.ProvinciaList, empresaDTO.DistritoDTO.ProvinciaId);
         WebHelper.SelectedListItemSetValue(model.DepartamentoList, empresaDTO.DistritoDTO.ProvinciaDTO.DepartamentoId);
         WebHelper.SelectedListItemSetValue(model.BancoList, empresaDTO.BancoId);

         List<EmpresaSedeDTO> empresaSedeDTOList = empresaSedeProxy.Search(new EmpresaSedeDTO() { EmpresaId = model.EmpresaDTO.EmpresaId });
         model.EmpresaSedeString = string.Join(SeparadorConstante.PUNTO_COMA, empresaSedeDTOList
            .Select(x => x.Id.ToString() + SeparadorConstante.DOS_PUNTOS + x.Nombre));

         model.MenuItemList = new List<MenuItem>() {
            new MenuItem() { MenuItemType = MenuItemType.New },
            new MenuItem() { MenuItemType = MenuItemType.Save },
            new MenuItem() { MenuItemType = MenuItemType.Cancel },
            new MenuItem() { MenuItemType = MenuItemType.Delete }
         };
      }
      private void cargarListas(EmpresaViewModel model)
      {

         List<SectorDTO> sectorDTOList = sectorProxy.Search(new SectorDTO() { Estado = EstadoConstante.ACTIVO });
         model.SectorList = WebHelper.ToSelectListItem(sectorDTOList, x => x.Id.ToString(), x => x.Nombre, 
            SelectListFirstOptionType.Select);

         List<TipoViaDTO> tipoViaDTOList = tipoViaProxy.Search(new TipoViaDTO() { Estado = EstadoConstante.ACTIVO });
         model.TipoViaList = WebHelper.ToSelectListItem(tipoViaDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<TipoZonaDTO> tipoZonaDTOList = tipoZonaProxy.Search(new TipoZonaDTO() { Estado = EstadoConstante.ACTIVO });
         model.TipoZonaList = WebHelper.ToSelectListItem(tipoZonaDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<BancoDTO> bancoDTOList = bancoProxy.Search(new BancoDTO() { Estado = EstadoConstante.ACTIVO });
         model.BancoList = WebHelper.ToSelectListItem(bancoDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Blank);

         List<PeriodoDTO> periodoDTOList = periodoProxy.Search(new PeriodoDTO() { Estado = EstadoConstante.ACTIVO });
         model.PeriodoList = WebHelper.ToSelectListItem(periodoDTOList, x => x.Id.ToString(), x => x.Codigo.ToString(),
            SelectListFirstOptionType.Select);

      }
      private void cargarUbigeo(EmpresaViewModel model, int departamentoId, int provinciaId)
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
      public ActionResult Grabar([FromBody]EmpresaViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            EmpresaDTO empresaDTO = cargarEmpresaDTO(model);

            int newId = model.EmpresaDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = empresaProxy.Insert(empresaDTO);
                  messageResourceKey = "AS_empresa_agregada";
                  break;

               case ViewModelEditAction.Edit:
                  empresaProxy.Update(empresaDTO);
                  messageResourceKey = "AS_empresa_actualizada";
                  break;
            }

            empresaDTO = empresaProxy.GetById(new EmpresaDTO() { EmpresaId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, empresaDTO.Nombre);
            resultResponse.Data = new
            {
               empresaDTO.Id
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
      private EmpresaDTO cargarEmpresaDTO(EmpresaViewModel model)
      {

         EmpresaDTO empresaDTO;
         try
         {

            empresaDTO = model.EmpresaDTO;

            if (string.IsNullOrEmpty(model.EmpresaDTO.Block))
               model.EmpresaDTO.Block = string.Empty;

            empresaDTO.Estado = EstadoConstante.ACTIVO;
            AuditDTO(empresaDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return empresaDTO;
      }
      [HttpPost("ListarEmpresaSede")]
      public ActionResult ListarEmpresaSede(string jsonModel)
      {

         Logger.InfoStart();

         EmpresaViewModel model = JsonSerializer.Deserialize<EmpresaViewModel>(jsonModel);

         model.EmpresaSedeDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.EmpresaSedeDTO.OrderBy = "es.empresa_sede";

         try
         {
            List<EmpresaSedeDTO> empresaSedeDTOList = empresaSedeProxy.Search(model.EmpresaSedeDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = empresaSedeDTOList.Count,
               rows = from f in empresaSedeDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.EmpresaSedeId,
                            f.EmpresaId,
                            f.Codigo,
                            f.Nombre,
                            f.Direccion,
                            EsCentral = f.EsCentral == SiNoConstante.S ? SiNoTextoConstante.S : SiNoTextoConstante.N,
                            f.CodigoSunat,
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
      [HttpPost("CrearSede")]
      public ActionResult CrearSede([FromBody] EmpresaViewModel model)
      {
         EmpresaViewModel empresaViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarSedeParaEditar(empresaViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         return PartialView("SedeEditPopup",empresaViewModel);
      }
      [HttpPost("EditarSede")]
      public ActionResult EditarSede([FromBody] EmpresaViewModel model)
      {
         EmpresaViewModel empresaViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarSedeParaEditar(empresaViewModel, model.EmpresaSedeDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         return PartialView("SedeEditPopup", empresaViewModel);
      }
      private void cargarSedeParaEditar(EmpresaViewModel model, int id)
      {

         EmpresaSedeDTO empresaSedeDTO;
         if (model.EditAction == ViewModelEditAction.New)
         {
            empresaSedeDTO = new EmpresaSedeDTO
            {
               Estado = EstadoConstante.ACTIVO
            };

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

            empresaSedeDTO.DistritoDTO = distritoProxy.SingleSearch(distritoDTO);
            empresaSedeDTO.DistritoId = empresaSedeDTO.DistritoDTO.Id;

            TipoViaDTO tipoViaDTO = tipoViaProxy.ObtenerPorCodigo(new TipoViaDTO() { Codigo = TipoViaConstante.DEFAULT });
            empresaSedeDTO.TipoViaId = tipoViaDTO.Id;

            TipoZonaDTO tipoZonaDTO = tipoZonaProxy.ObtenerPorCodigo(new TipoZonaDTO() { Codigo = TipoZonaConstante.DEFAULT });
            empresaSedeDTO.TipoZonaId = tipoZonaDTO.Id;

            empresaSedeDTO.EsCentral = SiNoConstante.S;
         }
         else
         {
            empresaSedeDTO = empresaSedeProxy.GetById(new EmpresaSedeDTO() { EmpresaSedeId = id });
         }

         empresaSedeDTO.EsCentral = empresaSedeDTO.EsCentral == SiNoConstante.S ? EstadoConstante.ACTIVO : EstadoConstante.INACTIVO;
         model.EmpresaSedeDTO = empresaSedeDTO;

         cargarListas(model);
         cargarUbigeo(model, model.EmpresaSedeDTO.DistritoDTO.ProvinciaDTO.DepartamentoId,
            model.EmpresaSedeDTO.DistritoDTO.ProvinciaId);

         WebHelper.SelectedListItemSetValue(model.TipoViaList, empresaSedeDTO.TipoViaId);
         WebHelper.SelectedListItemSetValue(model.TipoZonaList, empresaSedeDTO.TipoZonaId);
         WebHelper.SelectedListItemSetValue(model.DistritoList, empresaSedeDTO.DistritoId);
         WebHelper.SelectedListItemSetValue(model.ProvinciaList, empresaSedeDTO.DistritoDTO.ProvinciaId);
         WebHelper.SelectedListItemSetValue(model.DepartamentoList, empresaSedeDTO.DistritoDTO.ProvinciaDTO.DepartamentoId);
      }
      [HttpPost("GrabarSede")]
      public ActionResult GrabarSede([FromBody]EmpresaViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            EmpresaSedeDTO empresaSedeDTO = cargarEmpresaSedeDTO(model);

            int newId = model.EmpresaSedeDTO.Id;
            string messageResourceKey = string.Empty;
            switch (empresaSedeDTO.DTOState)
            {
               case DTOState.Added:
                  newId = empresaSedeProxy.Insert(empresaSedeDTO);
                  messageResourceKey = "AS_empresa_sede_agregada";
                  break;

               case DTOState.Modified:
                  empresaSedeProxy.Update(empresaSedeDTO);
                  messageResourceKey = "AS_empresa_sede_actualizada";
                  break;
            }

            resultResponse.Message = SR.GetString(messageResourceKey, empresaSedeDTO.Nombre);
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
      private EmpresaSedeDTO cargarEmpresaSedeDTO(EmpresaViewModel model)
      {

         EmpresaSedeDTO empresaSedeDTO;
         try
         {

            empresaSedeDTO = model.EmpresaSedeDTO;

            if (string.IsNullOrEmpty(model.EmpresaSedeDTO.Block))
               model.EmpresaSedeDTO.Block = string.Empty;

            empresaSedeDTO.Estado = EstadoConstante.ACTIVO;
            AuditDTO(empresaSedeDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return empresaSedeDTO;
      }
      [HttpPost("EliminarSede")]
      public ActionResult EliminarSede([FromBody] EmpresaViewModel model)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.EmpresaSedeDTO.DTOState = DTOState.Modified;
            AuditDTO(model.EmpresaSedeDTO);

            empresaSedeProxy.Delete(model.EmpresaSedeDTO);

            resultResponse.Message = SR.GetString("AS_empresa_sede_anulada");
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
      [HttpPost("ListarPeriodoEmpresaSede")]
      public ActionResult ListarPeriodoEmpresaSede(string jsonModel)
      {

         Logger.InfoStart();

         EmpresaViewModel model = JsonSerializer.Deserialize<EmpresaViewModel>(jsonModel);

         model.PeriodoEmpresaSedeDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.PeriodoEmpresaSedeDTO.OrderBy = "pes.empresa_sede_id";

         try
         {
            List<PeriodoEmpresaSedeDTO> periodoEmpresaSedeDTOList = periodoEmpresaSedeProxy.Search(model.PeriodoEmpresaSedeDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = periodoEmpresaSedeDTOList.Count,
               rows = from f in periodoEmpresaSedeDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.PeriodoEmpresaSedeId,
                            f.PeriodoId,
                            f.EmpresaSedeId,
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
      [HttpPost("GrabarPeriodoEmpresaSede")]
      public ActionResult GrabarPeriodoEmpresaSede([FromBody]EmpresaViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = cargarPeriodoEmpresaSedeDTO(model);

            int newId = model.PeriodoEmpresaSedeDTO.Id;
            string messageResourceKey = string.Empty;
            switch (periodoEmpresaSedeDTO.DTOState)
            {
               case DTOState.Added:
                  newId = periodoEmpresaSedeProxy.Insert(periodoEmpresaSedeDTO);
                  messageResourceKey = "AS_periodo_empresa_sede_agregada";
                  break;

               case DTOState.Modified:
                  periodoEmpresaSedeProxy.Update(periodoEmpresaSedeDTO);
                  messageResourceKey = "AS_periodo_empresa_sede_actualizada";
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
      private PeriodoEmpresaSedeDTO cargarPeriodoEmpresaSedeDTO(EmpresaViewModel model)
      {

         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO;
         try
         {

            periodoEmpresaSedeDTO = model.PeriodoEmpresaSedeDTO;

            periodoEmpresaSedeDTO.Estado = EstadoConstante.ACTIVO;
            AuditDTO(periodoEmpresaSedeDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return periodoEmpresaSedeDTO;
      }
      [HttpPost("EliminarPeriodoEmpresaSede")]
      public ActionResult EliminarPeriodoEmpresaSede([FromBody] EmpresaViewModel model)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.PeriodoEmpresaSedeDTO.DTOState = DTOState.Modified;
            AuditDTO(model.PeriodoEmpresaSedeDTO);

            periodoEmpresaSedeProxy.Delete(model.PeriodoEmpresaSedeDTO);

            resultResponse.Message = SR.GetString("AS_periodo_empresa_sede_anulada");
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
      [Route("SedeListPopup")]
      public IActionResult SedeListPopup([FromBody] EmpresaViewModel model)
      {
         ViewBag.ModalEditId = GetModalEditPanelId();

         if (model == null)
            model = new EmpresaViewModel();

         if (model.EmpresaSedeDTO == null)
            model.EmpresaSedeDTO = new EmpresaSedeDTO();

         List<EmpresaDTO> empresaDTOList = empresaProxy.Search(new EmpresaDTO() { Estado = EstadoConstante.ACTIVO });

         model.EmpresaList = WebHelper.ToSelectListItem(empresaDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Empty);

         return PartialView(model);
      }
   }
}