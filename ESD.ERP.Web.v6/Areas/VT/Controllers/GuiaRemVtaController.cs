using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service.DTO;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.FE.Service;
using ESD.ERP.Web.Areas.VT.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.VT.Controllers
{
   [Area("VT")]
   [Route("[controller]")]
   public class GuiaRemVtaController : AuthorizedController
   {
      private readonly IGuiaRemisionProxy guiaRemisionProxy;
      private readonly ITransportistaProxy transportistaProxy;
      private readonly ITransportistaConductorProxy transportistaConductorProxy;
      private readonly ITransportistaUnidadProxy transportistaUnidadProxy;
      private readonly IMotivoTrasladoProxy motivoTrasladoProxy;
      private readonly IGuiaRemisionProductoProxy guiaRemisionProductoProxy;
      private readonly IAlmacenProxy almacenProxy;
      private readonly IAlmacenSerieProxy almacenSerieProxy;
      private readonly IDepartamentoProxy departamentoProxy;
      private readonly IProvinciaProxy provinciaProxy;
      private readonly IDistritoProxy distritoProxy;
      private readonly IProveedorProxy proveedorProxy;
      private readonly IDocumentoComercialSerieProxy documentoComercialSerieProxy;
      private readonly IFacturacionService facturacionService;
      private readonly ITipoDocumentoComercialProxy tipoDocumentoComercialProxy;
      private readonly IEstadoGuiaRemisionProxy estadoGuiaRemisionProxy;
      private readonly AppHelper appHelper;

      public GuiaRemVtaController(
         IGuiaRemisionProxy guiaRemisionProxy,
         ITransportistaProxy transportistaProxy,
         ITransportistaConductorProxy transportistaConductorProxy,
         ITransportistaUnidadProxy transportistaUnidadProxy,
         IMotivoTrasladoProxy motivoTrasladoProxy,
         IGuiaRemisionProductoProxy guiaRemisionProductoProxy,
         IAlmacenProxy almacenProxy,
         IAlmacenSerieProxy almacenSerieProxy,
         IDepartamentoProxy departamentoProxy,
         IProvinciaProxy provinciaProxy,
         IDistritoProxy distritoProxy,
         IProveedorProxy proveedorProxy,
         IDocumentoComercialSerieProxy documentoComercialSerieProxy,
         IFacturacionService facturacionService,
         ITipoDocumentoComercialProxy tipoDocumentoComercialProxy,
         IEstadoGuiaRemisionProxy estadoGuiaRemisionProxy,
         AppHelper appHelper
         )
      {
         this.guiaRemisionProxy = guiaRemisionProxy;
         this.transportistaProxy = transportistaProxy;
         this.transportistaConductorProxy = transportistaConductorProxy;
         this.transportistaUnidadProxy = transportistaUnidadProxy;
         this.motivoTrasladoProxy = motivoTrasladoProxy;
         this.guiaRemisionProductoProxy = guiaRemisionProductoProxy;
         this.almacenProxy = almacenProxy;
         this.almacenSerieProxy = almacenSerieProxy;
         this.departamentoProxy = departamentoProxy;
         this.provinciaProxy = provinciaProxy;
         this.distritoProxy = distritoProxy;
         this.proveedorProxy = proveedorProxy;
         this.documentoComercialSerieProxy = documentoComercialSerieProxy;
         this.facturacionService = facturacionService;
         this.tipoDocumentoComercialProxy = tipoDocumentoComercialProxy;
         this.estadoGuiaRemisionProxy = estadoGuiaRemisionProxy;
         this.appHelper = appHelper;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         GuiaRemVtaViewModel guiaRemVtaViewModel = new()
         {
            MenuItemList = new List<MenuItem>
            {
               new MenuItem() { MenuItemType = MenuItemType.Edit },
               new MenuItem() { MenuItemType = MenuItemType.Delete },
               new MenuItem() { MenuItemType = MenuItemType.Preview }
            }
         };

         return PartialView(guiaRemVtaViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         GuiaRemVtaViewModel model = JsonSerializer.Deserialize<GuiaRemVtaViewModel>(jsonModel);

         model.GuiaRemisionDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.GuiaRemisionDTO.OrderBy = "gr.fecha DESC,gr.serie,gr.numero desc";

         model.GuiaRemisionDTO.PeriodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(model.ViewModelState.CID);
         model.GuiaRemisionDTO.EsVenta = SiNoConstante.S;
         model.GuiaRemisionDTO.EsInterno = SiNoConstante.S;

         try
         {
            List<GuiaRemisionDTO> guiaRemisionDTOList = guiaRemisionProxy.Search(model.GuiaRemisionDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = guiaRemisionDTOList.Count,
               rows = from f in guiaRemisionDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.GuiaRemisionId,
                            f.SerieNumero,
                            Fecha = f.Fecha.ToShortDateString(),
                            DocumentoSerieNumeroReferencial = f.MovimientoAlmacenDTO != null ? f.MovimientoAlmacenDTO.DocumentoSerieNumeroReferencial : string.Empty,
                            RazonSocial = f.ProveedorDestinoDTO.RazonSocial,
                            NumeroDocumentoIdentidad = f.ProveedorDestinoDTO.NumeroDocumentoIdentidad,
                            FechaInicioTraslado = f.FechaInicioTraslado.ToShortDateString(),
                            f.PuntoPartida,
                            f.PuntoLlegada,
                            Usuario = f.UsuarioDTO.UsuarioLogon,
                            EstadoGuiaRemision = f.EstadoGuiaRemisionDTO.Codigo,
                            EstadoGuiaRemisionNombre = f.EstadoGuiaRemisionDTO.Nombre,
                            EstadoSunat = f.EstadoSunatDTO != null ? f.EstadoSunatDTO.Codigo : string.Empty,
                            EstadoSunatNombre = f.EstadoSunatDTO != null ? f.EstadoSunatDTO.Nombre : string.Empty,
                            f.Impreso,
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
      [HttpPost("CreatePopup")]
      public ActionResult CreatePopup([FromBody] GuiaRemVtaViewModel model)
      {

         try
         {
            if (model.GuiaRemisionDTO == null)
               model.GuiaRemisionDTO = new GuiaRemisionDTO();

            model.GuiaRemisionDTO.Fecha = DateTime.Today;
            model.GuiaRemisionDTO.FechaInicioTraslado = DateTime.Today;

            AlmacenDTO almacenDTO = almacenProxy.GetById(new AlmacenDTO() { AlmacenId = model.AlmacenId });
            model.GuiaRemisionDTO.PuntoPartida = almacenDTO.Direccion;

            ProveedorDTO proveedorDTO = proveedorProxy.GetById(new ProveedorDTO()
            {
               ProveedorId= model.GuiaRemisionDTO.MovimientoAlmacenDTO.ProveedorId.Value
            });
            model.GuiaRemisionDTO.PuntoLlegada = proveedorDTO.Direccion;

            cargarListas(model);
            cargarUbigeo(model, almacenDTO.DistritoDTO.ProvinciaDTO.DepartamentoId,
               almacenDTO.DistritoDTO.ProvinciaId);
            cargarUbigeo2(model, proveedorDTO.DistritoDTO.ProvinciaDTO.DepartamentoId,
               proveedorDTO.DistritoDTO.ProvinciaId);

            model.DepartamentoId = almacenDTO.DistritoDTO.ProvinciaDTO.DepartamentoId.ToString();
            model.ProvinciaId = almacenDTO.DistritoDTO.ProvinciaDTO.ProvinciaId.ToString();
            model.GuiaRemisionDTO.DistritoId = almacenDTO.DistritoId;

            model.Departamento2Id = proveedorDTO.DistritoDTO.ProvinciaDTO.DepartamentoId.ToString();
            model.Provincia2Id = proveedorDTO.DistritoDTO.ProvinciaDTO.ProvinciaId.ToString();
            model.GuiaRemisionDTO.DistritoLlegadaId = proveedorDTO.DistritoId;
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         return PartialView(model);
      }
      [HttpGet("ListarUnidadConductor")]
      public JsonResult ListarUnidadConductor(int id)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {

            List<TransportistaUnidadDTO> transportistaUnidadDTOList = transportistaUnidadProxy.Search(new TransportistaUnidadDTO() { 
               TransportistaId = id,
               Estado = EstadoConstante.ACTIVO
            });

            List<TransportistaConductorDTO> transportistaConductorDTOList = transportistaConductorProxy.Search(new TransportistaConductorDTO()
            {
               TransportistaId = id,
               Estado = EstadoConstante.ACTIVO
            });

            resultResponse.Data = new
            {
               UnidadList = WebHelper.ToSelectListItem(transportistaUnidadDTOList, x => x.Id.ToString(), x => x.MarcaModeloPlata,
               SelectListFirstOptionType.Select),
               ConductorList = WebHelper.ToSelectListItem(transportistaConductorDTOList, x => x.Id.ToString(), x => x.ApellidosNombres,
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
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] GuiaRemVtaViewModel model)
      {

         Logger.InfoStart();

         GuiaRemVtaViewModel guiaRemVtaViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(guiaRemVtaViewModel, model.GuiaRemisionDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, guiaRemVtaViewModel);
      }
      private void cargarDataParaEditar(GuiaRemVtaViewModel model, int id)
      {

         GuiaRemisionDTO guiaRemisionDTO;
         if (model.EditAction == ViewModelEditAction.New)
         {
            guiaRemisionDTO = new GuiaRemisionDTO() {
               Fecha = DateTime.Today,
               FechaInicioTraslado = DateTime.Today
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
            guiaRemisionDTO.DistritoDTO = distritoProxy.SingleSearch(distritoDTO);
            guiaRemisionDTO.DistritoId = guiaRemisionDTO.DistritoDTO.Id;

            guiaRemisionDTO.DistritoLlegadaDTO = distritoProxy.SingleSearch(distritoDTO);
            guiaRemisionDTO.DistritoLlegadaId = guiaRemisionDTO.DistritoDTO.Id;

            EstadoGuiaRemisionDTO estadoGuiaRemisionDTO = estadoGuiaRemisionProxy.ObtenerPorCodigo(new EstadoGuiaRemisionDTO() { Codigo = EstadoGuiaRemisionConstante.EN_EDICION});
            guiaRemisionDTO.EstadoGuiaRemisionId = estadoGuiaRemisionDTO.Id;
            guiaRemisionDTO.EstadoGuiaRemisionDTO = estadoGuiaRemisionDTO;

         }
         else
         {
            guiaRemisionDTO = guiaRemisionProxy.GetById(new GuiaRemisionDTO() { GuiaRemisionId = id });
            model.AlmacenId = guiaRemisionDTO.MovimientoAlmacenDTO.AlmacenId;
         }

         model.GuiaRemisionDTO = guiaRemisionDTO;

         cargarListas(model);
         cargarUbigeo(model, model.GuiaRemisionDTO.DistritoDTO.ProvinciaDTO.DepartamentoId,
            model.GuiaRemisionDTO.DistritoDTO.ProvinciaId);
         cargarUbigeo2(model, model.GuiaRemisionDTO.DistritoLlegadaDTO.ProvinciaDTO.DepartamentoId,
            model.GuiaRemisionDTO.DistritoLlegadaDTO.ProvinciaId);

         WebHelper.SelectedListItemSetValue(model.TransportistaConductorList, guiaRemisionDTO.TransportistaConductorId);
         WebHelper.SelectedListItemSetValue(model.TransportistaUnidadList, guiaRemisionDTO.TransportistaUnidadId);
         WebHelper.SelectedListItemSetValue(model.MotivoTrasladoList, guiaRemisionDTO.MotivoTrasladoId);
         WebHelper.SelectedListItemSetValue(model.DistritoList, guiaRemisionDTO.DistritoId);
         WebHelper.SelectedListItemSetValue(model.ProvinciaList, guiaRemisionDTO.DistritoDTO.ProvinciaId);
         WebHelper.SelectedListItemSetValue(model.DepartamentoList, guiaRemisionDTO.DistritoDTO.ProvinciaDTO.DepartamentoId);
         WebHelper.SelectedListItemSetValue(model.DistritoList2, guiaRemisionDTO.DistritoLlegadaId);
         WebHelper.SelectedListItemSetValue(model.ProvinciaList2, guiaRemisionDTO.DistritoLlegadaDTO.ProvinciaId);
         WebHelper.SelectedListItemSetValue(model.DepartamentoList2, guiaRemisionDTO.DistritoLlegadaDTO.ProvinciaDTO.DepartamentoId);

         model.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { Key = "emitir", Content = "Emitir", Disabled = true},
            new MenuItem() { Key = "actualizar", Content = "Actualizar CDR", Disabled = true},
            new MenuItem() { MenuItemType = MenuItemType.Save},
            new MenuItem() { MenuItemType = MenuItemType.Exit}
         };

      }
      private void cargarListas(GuiaRemVtaViewModel model)
      {

         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

         List<AlmacenSerieDTO> almacenSerieDTOList = almacenSerieProxy.Search(new AlmacenSerieDTO()
         {
            AlmacenId = model.AlmacenId,
            Estado = EstadoConstante.ACTIVO
         });

         model.SerieList = WebHelper.ToSelectListItem(almacenSerieDTOList, x => x.DocumentoComercialSerieDTO.Id.ToString(), x => x.DocumentoComercialSerieDTO.Serie,
               SelectListFirstOptionType.Select);

         List<TransportistaDTO> transportistaDTOList = transportistaProxy.Search(new TransportistaDTO() 
         {
            EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId 
         });
         model.TransportistaList = WebHelper.ToSelectListItem(transportistaDTOList, x => x.Id.ToString(), x => x.RazonSocial,
               SelectListFirstOptionType.Select);

         TransportistaConductorDTO transportistaConductorDTO = null;
         if (model.GuiaRemisionDTO.TransportistaConductorId.HasValue)
            transportistaConductorDTO = transportistaConductorProxy.GetById(new TransportistaConductorDTO()
            {
               TransportistaConductorId = model.GuiaRemisionDTO.TransportistaConductorId.Value
            });

         if (model.GuiaRemisionDTO.TransportistaConductorId.HasValue)
         {
            List<TransportistaConductorDTO> transportistaConductorDTOList = transportistaConductorProxy.Search(new TransportistaConductorDTO()
            {
               TransportistaId = transportistaConductorDTO.TransportistaId
            });
            model.TransportistaConductorList = WebHelper.ToSelectListItem(transportistaConductorDTOList, x => x.Id.ToString(), x => x.ApellidosNombres,
                  SelectListFirstOptionType.Select);

            WebHelper.SelectedListItemSetValue(model.TransportistaList, transportistaConductorDTOList[0].TransportistaId);
         }
         else
            model.TransportistaConductorList = WebHelper.ToSelectListItem(SelectListFirstOptionType.Select);

         if (model.GuiaRemisionDTO.TransportistaConductorId.HasValue)
         {
            List<TransportistaUnidadDTO> transportistaUnidadDTOList = transportistaUnidadProxy.Search(new TransportistaUnidadDTO()
            {
               TransportistaId = transportistaConductorDTO.TransportistaId
            });
            model.TransportistaUnidadList = WebHelper.ToSelectListItem(transportistaUnidadDTOList, x => x.Id.ToString(), x => x.MarcaModeloPlata,
                  SelectListFirstOptionType.Select);
         }
         else
            model.TransportistaUnidadList = WebHelper.ToSelectListItem(SelectListFirstOptionType.Select);

         List<MotivoTrasladoDTO> motivoTrasladoDTOList = motivoTrasladoProxy.Search(new MotivoTrasladoDTO() { Estado = EstadoConstante.ACTIVO });
         model.MotivoTrasladoList = WebHelper.ToSelectListItem(motivoTrasladoDTOList, x => x.Id.ToString(), x => x.Nombre,
               SelectListFirstOptionType.Select);

         if (model.SelectedItemList != null)
         {
            SelectedItem selectedItem = model.SelectedItemList.Where(x => x.Name == "MotivoTraslado").FirstOrDefault();
            if (selectedItem != null)
            {
               MotivoTrasladoDTO motivoTrasladoDTO = motivoTrasladoDTOList.Where(x => x.Codigo == selectedItem.Value).FirstOrDefault();
               if (motivoTrasladoDTO != null)
                  WebHelper.SelectedListItemSetValue(model.MotivoTrasladoList, motivoTrasladoDTO.Id);
            }

         }
      }
      private void cargarUbigeo(GuiaRemVtaViewModel model, int departamentoId, int provinciaId)
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
      private void cargarUbigeo2(GuiaRemVtaViewModel model, int departamentoId, int provinciaId)
      {

         List<DepartamentoDTO> departamentoDTOList = departamentoProxy.Search(new DepartamentoDTO() { Estado = EstadoConstante.ACTIVO });
         model.DepartamentoList2 = WebHelper.ToSelectListItem(departamentoDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<ProvinciaDTO> provinciaDTOList = obtenerListaProvincia(departamentoId);
         model.ProvinciaList2 = WebHelper.ToSelectListItem(provinciaDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<DistritoDTO> distritoDTOList = obtenerListaDistrito(provinciaId);
         model.DistritoList2 = WebHelper.ToSelectListItem(distritoDTOList, x => x.Id.ToString(), x => x.Nombre,
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
      public ActionResult Grabar([FromBody] GuiaRemVtaViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            GuiaRemisionDTO guiaRemisionDTO = cargarGuiaRemisionDTO(model);

            int newId = model.GuiaRemisionDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = guiaRemisionProxy.Insert(guiaRemisionDTO);
                  messageResourceKey = "IV_guia_remision_agregada";
                  break;

               case ViewModelEditAction.Edit:
                  guiaRemisionProxy.Update(guiaRemisionDTO);
                  messageResourceKey = "IV_guia_remision_actualizada";  
                  break;
            }

            guiaRemisionDTO = guiaRemisionProxy.GetById(new GuiaRemisionDTO() { GuiaRemisionId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey);
            resultResponse.Data = new
            {
               guiaRemisionDTO.Id,
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
      private GuiaRemisionDTO cargarGuiaRemisionDTO(GuiaRemVtaViewModel model)
      {

         GuiaRemisionDTO guiaRemisionDTO;
         try
         {

            guiaRemisionDTO = model.GuiaRemisionDTO;

            if (model.EditAction == ViewModelEditAction.New)
            {
               guiaRemisionDTO.PeriodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(model.ViewModelState.CID);

               TipoDocumentoComercialDTO tipoDocumentoComercialDTO = tipoDocumentoComercialProxy.ObtenerPorCodigo(new TipoDocumentoComercialDTO() { 
                  Codigo = TipoDocumentoComercialConstante.GUIA_REMISION_REMITENTE
               });

               guiaRemisionDTO.TipoDocumentoComercialDTO = tipoDocumentoComercialDTO;
               guiaRemisionDTO.TipoDocumentoComercialId = tipoDocumentoComercialDTO.TipoDocumentoComercialId;
               guiaRemisionDTO.ObservacionAnulacion = string.Empty;
            }
            else
            {
               guiaRemisionDTO = guiaRemisionProxy.GetById(new GuiaRemisionDTO() { GuiaRemisionId = model.GuiaRemisionDTO.Id });

               model.GuiaRemisionDTO.PeriodoEmpresaSedeId = guiaRemisionDTO.PeriodoEmpresaSedeId;
               model.GuiaRemisionDTO.TipoDocumentoComercialId = guiaRemisionDTO.TipoDocumentoComercialId;
               model.GuiaRemisionDTO.EsInterno = guiaRemisionDTO.EsInterno;
               model.GuiaRemisionDTO.ProveedorId = guiaRemisionDTO.ProveedorId;
               model.GuiaRemisionDTO.SerieDocumentoReferencia = guiaRemisionDTO.SerieDocumentoReferencia;
               model.GuiaRemisionDTO.NumeroDocumentoReferencia = guiaRemisionDTO.NumeroDocumentoReferencia;
               model.GuiaRemisionDTO.ProveedorDestinoId = guiaRemisionDTO.ProveedorDestinoId;

               guiaRemisionDTO = model.GuiaRemisionDTO;
            }

            guiaRemisionDTO.CodigoSunatLlegada = string.Empty;
            guiaRemisionDTO.Estado = EstadoConstante.ACTIVO;
            AuditDTO(guiaRemisionDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return guiaRemisionDTO;
      }
      [HttpPost("Emitir")]
      public ActionResult Emitir([FromBody] GuiaRemVtaViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            GuiaRemisionDTO guiaRemisionDTO = cargarGuiaRemisionDTO(model);

            int newId = model.GuiaRemisionDTO.Id;

            guiaRemisionProxy.EmitirSunat(guiaRemisionDTO);

            guiaRemisionDTO = guiaRemisionProxy.GetById(new GuiaRemisionDTO() { GuiaRemisionId = newId });

            generarDocumentoElectronico(guiaRemisionDTO);

            guiaRemisionDTO = guiaRemisionProxy.GetById(new GuiaRemisionDTO() { GuiaRemisionId = newId });

            resultResponse.Message = SR.GetString("IV_guia_remision_emitida");
            if (guiaRemisionDTO.EstadoSunatDTO != null)
            {
               if (guiaRemisionDTO.EstadoSunatDTO.Codigo != EstadoSunatConstante.APROBADO)
                  resultResponse.Message = obtenerMensajeSunat(guiaRemisionDTO.ComprobanteElectronicoId.Value);

               if (guiaRemisionDTO.EstadoSunatDTO.Codigo == EstadoSunatConstante.APROBADO ||
                   guiaRemisionDTO.EstadoSunatDTO.Codigo == EstadoSunatConstante.VERIFICACION)
               {
                  EstadoGuiaRemisionDTO estadoGuiaRemisionDTO = estadoGuiaRemisionProxy.ObtenerPorCodigo(new EstadoGuiaRemisionDTO() { Codigo = EstadoGuiaRemisionConstante.EMITIDO });
                  guiaRemisionDTO.EstadoGuiaRemisionId = estadoGuiaRemisionDTO.Id;
                  guiaRemisionDTO.EstadoGuiaRemisionDTO = estadoGuiaRemisionDTO;

                  guiaRemisionProxy.ActualizarEstado(new GuiaRemisionDTO() { 
                     GuiaRemisionId = guiaRemisionDTO.Id,
                     EstadoGuiaRemisionId = estadoGuiaRemisionDTO.Id
                  });
               }
            }

            resultResponse.Data = new
            {
               guiaRemisionDTO.Id,
               guiaRemisionDTO.Numero,
               EstadoGuiaRemision = guiaRemisionDTO.EstadoGuiaRemisionDTO.Codigo,
               EstadoSunat = guiaRemisionDTO?.EstadoSunatDTO?.Codigo
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
      private void generarDocumentoElectronico(GuiaRemisionDTO guiaRemisionDTO)
      {

         DocumentoComercialSerieDTO documentoComercialSerieDTO = documentoComercialSerieProxy.SingleSearch(new DocumentoComercialSerieDTO()
         {
            EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = guiaRemisionDTO.PeriodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId },
            TipoDocumentoComercialId = guiaRemisionDTO.TipoDocumentoComercialId,
            Serie = guiaRemisionDTO.Serie
         });

         List<ParametroSistemaDTO> parametroSistemaDTOList = appHelper
            .ObtenerGrupoParametroSistema(GrupoParametroSistemaConstante.FACTURACION_ELECTRONICA);

         ParametroSistemaDTO parametroSistemaDTO = parametroSistemaDTOList.
            FirstOrDefault(i => i.Codigo == ParametroSistemaConstante.FACTURACION_E_URL && i.Estado == EstadoConstante.ACTIVO);

         if (parametroSistemaDTO == null)
            return;

         facturacionService.GenerarEnviarDocumento(parametroSistemaDTO.ValorCadena, guiaRemisionDTO.TipoDocumentoComercialDTO.Codigo,
            guiaRemisionDTO.Id, documentoComercialSerieDTO.EnvioEnLinea);

      }
      private string obtenerMensajeSunat(int comprobanteElectronicoId)
      {

         List<ParametroSistemaDTO> parametroSistemaDTOList = appHelper
            .ObtenerGrupoParametroSistema(GrupoParametroSistemaConstante.FACTURACION_ELECTRONICA);

         ParametroSistemaDTO parametroSistemaDTO = parametroSistemaDTOList.
            FirstOrDefault(i => i.Codigo == ParametroSistemaConstante.FACTURACION_E_URL && i.Estado == EstadoConstante.ACTIVO);

         FE.Models.DTO.ResponseDTO responseDTO = facturacionService.ConsultarDocumentoEnviado(parametroSistemaDTO.ValorCadena, comprobanteElectronicoId);

         return responseDTO.SunatDescripcion;

      }
      [HttpPost("ActualizarCDR")]
      public ActionResult ActualizarCDR([FromBody] GuiaRemVtaViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            int newId = model.GuiaRemisionDTO.Id;

            GuiaRemisionDTO guiaRemisionDTO = guiaRemisionProxy.GetById(new GuiaRemisionDTO() { GuiaRemisionId = newId });

            actualizarGuiaRemisionCDR(guiaRemisionDTO);

            guiaRemisionDTO = guiaRemisionProxy.GetById(new GuiaRemisionDTO() { GuiaRemisionId = newId });

            resultResponse.Message = SR.GetString("IV_guia_remision_emitida");
            if (guiaRemisionDTO.EstadoSunatDTO != null)
            {
               if (guiaRemisionDTO.EstadoSunatDTO.Codigo != EstadoSunatConstante.APROBADO)
                  resultResponse.Message = obtenerMensajeSunat(guiaRemisionDTO.ComprobanteElectronicoId.Value);
            }

            resultResponse.Data = new
            {
               EstadoGuiaRemision = guiaRemisionDTO.EstadoGuiaRemisionDTO.Codigo,
               EstadoSunat = guiaRemisionDTO?.EstadoSunatDTO?.Codigo
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
      private void actualizarGuiaRemisionCDR(GuiaRemisionDTO guiaRemisionDTO)
      {

         List<ParametroSistemaDTO> parametroSistemaDTOList = appHelper
            .ObtenerGrupoParametroSistema(GrupoParametroSistemaConstante.FACTURACION_ELECTRONICA);

         ParametroSistemaDTO parametroSistemaDTO = parametroSistemaDTOList.
            FirstOrDefault(i => i.Codigo == ParametroSistemaConstante.FACTURACION_E_URL && i.Estado == EstadoConstante.ACTIVO);

         if (parametroSistemaDTO == null)
            return;

         facturacionService.ActualizarCDR(parametroSistemaDTO.ValorCadena, guiaRemisionDTO.TipoDocumentoComercialDTO.Codigo,
            guiaRemisionDTO.Id);

      }
      [HttpPost("AnularEditPopup")]
      public ActionResult AnularEditPopup([FromBody] GuiaRemVtaViewModel model)
      {
         return PartialView(model);
      }
      [HttpPost("Eliminar")]
      public ActionResult Eliminar([FromBody] GuiaRemVtaViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            UsuarioDTO usuarioDTO = ObtenerUsuarioDTO();

            GuiaRemisionDTO guiaRemisionDTO = guiaRemisionProxy.GetById(new GuiaRemisionDTO() { GuiaRemisionId = model.GuiaRemisionDTO.Id });

            if (string.IsNullOrEmpty(guiaRemisionDTO.ObservacionAnulacion))
               guiaRemisionDTO.ObservacionAnulacion = string.Empty;
            else
               guiaRemisionDTO.ObservacionAnulacion = string.Concat(usuarioDTO.UsuarioLogon, SeparadorConstante.GUION, model.GuiaRemisionDTO.ObservacionAnulacion);
            guiaRemisionDTO.DTOState = DTOState.Modified;

            AuditDTO(guiaRemisionDTO);

            guiaRemisionProxy.Delete(guiaRemisionDTO);

            resultResponse.Message = SR.GetString("IV_guia_remision_anulada");
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
      public FileContentResult ReportTo(string format, string cid, string guiaRemisionId)
      {
         Dictionary<string, string> parameters = new()
         {
            { "guiaRemisionId", guiaRemisionId }
         };

         return appHelper.ReportToPdf("IV_GuiaRemisionE", parameters, "IV_GuiaRemisionE");
      }
      [HttpPost("ListarGuiaRemisionProducto")]
      public ActionResult ListarGuiaRemisionProducto(string jsonModel)
      {

         Logger.InfoStart();

         GuiaRemVtaViewModel model = JsonSerializer.Deserialize<GuiaRemVtaViewModel>(jsonModel);

         model.GuiaRemisionProductoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.GuiaRemisionProductoDTO.OrderBy = "p.descripcion";

         try
         {
            List<GuiaRemisionProductoDTO> guiaRemisionProductoDTOList = guiaRemisionProductoProxy.Search(model.GuiaRemisionProductoDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = guiaRemisionProductoDTOList.Count,
               rows = from f in guiaRemisionProductoDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.GuiaRemisionProductoId,
                            f.GuiaRemisionId,
                            f.MovimientoAlmacenProductoId,
                            f.MovimientoAlmacenProductoDTO.ProductoDTO.Codigo,
                            ProductoNombre = f.MovimientoAlmacenProductoDTO.ProductoDTO.Descripcion,
                            UnidadMedidaNombre = f.MovimientoAlmacenProductoDTO.ProductoDTO.UnidadMedidaDTO.Abreviatura,
                            f.Descripcion,
                            f.Cantidad,
                            PesoNeto = f.ProductoDTO.PesoNeto,
                            PesoBruto = f.ProductoDTO.PesoBruto,
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
      [HttpPost("GrabarGuiaRemisionProducto")]
      public ActionResult GrabarGuiaRemisionProducto([FromBody] GuiaRemVtaViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            GuiaRemisionProductoDTO guiaRemisionProductoDTO = cargarGuiaRemisionProductoDTO(model);

            int newId = model.GuiaRemisionProductoDTO.Id;
            string messageResourceKey = string.Empty;
            switch (guiaRemisionProductoDTO.DTOState)
            {
               case DTOState.Added:
                  newId = guiaRemisionProductoProxy.Insert(guiaRemisionProductoDTO);
                  messageResourceKey = "IV_producto_agregado";
                  break;

               case DTOState.Modified:
                  guiaRemisionProductoProxy.Update(guiaRemisionProductoDTO);
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
      private GuiaRemisionProductoDTO cargarGuiaRemisionProductoDTO(GuiaRemVtaViewModel model)
      {

         GuiaRemisionProductoDTO guiaRemisionProductoDTO;
         try
         {

            guiaRemisionProductoDTO = model.GuiaRemisionProductoDTO;
            guiaRemisionProductoDTO.CodigoProducto = string.Empty;

            AuditDTO(guiaRemisionProductoDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return guiaRemisionProductoDTO;
      }
      [HttpPost("EliminarGuiaRemisionProducto")]
      public ActionResult EliminarGuiaRemisionProducto([FromBody] GuiaRemVtaViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.GuiaRemisionProductoDTO.DTOState = DTOState.Modified;
            AuditDTO(model.GuiaRemisionProductoDTO);

            guiaRemisionProductoProxy.Delete(model.GuiaRemisionProductoDTO);

            resultResponse.Message = SR.GetString("IV_producto_eliminado");
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
            GuiaRemisionDTO guiaRemisionDTO = guiaRemisionProxy.GetById(new GuiaRemisionDTO() { GuiaRemisionId = id });

            AlmacenDTO almacenDTO = almacenProxy.GetById(new AlmacenDTO() { AlmacenId = guiaRemisionDTO.MovimientoAlmacenDTO.AlmacenId });

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