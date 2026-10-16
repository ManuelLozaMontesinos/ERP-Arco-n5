using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service.DTO;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.AS.Models;
using ESD.ERP.Web.Areas.PD.Models;
using ESD.ERP.Web.Areas.VT.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using ESD.ERP.WebApiProxy.Impl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.VisualBasic;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace ESD.ERP.Web.Areas.PD.Controllers
{
    [Area("PD")]
    [Route("[controller]")]
    public class AsigProdController : AuthorizedController
    {
        private readonly IOrdenProduccionProxy ordenProduccionProxy;
        private readonly IOrdenProduccionProductoProxy ordenProduccionProductoProxy;
        private readonly IOrdenProduccionSubProcesoProxy ordenProduccionSubProcesoProxy;
        private readonly IEstadoOrdenProduccionProxy estadoOrdenProduccionProxy;
        private readonly IProveedorProxy proveedorProxy;
        private readonly IAlmacenProxy almacenProxy;
        private readonly IVendedorProxy vendedorProxy;
        private readonly IMaquinaProxy maquinaProxy;
        private readonly IPersonalMaquinaProxy personalMaquinaProxy;
        private readonly AppHelper appHelper;

        public AsigProdController(
           IOrdenProduccionProxy ordenProduccionProxy,
           IOrdenProduccionProductoProxy ordenProduccionProductoProxy,
           IEstadoOrdenProduccionProxy estadoOrdenProduccionProxy,
           IOrdenProduccionSubProcesoProxy ordenProduccionSubProcesoProxy,
           IProveedorProxy proveedorProxy,
           IAlmacenProxy almacenProxy,
           IVendedorProxy vendedorProxy,
           IMaquinaProxy maquinaProxy,
           IPersonalMaquinaProxy personalMaquinaProxy,
        AppHelper appHelper
           )
        {
            this.ordenProduccionProxy = ordenProduccionProxy;
            this.ordenProduccionProductoProxy = ordenProduccionProductoProxy;
            this.ordenProduccionSubProcesoProxy = ordenProduccionSubProcesoProxy;
            this.estadoOrdenProduccionProxy = estadoOrdenProduccionProxy;
            this.proveedorProxy = proveedorProxy;
            this.almacenProxy = almacenProxy;
            this.vendedorProxy = vendedorProxy;
            this.maquinaProxy = maquinaProxy;
            this.personalMaquinaProxy = personalMaquinaProxy;
            this.appHelper = appHelper;
        }

        [Route("[action]")]
        public IActionResult Index()
        {

            AsigProdViewModel asigProdViewModel = new();

            asigProdViewModel.Fecha = DateTime.Today;

            asigProdViewModel.MenuItemList = new List<MenuItem>
            {
            new MenuItem() { Key = "planos", Content = "Designar Cortador", Disabled = false },
            new MenuItem() { Key = "cortadores", Content= "Ver Cortadores", Disabled = false}
            };

            if (!EsAdministrador())
            {
                VendedorDTO vendedorDTO = vendedorProxy.SingleSearch(new VendedorDTO()
                {
                    ProveedorDTO = new ProveedorDTO()
                    {
                        EmpresaId = ObtenerEmpresaId(),
                        UsuarioId = ObtenerUsuarioId()
                    }
                });
                if (vendedorDTO != null)
                    asigProdViewModel.Vendedor = vendedorDTO.ProveedorDTO.RazonSocial;
            }

            return PartialView(asigProdViewModel);

        }
        [HttpPost("ListarBandeja")]
        public ActionResult ListarBandeja(string jsonModel)
        {

            Logger.InfoStart();

            AsigProdViewModel model = JsonSerializer.Deserialize<AsigProdViewModel>(jsonModel);

            model.OrdenProduccionDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
            model.OrdenProduccionDTO.OrderBy = "op.fecha desc,op.orden_pro desc";

            model.OrdenProduccionDTO.PeriodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(model.ViewModelState.CID);

            try
            {
                List<OrdenProduccionDTO> ordenProduccionDTOList = ordenProduccionProxy.Search(model.OrdenProduccionDTO);

                int index = 1;
                var jsonData = new
                {
                    total = 1,
                    page = 1,
                    records = ordenProduccionDTOList.Count,
                    rows = from f in ordenProduccionDTOList.AsEnumerable()
                           select new
                           {
                               id = index++,
                               cell = new
                               {
                                   f.OrdenProduccionId,
                                   f.Codigo,
                                   Fecha = f.Fecha.ToShortDateString(),
                                   Cotizacion = f?.CotizacionDTO?.Codigo,
                                   f.ProveedorId,
                                   Vendedor = f.CotizacionDTO.VendedorDTO.Abreviatura,
                                   f.Modificaciones,
                                   Proveedor = f.ProveedorDTO.Codigo,
                                   f.ProveedorDTO.RazonSocial,
                                   f.ProveedorDTO.NumeroDocumentoIdentidad,
                                   f?.EstadoPlanoMelId,
                                   EstadoPlanoMelamina = f?.EstadoPlanoMelDTO?.Nombre,
                                   f?.EstadoPlanoMdfId,
                                   EstadoPlanoMdf = f?.EstadoPlanoMdfDTO?.Nombre,
                                   f?.Comentarios,
                                   EstadoOrdenProduccionNombre = f.EstadoOrdenProduccionDTO.Nombre,
                                   EstadoDespacho = f?.EstadoDespachoDTO?.Codigo,
                                   EstadoDespachoNombre = f?.EstadoDespachoDTO?.Nombre,
                                   FechaEntrega = f.FechaEntrega.ToString(),
                                   f.Colores,
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

        private void cargarDataParaEditar(AsigProdViewModel model, int id)
        {

            OrdenProduccionDTO ordenProduccionDTO;
            if (model.EditAction == ViewModelEditAction.New)
            {
                ordenProduccionDTO = new OrdenProduccionDTO()
                {
                    Fecha = DateTime.Today
                };

                PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

                ProveedorDTO proveedorDTO = proveedorProxy.SingleSearch(new ProveedorDTO()
                {
                    EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId,
                    Codigo = ProveedorConstante.GENERICO
                });

                ordenProduccionDTO.ProveedorDTO = proveedorDTO;
                ordenProduccionDTO.ProveedorId = proveedorDTO.Id;

                EstadoOrdenProduccionDTO estadoOrdenProduccionDTO = estadoOrdenProduccionProxy.ObtenerPorCodigo(new EstadoOrdenProduccionDTO()
                {
                    Codigo = EstadoOrdenProduccionConstante.PENDIENTE
                });
                ordenProduccionDTO.EstadoOrdenProduccionId = estadoOrdenProduccionDTO.Id;
                ordenProduccionDTO.EstadoOrdenProduccionDTO = estadoOrdenProduccionDTO;
            }
            else
            {
                ordenProduccionDTO = ordenProduccionProxy.GetById(new OrdenProduccionDTO() { OrdenProduccionId = id });
            }

            model.OrdenProduccionDTO = ordenProduccionDTO;

            cargarListas(model);

            model.MenuItemList = new List<MenuItem>
            {
            new MenuItem() { MenuItemType = MenuItemType.Save },
            new MenuItem() { Key="solicitar", Content="Solicitar Aprobación" },
            new MenuItem() { MenuItemType = MenuItemType.Exit}
            };

            WebHelper.SelectedListItemSetValue(model.AlmacenList, ordenProduccionDTO.AlmacenId);
        }
        private void cargarListas(AsigProdViewModel model)
        {
            cargarListasCompartidas(model);
        }
        private void cargarListasCompartidas(AsigProdViewModel model)
        {
            PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

            List<AlmacenDTO> almacenDTOList = almacenProxy.Search(new AlmacenDTO()
            {
                EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId },
                EmpresaSedeId = periodoEmpresaSedeDTO.EmpresaSedeId,
                Estado = EstadoConstante.ACTIVO
            });
            model.AlmacenList = WebHelper.ToSelectListItem(almacenDTOList, x => x.Id.ToString(), x => x.Nombre,
               SelectListFirstOptionType.Select);

            List<MaquinaDTO> maquinaDTOList = maquinaProxy.Search(new MaquinaDTO()
            {
                Estado = EstadoConstante.ACTIVO,
            });
            model.MaquinaList = string.Join(SeparadorConstante.PUNTO_COMA, maquinaDTOList.Select(x => x.Id.ToString() + SeparadorConstante.DOS_PUNTOS + x.Nombre));

        }

        private OrdenProduccionDTO cargarOrdenProduccionDTO(AsigProdViewModel model)
        {

            OrdenProduccionDTO ordenProduccionDTO;
            try
            {

                ordenProduccionDTO = model.OrdenProduccionDTO;

                if (model.EditAction == ViewModelEditAction.New)
                {
                    PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

                    ordenProduccionDTO = model.OrdenProduccionDTO;

                    ordenProduccionDTO.PeriodoEmpresaSedeDTO = periodoEmpresaSedeDTO;
                    ordenProduccionDTO.PeriodoEmpresaSedeId = periodoEmpresaSedeDTO.PeriodoEmpresaSedeId;

                    EstadoOrdenProduccionDTO estadoOrdenProduccionDTO = estadoOrdenProduccionProxy.ObtenerPorCodigo(new EstadoOrdenProduccionDTO()
                    {
                        Codigo = EstadoOrdenProduccionConstante.PENDIENTE
                    });
                    ordenProduccionDTO.EstadoOrdenProduccionDTO = estadoOrdenProduccionDTO;
                    ordenProduccionDTO.EstadoOrdenProduccionId = estadoOrdenProduccionDTO.Id;
                }
                else
                {
                    ordenProduccionDTO = ordenProduccionProxy.GetById(new OrdenProduccionDTO() { OrdenProduccionId = model.OrdenProduccionDTO.Id });

                    PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);
                    model.OrdenProduccionDTO.PeriodoEmpresaSedeId = ordenProduccionDTO.PeriodoEmpresaSedeId;
                    model.OrdenProduccionDTO.AlmacenId = ordenProduccionDTO.AlmacenId;
                    model.OrdenProduccionDTO.EstadoOrdenProduccionDTO = ordenProduccionDTO.EstadoOrdenProduccionDTO;
                    model.OrdenProduccionDTO.EstadoOrdenProduccionId = ordenProduccionDTO.EstadoOrdenProduccionId;
                    model.OrdenProduccionId = ordenProduccionDTO.OrdenProduccionId;
                }

                model.OrdenProduccionId = ordenProduccionDTO.OrdenProduccionId;
                ordenProduccionDTO.Estado = EstadoConstante.ACTIVO;
                AuditDTO(ordenProduccionDTO);

            }
            catch (Exception)
            {
                throw;
            }

            return ordenProduccionDTO;
        }

        [Route("CambiarEstadoEditPopup")]
        public IActionResult CambiarEstadoEditPopup([FromBody] AsigProdViewModel model)
        {

            OrdenProduccionSubProcesoDTO ordenProduccionSubProcesoDTO = ordenProduccionSubProcesoProxy.GetById(new OrdenProduccionSubProcesoDTO() { OrdenProduccionId = model.OrdenProduccionSubProcesoDTO.OrdenProduccionId });

            model.OrdenProduccionId = model.OrdenProduccionSubProcesoDTO.OrdenProduccionId;
            List<MaquinaDTO> maquinaDTOList = maquinaProxy.Search(new MaquinaDTO()
            {
                Estado = EstadoConstante.ACTIVO,
            });
            model.MaquinaList = string.Join(SeparadorConstante.PUNTO_COMA, maquinaDTOList.Select(x => x.Id.ToString() + SeparadorConstante.DOS_PUNTOS + x.Nombre));

            return PartialView(model);
        }

        [Route("CambiarCortadoresEditPopup")]
        public IActionResult CambiarCortadoresEditPopup([FromBody] AsigProdViewModel model)
        {
            return PartialView(model);
        }

        [HttpPost("ListarPersonalMaquina")]
        public ActionResult ListarPersonalMaquina(string jsonModel)
        {

            Logger.InfoStart();

            AsigProdViewModel model = JsonSerializer.Deserialize<AsigProdViewModel>(jsonModel);

            model.OrdenProduccionSubProcesoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
            model.PersonalMaquinaDTO.Fecha = DateTime.Today;

            try
            {
                List<PersonalMaquinaDTO> personalMaquinaDTOList = personalMaquinaProxy.Search(model.PersonalMaquinaDTO);

                int index = 1;
                var jsonData = new
                {
                    total = 1,
                    page = 1,
                    records = personalMaquinaDTOList.Count,
                    rows = from f in personalMaquinaDTOList.AsEnumerable()
                           select new
                           {
                               id = index++,
                               cell = new
                               {
                                   f.PersonalMaquinaId,
                                   f.MaquinaId,
                                   MaquinaNombre = f.MaquinaDTO.Nombre,
                                   f.PersonalId,
                                   PersonalNombre = f.PersonalDTO.ProveedorDTO.RazonSocial,
                                   f.AsistenteId,
                                   AsistenteNombre = f.AsistenteDTO.ProveedorDTO.RazonSocial,
                                   Fecha = f.Fecha.ToString(),
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

        [HttpPost("ListarOrdenProduccionSubProceso")]
        public ActionResult ListarOrdenProduccionSubProceso(string jsonModel)
        {

            Logger.InfoStart();

            AsigProdViewModel model = JsonSerializer.Deserialize<AsigProdViewModel>(jsonModel);

            model.OrdenProduccionSubProcesoDTO.Estado = EstadoConstante.ACTIVO;
            model.OrdenProduccionSubProcesoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
            model.OrdenProduccionSubProcesoDTO.OrderBy = "opsp.orden";
            model.OrdenProduccionSubProcesoDTO.SubProcesoProduccionId = 1;

            model.OrdenProduccionSubProcesoDTO.OrdenProduccionId = NumberUtil.WhenValueIsZero(model.OrdenProduccionSubProcesoDTO.OrdenProduccionId);

            try
            {
                List<OrdenProduccionSubProcesoDTO> ordenProduccionSubProcesoDTOList = ordenProduccionSubProcesoProxy.Search(model.OrdenProduccionSubProcesoDTO);

                int index = 1;
                var jsonData = new
                {
                    total = 1,
                    page = 1,
                    records = ordenProduccionSubProcesoDTOList.Count,
                    rows = from f in ordenProduccionSubProcesoDTOList.AsEnumerable()
                           select new
                           {
                               id = index++,
                               cell = new
                               {
                                   f.OrdenProduccionSubProcesoId,
                                   f.OrdenProduccionId,
                                   SubProcesoProduccionNombre = f.SubProcesoProduccionDTO.Nombre,
                                   f?.MaquinaId,
                                   Maquina = f.MaquinaDTO?.Nombre,
                                   f?.PersonalId,
                                   Operador = f?.PersonalDTO?.ProveedorDTO?.RazonSocial,
                                   EstadoOrdenProduccionSubProcesoNombre = f.EstadoOrdenProduccionSubProcesoDTO.Nombre,
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


        [HttpPost("CambiarEstado")]
        public ActionResult CambiarEstado([FromBody] AsigProdViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);
                model.OrdenProduccionDTO.PeriodoEmpresaSedeId = periodoEmpresaSedeDTO.PeriodoEmpresaSedeId;
                model.OrdenProduccionDTO.PeriodoEmpresaSedeDTO = periodoEmpresaSedeDTO;

                AuditDTO(model.OrdenProduccionDTO);

                model.OrdenProduccionDTO.OrdenProduccionSubProcesoDTOList = model.OrdenProduccionSubProcesoDTOList;

                ordenProduccionProxy.AsignarMaquina(model.OrdenProduccionDTO);

                resultResponse.Message = SR.GetString("VT_pedido_enviado_produccion");

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

        [HttpPost("GrabarOrdenProduccionSubProceso")]
        public ActionResult GrabarOrdenProduccionSubProceso([FromBody] AsigProdViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                OrdenProduccionSubProcesoDTO ordenProduccionSubProcesoDTO = cargarOrdenProduccionSubProcesoDTO(model);

                int newId = model.OrdenProduccionSubProcesoDTO.Id;
                string messageResourceKey = string.Empty;
                switch (ordenProduccionSubProcesoDTO.DTOState)
                {
                    case DTOState.Added:
                        newId = ordenProduccionSubProcesoProxy.Insert(ordenProduccionSubProcesoDTO);
                        messageResourceKey = "PD_sub_proceso_agregado";
                        break;

                    case DTOState.Modified:
                        ordenProduccionSubProcesoProxy.Update(ordenProduccionSubProcesoDTO);
                        messageResourceKey = "PD_sub_proceso_actualizado";
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
        private OrdenProduccionSubProcesoDTO cargarOrdenProduccionSubProcesoDTO(AsigProdViewModel model)
        {

            OrdenProduccionSubProcesoDTO ordenProduccionSubProcesoDTO;
            try
            {

                if (model.OrdenProduccionSubProcesoDTO.DTOState == DTOState.Added)
                {
                    ordenProduccionSubProcesoDTO = model.OrdenProduccionSubProcesoDTO;
                }
                else
                {
                    ordenProduccionSubProcesoDTO = ordenProduccionSubProcesoProxy.GetById(new OrdenProduccionSubProcesoDTO()
                    {
                        OrdenProduccionSubProcesoId = model.OrdenProduccionSubProcesoDTO.Id
                    });

                    ordenProduccionSubProcesoDTO = model.OrdenProduccionSubProcesoDTO;
                }

                AuditDTO(ordenProduccionSubProcesoDTO);
            }
            catch (Exception)
            {
                throw;
            }

            return ordenProduccionSubProcesoDTO;
        }
    }
}