using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service.DTO;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.FE.Service;
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
   public class GuiaRemOtroController : AuthorizedController
   {
      private readonly IAlmacenProxy almacenProxy;
      private readonly IGuiaRemisionProxy guiaRemisionProxy;
      private readonly ITransportistaProxy transportistaProxy;
      private readonly ITransportistaConductorProxy transportistaConductorProxy;
      private readonly ITransportistaUnidadProxy transportistaUnidadProxy;
      private readonly IMotivoTrasladoProxy motivoTrasladoProxy;
      private readonly IGuiaRemisionProductoProxy guiaRemisionProductoProxy;
      private readonly IAlmacenSerieProxy almacenSerieProxy;
      private readonly IEstadoGuiaRemisionProxy estadoGuiaRemisionProxy;
      private readonly IDepartamentoProxy departamentoProxy;
      private readonly IProvinciaProxy provinciaProxy;
      private readonly IDistritoProxy distritoProxy;
      private readonly IProveedorProxy proveedorProxy;
      private readonly ITipoDocumentoComercialProxy tipoDocumentoComercialProxy;
      private readonly IDocumentoComercialSerieProxy documentoComercialSerieProxy;
      private readonly IFacturacionService facturacionService;
      private readonly AppHelper appHelper;

      public GuiaRemOtroController(
         IAlmacenProxy almacenProxy,
         IGuiaRemisionProxy guiaRemisionProxy,
         ITransportistaProxy transportistaProxy,
         ITransportistaConductorProxy transportistaConductorProxy,
         ITransportistaUnidadProxy transportistaUnidadProxy,
         IMotivoTrasladoProxy motivoTrasladoProxy,
         IGuiaRemisionProductoProxy guiaRemisionProductoProxy,
         IAlmacenSerieProxy almacenSerieProxy,
         IEstadoGuiaRemisionProxy estadoGuiaRemisionProxy,
         IDepartamentoProxy departamentoProxy,
         IProvinciaProxy provinciaProxy,
         IDistritoProxy distritoProxy,
         IProveedorProxy proveedorProxy,
         IDocumentoComercialSerieProxy documentoComercialSerieProxy,
         ITipoDocumentoComercialProxy tipoDocumentoComercialProxy,
         IFacturacionService facturacionService,
         AppHelper appHelper
         )
      {
         this.almacenProxy = almacenProxy;
         this.guiaRemisionProxy = guiaRemisionProxy;
         this.transportistaProxy = transportistaProxy;
         this.transportistaConductorProxy = transportistaConductorProxy;
         this.transportistaUnidadProxy = transportistaUnidadProxy;
         this.motivoTrasladoProxy = motivoTrasladoProxy;
         this.guiaRemisionProductoProxy = guiaRemisionProductoProxy;
         this.almacenSerieProxy = almacenSerieProxy;
         this.estadoGuiaRemisionProxy = estadoGuiaRemisionProxy;
         this.departamentoProxy = departamentoProxy;
         this.provinciaProxy = provinciaProxy;
         this.distritoProxy = distritoProxy;
         this.proveedorProxy = proveedorProxy;
         this.tipoDocumentoComercialProxy = tipoDocumentoComercialProxy;
         this.documentoComercialSerieProxy = documentoComercialSerieProxy;
         this.facturacionService = facturacionService;
         this.appHelper = appHelper;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         GuiaRemOtroViewModel guiaRemOtroViewModel = new()
         {
            MenuItemList = new List<MenuItem>
            {
               new MenuItem() { MenuItemType = MenuItemType.New },
               new MenuItem() { MenuItemType = MenuItemType.Edit },
               new MenuItem() { MenuItemType = MenuItemType.Delete },
               new MenuItem() { MenuItemType = MenuItemType.Preview }
            }
         };

         cargarListaTipoMovimientoAlmacen(guiaRemOtroViewModel);

         return PartialView(guiaRemOtroViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         GuiaRemOtroViewModel model = JsonSerializer.Deserialize<GuiaRemOtroViewModel>(jsonModel);

         model.GuiaRemisionDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.GuiaRemisionDTO.OrderBy = "gr.fecha desc,gr.serie,gr.numero";

         model.GuiaRemisionDTO.PeriodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(model.ViewModelState.CID);
         model.GuiaRemisionDTO.EsVenta = SiNoConstante.N;
         model.GuiaRemisionDTO.EsInterno = SiNoConstante.N;

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
                            FechaInicioTraslado = f.FechaInicioTraslado.ToShortDateString(),
                            Proveedor = f.ProveedorDestinoDTO.RazonSocial,
                            ProveedorNumeroDocumento = f.ProveedorDestinoDTO.NumeroDocumentoIdentidad,
                            f.PuntoPartida,
                            Destinatario = f.ProveedorDTO.RazonSocial,
                            DestinatarioNumeroDocumento = f.ProveedorDTO.NumeroDocumentoIdentidad,
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
      [HttpGet("ListarUnidadConductor")]
      public JsonResult ListarUnidadConductor(int id)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {

            List<TransportistaUnidadDTO> transportistaUnidadDTOList = transportistaUnidadProxy.Search(new TransportistaUnidadDTO()
            {
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
      [HttpPost("Crear")]
      public ActionResult Crear([FromBody] GuiaRemOtroViewModel model)
      {
         Logger.InfoStart();

         GuiaRemOtroViewModel guiaRemOtroViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(guiaRemOtroViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, guiaRemOtroViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] GuiaRemOtroViewModel model)
      {

         Logger.InfoStart();

         GuiaRemOtroViewModel guiaRemOtroViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(guiaRemOtroViewModel, model.GuiaRemisionDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, guiaRemOtroViewModel);
      }
      private void cargarDataParaEditar(GuiaRemOtroViewModel model, int id)
      {

         GuiaRemisionDTO guiaRemisionDTO;
         if (model.EditAction == ViewModelEditAction.New)
         {
            guiaRemisionDTO = new GuiaRemisionDTO()
            {
               Fecha = DateTime.Today,
               FechaInicioTraslado = DateTime.Today,
               ProveedorDTO = new ProveedorDTO(),
               ProveedorDestinoDTO = new ProveedorDTO()
            };

            cargarUbigeoPorDefecto(guiaRemisionDTO);

            EstadoGuiaRemisionDTO estadoGuiaRemisionDTO = estadoGuiaRemisionProxy.ObtenerPorCodigo(new EstadoGuiaRemisionDTO()
            {
               Codigo = EstadoGuiaRemisionConstante.EN_EDICION
            });

            guiaRemisionDTO.EstadoGuiaRemisionDTO = estadoGuiaRemisionDTO;
         }
         else
         {
            guiaRemisionDTO = guiaRemisionProxy.GetById(new GuiaRemisionDTO() { GuiaRemisionId = id });

            if (guiaRemisionDTO.DistritoDTO == null)
               cargarUbigeoPorDefecto(guiaRemisionDTO);
         }

         model.GuiaRemisionDTO = guiaRemisionDTO;

         cargarListas(model);
         cargarUbigeo(model, model.GuiaRemisionDTO.DistritoDTO.ProvinciaDTO.DepartamentoId,
            model.GuiaRemisionDTO.DistritoDTO.ProvinciaId);
         cargarUbigeo2(model, model.GuiaRemisionDTO.DistritoLlegadaDTO.ProvinciaDTO.DepartamentoId,
            model.GuiaRemisionDTO.DistritoLlegadaDTO.ProvinciaId);

         //WebHelper.SelectedListItemSetValue(model.AlmacenList, guiaRemisionDTO.MovimientoAlmacenDTO.AlmacenId);
         WebHelper.SelectedListItemSetValue(model.TransportistaConductorList, guiaRemisionDTO.TransportistaConductorId);
         WebHelper.SelectedListItemSetValue(model.TransportistaUnidadList, guiaRemisionDTO.TransportistaUnidadId);
         WebHelper.SelectedListItemSetValue(model.MotivoTrasladoList, guiaRemisionDTO.MotivoTrasladoId);
         WebHelper.SelectedListItemSetValue(model.DistritoList, guiaRemisionDTO.DistritoId);
         WebHelper.SelectedListItemSetValue(model.ProvinciaList, guiaRemisionDTO.DistritoDTO.ProvinciaId);
         WebHelper.SelectedListItemSetValue(model.DepartamentoList, guiaRemisionDTO.DistritoDTO.ProvinciaDTO.DepartamentoId);
         WebHelper.SelectedListItemSetValue(model.DistritoList2, guiaRemisionDTO.DistritoLlegadaId);
         WebHelper.SelectedListItemSetValue(model.ProvinciaList2, guiaRemisionDTO.DistritoLlegadaDTO.ProvinciaId);
         WebHelper.SelectedListItemSetValue(model.DepartamentoList2, guiaRemisionDTO.DistritoLlegadaDTO.ProvinciaDTO.DepartamentoId);
         WebHelper.SelectedListItemSetValue(model.TipoDocumentoComercialList, guiaRemisionDTO.TipoDocumentoComercialReferenciaId);

         model.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { Key="emitir", Content="Emitir", Disabled = true },
            new MenuItem() { Key = "actualizar", Content = "Actualizar CDR", Disabled = true},
            new MenuItem() { MenuItemType = MenuItemType.Save },
            new MenuItem() { MenuItemType = MenuItemType.Exit}
         };
      }
      private void cargarUbigeoPorDefecto(GuiaRemisionDTO guiaRemisionDTO)
      {
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
      }
      private void cargarListas(GuiaRemOtroViewModel model)
      {

         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

         List<AlmacenDTO> almacenDTOList = almacenProxy.Search(new AlmacenDTO()
         {
            EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId },
            EmpresaSedeId = periodoEmpresaSedeDTO.EmpresaSedeId,
            Estado = EstadoConstante.ACTIVO
         });

         List<AlmacenSerieDTO> almacenSerieDTOList = new();
         foreach (AlmacenDTO almacenDTO in almacenDTOList)
         {
            List<AlmacenSerieDTO> almacenSerieDTOTmpList = almacenSerieProxy.Search(new AlmacenSerieDTO()
            {
               AlmacenId = almacenDTO.Id,
               Estado = EstadoConstante.ACTIVO
            });

            if (almacenSerieDTOTmpList.Count > 0)
               almacenSerieDTOList.Add(almacenSerieDTOTmpList[0]);
         }

         model.SerieList = WebHelper.ToSelectListItem(almacenSerieDTOList, x => x.DocumentoComercialSerieDTO.Id.ToString(), x => x.DocumentoComercialSerieDTO.Serie,
               SelectListFirstOptionType.Empty);

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

         List<TipoDocumentoComercialDTO> tipoDocumentoComercialDTOList = tipoDocumentoComercialProxy.Search(new TipoDocumentoComercialDTO()
         {
            TipoDocumentoComercialList = new List<string> { TipoDocumentoComercialConstante.FACTURA,TipoDocumentoComercialConstante.BOLETA_VENTA}
         });
         model.TipoDocumentoComercialList = WebHelper.ToSelectListItem(tipoDocumentoComercialDTOList, x => x.Id.ToString(), x => x.Abreviatura,
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
      private void cargarUbigeo(GuiaRemOtroViewModel model, int departamentoId, int provinciaId)
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
      private void cargarUbigeo2(GuiaRemOtroViewModel model, int departamentoId, int provinciaId)
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
      private void cargarListaTipoMovimientoAlmacen(GuiaRemOtroViewModel model)
      {

         List<EstadoGuiaRemisionDTO> estadoGuiaRemisionDTOList = estadoGuiaRemisionProxy.Search(new EstadoGuiaRemisionDTO()
         {
            Estado = EstadoConstante.ACTIVO
         });
         model.EstadoGuiaRemisionList = WebHelper.ToSelectListItem(estadoGuiaRemisionDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);
      }
      [HttpPost("ObtenerDireccion")]
      public JsonResult ObtenerDireccion([FromBody] GuiaRemOtroViewModel model)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {

            ProveedorDTO proveedorDTO = proveedorProxy.GetById(new ProveedorDTO()
            {
               ProveedorId = model.GuiaRemisionDTO.ProveedorId
            });

            string direccion = proveedorDTO.Direccion;
            int departamentoId = proveedorDTO.DistritoDTO.ProvinciaDTO.DepartamentoDTO.DepartamentoId;
            int provinciaId = proveedorDTO.DistritoDTO.ProvinciaDTO.ProvinciaId;
            int distritoId = proveedorDTO.DistritoDTO.DistritoId;

            List<ProvinciaDTO> provinciaDTOList = obtenerListaProvincia(departamentoId);
            List<DistritoDTO> distritoDTOList = obtenerListaDistrito(provinciaId);

            resultResponse.Data = new
            {
               direccion,
               DepartamentoId = departamentoId,
               ProvinciaId = provinciaId,
               DistritoId = distritoId,
               ProvinciaList = WebHelper.ToSelectListItem(provinciaDTOList, x => x.Id.ToString(), x => x.Nombre,
                  SelectListFirstOptionType.Empty),
               DistritoList = WebHelper.ToSelectListItem(distritoDTOList, x => x.Id.ToString(), x => x.Nombre,
                  SelectListFirstOptionType.Empty)
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
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody] GuiaRemOtroViewModel model)
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
               guiaRemisionDTO.Numero
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
      private GuiaRemisionDTO cargarGuiaRemisionDTO(GuiaRemOtroViewModel model)
      {

         GuiaRemisionDTO guiaRemisionDTO;
         try
         {

            guiaRemisionDTO = model.GuiaRemisionDTO;

            if (model.EditAction == ViewModelEditAction.New)
            {
               guiaRemisionDTO.PeriodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);
               guiaRemisionDTO.PeriodoEmpresaSedeId = guiaRemisionDTO.PeriodoEmpresaSedeDTO.Id;

               TipoDocumentoComercialDTO tipoDocumentoComercialDTO = tipoDocumentoComercialProxy.ObtenerPorCodigo(new TipoDocumentoComercialDTO()
               {
                  Codigo = TipoDocumentoComercialConstante.GUIA_REMISION_REMITENTE
               });

               guiaRemisionDTO.TipoDocumentoComercialDTO = tipoDocumentoComercialDTO;
               guiaRemisionDTO.TipoDocumentoComercialId = tipoDocumentoComercialDTO.TipoDocumentoComercialId;

               guiaRemisionDTO.EsVenta = SiNoConstante.N;
               guiaRemisionDTO.Impreso = SiNoConstante.N;
               guiaRemisionDTO.EsModificacion = SiNoConstante.N;

               EstadoGuiaRemisionDTO estadoGuiaRemisionDTO = estadoGuiaRemisionProxy.ObtenerPorCodigo(new EstadoGuiaRemisionDTO()
               {
                  Codigo = EstadoGuiaRemisionConstante.EN_EDICION
               });

               guiaRemisionDTO.EstadoGuiaRemisionId = estadoGuiaRemisionDTO.Id;
               guiaRemisionDTO.ObservacionAnulacion = string.Empty;
            }
            else
            {
               guiaRemisionDTO = guiaRemisionProxy.GetById(new GuiaRemisionDTO() { GuiaRemisionId = guiaRemisionDTO.Id });

               model.GuiaRemisionDTO.PeriodoEmpresaSedeId = guiaRemisionDTO.PeriodoEmpresaSedeId;
               model.GuiaRemisionDTO.TipoDocumentoComercialId = guiaRemisionDTO.TipoDocumentoComercialId;
               model.GuiaRemisionDTO.ProveedorId = guiaRemisionDTO.ProveedorId;
               model.GuiaRemisionDTO.EsModificacion = SiNoConstante.N;

               guiaRemisionDTO = model.GuiaRemisionDTO;
            }

            guiaRemisionDTO.EsInterno = SiNoConstante.N;
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
      public ActionResult Emitir([FromBody] GuiaRemOtroViewModel model)
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

                  guiaRemisionProxy.ActualizarEstado(new GuiaRemisionDTO()
                  {
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
      public ActionResult ActualizarCDR([FromBody] GuiaRemOtroViewModel model)
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
      public ActionResult AnularEditPopup([FromBody] GuiaRemOtroViewModel model)
      {
         return PartialView(model);
      }
      [HttpPost("Eliminar")]
      public ActionResult Eliminar([FromBody] GuiaRemOtroViewModel model)
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

         return appHelper.ReportToPdf("IV_GuiaRemOtroE", parameters, "IV_GuiaRemOtroE");
      }
      [HttpPost("ListarGuiaRemisionProducto")]
      public ActionResult ListarGuiaRemisionProducto(string jsonModel)
      {

         Logger.InfoStart();

         GuiaRemOtroViewModel model = JsonSerializer.Deserialize<GuiaRemOtroViewModel>(jsonModel);

         model.GuiaRemisionProductoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.GuiaRemisionProductoDTO.OrderBy = "grp.cod_produ";

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
                            f.CodigoProducto,
                            f.Descripcion,
                            UnidadMedidaNombre = "NIU",
                            f.Cantidad,
                            f.PesoNeto,
                            f.PesoBruto,
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
      public ActionResult GrabarGuiaRemisionProducto([FromBody] GuiaRemOtroViewModel model)
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
      private GuiaRemisionProductoDTO cargarGuiaRemisionProductoDTO(GuiaRemOtroViewModel model)
      {

         GuiaRemisionProductoDTO guiaRemisionProductoDTO;
         try
         {

            guiaRemisionProductoDTO = model.GuiaRemisionProductoDTO;

            AuditDTO(guiaRemisionProductoDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return guiaRemisionProductoDTO;
      }
      [HttpPost("EliminarGuiaRemisionProducto")]
      public ActionResult EliminarGuiaRemisionProducto([FromBody] GuiaRemOtroViewModel model)
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
   }
}