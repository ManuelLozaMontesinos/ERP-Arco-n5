using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service.DTO;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.CM.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.CM.Controllers
{
   [Area("CM")]
   [Route("[controller]")]
   public class ProveCompraController : AuthorizedController
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
      private readonly ITipoProveedorProxy tipoProveedorProxy;
      private readonly IProveedorProductoProxy proveedorProductoProxy;
      private readonly ICondicionPagoProxy condicionPagoProxy;
      private readonly IProductoProxy productoProxy;
      private readonly IProveedorListaPrecioProxy proveedorListaPrecioProxy;
      private readonly IProveedorListaPrecioProductoProxy proveedorListaPrecioProductoProxy;
      private readonly IMonedaProxy monedaProxy;
      private readonly IImpuestoTasaProxy impuestoTasaProxy;
      private readonly IDataImportItemProxy dataImportItemProxy;
      private readonly AppHelper appHelper;

      public ProveCompraController(
         IProveedorProxy proveedorProxy,
         ITipoNaturalezaJuridicaProxy tipoNaturalezaJuridicaProxy,
         ITipoDocumentoIdentidadProxy tipoDocumentoIdentidadProxy,
         ITipoViaProxy tipoViaProxy,
         ITipoZonaProxy tipoZonaProxy,
         IDepartamentoProxy departamentoProxy,
         IProvinciaProxy provinciaProxy,
         IDistritoProxy distritoProxy,
         IOrigenRegistroProxy origenRegistroProxy,
         ITipoProveedorProxy tipoProveedorProxy,
         IProveedorProductoProxy proveedorProductoProxy,
         ICondicionPagoProxy condicionPagoProxy,
         IProductoProxy productoProxy,
         IProveedorListaPrecioProxy proveedorListaPrecioProxy,
         IProveedorListaPrecioProductoProxy proveedorListaPrecioProductoProxy,
         IMonedaProxy monedaProxy,
         IImpuestoTasaProxy impuestoTasaProxy,
         IDataImportItemProxy dataImportItemProxy,
         AppHelper appHelper
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
         this.tipoProveedorProxy = tipoProveedorProxy;
         this.proveedorProductoProxy = proveedorProductoProxy;
         this.condicionPagoProxy = condicionPagoProxy;
         this.productoProxy = productoProxy;
         this.proveedorListaPrecioProxy = proveedorListaPrecioProxy;
         this.proveedorListaPrecioProductoProxy = proveedorListaPrecioProductoProxy;
         this.monedaProxy = monedaProxy;
         this.impuestoTasaProxy = impuestoTasaProxy;
         this.dataImportItemProxy = dataImportItemProxy;
         this.appHelper = appHelper;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         ProveCompraViewModel proveCompraViewModel = new();

         cargarListasCompartidas(proveCompraViewModel);

         MenuBarTagHelper menuBarTagHelper = new();

         menuBarTagHelper.CreateMenuStyleList(MenuItemStyle.CRUD);

         proveCompraViewModel.MenuItemList = menuBarTagHelper.CreateMenuStyleList(MenuItemStyle.CRUD);

         proveCompraViewModel.MenuItemList.Add(new MenuItem() { 
            Key = "maestro",
            Content = "Agregar desde Maestros",
            Position = 0 });

         return PartialView(proveCompraViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         ProveCompraViewModel model = JsonSerializer.Deserialize<ProveCompraViewModel>(jsonModel);

         model.ProveedorDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.ProveedorDTO.OrderBy = "p.razon_social";
         model.ProveedorDTO.TipoProveedorDTO = new TipoProveedorDTO() { Codigo = TipoProveedorConstante.PROVEEDOR };

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
                            f.EsLocal,
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
      [HttpPost("AgregarDesdeMaestro")]
      public ActionResult AgregarDesdeMaestro([FromBody] ProveCompraViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            TipoProveedorDTO tipoProveedorDTO = tipoProveedorProxy.ObtenerPorCodigo(new TipoProveedorDTO() { Codigo = TipoProveedorConstante.PROVEEDOR });
            model.ProveedorDTO.TipoProveedorId = tipoProveedorDTO.Id;

            proveedorProxy.ActualizarTipoProveedor(model.ProveedorDTO);

            resultResponse.Message = SR.GetString("CM_proveedor_agregado");
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
      [HttpPost("Crear")]
      public ActionResult Crear([FromBody] ProveCompraViewModel model)
      {
         Logger.InfoStart();

         ProveCompraViewModel proveCompraViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(proveCompraViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, proveCompraViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] ProveCompraViewModel model)
      {

         Logger.InfoStart();

         ProveCompraViewModel proveCompraViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(proveCompraViewModel, model.ProveedorDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, proveCompraViewModel);
      }
      private void cargarDataParaEditar(ProveCompraViewModel model, int id)
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

         proveedorDTO.EsLocal = proveedorDTO.EsLocal == SiNoConstante.N ? EstadoConstante.INACTIVO : EstadoConstante.ACTIVO;

         WebHelper.SelectedListItemSetValue(model.TipoNaturalezaJuridicaList, proveedorDTO.TipoNaturalezaJuridicaId);
         WebHelper.SelectedListItemSetValue(model.TipoDocumentoIdentidadList, proveedorDTO.TipoDocumentoIdentidadId);
         WebHelper.SelectedListItemSetValue(model.TipoViaList, proveedorDTO.TipoViaId);
         WebHelper.SelectedListItemSetValue(model.TipoZonaList, proveedorDTO.TipoZonaId);
         WebHelper.SelectedListItemSetValue(model.DistritoList, proveedorDTO.DistritoId);
         WebHelper.SelectedListItemSetValue(model.ProvinciaList, proveedorDTO.DistritoDTO.ProvinciaId);
         WebHelper.SelectedListItemSetValue(model.DepartamentoList, proveedorDTO.DistritoDTO.ProvinciaDTO.DepartamentoId);
         WebHelper.SelectedListItemSetValue(model.CondicionPagoList, proveedorDTO.CondicionPagoId);

         model.MenuItemList = new List<MenuItem>() {
            new MenuItem(){ MenuItemType = MenuItemType.New },
            new MenuItem(){ MenuItemType = MenuItemType.Edit},
            new MenuItem(){ MenuItemType = MenuItemType.Save},
            new MenuItem(){ MenuItemType = MenuItemType.Cancel},
            new MenuItem(){ MenuItemType = MenuItemType.Delete},
            new MenuItem(){ MenuItemType = MenuItemType.ImportXLS},
            new MenuItem(){ MenuItemType = MenuItemType.ExportXLS}
         };
      }
      private void cargarListas(ProveCompraViewModel model)
      {
         cargarListasCompartidas(model);

         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

         List<TipoNaturalezaJuridicaDTO> tipoNaturalezaJuridicaDTOList = tipoNaturalezaJuridicaProxy.Search(new TipoNaturalezaJuridicaDTO() { Estado = EstadoConstante.ACTIVO });
         model.TipoNaturalezaJuridicaList = WebHelper.ToSelectListItem(tipoNaturalezaJuridicaDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<TipoViaDTO> tipoViaDTOList = tipoViaProxy.Search(new TipoViaDTO() { Estado = EstadoConstante.ACTIVO });
         model.TipoViaList = WebHelper.ToSelectListItem(tipoViaDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<TipoZonaDTO> tipoZonaDTOList = tipoZonaProxy.Search(new TipoZonaDTO() { Estado = EstadoConstante.ACTIVO });
         model.TipoZonaList = WebHelper.ToSelectListItem(tipoZonaDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<CondicionPagoDTO> condicionPagoDTOList = condicionPagoProxy.Search(new CondicionPagoDTO()
         {
            EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId,
            Estado = EstadoConstante.ACTIVO
         });
         model.CondicionPagoList = WebHelper.ToSelectListItem(condicionPagoDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<MonedaDTO> monedaDTOList = monedaProxy.Search(new MonedaDTO() { Estado = EstadoConstante.ACTIVO });
         model.MonedaString = string.Join(SeparadorConstante.PUNTO_COMA, monedaDTOList
            .Select(x => x.Id.ToString() + SeparadorConstante.DOS_PUNTOS + x.Nombre));

         List<ImpuestoTasaDTO> impuestoTasaDTOList = impuestoTasaProxy.Search(new ImpuestoTasaDTO() { Estado = EstadoConstante.ACTIVO });
         model.ImpuestoTasaString = string.Join(SeparadorConstante.PUNTO_COMA, impuestoTasaDTOList
            .Select(x => x.Id.ToString() + SeparadorConstante.DOS_PUNTOS + x.ImpuestoNombreTasa));
      }
      private void cargarListasCompartidas(ProveCompraViewModel model)
      {

         TipoDocumentoIdentidadDTO tipoDocumentoIdentidadDTO = new() { Estado = EstadoConstante.ACTIVO };

         if(model.ProveedorDTO != null)
         {
            if(model.ProveedorDTO.EsLocal == SiNoConstante.N)
               tipoDocumentoIdentidadDTO.Codigo = TipoDocumentoIdentidadConstante.DOC_ID_EXTRANJERO;
            else
            {
               if (model.ProveedorDTO.TipoNaturalezaJuridicaDTO.Codigo == TipoNaturalezaJuridicaConstante.JURIDICO)
                  tipoDocumentoIdentidadDTO.Codigo = TipoDocumentoIdentidadConstante.RUC;
               else
                  tipoDocumentoIdentidadDTO.NotTipoDocumentoIdentidadList = new List<string>() { TipoDocumentoIdentidadConstante.RUC };
            }
         }

         List<TipoDocumentoIdentidadDTO> tipoDocumentoIdentidadDTOList = tipoDocumentoIdentidadProxy.Search(tipoDocumentoIdentidadDTO);
         model.TipoDocumentoIdentidadList = WebHelper.ToSelectListItem(tipoDocumentoIdentidadDTOList, x => x.Id.ToString(), x => x.Abreviatura,
            SelectListFirstOptionType.Select);
      }
      private void cargarUbigeo(ProveCompraViewModel model, int departamentoId, int provinciaId)
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
      [HttpPost("ObtenerPorTipoNaturalezaJuridica")]
      public JsonResult ObtenerPorTipoNaturalezaJuridica([FromBody] ProveCompraViewModel model)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {
            TipoNaturalezaJuridicaDTO tipoNaturalezaJuridicaDTO = tipoNaturalezaJuridicaProxy.
               GetById(new TipoNaturalezaJuridicaDTO() { TipoNaturalezaJuridicaId = model.ProveedorDTO.TipoNaturalezaJuridicaId });

            List<TipoDocumentoIdentidadDTO> tipoDocumentoIdentidadDTOList = null;
            string formatoValidacion = string.Empty;
            int tipoDocumentoIdentidadId = 0;

            if (model.ProveedorDTO.EsLocal == SiNoConstante.N) {
               tipoDocumentoIdentidadDTOList = tipoDocumentoIdentidadProxy.
                  Search(new TipoDocumentoIdentidadDTO()
                  {
                     Codigo = TipoDocumentoIdentidadConstante.DOC_ID_EXTRANJERO,
                     Estado = EstadoConstante.ACTIVO
                  });
               formatoValidacion = tipoDocumentoIdentidadDTOList[0].FormatoValidacion;
               tipoDocumentoIdentidadId = tipoDocumentoIdentidadDTOList[0].Id;
            }
            else
            {
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
            }

            resultResponse.Data = new
            {
               tipoNaturalezaJuridicaDTO.Codigo,
               List = WebHelper.ToSelectListItem(tipoDocumentoIdentidadDTOList, x => x.Id.ToString(), x => x.Abreviatura, SelectListFirstOptionType.Select),
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
      public ActionResult Grabar([FromBody] ProveCompraViewModel model)
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
      private ProveedorDTO cargarProveedorDTO(ProveCompraViewModel model)
      {

         ProveedorDTO proveedorDTO;
         try
         {

            proveedorDTO = model.ProveedorDTO;

            if(model.ViewModelState.EditAction == ViewModelEditAction.New)
            {
               PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);
               proveedorDTO.EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId;

               TipoProveedorDTO tipoProveedorDTO = tipoProveedorProxy.ObtenerPorCodigo(new TipoProveedorDTO() { Codigo = TipoProveedorConstante.PROVEEDOR });
               proveedorDTO.TipoProveedorId = tipoProveedorDTO.Id;

               OrigenRegistroDTO origenRegistroDTO = origenRegistroProxy.ObtenerPorCodigo(new OrigenRegistroDTO() { Codigo = OrigenRegistroConstante.USUARIO });
               proveedorDTO.OrigenRegistroId = origenRegistroDTO.Id;
            }

            if (proveedorDTO.TipoNaturalezaJuridicaDTO.Codigo == TipoNaturalezaJuridicaConstante.NATURAL)
               proveedorDTO.RazonSocial = string.Concat(proveedorDTO.ApellidoPaterno,
                  string.IsNullOrEmpty(proveedorDTO.ApellidoMaterno) ? string.Empty : string.Concat(SeparadorConstante.ESPACIO, proveedorDTO.ApellidoMaterno),
                  string.Concat(SeparadorConstante.ESPACIO, proveedorDTO.Nombres));
            else
               if(proveedorDTO.EsLocal == SiNoConstante.S)
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
      [HttpPost("ListarProveedorProducto")]
      public ActionResult ListarProveedorProducto(string jsonModel)
      {

         Logger.InfoStart();

         ProveCompraViewModel model = JsonSerializer.Deserialize<ProveCompraViewModel>(jsonModel);

         model.ProveedorProductoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.ProveedorProductoDTO.OrderBy = "p.producto";

         try
         {
            List<ProveedorProductoDTO> proveedorProductoDTOList = proveedorProductoProxy.Search(model.ProveedorProductoDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = proveedorProductoDTOList.Count,
               rows = from f in proveedorProductoDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.ProveedorProductoId,
                            f.ProveedorId,
                            f.ProductoId,
                            f.ProductoDTO.Codigo,
                            ProductoNombre = f.ProductoDTO.Descripcion,
                            f.ProductoDTO.UnidadMedidaDTO.UnidadMedidaId,
                            UnidadMedidaNombre = f.ProductoDTO.UnidadMedidaDTO.Abreviatura,
                            FamiliaProductoNombra = f.ProductoDTO.FamiliaProductoDTO.Nombre,
                            f.CodigoReferencial,
                            f.PesoBruto,
                            f.PesoNeto,
                            f.MasterPack,
                            f.Volumen,
                            f.ProductoDTO.TipoIgvId,
                            TipoIgv = f.ProductoDTO.TipoIgvDTO.Codigo,
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
      [HttpPost("GrabarProveedorProducto")]
      public ActionResult GrabarProveedorProducto([FromBody] ProveCompraViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            ProveedorProductoDTO proveedorProductoDTO = cargarProveedorProductoDTO(model);

            int newId = model.ProveedorProductoDTO.Id;
            string messageResourceKey = string.Empty;
            switch (proveedorProductoDTO.DTOState)
            {
               case DTOState.Added:
                  newId = proveedorProductoProxy.Insert(proveedorProductoDTO);
                  messageResourceKey = "IV_producto_agregado";
                  break;

               case DTOState.Modified:
                  proveedorProductoProxy.Update(proveedorProductoDTO);
                  messageResourceKey = "IV_producto_actualizado";
                  break;
            }

            if(model.ActualizarMaestro == SiNoConstante.S)
            {
               productoProxy.ActualizarPesoMasterPack(new ProductoDTO() { 
                  ProductoId = proveedorProductoDTO.ProductoId,
                  CodigoReferencial = proveedorProductoDTO.CodigoReferencial,
                  PesoNeto = proveedorProductoDTO.PesoNeto,
                  PesoBruto = proveedorProductoDTO.PesoBruto,
                  MasterPack = proveedorProductoDTO.MasterPack,
                  Volumen = proveedorProductoDTO.Volumen,
                  UsuarioCreacionId = proveedorProductoDTO.UsuarioCreacionId
               });
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
      private ProveedorProductoDTO cargarProveedorProductoDTO(ProveCompraViewModel model)
      {

         ProveedorProductoDTO proveedorProductoDTO;
         try
         {

            proveedorProductoDTO = model.ProveedorProductoDTO;

            AuditDTO(proveedorProductoDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return proveedorProductoDTO;
      }
      [HttpPost("EliminarProveedorProducto")]
      public ActionResult EliminarProveedorProducto([FromBody] ProveCompraViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.ProveedorProductoDTO.DTOState = DTOState.Modified;
            AuditDTO(model.ProveedorProductoDTO);

            ProveedorProductoDTO proveedorProductoDTO = proveedorProductoProxy.GetById(model.ProveedorProductoDTO);

            proveedorProductoProxy.Delete(proveedorProductoDTO);

            resultResponse.Message = SR.GetString("VT_producto_anulado");
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
      [HttpPost("SaveEditPopup")]
      public ActionResult SaveEditPopup([FromBody] PedidoCompraViewModel model)
      {
         return PartialView(model);
      }
      [HttpPost("ProductoListPopup")]
      public ActionResult ProductoListPopup([FromBody] ProveCompraViewModel model)
      {
         return PartialView(model);
      }
      [HttpPost("ListarProveedorListaPrecio")]
      public ActionResult ListarProveedorListaPrecio(string jsonModel)
      {

         Logger.InfoStart();

         ProveCompraViewModel model = JsonSerializer.Deserialize<ProveCompraViewModel>(jsonModel);

         model.ProveedorListaPrecioDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.ProveedorListaPrecioDTO.OrderBy = "m.moneda";

         try
         {
            List<ProveedorListaPrecioDTO> proveedorListaPrecioDTOList = proveedorListaPrecioProxy.Search(model.ProveedorListaPrecioDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = proveedorListaPrecioDTOList.Count,
               rows = from f in proveedorListaPrecioDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.ProveedorListaPrecioId,
                            f.ProveedorId,
                            f.MonedaId,
                            MonedaNombre = f.MonedaDTO.Nombre,
                            f.ImpuestoTasaId,
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
      [HttpPost("GrabarProveedorListaPrecio")]
      public ActionResult GrabarProveedorListaPrecio([FromBody] ProveCompraViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            ProveedorListaPrecioDTO proveedorListaPrecioDTO = cargarProveedorListaPrecioDTO(model);

            int newId = model.ProveedorListaPrecioDTO.Id;
            string messageResourceKey = string.Empty;
            switch (proveedorListaPrecioDTO.DTOState)
            {
               case DTOState.Added:
                  newId = proveedorListaPrecioProxy.Insert(proveedorListaPrecioDTO);
                  messageResourceKey = "CM_lista_precio_agregada";
                  break;

               case DTOState.Modified:
                  proveedorListaPrecioProxy.Update(proveedorListaPrecioDTO);
                  messageResourceKey = "CM_lista_precio_actualizada";
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
      private ProveedorListaPrecioDTO cargarProveedorListaPrecioDTO(ProveCompraViewModel model)
      {

         ProveedorListaPrecioDTO proveedorListaPrecioDTO;
         try
         {

            proveedorListaPrecioDTO = model.ProveedorListaPrecioDTO;

            AuditDTO(proveedorListaPrecioDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return proveedorListaPrecioDTO;
      }
      [HttpPost("EliminarProveedorListaPrecio")]
      public ActionResult EliminarProveedorListaPrecio([FromBody] ProveCompraViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.ProveedorListaPrecioDTO.DTOState = DTOState.Modified;
            AuditDTO(model.ProveedorListaPrecioDTO);

            proveedorListaPrecioProxy.Delete(model.ProveedorListaPrecioDTO);

            resultResponse.Message = SR.GetString("CM_lista_precio_anulada");
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
      [HttpPost("ListarProveedorListaPrecioProducto")]
      public ActionResult ListarProveedorListaPrecioProducto(string jsonModel)
      {

         Logger.InfoStart();

         ProveCompraViewModel model = JsonSerializer.Deserialize<ProveCompraViewModel>(jsonModel);

         model.ProveedorListaPrecioProductoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.ProveedorListaPrecioProductoDTO.OrderBy = "p.descripcion";

         try
         {
            List<ProveedorListaPrecioProductoDTO> proveedorListaPrecioProductoDTOList = proveedorListaPrecioProductoProxy.Search(model.ProveedorListaPrecioProductoDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = proveedorListaPrecioProductoDTOList.Count,
               rows = from f in proveedorListaPrecioProductoDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.ProveedorListaPrecioProductoId,
                            f.ProveedorListaPrecioId,
                            f.ProductoId,
                            f.ProductoDTO.Codigo,
                            f.ProductoDTO.Descripcion,
                            f.ProductoDTO.TipoIgvId,
                            TipoIgv = f.ProductoDTO.TipoIgvDTO.Codigo,
                            f.ValorUnitarioAnterior,
                            f.PrecioUnitarioAnterior,
                            FechaAnterior = f.FechaAnterior == null ? string.Empty : f.FechaAnterior.Value.ToShortDateString(),
                            f.ValorUnitario,
                            f.PrecioUnitario,
                            FechaVigente = f.FechaVigente.ToShortDateString(),
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
      [HttpPost("GrabarProveedorListaPrecioProducto")]
      public ActionResult GrabarProveedorListaPrecioProducto([FromBody] ProveCompraViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            ProveedorListaPrecioProductoDTO proveedorListaPrecioProductoDTO = cargarProveedorListaPrecioProductoDTO(model);

            int newId = model.ProveedorListaPrecioProductoDTO.Id;
            string messageResourceKey = string.Empty;
            switch (proveedorListaPrecioProductoDTO.DTOState)
            {
               case DTOState.Added:
                  newId = proveedorListaPrecioProductoProxy.Insert(proveedorListaPrecioProductoDTO);
                  messageResourceKey = "CM_lista_precio_agregada";
                  break;

               case DTOState.Modified:
                  proveedorListaPrecioProductoProxy.Update(proveedorListaPrecioProductoDTO);
                  messageResourceKey = "CM_lista_precio_actualizada";
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
      private ProveedorListaPrecioProductoDTO cargarProveedorListaPrecioProductoDTO(ProveCompraViewModel model)
      {

         ProveedorListaPrecioProductoDTO proveedorListaPrecioProductoDTO;
         try
         {

            proveedorListaPrecioProductoDTO = model.ProveedorListaPrecioProductoDTO;

            AuditDTO(proveedorListaPrecioProductoDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return proveedorListaPrecioProductoDTO;
      }
      [HttpPost("EliminarProveedorListaPrecioProducto")]
      public ActionResult EliminarProveedorListaPrecioProducto([FromBody] ProveCompraViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.ProveedorListaPrecioProductoDTO.DTOState = DTOState.Modified;
            AuditDTO(model.ProveedorListaPrecioProductoDTO);

            proveedorListaPrecioProductoProxy.Delete(model.ProveedorListaPrecioProductoDTO);

            resultResponse.Message = SR.GetString("CM_lista_precio_anulada");
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
      [HttpGet("ObtenerImpuestoTasa")]
      public JsonResult ObtenerImpuestoTasa(int id)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {
            ProveedorListaPrecioDTO proveedorListaPrecioDTO = proveedorListaPrecioProxy.GetById(new ProveedorListaPrecioDTO() { ProveedorListaPrecioId = id });

            resultResponse.Data = new
            {
               proveedorListaPrecioDTO.ImpuestoTasaDTO.Tasa
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
      [HttpPost("CalcularPrecio")]
      public ActionResult CalcularPrecio([FromBody] ProveCompraViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            CalculadorVentaItemDTO calculadorVentaItemDTO = CalculadorVenta.CalcularPrecios(model.CalculadorVentaItemDTO,4);

            resultResponse.Data = new
            {
               calculadorVentaItemDTO.PrecioUnitario,
               calculadorVentaItemDTO.ValorUnitario
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
      [HttpPost("ImportarEditPopup")]
      public IActionResult ImportarEditPopup([FromBody] ProveCompraViewModel model)
      {
         List<DataImportItemDTO> dataImportItemDTOList = dataImportItemProxy.Search(new DataImportItemDTO()
         {
            OrderBy = "orden",
            DataImportDTO = new DataImportDTO() { Codigo = DataImportConstant.PROVEEDOR_LISTA_PRECIO_PRODUCTO }
         });

         if (model == null)
            model = new ProveCompraViewModel();

         model.DataImportItemDTOList = dataImportItemDTOList;

         return PartialView(model);
      }
      [HttpPost("SubirProveedorListaPrecioProducto")]
      public ActionResult SubirProveedorListaPrecioProducto(int? chunks, int? chunk, string name, string proveedorListaPrecioId)
      {
         var fileUpload = Request.Form.Files[0];
         chunk ??= 0;
         chunks ??= 0;

         List<DataImportItemDTO> dataImportItemDTOList = dataImportItemProxy.Search(new DataImportItemDTO()
         {
            OrderBy = "orden",
            DataImportDTO = new DataImportDTO()
            {
               Codigo = DataImportConstant.PROVEEDOR_LISTA_PRECIO_PRODUCTO
            }
         });

         ExcelPackage package = new(fileUpload.OpenReadStream());
         ExcelWorksheet workSheet = package.Workbook.Worksheets.First();

         List<ProveedorListaPrecioProductoDTO> proveedorListaPrecioProductoDTOList = new();
         StringBuilder sb = new();

         bool cancel = false;

         if (workSheet.Dimension.End.Row < 1)
         {
            sb.AppendLine(SR.GetString("GEN_upload_data_diferente_estructura"));
            cancel = true;
         }

         if (!cancel)
            if (workSheet.Dimension.End.Column != dataImportItemDTOList.Count)
            {
               sb.AppendLine(SR.GetString("GEN_upload_columna_diferente_estructura"));
               cancel = true;
            }

         if (!cancel)
         {
            int col = 1;
            foreach (DataImportItemDTO dataImportItemDTO in dataImportItemDTOList)
            {
               ExcelRange excelRange = workSheet.Cells[1, col];
               if (dataImportItemDTO.Columna != excelRange.Text)
               {
                  sb.AppendLine(SR.GetString("GEN_upload_columna_diferente_estructura"));
                  cancel = true;
                  break;
               }
               col++;
            }
         }

         if (!cancel)
         {
            ProveedorListaPrecioDTO proveedorListaPrecioDTO = proveedorListaPrecioProxy.GetById(new ProveedorListaPrecioDTO() { ProveedorListaPrecioId = Convert.ToInt32(proveedorListaPrecioId) });
            List<ProveedorListaPrecioProductoDTO> proveedorListaPrecioProductoDTOListTemp = proveedorListaPrecioProductoProxy.
               Search(new ProveedorListaPrecioProductoDTO() { ProveedorListaPrecioId = Convert.ToInt32(proveedorListaPrecioId) });

            List<string> productoCodigoList = new();

            for (int row = 2; row <= workSheet.Dimension.End.Row; row++)
            {

               ProveedorListaPrecioProductoDTO proveedorListaPrecioProductoDTO = new() { DTOState = DTOState.Unchanged };

               if (workSheet.Dimension.End.Column - 1 > dataImportItemDTOList.Count)
               {
                  sb.AppendLine("Fila " + row.ToString() + " : Las columnas no esta de acuerdo a la estructura requerida");
                  break;
               }

               proveedorListaPrecioProductoDTO.ProveedorListaPrecioId = Convert.ToInt32(proveedorListaPrecioId);

               ExcelRange excelRange = workSheet.Cells[row, 1];
               object cellValue = excelRange.Text;
               if (cellValue != null || !string.IsNullOrEmpty(cellValue.ToString()))
               {
                  int value = 0;
                  int.TryParse(cellValue.ToString(), out value);
                  proveedorListaPrecioProductoDTO.ProveedorListaPrecioProductoId = value;
                  if (proveedorListaPrecioProductoDTO.ProveedorListaPrecioProductoId != 0)
                  {
                     ProveedorListaPrecioProductoDTO proveedorListaPrecioProductoDTOTemp = proveedorListaPrecioProductoDTOListTemp.Where(x => x.ProveedorListaPrecioProductoId == value).FirstOrDefault();
                     proveedorListaPrecioProductoDTO.ProductoId = proveedorListaPrecioProductoDTOTemp.ProductoId;
                     proveedorListaPrecioProductoDTO.ProductoDTO = proveedorListaPrecioProductoDTOTemp.ProductoDTO;
                     proveedorListaPrecioProductoDTO.DTOState = DTOState.Modified;
                  }
               }

               if (proveedorListaPrecioProductoDTO.ProveedorListaPrecioProductoId == 0)
               {
                  excelRange = workSheet.Cells[row, 2];
                  cellValue = excelRange.Text;
                  if (cellValue != null || !string.IsNullOrEmpty(cellValue.ToString()))
                  {
                     int value = 0;
                     int.TryParse(cellValue.ToString(), out value);
                     proveedorListaPrecioProductoDTO.ProductoId = value;
                     if (proveedorListaPrecioProductoDTO.ProductoId != 0)
                     {
                        ProveedorListaPrecioProductoDTO proveedorListaPrecioProductoDTOTemp = proveedorListaPrecioProductoDTOListTemp.Where(x => x.ProductoId == value).FirstOrDefault();
                        if (proveedorListaPrecioProductoDTOTemp != null)
                        {
                           proveedorListaPrecioProductoDTO.ProveedorListaPrecioProductoId = proveedorListaPrecioProductoDTOTemp.ProveedorListaPrecioProductoId;
                           proveedorListaPrecioProductoDTO.ProductoId = proveedorListaPrecioProductoDTOTemp.ProductoId;
                           proveedorListaPrecioProductoDTO.ProductoDTO = proveedorListaPrecioProductoDTOTemp.ProductoDTO;
                           proveedorListaPrecioProductoDTO.DTOState = DTOState.Modified;
                        }
                     }
                  }
               }

               if (proveedorListaPrecioProductoDTO.ProductoId != 0)
               {
                  excelRange = workSheet.Cells[row, 3];
                  cellValue = excelRange.Text;
                  if (cellValue != null || !string.IsNullOrEmpty(cellValue.ToString()))
                  {
                     ProveedorListaPrecioProductoDTO proveedorListaPrecioProductoDTOTemp = proveedorListaPrecioProductoDTOListTemp.Where(x => x.ProductoDTO.Codigo == cellValue.ToString()).FirstOrDefault();
                     if (proveedorListaPrecioProductoDTOTemp != null)
                     {
                        proveedorListaPrecioProductoDTO.ProveedorListaPrecioProductoId = proveedorListaPrecioProductoDTOTemp.ProveedorListaPrecioProductoId;
                        proveedorListaPrecioProductoDTO.ProductoId = proveedorListaPrecioProductoDTOTemp.ProductoId;
                        proveedorListaPrecioProductoDTO.ProductoDTO = proveedorListaPrecioProductoDTOTemp.ProductoDTO;
                        proveedorListaPrecioProductoDTO.DTOState = DTOState.Modified;
                     }
                     else
                     {
                        productoCodigoList.Add(cellValue.ToString());
                        proveedorListaPrecioProductoDTO.ProductoDTO = new ProductoDTO() { Codigo = cellValue.ToString() };
                     }

                  }
               }

               excelRange = workSheet.Cells[row, 5];
               cellValue = excelRange.Text;
               if (cellValue != null || !string.IsNullOrEmpty(cellValue.ToString()))
               {
                  decimal value = 0;
                  decimal.TryParse(cellValue.ToString(), out value);
                  proveedorListaPrecioProductoDTO.ValorUnitario = value;
               }

               excelRange = workSheet.Cells[row, 6];
               cellValue = excelRange.Text;
               if (cellValue != null || !string.IsNullOrEmpty(cellValue.ToString()))
               {
                  decimal value = 0;
                  decimal.TryParse(cellValue.ToString(), out value);
                  proveedorListaPrecioProductoDTO.PrecioUnitario = value;
               }

               excelRange = workSheet.Cells[row, 7];
               cellValue = excelRange.Text;
               if (cellValue != null || !string.IsNullOrEmpty(cellValue.ToString()))
               {
                  DateTime value;
                  DateTime.TryParse(cellValue.ToString(), out value);
                  proveedorListaPrecioProductoDTO.FechaVigente = value;
               }
               else
                  proveedorListaPrecioProductoDTO.FechaVigente = DateTime.Today;

               if (string.IsNullOrEmpty(proveedorListaPrecioProductoDTO.Estado))
                  proveedorListaPrecioProductoDTO.Estado = EstadoConstante.ACTIVO;

               AuditDTO(proveedorListaPrecioProductoDTO);

               proveedorListaPrecioProductoDTOList.Add(proveedorListaPrecioProductoDTO);
            }

            if (productoCodigoList.Count > 0)
            {

               List<ProductoDTO> productoDTOList = productoProxy.Search(new ProductoDTO()
               {
                  FamiliaProductoDTO = new FamiliaProductoDTO()
                  {
                     ClaseProductoDTO = new ClaseProductoDTO()
                     {
                        GrupoProductoDTO = new GrupoProductoDTO
                        {
                           TipoProductoDTO = new TipoProductoDTO() { EmpresaId = proveedorListaPrecioDTO.ProveedorDTO.EmpresaId }
                        }
                     }
                  },
                  ProductoList = productoCodigoList
               });

               foreach (ProveedorListaPrecioProductoDTO proveedorListaPrecioProductoDTO in proveedorListaPrecioProductoDTOList)
               {
                  if (proveedorListaPrecioProductoDTO.DTOState != DTOState.Unchanged)
                     continue;

                  ProductoDTO productoDTO = productoDTOList.Where(x => x.Codigo == proveedorListaPrecioProductoDTO.ProductoDTO.Codigo).FirstOrDefault();
                  if (productoDTO != null)
                  {
                     proveedorListaPrecioProductoDTO.ProductoId = productoDTO.ProductoId;
                     proveedorListaPrecioProductoDTO.ProductoDTO = productoDTO;
                     proveedorListaPrecioProductoDTO.DTOState = DTOState.Modified;
                  }

               }
            }

            foreach (ProveedorListaPrecioProductoDTO proveedorListaPrecioProductoDTO in proveedorListaPrecioProductoDTOList)
            {
               if (proveedorListaPrecioProductoDTO.DTOState != DTOState.Modified)
                  continue;

               proveedorListaPrecioProductoDTO.ProveedorListaPrecioId = proveedorListaPrecioDTO.Id;

               CalculadorVentaItemDTO calculadorVentaItemDTO = null;
               if (proveedorListaPrecioProductoDTO.PrecioUnitario > decimal.Zero)
                  calculadorVentaItemDTO = CalculadorVenta.CalcularPrecios(new CalculadorVentaItemDTO()
                  {
                     TipoIGV = proveedorListaPrecioProductoDTO.ProductoDTO.TipoIgvDTO.Codigo,
                     ImpuestoTasa = proveedorListaPrecioDTO.ImpuestoTasaDTO.Tasa,
                     PrecioUnitario = proveedorListaPrecioProductoDTO.PrecioUnitario
                  },4);
               else
                  calculadorVentaItemDTO = CalculadorVenta.CalcularPrecios(new CalculadorVentaItemDTO()
                  {
                     TipoIGV = proveedorListaPrecioProductoDTO.ProductoDTO.TipoIgvDTO.Codigo,
                     ImpuestoTasa = proveedorListaPrecioDTO.ImpuestoTasaDTO.Tasa,
                     ValorUnitario = proveedorListaPrecioProductoDTO.ValorUnitario
                  },4);

               proveedorListaPrecioProductoDTO.ValorUnitario = calculadorVentaItemDTO.ValorUnitario;
               proveedorListaPrecioProductoDTO.PrecioUnitario = calculadorVentaItemDTO.PrecioUnitario;

            }
         }

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {

            if (sb.Length > 0)
               resultResponse.Data = sb.ToString();
            else
            {
               proveedorListaPrecioProductoProxy.ActualizarLote(proveedorListaPrecioProductoDTOList);

               resultResponse.Message = SR.GetString("GEN_proceso_concluido");
            }
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
      [HttpGet("ReportToExcel")]
      public FileContentResult ReportToExcel(string cid, string proveedorListaPrecioId)
      {
         Dictionary<string, string> parameters = new()
         {
            { "proveedorListaPrecioId", proveedorListaPrecioId }
         };

         return appHelper.ReportToExcelData("sp_rptcm_proveedorListaPrecio", parameters, "ProveedorListaPrecios");
      }
   }
}