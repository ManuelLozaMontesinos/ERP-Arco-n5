using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service.DTO;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.MK.Models;
using ESD.ERP.Web.Areas.VT.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using ESD.ERP.WebApiProxy.Impl;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.MK.Controllers
{
   [Area("MK")]
   [Route("[controller]")]
   public class ClienteController : AuthorizedController
   {
      private readonly IProveedorProxy proveedorProxy;
      private readonly ITipoNaturalezaJuridicaProxy tipoNaturalezaJuridicaProxy;
      private readonly ITipoDocumentoIdentidadProxy tipoDocumentoIdentidadProxy;
      private readonly IDepartamentoProxy departamentoProxy;
      private readonly IProvinciaProxy provinciaProxy;
      private readonly IDistritoProxy distritoProxy;
      private readonly IIntegrationApiProxy integrationApiProxy;
      private readonly ITipoProfesionOficioProxy tipoProfesionOficioProxy;
      private readonly ITipoVentaClienteProxy tipoVentaClienteProxy;
      private readonly IEmpresaSedeProxy empresaSedeProxy;
      private readonly IProveedorRedSocialProxy clienteRedSocialProxy;
      private readonly IRedSocialProxy redSocialProxy;
      private readonly ITipoViaProxy tipoViaProxy;
      private readonly ITipoZonaProxy tipoZonaProxy;
      private readonly IOrigenRegistroProxy origenRegistroProxy;
      private readonly ITipoProveedorProxy tipoProveedorProxy;
      private readonly IContactosProxy contactosProxy;
      private readonly IPuestoProxy puestoProxy;

      public ClienteController(
         IProveedorProxy proveedorProxy,
         ITipoNaturalezaJuridicaProxy tipoNaturalezaJuridicaProxy,
         ITipoDocumentoIdentidadProxy tipoDocumentoIdentidadProxy,
         IDepartamentoProxy departamentoProxy,
         IProvinciaProxy provinciaProxy,
         IDistritoProxy distritoProxy,
         IIntegrationApiProxy integrationApiProxy,
         ITipoProfesionOficioProxy tipoProfesionOficioProxy,
         ITipoVentaClienteProxy tipoVentaClienteProxy,
         IEmpresaSedeProxy empresaSedeProxy,
         IProveedorRedSocialProxy clienteRedSocialProxy,
         IRedSocialProxy redSocialProxy,
         ITipoViaProxy tipoViaProxy,
         ITipoZonaProxy tipoZonaProxy,
         IOrigenRegistroProxy origenRegistroProxy,
         ITipoProveedorProxy tipoProveedorProxy,
         IContactosProxy contactosProxy,
         IPuestoProxy  puestoProxy
         )
      {
         this.proveedorProxy = proveedorProxy;
         this.tipoNaturalezaJuridicaProxy = tipoNaturalezaJuridicaProxy;
         this.tipoDocumentoIdentidadProxy = tipoDocumentoIdentidadProxy;
         this.departamentoProxy = departamentoProxy;
         this.provinciaProxy = provinciaProxy;
         this.distritoProxy = distritoProxy;
         this.integrationApiProxy = integrationApiProxy;
         this.tipoProfesionOficioProxy = tipoProfesionOficioProxy;
         this.tipoVentaClienteProxy = tipoVentaClienteProxy;
         this.empresaSedeProxy = empresaSedeProxy;
         this.clienteRedSocialProxy = clienteRedSocialProxy;
         this.redSocialProxy = redSocialProxy;
         this.tipoViaProxy = tipoViaProxy;
         this.tipoZonaProxy = tipoZonaProxy;
         this.origenRegistroProxy = origenRegistroProxy;
         this.tipoProveedorProxy = tipoProveedorProxy;
         this.contactosProxy = contactosProxy;
         this.puestoProxy = puestoProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         ClienteViewModel clienteViewModel = new();

         cargarListasCompartidas(clienteViewModel);

         return PartialView(clienteViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         ClienteViewModel model = JsonSerializer.Deserialize<ClienteViewModel>(jsonModel);

         model.ProveedorDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.ProveedorDTO.OrderBy = "p.razon_social";

         model.ProveedorDTO.EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID);
         model.ProveedorDTO.EsProspecto = SiNoConstante.N;

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
                            f.Telefono,
                            f.Email,
                            TipoProfesionOficio = f?.TipoProfesionOficioDTO?.Nombre,
                            TipoVentaCliente = f?.TipoVentaClienteDTO?.Nombre,
                            f.Estado,
                            FechaUltimaVenta = f.FechaUltimaVenta == null ?String.Empty:f.FechaUltimaVenta.Value.ToShortDateString()
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
      public ActionResult Crear([FromBody] ClienteViewModel model)
      {
         Logger.InfoStart();

         ClienteViewModel clienteViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(clienteViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, clienteViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] ClienteViewModel model)
      {

         Logger.InfoStart();

         ClienteViewModel clienteViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(clienteViewModel, model.ProveedorDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, clienteViewModel);
      }
      private void cargarDataParaEditar(ClienteViewModel model, int id)
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
         WebHelper.SelectedListItemSetValue(model.TipoDocumentoIdentidad2List, proveedorDTO.TipoDocumentoIdentidad2Id);
         WebHelper.SelectedListItemSetValue(model.TipoProfesionOficioList, proveedorDTO.TipoProfesionOficioId);
         WebHelper.SelectedListItemSetValue(model.DistritoList, proveedorDTO.DistritoId);
         WebHelper.SelectedListItemSetValue(model.ProvinciaList, proveedorDTO.DistritoDTO.ProvinciaId);
         WebHelper.SelectedListItemSetValue(model.DepartamentoList, proveedorDTO.DistritoDTO.ProvinciaDTO.DepartamentoId);
         WebHelper.SelectedListItemSetValue(model.EmpresaSedeList, proveedorDTO.EmpresaSedeId);
         WebHelper.SelectedListItemSetValue(model.TipoVentaClienteList, proveedorDTO.TipoVentaClienteId);

         model.MenuItemList = new List<MenuItem>() {
            new MenuItem() { MenuItemType = MenuItemType.New },
            new MenuItem() { MenuItemType = MenuItemType.Save },
            new MenuItem() { MenuItemType = MenuItemType.Cancel },
            new MenuItem() { MenuItemType = MenuItemType.Delete }
         };
      }
      private void cargarListas(ClienteViewModel model)
      {
         cargarListasCompartidas(model);

         List<TipoNaturalezaJuridicaDTO> tipoNaturalezaJuridicaDTOList = tipoNaturalezaJuridicaProxy.Search(new TipoNaturalezaJuridicaDTO() { Estado = EstadoConstante.ACTIVO });
         model.TipoNaturalezaJuridicaList = WebHelper.ToSelectListItem(tipoNaturalezaJuridicaDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<TipoProfesionOficioDTO> tipoProfesionOficioDTOList = tipoProfesionOficioProxy.Search(new TipoProfesionOficioDTO() { Estado = EstadoConstante.ACTIVO });
         model.TipoProfesionOficioList = WebHelper.ToSelectListItem(tipoProfesionOficioDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<TipoVentaClienteDTO> tipoVentaProveedorDTOList = tipoVentaClienteProxy.Search(new TipoVentaClienteDTO() { Estado = EstadoConstante.ACTIVO });
         model.TipoVentaClienteList = WebHelper.ToSelectListItem(tipoVentaProveedorDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<EmpresaSedeDTO> empresaSedeDTOList = empresaSedeProxy.Search(new EmpresaSedeDTO()
         {
            EmpresaId = ObtenerEmpresaId(),
            Estado = EstadoConstante.ACTIVO
         });

         model.EmpresaSedeList = WebHelper.ToSelectListItem(empresaSedeDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<RedSocialDTO> redSocialDTOList = redSocialProxy.
         Search(new RedSocialDTO()
         {
            Estado = EstadoConstante.ACTIVO,
         });
         model.RedSocialString = string.Join(SeparadorConstante.PUNTO_COMA, redSocialDTOList.Select(x => x.Id.ToString() + SeparadorConstante.DOS_PUNTOS + x.Nombre));

         List<TipoDocumentoIdentidadDTO> tipoDocumentoIdentidadDTOList = tipoDocumentoIdentidadProxy.
         Search(new TipoDocumentoIdentidadDTO()
         {
            Estado = EstadoConstante.ACTIVO,
         });    
         model.TipoDocumentoString = string.Join(SeparadorConstante.PUNTO_COMA, tipoDocumentoIdentidadDTOList.Select(x => x.Id.ToString() + SeparadorConstante.DOS_PUNTOS + x.Abreviatura)); 
         
         List<PuestoDTO> puestoDTOList = puestoProxy.Search(new PuestoDTO()
         {
             Estado = EstadoConstante.ACTIVO,
         });
         model.PuestoString = string.Join(SeparadorConstante.PUNTO_COMA, puestoDTOList.Select(x => x.Id.ToString() + SeparadorConstante.DOS_PUNTOS + x.Nombre));
      }
      private void cargarListasCompartidas(ClienteViewModel model)
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


         model.TipoDocumentoIdentidad2List = WebHelper.ToSelectListItem(tipoDocumentoIdentidadDTOList, x => x.Id.ToString(), x => x.Abreviatura,
            SelectListFirstOptionType.Select);
      }
      private void cargarUbigeo(ClienteViewModel model, int departamentoId, int provinciaId)
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
      public ActionResult Grabar([FromBody] ClienteViewModel model)
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
                  messageResourceKey = "MK_cliente_agregado";
                  break;

               case ViewModelEditAction.Edit:
                  proveedorProxy.Update(proveedorDTO);
                  messageResourceKey = "MK_cliente_actualizado";
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
      private ProveedorDTO cargarProveedorDTO(ClienteViewModel model)
      {

         ProveedorDTO proveedorDTO;
         try
         {

            if(model.ViewModelState.EditAction == ViewModelEditAction.New)
            {
               proveedorDTO = model.ProveedorDTO;

               PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);
               proveedorDTO.EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId;
               proveedorDTO.EsProspecto = SiNoConstante.N;

               TipoViaDTO tipoViaDTO = tipoViaProxy.ObtenerPorCodigo(new TipoViaDTO() { Codigo = TipoViaConstante.DEFAULT });
               proveedorDTO.TipoViaId = tipoViaDTO.Id;

               TipoZonaDTO tipoZonaDTO = tipoZonaProxy.ObtenerPorCodigo(new TipoZonaDTO() { Codigo = TipoZonaConstante.DEFAULT });
               proveedorDTO.TipoZonaId = tipoZonaDTO.Id;

               TipoProveedorDTO tipoProveedorDTO = tipoProveedorProxy.ObtenerPorCodigo(new TipoProveedorDTO() { Codigo = TipoProveedorConstante.CLIENTE });
               proveedorDTO.TipoProveedorId = tipoProveedorDTO.Id;

               OrigenRegistroDTO origenRegistroDTO = origenRegistroProxy.ObtenerPorCodigo(new OrigenRegistroDTO() { Codigo = OrigenRegistroConstante.USUARIO });
               proveedorDTO.OrigenRegistroId = origenRegistroDTO.Id;
            }
            else
            {
               proveedorDTO = proveedorProxy.GetById(new ProveedorDTO() { ProveedorId = model.ProveedorDTO.Id });

               proveedorDTO.ApellidoPaterno = model.ProveedorDTO.ApellidoPaterno;
               proveedorDTO.ApellidoMaterno = model.ProveedorDTO.ApellidoMaterno;
               proveedorDTO.Nombres = model.ProveedorDTO.Nombres;
               proveedorDTO.Abreviatura = model.ProveedorDTO.Abreviatura;
               proveedorDTO.TipoDocumentoIdentidadId = model.ProveedorDTO.TipoDocumentoIdentidadId;
               proveedorDTO.NumeroDocumentoIdentidad = model.ProveedorDTO.NumeroDocumentoIdentidad;
               proveedorDTO.TipoDocumentoIdentidad2Id = model.ProveedorDTO.TipoDocumentoIdentidad2Id;
               proveedorDTO.NumeroDocumentoIdentidad2 = model.ProveedorDTO.NumeroDocumentoIdentidad2;
               proveedorDTO.Telefono = model.ProveedorDTO.Telefono;
               proveedorDTO.Email = model.ProveedorDTO.Email;
               proveedorDTO.Web = model.ProveedorDTO.Web;
               proveedorDTO.TipoProfesionOficioId = model.ProveedorDTO.TipoProfesionOficioId;
               proveedorDTO.DistritoId = model.ProveedorDTO.DistritoId;
               proveedorDTO.TipoVentaClienteId = model.ProveedorDTO.TipoVentaClienteId;
               proveedorDTO.EmpresaSedeId = model.ProveedorDTO.EmpresaSedeId;
               proveedorDTO.FechaNacimiento = model.ProveedorDTO.FechaNacimiento;
            }

            if (proveedorDTO.TipoNaturalezaJuridicaDTO.Codigo == TipoNaturalezaJuridicaConstante.NATURAL)
               proveedorDTO.RazonSocial = string.Concat(proveedorDTO.ApellidoPaterno,
                  string.IsNullOrEmpty(proveedorDTO.ApellidoMaterno) ? string.Empty : string.Concat(SeparadorConstante.ESPACIO, proveedorDTO.ApellidoMaterno),
                  string.Concat(SeparadorConstante.ESPACIO, proveedorDTO.Nombres));
            else
               proveedorDTO.NumeroDocumentoIdentidad = proveedorDTO.NumeroRuc;

            if (string.IsNullOrEmpty(model.ProveedorDTO.Contacto))
               model.ProveedorDTO.Contacto = string.Empty;
            if (string.IsNullOrEmpty(model.ProveedorDTO.NumeroCCI))
               model.ProveedorDTO.NumeroCCI = string.Empty;
            if (string.IsNullOrEmpty(model.ProveedorDTO.NombreVia))
               model.ProveedorDTO.NombreVia = string.Empty;
            if (string.IsNullOrEmpty(model.ProveedorDTO.NumeroVia))
               model.ProveedorDTO.NumeroVia = string.Empty;
            if (string.IsNullOrEmpty(model.ProveedorDTO.NombreZona))
               model.ProveedorDTO.NombreZona = string.Empty;
            if (string.IsNullOrEmpty(model.ProveedorDTO.Referencia))
               model.ProveedorDTO.Referencia = string.Empty;
            if (string.IsNullOrEmpty(model.ProveedorDTO.Departamento))
               model.ProveedorDTO.Departamento = string.Empty;
            if (string.IsNullOrEmpty(model.ProveedorDTO.Interior))
               model.ProveedorDTO.Interior = string.Empty;
            if (string.IsNullOrEmpty(model.ProveedorDTO.Manzana))
               model.ProveedorDTO.Manzana = string.Empty;
            if (string.IsNullOrEmpty(model.ProveedorDTO.Lote))
               model.ProveedorDTO.Lote = string.Empty;
            if (string.IsNullOrEmpty(model.ProveedorDTO.Kilometro))
               model.ProveedorDTO.Kilometro = string.Empty;
            if (string.IsNullOrEmpty(model.ProveedorDTO.Block))
               model.ProveedorDTO.Block = string.Empty;
            if (string.IsNullOrEmpty(model.ProveedorDTO.Etapa))
               model.ProveedorDTO.Etapa = string.Empty;
            if (string.IsNullOrEmpty(model.ProveedorDTO.EsLocal))
               model.ProveedorDTO.EsLocal = SiNoConstante.S;
            if (string.IsNullOrEmpty(model.ProveedorDTO.Cargo))
               model.ProveedorDTO.Cargo = string.Empty;
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
                     rucDTO.NombreVia,
                     NumeroVia = rucDTO.Numero,
                     rucDTO.Interior,
                     rucDTO.Departamento,
                     rucDTO.Lote,
                     rucDTO.Manzana,
                     rucDTO.Kilometro,
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
      [HttpPost("ListarProveedorRedSocial")]
      public ActionResult ListarProveedorRedSocial(string jsonModel)
      {

         Logger.InfoStart();

         ClienteViewModel model = JsonSerializer.Deserialize<ClienteViewModel>(jsonModel);

         model.ProveedorRedSocialDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.ProveedorRedSocialDTO.OrderBy = "prs.red_social_id";


         try
         {
            List<ProveedorRedSocialDTO> clienteRedSocialDTOList = clienteRedSocialProxy.Search(model.ProveedorRedSocialDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = clienteRedSocialDTOList.Count,
               rows = from f in clienteRedSocialDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.ProveedorRedSocialId,
                            f.ProveedorId,
                            f.RedSocialId,
                            f.Url,
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
      [HttpPost("GrabarProveedorRedSocial")]
      public ActionResult GrabarProveedorRedSocial([FromBody] ClienteViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            ProveedorRedSocialDTO clienteRedSocialDTO = cargarProveedorRedSocialDTO(model);

            int newId = model.ProveedorRedSocialDTO.Id;
            string messageResourceKey = string.Empty;
            switch (clienteRedSocialDTO.DTOState)
            {
               case DTOState.Added:
                  newId = clienteRedSocialProxy.Insert(clienteRedSocialDTO);
                  messageResourceKey = "MK_cliente_red_social_agregada";
                  break;

               case DTOState.Modified:
                  clienteRedSocialProxy.Update(clienteRedSocialDTO);
                  messageResourceKey = "MK_cliente_red_social_actualizada";
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
      private ProveedorRedSocialDTO cargarProveedorRedSocialDTO(ClienteViewModel model)
      {

         ProveedorRedSocialDTO clienteRedSocialDTO;
         try
         {

            clienteRedSocialDTO = model.ProveedorRedSocialDTO;

            clienteRedSocialDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(clienteRedSocialDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return clienteRedSocialDTO;
      }
      private ContactosDTO cargarContactosDTO(ClienteViewModel model)
      {
          ContactosDTO contactosDTO;
          try
          {

              contactosDTO = model.ContactosDTO;

              contactosDTO.Estado = EstadoConstante.ACTIVO;

              AuditDTO(contactosDTO);
          }
          catch (Exception)
          {
              throw;
          }
         return contactosDTO;
      }

      [HttpPost("EliminarProveedorRedSocial")]
      public ActionResult EliminarProveedorRedSocial([FromBody] ClienteViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.ProveedorRedSocialDTO.DTOState = DTOState.Modified;
            AuditDTO(model.ProveedorRedSocialDTO);

            clienteRedSocialProxy.Delete(model.ProveedorRedSocialDTO);

            resultResponse.Message = SR.GetString("MK_cliente_red_social_eliminada");
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
    
      /*[HttpPost("ListarPuestoPopup")]
      public ActionResult ListarPuestoPopup(string jsonModel)
      {
            Logger.InfoStart();

            ClienteViewModel model = JsonSerializer.Deserialize<ClienteViewModel>(jsonModel);

            try
            {
                List<PuestoDTO> puestoDTOList = puestoProxy.Search(model.PuestoDTO);

                int index = 1;
                var jsonData = new
                {
                    total = 1,
                    page = 1,
                    records = puestoDTOList.Count,
                    rows = from f in puestoDTOList.AsEnumerable()
                           select new
                           {
                               id = index++,
                               cell = new
                               {
                                   f.PuestoId,
                                   f.Nombre,
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
      }*/

        [HttpPost("ListarProveedorContactos")]
        public ActionResult ListarProveedorContactos(string jsonModel)
        {

            Logger.InfoStart();

            ClienteViewModel model = JsonSerializer.Deserialize<ClienteViewModel>(jsonModel);

            model.ContactosDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
            //model.ContactosDTO.OrderBy = "c.contactos_id";

            try
            {
                List<ContactosDTO> contactosDTOList = contactosProxy.Search(model.ContactosDTO);

                int index = 1;
                var jsonData = new
                {
                    total = 1,
                    page = 1,
                    records = contactosDTOList.Count,
                    rows = from f in contactosDTOList.AsEnumerable()
                           select new
                           {
                               id = index++,
                               cell = new
                               {
                                   f.ContactosId,
                                   f.ProveedorId,
                                   Nombre = f.NombreC,
                                   f.ApellidoPaterno,
                                   f.ApellidoMaterno,
                                   f.NumeroDocumentoIdentidadC,
                                   f.Telefono,
                                   f.Correo,
                                   TipDoc = f.TipoDocumentoIdentidadDTO.Abreviatura,
                                   f.TipoDocumentoIdentidadId,
                                   f.PuestoId,
                                   Puesto = f.PuestoDTO.Nombre,
                                   RazonSocial = f.ProveedorDTO.RazonSocial,
                                   RUC = f.ProveedorDTO.NumeroDocumentoIdentidad,
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

   [HttpPost("GrabarProveedorContactos")]
   public ActionResult GrabarProveedorContactos([FromBody] ClienteViewModel model)
   {
        Logger.InfoStart();
        ResultResponse resultResponse = new();

        try
        {    
            ContactosDTO contactosDTO= cargarContactosDTO(model);

            int newId = model.ContactosDTO.Id;
            string messageResourceKey = string.Empty;
            switch (contactosDTO.DTOState)
            {
                case DTOState.Added:
                    newId = contactosProxy.Insert(contactosDTO);
                    messageResourceKey = "MK_cliente_contacto_agregado";
                break;

                case DTOState.Modified:
                    contactosProxy.Update(contactosDTO);
                    messageResourceKey = "MK_cliente_contacto_actualizado";
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