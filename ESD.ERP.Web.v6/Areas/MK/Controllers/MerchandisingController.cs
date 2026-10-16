using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service.DTO;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.IV.Models;
using ESD.ERP.Web.Areas.MK.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.MK.Controllers
{
    [Area("MK")]
    [Route("[controller]")]

    public class MerchandisingController : AuthorizedController
    {
        private readonly IAlmacenProxy almacenProxy;
        private readonly ITransaccionAlmacenProxy transaccionAlmacenProxy;
        private readonly IMovimientoAlmacenProxy movimientoAlmacenProxy;
        private readonly ITipoMovimientoAlmacenProxy tipoMovimientoAlmacenProxy;
        private readonly IAlmacenTransaccionProxy almacenTransaccionProxy;
        private readonly IAlmacenTransaccionConfiguracionProxy almacenTransaccionConfiguracionProxy;
        private readonly IMesContableProxy mesContableProxy;
        private readonly IEstadoMovimientoAlmacenProxy estadoMovimientoAlmacenProxy;
        private readonly ITipoDocumentoComercialProxy tipoDocumentoComercialProxy;
        private readonly IMovimientoAlmacenProductoProxy movimientoAlmacenProductoProxy;
        private readonly IOrdenCompraProxy ordenCompraProxy;
        private readonly ITipoCambioDiaProxy tipoCambioDiaProxy;
        public MerchandisingController(
           IAlmacenProxy almacenProxy,
           ITransaccionAlmacenProxy transaccionAlmacenProxy,
           IMovimientoAlmacenProxy movimientoAlmacenProxy,
           ITipoMovimientoAlmacenProxy tipoMovimientoAlmacenProxy,
           IAlmacenTransaccionProxy almacenTransaccionProxy,
           IAlmacenTransaccionConfiguracionProxy almacenTransaccionConfiguracionProxy,
           IMesContableProxy mesContableProxy,
           ITipoDocumentoComercialProxy tipoDocumentoComercialProxy,
           IEstadoMovimientoAlmacenProxy estadoMovimientoAlmacenProxy,
           IMovimientoAlmacenProductoProxy movimientoAlmacenProductoProxy,
           IOrdenCompraProxy ordenCompraProxy,
           ITipoCambioDiaProxy tipoCambioDiaProxy
           )
        {
            this.almacenProxy = almacenProxy;
            this.movimientoAlmacenProxy = movimientoAlmacenProxy;
            this.transaccionAlmacenProxy = transaccionAlmacenProxy;
            this.tipoMovimientoAlmacenProxy = tipoMovimientoAlmacenProxy;
            this.almacenTransaccionProxy = almacenTransaccionProxy;
            this.almacenTransaccionConfiguracionProxy = almacenTransaccionConfiguracionProxy;
            this.mesContableProxy = mesContableProxy;
            this.estadoMovimientoAlmacenProxy = estadoMovimientoAlmacenProxy;
            this.tipoDocumentoComercialProxy = tipoDocumentoComercialProxy;
            this.movimientoAlmacenProductoProxy = movimientoAlmacenProductoProxy;
            this.ordenCompraProxy = ordenCompraProxy;
            this.tipoCambioDiaProxy = tipoCambioDiaProxy;
        }
        [Route("[action]")]
        public IActionResult Index()
        {
            MerchandisingViewModel merchandisingViewModel = new MerchandisingViewModel();

            cargarListasCompartidas(merchandisingViewModel);
            cargarListaTransaccionAlmacen(merchandisingViewModel);

            return PartialView(merchandisingViewModel);
        }
        [HttpPost("ListarBandeja")]
        public ActionResult ListarBandeja(string jsonModel)
        {

            Logger.InfoStart();

            MerchandisingViewModel model = JsonSerializer.Deserialize<MerchandisingViewModel>(jsonModel);

            
            model.MovimientoAlmacenDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
            model.MovimientoAlmacenDTO.OrderBy = "ma.fecha desc,tma.tipo_mov_alma,ma.mov_almacen";

            model.MovimientoAlmacenDTO.PeriodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(model.ViewModelState.CID);

            model.MovimientoAlmacenDTO.AlmacenId = 4;

            string transaccionAlmacenRestringida = string.Concat(TransaccionAlmacenConstante.SALIDA_POR_VENTAS, SeparadorConstante.COMA,
               TransaccionAlmacenConstante.DEVOLUCION_DE_VENTA, TransaccionAlmacenConstante.RECLASIFICACION_DE_ITEMS_INGRESO,
               TransaccionAlmacenConstante.RECLASIFICACION_DE_ITEMS_SALIDA);

            try
            {
                List<MovimientoAlmacenDTO> movimientoAlmacenDTOList = movimientoAlmacenProxy.Search(model.MovimientoAlmacenDTO);

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
                                   f.NumeroOrdenCompra,
                                   TransaccionAlmacenNombre = f.TransaccionAlmacenDTO.Nombre,
                                   TipoMovimientoAlmacen = f.TipoMovimientoAlmacenDTO.Codigo,
                                   AlmacenNombre = f.AlmacenDTO.Nombre,
                                   f.AlmacenDestinoId,
                                   AlmacenDestinoNombre = f.AlmacenDestinoDTO == null ? string.Empty : f.AlmacenDestinoDTO.Nombre,
                                   f.NumeroGuiaRemision,
                                   f.DocumentoSerieNumeroReferencial,
                                   f.ProveedorId,
                                   RazonSocial = f.ProveedorDTO == null ? string.Empty : f.ProveedorDTO.RazonSocial,
                                   EstadoMovimientoAlmacenNombre = f.EstadoMovimientoAlmacenDTO.Nombre,
                                   Usuario = f.UsuarioDTO.UsuarioLogon,
                                   RegRestringido = transaccionAlmacenRestringida.IndexOf(f.TransaccionAlmacenDTO.Codigo) >= 0 ? SiNoConstante.S : SiNoConstante.N,
                                   f.Observacion,
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
        public ActionResult Crear([FromBody] MerchandisingViewModel model)
        {
            Logger.InfoStart();

            MerchandisingViewModel merchandisingViewModel = new MerchandisingViewModel
            {
                EditAction = ViewModelEditAction.New,
                IsNew = true
            };

            try
            {
                cargarDataParaEditar(merchandisingViewModel, 0);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw;
            }

            Logger.InfoFinish();

            return PartialView(PartialViewNameConstant.CRUD, merchandisingViewModel);
        }
        [HttpPost("Editar")]
        public ActionResult Editar([FromBody] MerchandisingViewModel model)
        {

            Logger.InfoStart();

            MerchandisingViewModel merchandisingViewModel = new MerchandisingViewModel
            {
                EditAction = ViewModelEditAction.Edit
            };

            try
            {
                cargarDataParaEditar(merchandisingViewModel, model.MovimientoAlmacenDTO.Id);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw;
            }

            Logger.InfoFinish();

            return PartialView(PartialViewNameConstant.CRUD, merchandisingViewModel);
        }
        private void cargarDataParaEditar(MerchandisingViewModel model, int id)
        {

            MovimientoAlmacenDTO movimientoAlmacenDTO;
            if (model.EditAction == ViewModelEditAction.New)
            {
                movimientoAlmacenDTO = new MovimientoAlmacenDTO()
                {
                    TipoMovimientoAlmacenDTO = new TipoMovimientoAlmacenDTO(),
                    TransaccionAlmacenDTO = new TransaccionAlmacenDTO() { Codigo = string.Empty },
                    UsuarioDTO = new UsuarioDTO()
                };
                movimientoAlmacenDTO.Fecha = DateTime.Today;
            }
            else
            {
                movimientoAlmacenDTO = movimientoAlmacenProxy.GetById(new MovimientoAlmacenDTO() { MovimientoAlmacenId = id });
            }

            model.MovimientoAlmacenDTO = movimientoAlmacenDTO;

            cargarListas(model);

            int almacenTransaccionId = 0;
            if (model.MovimientoAlmacenDTO.AlmacenId == 0)
                model.AlmacenTransaccionList = WebHelper.ToSelectListItem(SelectListFirstOptionType.Select);
            else
            {
                List<AlmacenTransaccionDTO> almacenTransaccionDTOList = obtenerListaAlmacenTransaccion(model.MovimientoAlmacenDTO.AlmacenId,
                   model.MovimientoAlmacenDTO.TipoMovimientoAlmacenId);

                model.AlmacenTransaccionList = WebHelper.ToSelectListItem(almacenTransaccionDTOList, x => x.AlmacenTransaccionId.ToString(),
                   x => x.TransaccionAlmacenDTO.Nombre, SelectListFirstOptionType.Select);

                AlmacenTransaccionDTO almacenTransaccionDTO = almacenTransaccionDTOList
                   .Where(x => x.TransaccionAlmacenId == movimientoAlmacenDTO.TransaccionAlmacenId).FirstOrDefault();

                if (almacenTransaccionDTO != null)
                {
                    almacenTransaccionId = almacenTransaccionDTO.Id;

                    var configList = obtenerAlmacenTransaccionConfiguracion(almacenTransaccionId);

                    model.MovimientoConfiguracion = JsonSerializer.Serialize(configList);
                }
            }

            string transaccionAlmacenRestringida = string.Concat(TransaccionAlmacenConstante.SALIDA_POR_VENTAS, SeparadorConstante.COMA,
               TransaccionAlmacenConstante.DEVOLUCION_DE_VENTA);

            if (!string.IsNullOrEmpty(movimientoAlmacenDTO.TransaccionAlmacenDTO.Codigo))
                model.RegRestringido = transaccionAlmacenRestringida.IndexOf(movimientoAlmacenDTO.TransaccionAlmacenDTO.Codigo) >= 0 ? SiNoConstante.S : SiNoConstante.N;

            if (string.IsNullOrEmpty(model.RegRestringido))
                model.RegRestringido = SiNoConstante.N;

            WebHelper.SelectedListItemSetValue(model.AlmacenList, movimientoAlmacenDTO.AlmacenId);
            WebHelper.SelectedListItemSetValue(model.TipoMovimientoAlmacenList, movimientoAlmacenDTO.TipoMovimientoAlmacenId);
            WebHelper.SelectedListItemSetValue(model.AlmacenTransaccionList, almacenTransaccionId);
            WebHelper.SelectedListItemSetValue(model.TipoDocumentoComercialList, movimientoAlmacenDTO.TipoDocumentoComercialId);
        }
        private void cargarListas(MerchandisingViewModel model)
        {
            cargarListasCompartidas(model);

            List<TipoDocumentoComercialDTO> tipoDocumentoComercialDTOList = tipoDocumentoComercialProxy.Search(new TipoDocumentoComercialDTO() { Estado = EstadoConstante.ACTIVO });
            model.TipoDocumentoComercialList = WebHelper.ToSelectListItem(tipoDocumentoComercialDTOList, x => x.Id.ToString(), x => x.Nombre,
               SelectListFirstOptionType.Blank);

            List<TipoMovimientoAlmacenDTO> tipoMovimientoAlmacenDTOList = tipoMovimientoAlmacenProxy.Search(new TipoMovimientoAlmacenDTO() { Estado = EstadoConstante.ACTIVO });
            model.TipoMovimientoAlmacenList = WebHelper.ToSelectListItem(tipoMovimientoAlmacenDTOList, x => x.Id.ToString(), x => x.Nombre,
               SelectListFirstOptionType.Select);

        }
        private void cargarListasCompartidas(MerchandisingViewModel model)
        {
            PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

            List<AlmacenDTO> almacenDTOList = almacenProxy.Search(new AlmacenDTO()
            {
                EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId },
                EmpresaSedeId = periodoEmpresaSedeDTO.EmpresaSedeId,
                Nombre = "ALMACEN MARKETING LIMA",
                Estado = EstadoConstante.ACTIVO
            });
            model.AlmacenList = WebHelper.ToSelectListItem(almacenDTOList, x => x.Id.ToString(), x => x.Nombre,
               SelectListFirstOptionType.Select);
        }
        private void cargarListaTransaccionAlmacen(MerchandisingViewModel model)
        {
            PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

            List<TransaccionAlmacenDTO> transaccionAlmacenDTOList = transaccionAlmacenProxy.Search(new TransaccionAlmacenDTO()
            {
                EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID),
                TipoMovimientoAlmacenDTO = model?.MovimientoAlmacenDTO?.TransaccionAlmacenDTO?.TipoMovimientoAlmacenDTO,
                Estado = EstadoConstante.ACTIVO
            });
            model.TransaccionAlmacenList = WebHelper.ToSelectListItem(transaccionAlmacenDTOList, x => x.Id.ToString(), x => x.Nombre,
               SelectListFirstOptionType.Select);
        }
        private List<AlmacenTransaccionDTO> obtenerListaAlmacenTransaccion(int almacenId, int tipoMovimientoAlmacenId)
        {
            List<AlmacenTransaccionDTO> almacenTransaccionDTOList = almacenTransaccionProxy.Search(new AlmacenTransaccionDTO()
            {
                AlmacenId = almacenId,
                TransaccionAlmacenDTO = new TransaccionAlmacenDTO() { TipoMovimientoAlmacenId = tipoMovimientoAlmacenId },
                Estado = EstadoConstante.ACTIVO
            });

            return almacenTransaccionDTOList;
        }
        [HttpPost("ListarAlmacenTransaccion")]
        public JsonResult ListarAlmacenTransaccion([FromBody] MerchandisingViewModel model)
        {

            Logger.InfoStart();
            ResultResponse resultResponse = new();

            try
            {
                TipoMovimientoAlmacenDTO tipoMovimientoAlmacenDTO = tipoMovimientoAlmacenProxy.GetById(new TipoMovimientoAlmacenDTO()
                {
                    TipoMovimientoAlmacenId = model.AlmacenTransaccionDTO.TransaccionAlmacenDTO.TipoMovimientoAlmacenId.Value
                });

                List<AlmacenTransaccionDTO> almacenTransaccionDTOList = obtenerListaAlmacenTransaccion(model.AlmacenTransaccionDTO.AlmacenId,
                   model.AlmacenTransaccionDTO.TransaccionAlmacenDTO.TipoMovimientoAlmacenId.Value);

                resultResponse.Data = new
                {
                    TipoMovimientoAlmacen = tipoMovimientoAlmacenDTO.Codigo,
                    List = WebHelper.ToSelectListItem(almacenTransaccionDTOList, x => x.AlmacenTransaccionId.ToString(), x => x.TransaccionAlmacenDTO.Nombre,
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
        [HttpGet("ObtenerAlmacenTransaccion")]
        public JsonResult ObtenerAlmacenTransaccion(int id)
        {

            Logger.InfoStart();
            ResultResponse resultResponse = new();

            try
            {
                AlmacenTransaccionDTO almacenTransaccionDTO = almacenTransaccionProxy.GetById(new AlmacenTransaccionDTO() { AlmacenTransaccionId = id });

                var configList = obtenerAlmacenTransaccionConfiguracion(almacenTransaccionDTO.Id);

                resultResponse.Data = new
                {
                    almacenTransaccionDTO.TransaccionAlmacenId,
                    almacenTransaccionDTO.TransaccionAlmacenDTO.TieneMantenimiento,
                    TransaccionAlmacen = almacenTransaccionDTO.TransaccionAlmacenDTO.Codigo,
                    List = configList
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
        private dynamic obtenerAlmacenTransaccionConfiguracion(int almacenTransaccionId)
        {
            List<AlmacenTransaccionConfiguracionDTO> almacenTransaccionConfiguracionDTOList = almacenTransaccionConfiguracionProxy
                          .Search(new AlmacenTransaccionConfiguracionDTO()
                          {
                              AlmacenTransaccionId = almacenTransaccionId,
                              Visible = SiNoConstante.S,
                              Estado = EstadoConstante.ACTIVO
                          });

            var newList = from x in almacenTransaccionConfiguracionDTOList
                          select new
                          {
                              x.AtributoConfiguracionDTO.Atributo,
                              x.AtributoConfiguracionDTO.Editable,
                              x.AtributoConfiguracionDTO.TieneBoton,
                              x.AtributoConfiguracionDTO.EsGrupo,
                              x.AtributoConfiguracionDTO.AtributoLista,
                              x.Visible,
                              x.Requerido,
                              Mensaje = SR.GetString("IV_atributo_" + x.AtributoConfiguracionDTO.Atributo)
                          };

            return newList;
        }
        [HttpPost("Grabar")]
        public ActionResult Grabar([FromBody] MerchandisingViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                MovimientoAlmacenDTO movimientoAlmacenDTO = cargarMovimientoAlmacenDTO(model);

                int newId = model.MovimientoAlmacenDTO.Id;
                string messageResourceKey = string.Empty;
                switch (model.EditAction)
                {
                    case ViewModelEditAction.New:
                        newId = movimientoAlmacenProxy.Insert(movimientoAlmacenDTO);
                        messageResourceKey = "IV_movimiento_almacen_agregado";
                        break;

                    case ViewModelEditAction.Edit:
                        movimientoAlmacenProxy.Update(movimientoAlmacenDTO);
                        messageResourceKey = "IV_movimiento_almacen_actualizado";
                        break;
                }

                movimientoAlmacenDTO = movimientoAlmacenProxy.GetById(new MovimientoAlmacenDTO() { MovimientoAlmacenId = newId });

                resultResponse.Message = SR.GetString(messageResourceKey, movimientoAlmacenDTO.Codigo);
                resultResponse.Data = new
                {
                    movimientoAlmacenDTO.Id,
                    movimientoAlmacenDTO.Codigo,
                    movimientoAlmacenDTO.MesContableId
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
        private MovimientoAlmacenDTO cargarMovimientoAlmacenDTO(MerchandisingViewModel model)
        {

            MovimientoAlmacenDTO movimientoAlmacenDTO;
            try
            {

                movimientoAlmacenDTO = model.MovimientoAlmacenDTO;

                if (model.EditAction == ViewModelEditAction.New)
                {
                    movimientoAlmacenDTO.PeriodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(model.ViewModelState.CID);

                    string mesContable = string.Format(StringFormatConstant.DATE_MONTH, movimientoAlmacenDTO.Fecha);

                    MesContableDTO mesContableDTO = mesContableProxy.ObtenerPorCodigo(new MesContableDTO() { Codigo = mesContable });
                    movimientoAlmacenDTO.MesContableId = mesContableDTO.Id;

                    EstadoMovimientoAlmacenDTO estadoMovimientoAlmacenDTO = estadoMovimientoAlmacenProxy.ObtenerPorCodigo(
                       new EstadoMovimientoAlmacenDTO() { Codigo = EstadoMovimientoAlmacenConstante.MOVIMIENTO });
                    movimientoAlmacenDTO.EstadoMovimientoAlmacenId = estadoMovimientoAlmacenDTO.Id;
                }
                else
                {
                    movimientoAlmacenDTO = movimientoAlmacenProxy.GetById(new MovimientoAlmacenDTO() { MovimientoAlmacenId = movimientoAlmacenDTO.Id });

                    model.MovimientoAlmacenDTO.AlmacenDestinoId = movimientoAlmacenDTO.AlmacenDestinoId;
                    model.MovimientoAlmacenDTO.MesContableId = movimientoAlmacenDTO.MesContableId;
                    model.MovimientoAlmacenDTO.ComprobanteId = movimientoAlmacenDTO.ComprobanteId;
                    model.MovimientoAlmacenDTO.TieneGuiaRemision = movimientoAlmacenDTO.TieneGuiaRemision;
                    model.MovimientoAlmacenDTO.GuiaRemisionEnModificacion = movimientoAlmacenDTO.GuiaRemisionEnModificacion;
                    model.MovimientoAlmacenDTO.PersonalId = movimientoAlmacenDTO.PersonalId;
                    model.MovimientoAlmacenDTO.CentroCostoId = movimientoAlmacenDTO.CentroCostoId;
                    model.MovimientoAlmacenDTO.PersonalAutorizaId = movimientoAlmacenDTO.PersonalAutorizaId;
                    model.MovimientoAlmacenDTO.AfectaSaldo = movimientoAlmacenDTO.AfectaSaldo;
                    model.MovimientoAlmacenDTO.EstadoMovimientoAlmacenId = movimientoAlmacenDTO.EstadoMovimientoAlmacenId;

                    movimientoAlmacenDTO = model.MovimientoAlmacenDTO;
                }

                if (string.IsNullOrEmpty(movimientoAlmacenDTO.NumeroOrdenCompra))
                    movimientoAlmacenDTO.NumeroOrdenCompra = string.Empty;

                if (string.IsNullOrEmpty(movimientoAlmacenDTO.NumeroGuiaRemision))
                    movimientoAlmacenDTO.NumeroGuiaRemision = string.Empty;

                if (string.IsNullOrEmpty(movimientoAlmacenDTO.SerieDocumento))
                    movimientoAlmacenDTO.SerieDocumento = string.Empty;

                if (string.IsNullOrEmpty(movimientoAlmacenDTO.NumeroDocumento))
                    movimientoAlmacenDTO.NumeroDocumento = string.Empty;

                if (string.IsNullOrEmpty(movimientoAlmacenDTO.TieneGuiaRemision))
                    movimientoAlmacenDTO.TieneGuiaRemision = SiNoConstante.N;
                if (string.IsNullOrEmpty(movimientoAlmacenDTO.GuiaRemisionEnModificacion))
                    movimientoAlmacenDTO.GuiaRemisionEnModificacion = SiNoConstante.N;

                if (string.IsNullOrEmpty(movimientoAlmacenDTO.AfectaSaldo))
                    movimientoAlmacenDTO.AfectaSaldo = SiNoConstante.S;

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
        public ActionResult Eliminar([FromBody] MerchandisingViewModel model)
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

            MerchandisingViewModel model = JsonSerializer.Deserialize<MerchandisingViewModel>(jsonModel);

            model.MovimientoAlmacenProductoDTO.PageSize = 1000;
            model.MovimientoAlmacenProductoDTO.OrderBy = "p.descripcion";

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
                                   f.RegistroReferencialId,
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
        [HttpPost("GrabarMovimientoAlmacenProducto")]
        public ActionResult GrabarMovimientoAlmacenProducto([FromBody] MerchandisingViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                MovimientoAlmacenProductoDTO movimientoAlmacenProductoDTO = cargarMovimientoAlmacenProductoDTO(model);

                int newId = model.MovimientoAlmacenProductoDTO.Id;
                string messageResourceKey = string.Empty;
                switch (movimientoAlmacenProductoDTO.DTOState)
                {
                    case DTOState.Added:
                        newId = movimientoAlmacenProductoProxy.Insert(movimientoAlmacenProductoDTO);
                        messageResourceKey = "IV_producto_agregado";
                        break;

                    case DTOState.Modified:
                        movimientoAlmacenProductoProxy.Update(movimientoAlmacenProductoDTO);
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
        private MovimientoAlmacenProductoDTO cargarMovimientoAlmacenProductoDTO(MerchandisingViewModel model)
        {

            MovimientoAlmacenProductoDTO movimientoAlmacenProductoDTO;
            try
            {

                movimientoAlmacenProductoDTO = model.MovimientoAlmacenProductoDTO;

                AuditDTO(movimientoAlmacenProductoDTO);

            }
            catch (Exception)
            {
                throw;
            }

            return movimientoAlmacenProductoDTO;
        }
        [HttpPost("EliminarMovimientoAlmacenProducto")]
        public ActionResult EliminarMovimientoAlmacenProducto([FromBody] MerchandisingViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                model.MovimientoAlmacenProductoDTO.DTOState = DTOState.Modified;
                AuditDTO(model.MovimientoAlmacenProductoDTO);

                movimientoAlmacenProductoProxy.Delete(model.MovimientoAlmacenProductoDTO);

                resultResponse.Message = SR.GetString("IV_producto_anulado");
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
        public ActionResult ListPopup([FromBody] MerchandisingViewModel model)
        {
            try
            {
                PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO();

                List<AlmacenDTO> almacenDTOList = almacenProxy.Search(new AlmacenDTO()
                {
                    EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId },
                    EmpresaSedeId = periodoEmpresaSedeDTO.EmpresaSedeId,
                    Nombre = "ALMACEN MARKETING LIMA",
                    Estado = EstadoConstante.ACTIVO
                });
                model.AlmacenList = WebHelper.ToSelectListItem(almacenDTOList, x => x.Id.ToString(), x => x.Nombre,
                   SelectListFirstOptionType.Select);

                cargarListaTransaccionAlmacen(model);

                SelectedItem selectedItem = model.SelectedItemList.Where(x => x.Name == "AlmacenId").FirstOrDefault();
                if (selectedItem != null)
                {
                    if (selectedItem.SelectedItemType == SelectedItemType.Value)
                    {
                        if (almacenDTOList.Count > 0)
                            WebHelper.SelectedListItemSetValue(model.AlmacenList, Convert.ToInt32(selectedItem.Value));
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            return PartialView(model);
        }
        [HttpPost("AgregarOrdenCompraProducto")]
        public ActionResult AgregarOrdenCompraProducto([FromBody] MerchandisingViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {

                OrdenCompraDTO ordenCompraDTO = ordenCompraProxy.GetById(new OrdenCompraDTO()
                {
                    OrdenCompraId =
                   model.MovimientoAlmacenProductoDTO.MovimientoAlmacenDTO.OrdenCompraId.Value
                });

                if (ordenCompraDTO.MonedaDTO.Codigo == MonedaConstante.DOLARES)
                {
                    TipoCambioDiaDTO tipoCambioDiaDTO = tipoCambioDiaProxy.SingleSearch(new TipoCambioDiaDTO()
                    {
                        TipoCambioDTO = new TipoCambioDTO() { Codigo = TipoCambioConstante.OFICIAL },
                        Fecha = model.MovimientoAlmacenProductoDTO.MovimientoAlmacenDTO.Fecha,
                        MonedaId = ordenCompraDTO.MonedaId
                    });

                    if (tipoCambioDiaDTO == null)
                        throw new Exception(SR.GetString("MG_tipo_cambio_oficial_dia_no_registrado",
                           model.MovimientoAlmacenProductoDTO.MovimientoAlmacenDTO.Fecha.ToShortDateString()));
                }

                AuditDTO(model.MovimientoAlmacenProductoDTO);

                movimientoAlmacenProductoProxy.AgregarOrdenCompraProducto(model.MovimientoAlmacenProductoDTO);

                resultResponse.Message = SR.GetString("IV_orden_compra_producto_multiple_seleccion_agregado");
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