using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service.DTO;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.IV.Models;
using ESD.ERP.Web.Areas.MG.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using ESD.ERP.WebApiProxy.Impl;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.MG.Controllers
{
   [Area("MG")]
   [Route("[controller]")]
   public class ProveedorController : AuthorizedController
   {
      private readonly IProveedorProxy proveedorProxy;
      private readonly ITipoNaturalezaJuridicaProxy tipoNaturalezaJuridicaProxy;
      private readonly ITipoDocumentoIdentidadProxy tipoDocumentoIdentidadProxy;
      private readonly ITipoViaProxy tipoViaProxy;
      private readonly ITipoZonaProxy tipoZonaProxy;
      private readonly IDepartamentoProxy departamentoProxy;
      private readonly IProvinciaProxy provinciaProxy;
      private readonly IDistritoProxy distritoProxy;
      private readonly IOrigenRegistroProxy origenRegistroProxy;
      private readonly IIntegrationApiProxy integrationApiProxy;
      private readonly ITipoProveedorProxy tipoProveedorProxy;

      public ProveedorController(
         IProveedorProxy proveedorProxy,
         ITipoNaturalezaJuridicaProxy tipoNaturalezaJuridicaProxy,
         ITipoDocumentoIdentidadProxy tipoDocumentoIdentidadProxy,
         ITipoViaProxy tipoViaProxy,
         ITipoZonaProxy tipoZonaProxy,
         IDepartamentoProxy departamentoProxy,
         IProvinciaProxy provinciaProxy,
         IDistritoProxy distritoProxy,
         IOrigenRegistroProxy origenRegistroProxy,
         IIntegrationApiProxy integrationApiProxy,
         ITipoProveedorProxy tipoProveedorProxy
         )
      {
         this.proveedorProxy = proveedorProxy;
         this.tipoNaturalezaJuridicaProxy = tipoNaturalezaJuridicaProxy;
         this.tipoDocumentoIdentidadProxy = tipoDocumentoIdentidadProxy;
         this.tipoViaProxy = tipoViaProxy;
         this.tipoZonaProxy = tipoZonaProxy;
         this.departamentoProxy = departamentoProxy;
         this.provinciaProxy = provinciaProxy;
         this.distritoProxy = distritoProxy;
         this.origenRegistroProxy = origenRegistroProxy;
         this.integrationApiProxy = integrationApiProxy;
         this.tipoProveedorProxy = tipoProveedorProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         ProveedorViewModel proveedorViewModel = new();

         cargarListasCompartidas(proveedorViewModel);

         return PartialView(proveedorViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         ProveedorViewModel model = JsonSerializer.Deserialize<ProveedorViewModel>(jsonModel);

         model.ProveedorDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.ProveedorDTO.OrderBy = "p.razon_social";

         model.ProveedorDTO.EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID);

         try
         {
            List<ProveedorDTO> proveedorDTOList = proveedorProxy.Search(model.ProveedorDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = proveedorDTOList.Count,
               rows = from f in proveedorDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.ProveedorId,
                            f.Codigo,
                            TipoNaturalezaJuridica = f.TipoNaturalezaJuridicaDTO.Codigo,
                            TipoDocumentoIdentidadNombre = f.TipoDocumentoIdentidadDTO.Abreviatura,
                            f.NumeroDocumentoIdentidad,
                            f.NumeroRuc,
                            f.RazonSocial,
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
      public ActionResult Crear([FromBody] ProveedorViewModel model)
      {
         Logger.InfoStart();

         ProveedorViewModel proveedorViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(proveedorViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, proveedorViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] ProveedorViewModel model)
      {

         Logger.InfoStart();

         ProveedorViewModel proveedorViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(proveedorViewModel, model.ProveedorDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, proveedorViewModel);
      }
      private void cargarDataParaEditar(ProveedorViewModel model, int id)
      {

         ProveedorDTO proveedorDTO;
         if (model.EditAction == ViewModelEditAction.New)
         {
            proveedorDTO = new ProveedorDTO
            {
               TipoDocumentoIdentidadDTO = new TipoDocumentoIdentidadDTO(),
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

            proveedorDTO.DistritoDTO = distritoProxy.SingleSearch(distritoDTO);
            proveedorDTO.DistritoId = proveedorDTO.DistritoDTO.Id;

            TipoNaturalezaJuridicaDTO tipoNaturalezaJuridicaDTO = tipoNaturalezaJuridicaProxy.ObtenerPorCodigo(new TipoNaturalezaJuridicaDTO() { Codigo = TipoNaturalezaJuridicaConstante.NATURAL });
            proveedorDTO.TipoNaturalezaJuridicaId = tipoNaturalezaJuridicaDTO.Id;
            proveedorDTO.TipoNaturalezaJuridicaDTO = tipoNaturalezaJuridicaDTO;

            TipoViaDTO tipoViaDTO = tipoViaProxy.ObtenerPorCodigo(new TipoViaDTO() { Codigo = TipoViaConstante.DEFAULT });
            proveedorDTO.TipoViaId = tipoViaDTO.Id;

            TipoZonaDTO tipoZonaDTO = tipoZonaProxy.ObtenerPorCodigo(new TipoZonaDTO() { Codigo = TipoZonaConstante.DEFAULT });
            proveedorDTO.TipoZonaId = tipoZonaDTO.Id;
         }
         else
         {
            proveedorDTO = proveedorProxy.GetById(new ProveedorDTO() { ProveedorId = id });
         }

         model.ProveedorDTO = proveedorDTO;

         cargarListas(model);
         cargarUbigeo(model, model.ProveedorDTO.DistritoDTO.ProvinciaDTO.DepartamentoId,
            model.ProveedorDTO.DistritoDTO.ProvinciaId);

         WebHelper.SelectedListItemSetValue(model.TipoNaturalezaJuridicaList, proveedorDTO.TipoNaturalezaJuridicaId);
         WebHelper.SelectedListItemSetValue(model.TipoDocumentoIdentidadList, proveedorDTO.TipoDocumentoIdentidadId);
         WebHelper.SelectedListItemSetValue(model.TipoViaList, proveedorDTO.TipoViaId);
         WebHelper.SelectedListItemSetValue(model.TipoZonaList, proveedorDTO.TipoZonaId);
         WebHelper.SelectedListItemSetValue(model.DistritoList, proveedorDTO.DistritoId);
         WebHelper.SelectedListItemSetValue(model.ProvinciaList, proveedorDTO.DistritoDTO.ProvinciaId);
         WebHelper.SelectedListItemSetValue(model.DepartamentoList, proveedorDTO.DistritoDTO.ProvinciaDTO.DepartamentoId);

         model.MenuItemList = new List<MenuItem>() {
            new MenuItem() { MenuItemType = MenuItemType.New },
            new MenuItem() { MenuItemType = MenuItemType.Save },
            new MenuItem() { MenuItemType = MenuItemType.Cancel },
            new MenuItem() { MenuItemType = MenuItemType.Delete }
         };
      }
      private void cargarListas(ProveedorViewModel model)
      {
         cargarListasCompartidas(model);

         List<TipoNaturalezaJuridicaDTO> tipoNaturalezaJuridicaDTOList = tipoNaturalezaJuridicaProxy.Search(new TipoNaturalezaJuridicaDTO() { Estado = EstadoConstante.ACTIVO });
         model.TipoNaturalezaJuridicaList = WebHelper.ToSelectListItem(tipoNaturalezaJuridicaDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<TipoViaDTO> tipoViaDTOList = tipoViaProxy.Search(new TipoViaDTO() { Estado = EstadoConstante.ACTIVO });
         model.TipoViaList = WebHelper.ToSelectListItem(tipoViaDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<TipoZonaDTO> tipoZonaDTOList = tipoZonaProxy.Search(new TipoZonaDTO() { Estado = EstadoConstante.ACTIVO });
         model.TipoZonaList = WebHelper.ToSelectListItem(tipoZonaDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);
      }
      private void cargarListasCompartidas(ProveedorViewModel model)
      {

         TipoDocumentoIdentidadDTO tipoDocumentoIdentidadDTO = new() { Estado = EstadoConstante.ACTIVO };

         if(model.ProveedorDTO != null)
            if (model.ProveedorDTO.TipoNaturalezaJuridicaDTO.Codigo == TipoNaturalezaJuridicaConstante.JURIDICO)
               tipoDocumentoIdentidadDTO.Codigo = TipoDocumentoIdentidadConstante.RUC;
            else
               tipoDocumentoIdentidadDTO.NotTipoDocumentoIdentidadList = new List<string>() { TipoDocumentoIdentidadConstante.RUC };

         List<TipoDocumentoIdentidadDTO> tipoDocumentoIdentidadDTOList = tipoDocumentoIdentidadProxy.Search(tipoDocumentoIdentidadDTO);
         model.TipoDocumentoIdentidadList = WebHelper.ToSelectListItem(tipoDocumentoIdentidadDTOList, x => x.Id.ToString(), x => x.Abreviatura,
            SelectListFirstOptionType.Select);
      }
      private void cargarUbigeo(ProveedorViewModel model, int departamentoId, int provinciaId)
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
      [HttpGet("ObtenerPorTipoNaturalezaJuridica")]
      public JsonResult ObtenerPorTipoNaturalezaJuridica(int id)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {
            TipoNaturalezaJuridicaDTO tipoNaturalezaJuridicaDTO = tipoNaturalezaJuridicaProxy.
               GetById(new TipoNaturalezaJuridicaDTO() { TipoNaturalezaJuridicaId = id });

            List<TipoDocumentoIdentidadDTO> tipoDocumentoIdentidadDTOList = null;
            string formatoValidacion = string.Empty;
            int tipoDocumentoIdentidadId = 0;

            if (tipoNaturalezaJuridicaDTO.Codigo == TipoNaturalezaJuridicaConstante.JURIDICO)
            {
               tipoDocumentoIdentidadDTOList = tipoDocumentoIdentidadProxy.
                  Search(new TipoDocumentoIdentidadDTO()
                  {
                     Codigo = TipoDocumentoIdentidadConstante.RUC,
                     Estado = EstadoConstante.ACTIVO
                  });
               formatoValidacion = tipoDocumentoIdentidadDTOList[0].FormatoValidacion;
               tipoDocumentoIdentidadId = tipoDocumentoIdentidadDTOList[0].Id;
            }
            else
               tipoDocumentoIdentidadDTOList = tipoDocumentoIdentidadProxy.
                  Search(new TipoDocumentoIdentidadDTO()
                  {
                     NotTipoDocumentoIdentidadList = new List<string>() { TipoDocumentoIdentidadConstante.RUC },
                     Estado = EstadoConstante.ACTIVO
                  });

            resultResponse.Data = new
            {
               tipoNaturalezaJuridicaDTO.Codigo,
               List = WebHelper.ToSelectListItem(tipoDocumentoIdentidadDTOList, x => x.Id.ToString(), x => x.Abreviatura,SelectListFirstOptionType.Select),
               TipoDocumentoIdentidadId = tipoDocumentoIdentidadId,
               FormatoValidacion = formatoValidacion
            };
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
            List<DistritoDTO> distritoDTOList = obtenerListaDistrito(id);
            resultResponse.Data = WebHelper.ToSelectListItem(distritoDTOList, x => x.Id.ToString(), x => x.Nombre,
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
      public ActionResult Grabar([FromBody] ProveedorViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            ProveedorDTO proveedorDTO = cargarProveedorDTO(model);

            int newId = model.ProveedorDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = proveedorProxy.Insert(proveedorDTO);
                  messageResourceKey = "MG_proveedor_agregado";
                  break;

               case ViewModelEditAction.Edit:
                  proveedorProxy.Update(proveedorDTO);
                  messageResourceKey = "MG_proveedor_actualizado";
                  break;
            }

            proveedorDTO = proveedorProxy.GetById(new ProveedorDTO() { ProveedorId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, proveedorDTO.RazonSocial);
            resultResponse.Data = new
            {
               proveedorDTO.Id,
               proveedorDTO.Codigo,
               proveedorDTO.RazonSocial,
               proveedorDTO.NumeroDocumentoIdentidad
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
      private ProveedorDTO cargarProveedorDTO(ProveedorViewModel model)
      {

         ProveedorDTO proveedorDTO;
         try
         {

            if(model.ViewModelState.EditAction == ViewModelEditAction.New)
            {
               proveedorDTO = model.ProveedorDTO;

               PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);
               proveedorDTO.EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId;

               TipoProveedorDTO tipoProveedorDTO = tipoProveedorProxy.ObtenerPorCodigo(new TipoProveedorDTO() { Codigo = TipoProveedorConstante.CLIENTE });
               proveedorDTO.TipoProveedorId = tipoProveedorDTO.Id;

               OrigenRegistroDTO origenRegistroDTO = origenRegistroProxy.ObtenerPorCodigo(new OrigenRegistroDTO() { Codigo = OrigenRegistroConstante.USUARIO });
               proveedorDTO.OrigenRegistroId = origenRegistroDTO.Id;
            }
            else
            {
               proveedorDTO = proveedorProxy.GetById(new ProveedorDTO() { ProveedorId = model.ProveedorDTO.Id });

               model.ProveedorDTO.CondicionPagoId = proveedorDTO.CondicionPagoId;

               proveedorDTO = model.ProveedorDTO;
            }

            if (proveedorDTO.TipoNaturalezaJuridicaDTO.Codigo == TipoNaturalezaJuridicaConstante.NATURAL)
               proveedorDTO.RazonSocial = string.Concat(proveedorDTO.ApellidoPaterno,
                  string.IsNullOrEmpty(proveedorDTO.ApellidoMaterno) ? string.Empty : string.Concat(SeparadorConstante.ESPACIO, proveedorDTO.ApellidoMaterno),
                  string.Concat(SeparadorConstante.ESPACIO, proveedorDTO.Nombres));
            else
               proveedorDTO.NumeroDocumentoIdentidad = proveedorDTO.NumeroRuc;

            if (string.IsNullOrEmpty(model.ProveedorDTO.Contacto))
               model.ProveedorDTO.Contacto = string.Empty;
            if (string.IsNullOrEmpty(model.ProveedorDTO.Telefono))
               model.ProveedorDTO.Telefono = string.Empty;
            if (string.IsNullOrEmpty(model.ProveedorDTO.NumeroCCI))
               model.ProveedorDTO.NumeroCCI = string.Empty;
            if (string.IsNullOrEmpty(model.ProveedorDTO.Email))
               model.ProveedorDTO.Email = string.Empty;
            if (string.IsNullOrEmpty(model.ProveedorDTO.Web))
               model.ProveedorDTO.Web = string.Empty;
            if (string.IsNullOrEmpty(model.ProveedorDTO.CodigoExterno))
               model.ProveedorDTO.CodigoExterno = string.Empty;
            if (string.IsNullOrEmpty(model.ProveedorDTO.EsLocal))
               model.ProveedorDTO.EsLocal = SiNoConstante.S;
            if (string.IsNullOrEmpty(model.ProveedorDTO.EsProspecto))
               model.ProveedorDTO.EsProspecto = SiNoConstante.S;
            if (string.IsNullOrEmpty(model.ProveedorDTO.NumeroDocumentoIdentidad2))
               model.ProveedorDTO.NumeroDocumentoIdentidad2 = string.Empty;
            if (string.IsNullOrEmpty(model.ProveedorDTO.Cargo))
               model.ProveedorDTO.Cargo = string.Empty;

            proveedorDTO.Estado = EstadoConstante.ACTIVO;
            AuditDTO(proveedorDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return proveedorDTO;
      }
      [HttpGet("ObtenerTipoDocumentoIdentidad")]
      public JsonResult ObtenerTipoDocumentoIdentidad(int id)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {
            TipoDocumentoIdentidadDTO tipoDocumentoIdentidadDTO = tipoDocumentoIdentidadProxy.
               GetById(new TipoDocumentoIdentidadDTO() { TipoDocumentoIdentidadId = id });

            resultResponse.Data = new
            {
               tipoDocumentoIdentidadDTO.Codigo,
               tipoDocumentoIdentidadDTO.FormatoValidacion
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
      [Route("ListPopup")]
      public IActionResult ListPopup([FromBody] ProveedorViewModel proveedorViewModel)
      {
         if(proveedorViewModel == null)
            proveedorViewModel = new ProveedorViewModel();

         cargarListasCompartidas(proveedorViewModel);

         proveedorViewModel.ModalEditId = GetModalEditPanelId();
         return PartialView(proveedorViewModel);
      }
      [Route("CreatePopup")]
      public IActionResult CreatePopup([FromBody] ProveedorViewModel proveedorViewModel)
      {
         proveedorViewModel.EditAction = ViewModelEditAction.New;
         proveedorViewModel.IsNew = true;
         cargarDataParaEditar(proveedorViewModel, 0);

         proveedorViewModel.ModalEditId = GetModalEditPanelId();
         return PartialView(proveedorViewModel);
      }
      [HttpPost("GrabarPopup")]
      public ActionResult GrabarPopup([FromBody] ProveedorViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.ViewModelState.EditAction = ViewModelEditAction.New;
            ProveedorDTO proveedorDTO = cargarProveedorDTO(model);

            int newId = proveedorProxy.Insert(proveedorDTO);

            proveedorDTO = proveedorProxy.GetById(new ProveedorDTO() { ProveedorId = newId });

            resultResponse.Message = SR.GetString("MG_proveedor_agregado", proveedorDTO.RazonSocial);
            resultResponse.Data = new
            {
               proveedorDTO.Id,
               proveedorDTO.Codigo,
               proveedorDTO.TipoNaturalezaJuridicaId,
               TipoNaturalezaJuridica = proveedorDTO.TipoNaturalezaJuridicaDTO.Codigo,
               proveedorDTO.RazonSocial,
               proveedorDTO.NumeroDocumentoIdentidad
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
      [HttpGet("ObtenerDesdeSunat")]
      public JsonResult ObtenerDesdeSunat(string id)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {
            resultResponse.Success = false;

            object result = integrationApiProxy.ObtenerPorRuc(id);
            if(result != null)
            {
               RucDTO rucDTO = JsonSerializer.Deserialize<RucDTO>(result.ToString());

               if(rucDTO != null)
               {
                  resultResponse.Success = true;
                  string tipoNaturalezaJuridica = rucDTO.RUC.Substring(0, 2) == DigitoConstante.DIEZ ? TipoNaturalezaJuridicaConstante.NATURAL :
                        TipoNaturalezaJuridicaConstante.JURIDICO;

                  TipoNaturalezaJuridicaDTO tipoNaturalezaJuridicaDTO = tipoNaturalezaJuridicaProxy.ObtenerPorCodigo(new TipoNaturalezaJuridicaDTO() { Codigo = tipoNaturalezaJuridica });

                  TipoViaDTO tipoViaDTO = null;
                  if (!string.IsNullOrEmpty(rucDTO.TipoVia))
                     tipoViaDTO = tipoViaProxy.SingleSearch(new TipoViaDTO() { Sigla = rucDTO.TipoVia });
                  if (tipoViaDTO == null)
                     tipoViaDTO = tipoViaProxy.ObtenerPorCodigo(new TipoViaDTO() { Codigo = TipoViaConstante.DEFAULT });

                  TipoZonaDTO tipoZonaDTO = null;
                  if (!string.IsNullOrEmpty(rucDTO.TipoZona))
                     tipoZonaDTO = tipoZonaProxy.SingleSearch(new TipoZonaDTO() { Sigla = rucDTO.TipoZona });
                  if (tipoZonaDTO == null)
                     tipoZonaDTO = tipoZonaProxy.ObtenerPorCodigo(new TipoZonaDTO() { Codigo = TipoZonaConstante.DEFAULT });

                  if (string.IsNullOrEmpty(rucDTO.UbigeoDistrito))
                  {
                     rucDTO.UbigeoDistrito = DistritoConstante.DEFAULT;
                     rucDTO.UbigeoProvincia = ProvinciaConstante.DEFAULT;
                     rucDTO.UbigeoDepartamento = DepartamentoConstante.DEFAULT;
                  }

                  DistritoDTO distritoDTO = distritoProxy.SingleSearch(new DistritoDTO()
                  {
                     Codigo = rucDTO.UbigeoDistrito,
                     ProvinciaDTO = new ProvinciaDTO()
                     {
                        Codigo = rucDTO.UbigeoProvincia,
                        DepartamentoDTO = new DepartamentoDTO() { Codigo = rucDTO.UbigeoDepartamento }
                     }
                  });

                  List<ProvinciaDTO> provinciaDTOList = obtenerListaProvincia(distritoDTO.ProvinciaDTO.DepartamentoId);
                  List<DistritoDTO> distritoDTOList = obtenerListaDistrito(distritoDTO.ProvinciaId);

                  resultResponse.Data = new
                  {
                     TipoNaturalezaJuridica = tipoNaturalezaJuridica,
                     TipoNaturalezaJuridicaId = tipoNaturalezaJuridicaDTO.Id,
                     rucDTO.RazonSocial,
                     TipoViaId = tipoViaDTO.Id,
                     rucDTO.NombreVia,
                     NumeroVia = rucDTO.Numero,
                     rucDTO.Interior,
                     rucDTO.Departamento,
                     rucDTO.Lote,
                     rucDTO.Manzana,
                     rucDTO.Kilometro,
                     TipoZonaId = tipoZonaDTO.Id,
                     rucDTO.NombreZona,
                     distritoDTO.ProvinciaDTO.DepartamentoId,
                     distritoDTO.ProvinciaId,
                     ProvinciaList = WebHelper.ToSelectListItem(provinciaDTOList, x => x.Id.ToString(), x => x.Nombre,
                        SelectListFirstOptionType.Select),
                     DistritoId = distritoDTO.Id,
                     DistritoList = WebHelper.ToSelectListItem(distritoDTOList, x => x.Id.ToString(), x => x.Nombre,
                        SelectListFirstOptionType.Select)
                  };
               }
            }

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

      [HttpPost("Eliminar")]
      public ActionResult Eliminar([FromBody] ProveedorViewModel model)
      {
          Logger.InfoStart();
          ResultResponse resultResponse = new();

            try
            {
                model.ProveedorDTO.DTOState = DTOState.Modified;
                AuditDTO(model.ProveedorDTO);

                proveedorProxy.Delete(model.ProveedorDTO);

                resultResponse.Message = SR.GetString("MG_proveedor_anulado", model.ProveedorDTO.Codigo);
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

      [HttpGet("ObtenerProveedor")]
      public JsonResult ObtenerProveedor(string id)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {

            ProveedorDTO proveedorDTO = proveedorProxy.GetById(new ProveedorDTO() { 
               ProveedorId = Convert.ToInt32(id)
            });

            List<ProvinciaDTO> provinciaDTOList = obtenerListaProvincia(proveedorDTO.DistritoDTO.ProvinciaDTO.DepartamentoId);
            List<DistritoDTO> distritoDTOList = obtenerListaDistrito(proveedorDTO.DistritoDTO.ProvinciaId);

            resultResponse.Data = new
            {
               proveedorDTO.ProveedorId,
               TipoNaturalezaJuridica = proveedorDTO.TipoNaturalezaJuridicaDTO.Codigo,
               TipoNaturalezaJuridicaId = proveedorDTO.TipoNaturalezaJuridicaDTO.Id,
               proveedorDTO.RazonSocial,
               proveedorDTO.ApellidoPaterno,
               proveedorDTO.ApellidoMaterno,
               proveedorDTO.Nombres,
               TipoDocumentoIdentidad = proveedorDTO.TipoDocumentoIdentidadDTO.Codigo,
               TipoDocumentoIdentidadId = proveedorDTO.TipoDocumentoIdentidadDTO.Id,
               proveedorDTO.NumeroRuc,
               proveedorDTO.NumeroDocumentoIdentidad,
               TipoViaId = proveedorDTO.TipoViaDTO.Id,
               proveedorDTO.NombreVia,
               proveedorDTO.NumeroVia,
               proveedorDTO.Interior,
               proveedorDTO.Departamento,
               proveedorDTO.Lote,
               proveedorDTO.Manzana,
               proveedorDTO.Kilometro,
               proveedorDTO.Block,
               proveedorDTO.Etapa,
               TipoZonaId = proveedorDTO.TipoZonaDTO.Id,
               proveedorDTO.NombreZona,
               proveedorDTO.Referencia,
               proveedorDTO.DistritoDTO.ProvinciaDTO.DepartamentoId,
               proveedorDTO.DistritoDTO.ProvinciaId,
               ProvinciaList = WebHelper.ToSelectListItem(provinciaDTOList, x => x.Id.ToString(), x => x.Nombre,
                  SelectListFirstOptionType.Select),
               DistritoId = proveedorDTO.DistritoDTO.Id,
               DistritoList = WebHelper.ToSelectListItem(distritoDTOList, x => x.Id.ToString(), x => x.Nombre,
                  SelectListFirstOptionType.Select)
            };

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
   }
   internal class RucDTO
   {
      public string RUC { get; set; }
      public string RazonSocial { get; set; }
      public string EstadoNombre { get; set; }
      public string Activo { get; set; }
      public string CondicionDomicilio { get; set; }
      public string UbigeoDepartamento { get; set; }
      public string UbigeoProvincia { get; set; }
      public string UbigeoDistrito { get; set; }
      public string TipoVia { get; set; }
      public string NombreVia { get; set; }
      public string TipoZona { get; set; }
      public string NombreZona { get; set; }
      public string Numero { get; set; }
      public string Interior { get; set; }
      public string Lote { get; set; }
      public string Departamento { get; set; }
      public string Manzana { get; set; }
      public string Kilometro { get; set; }
   }
}