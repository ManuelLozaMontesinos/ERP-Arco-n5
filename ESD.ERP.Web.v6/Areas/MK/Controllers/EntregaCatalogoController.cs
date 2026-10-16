using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service.DTO;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.IV.Models;
using ESD.ERP.Web.Areas.VT.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using ESD.ERP.WebApiProxy.Impl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.VT.Controllers
{
    [Area("VT")]
    [Route("[controller]")]
    public class EntregaCatalogoController : AuthorizedController
    {
        private readonly IEstadoCotizacionProxy estadoCotizacionProxy;
        private readonly ICotizacionProxy cotizacionProxy;
        private readonly ICotizacionProductoProxy cotizacionProductoProxy;
        private readonly IMonedaProxy monedaProxy;
        private readonly IListaPrecioProxy listaPrecioProxy;
        private readonly IVendedorProxy vendedorProxy;
        private readonly IImpuestoTasaProxy impuestoTasaProxy;
        private readonly ITipoAfectacionIgvProxy tipoAfectacionIgvProxy;
        private readonly IParametroSistemaProxy parametroSistemaProxy;
        private readonly ITipoCondicionPagoProxy tipoCondicionPagoProxy;
        private readonly ITipoAutorizacionModuloProxy tipoAutorizacionModuloProxy;
        private readonly ICotizacionAutorizacionProxy cotizacionAutorizacionProxy;
        private readonly IAnticipoClienteProxy anticipoClienteProxy;
        private readonly IMesContableProxy mesContableProxy;
        private readonly IMovimientoAlmacenSaldoProxy movimientoAlmacenSaldoProxy;
        private readonly IListaPrecioProveedorProxy listaPrecioProveedorProxy;
        private readonly IGrupoProveedorItemProxy grupoProveedorItemProxy;
        private readonly IComboProductoProxy comboProductoProxy;
        private readonly IOrdenProduccionProxy ordenProduccionProxy;
        private readonly IOrdenProduccionSubProcesoProxy ordenProduccionSubProcesoProxy;
        private readonly ITipoDescuentoProxy tipoDescuentoProxy;
        private readonly IProductoDetalleProxy productoDetalleProxy;
        private readonly AppHelper appHelper;

        public EntregaCatalogoController(
           IEstadoCotizacionProxy estadoCotizacionProxy,
           ICotizacionProxy cotizacionProxy,
           ICotizacionProductoProxy cotizacionProductoProxy,
           IMonedaProxy monedaProxy,
           IListaPrecioProxy listaPrecioProxy,
           IVendedorProxy vendedorProxy,
           IImpuestoTasaProxy impuestoTasaProxy,
           ITipoAfectacionIgvProxy tipoAfectacionIgvProxy,
           IParametroSistemaProxy parametroSistemaProxy,
           ITipoCondicionPagoProxy tipoCondicionPagoProxy,
           ITipoAutorizacionModuloProxy tipoAutorizacionModuloProxy,
           ICotizacionAutorizacionProxy cotizacionAutorizacionProxy,
           IAnticipoClienteProxy anticipoClienteProxy,
           IMesContableProxy mesContableProxy,
           IMovimientoAlmacenSaldoProxy movimientoAlmacenSaldoProxy,
           IListaPrecioProveedorProxy listaPrecioProveedorProxy,
           IGrupoProveedorItemProxy grupoProveedorItemProxy,
           IComboProductoProxy comboProductoProxy,
           IOrdenProduccionProxy ordenProduccionProxy,
           IOrdenProduccionSubProcesoProxy ordenProduccionSubProcesoProxy,
           ITipoDescuentoProxy tipoDescuentoProxy,
           IProductoDetalleProxy productoDetalleProxy,
           AppHelper appHelper
           )
        {
            this.estadoCotizacionProxy = estadoCotizacionProxy;
            this.cotizacionProxy = cotizacionProxy;
            this.cotizacionProductoProxy = cotizacionProductoProxy;
            this.monedaProxy = monedaProxy;
            this.listaPrecioProxy = listaPrecioProxy;
            this.vendedorProxy = vendedorProxy;
            this.impuestoTasaProxy = impuestoTasaProxy;
            this.tipoAfectacionIgvProxy = tipoAfectacionIgvProxy;
            this.parametroSistemaProxy = parametroSistemaProxy;
            this.tipoCondicionPagoProxy = tipoCondicionPagoProxy;
            this.tipoAutorizacionModuloProxy = tipoAutorizacionModuloProxy;
            this.cotizacionAutorizacionProxy = cotizacionAutorizacionProxy;
            this.anticipoClienteProxy = anticipoClienteProxy;
            this.mesContableProxy = mesContableProxy;
            this.movimientoAlmacenSaldoProxy = movimientoAlmacenSaldoProxy;
            this.listaPrecioProveedorProxy = listaPrecioProveedorProxy;
            this.grupoProveedorItemProxy = grupoProveedorItemProxy;
            this.comboProductoProxy = comboProductoProxy;
            this.ordenProduccionProxy = ordenProduccionProxy;
            this.ordenProduccionSubProcesoProxy = ordenProduccionSubProcesoProxy;
            this.tipoDescuentoProxy = tipoDescuentoProxy;
            this.productoDetalleProxy = productoDetalleProxy;
            this.appHelper = appHelper;
        }

       /*[Route("[action]")]
        public IActionResult Index()
        {
            EntregaCatalogoViewModel entregaCatalogoViewModel = new();

            cargarListas(entregaCatalogoViewModel);

            return PartialView(entregaCatalogoViewModel);
        }
        [HttpPost("ListarBandeja")]
        public ActionResult ListarBandeja(string jsonModel)
        {

            Logger.InfoStart();

            EntregaCatalogoViewModel model = JsonSerializer.Deserialize<EntregaCatalogoViewModel>(jsonModel);

            model.EntregaCatalogoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
            model.EntregaCatalogoDTO.OrderBy = "mr.fecha desc,mr.mov_reclasi";

            model.EntregaCatalogoDTO.PeriodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(model.ViewModelState.CID);

            try
            {
                List<EntregaCatalogoDTO> entregaCatalogoDTOList = movimientoMarketingProxy.Search(model.EntregaCatalogoDTO);

                int index = 1;
                var jsonData = new
                {
                    total = 1,
                    page = 1,
                    records = entregaCatalogoDTOList.Count,
                    rows = from f in entregaCatalogoDTOList.AsEnumerable()
                           select new
                           {
                               id = index++,
                               cell = new
                               {
                                   f.MovimientoReclasificacionId,
                                   f.Codigo,
                                   Fecha = f.Fecha.ToShortDateString(),
                                   f.MovimientoAlmacenId,
                                   MovimientoAlmacen = f.MovimientoAlmacenDTO.Codigo,
                                   TransaccionAlmacenNombre = f.MovimientoAlmacenDTO.TransaccionAlmacenDTO.Nombre,
                                   TipoMovimientoAlmacen = f.MovimientoAlmacenDTO.TipoMovimientoAlmacenDTO.Codigo,
                                   AlmacenNombre = f.MovimientoAlmacenDTO.AlmacenDTO.Nombre,
                                   f.MovimientoAlmacenDTO.DocumentoSerieNumeroReferencial,
                                   f.MovimientoAlmacenDTO.ProveedorDTO?.RazonSocial,
                                   EstadoMovimientoReclasificacion = f.EstadoMovimientoReclasificacionDTO.Codigo,
                                   EstadoMovimientoReclasificacionNombre = f.EstadoMovimientoReclasificacionDTO.Nombre,
                                   f.Estado,
                                   UsuarioAutoriza = f.UsuarioAutorizaDTO?.NombreCompleto
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
        public ActionResult Crear([FromBody] EntregaCatalogoViewModel model)
        {
            Logger.InfoStart();

            EntregaCatalogoViewModel entregaCatalogoViewModel = new()
            {
                EditAction = ViewModelEditAction.New,
                IsNew = true,
            };

            try
            {
                cargarDataParaEditar(entregaCatalogoViewModel, 0);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw;
            }

            Logger.InfoFinish();

            return PartialView(PartialViewNameConstant.CRUD, entregaCatalogoViewModel);
        }
        [HttpPost("Editar")]
        public ActionResult Editar([FromBody] EntregaCatalogoViewModel model)
        {

            Logger.InfoStart();

            EntregaCatalogoViewModel entregaCatalogoViewModel = new()
            {
                EditAction = ViewModelEditAction.Edit
            };

            try
            {
                cargarDataParaEditar(entregaCatalogoViewModel, model.EntregaCatalogoDTO.Id);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw;
            }

            Logger.InfoFinish();

            return PartialView(PartialViewNameConstant.CRUD, entregaCatalogoViewModel);
        }
        private void cargarDataParaEditar(EntregaCatalogoViewModel model, int id)
        {

            EntregaCatalogoDTO entregaCatalogoDTO;
            if (model.EditAction == ViewModelEditAction.New)
            {
                entregaCatalogoDTO = new entregaCatalogoDTO()
                {
                    Fecha = DateTime.Today,
                    MovimientoAlmacenDTO = new MovimientoAlmacenDTO(),
                    EstadoMovimientoReclasificacionDTO = new EstadoMovimientoReclasificacionDTO()
                };
            }
            else
            {
                movimientoReclasificacionDTO = movimientoReclasificacionProxy.GetById(new MovimientoReclasificacionDTO() { MovimientoReclasificacionId = id });
            }

            model.EntregaCatalogoDTO = entregaCatalogoDTO;

            cargarListas(model);

            WebHelper.SelectedListItemSetValue(model.AlmacenList, movimientoReclasificacionDTO.MovimientoAlmacenDTO.AlmacenId);

            model.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { MenuItemType = MenuItemType.Save },
            new MenuItem() { Key = "Solicitar", Content = "Solicitar Aprobacion" },
            new MenuItem() { MenuItemType = MenuItemType.Exit }
         };

        }
        private void cargarListas(EntregaCatalogoViewModel model)
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

        }
        [HttpPost("Grabar")]
        public ActionResult Grabar([FromBody] EntregaCatalogoViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                MovimientoReclasificacionDTO movimientoReclasificacionDTO = cargarMovimientoReclasificacionDTO(model);

                int newId = model.EntregaCatalogoDTO.Id;
                string messageResourceKey = string.Empty;
                switch (model.EditAction)
                {
                    case ViewModelEditAction.New:
                        newId = movimientoReclasificacionProxy.Insert(movimientoReclasificacionDTO);
                        messageResourceKey = "IV_movimiento_reclasificacion_agregado";
                        break;

                    case ViewModelEditAction.Edit:
                        movimientoReclasificacionProxy.Update(movimientoReclasificacionDTO);
                        messageResourceKey = "IV_movimiento_reclasificacion_actualizado";
                        break;
                }

                movimientoReclasificacionDTO = movimientoReclasificacionProxy.GetById(new MovimientoReclasificacionDTO() { MovimientoReclasificacionId = newId });

                resultResponse.Message = SR.GetString(messageResourceKey, movimientoReclasificacionDTO.Codigo);
                resultResponse.Data = new
                {
                    movimientoReclasificacionDTO.Id,
                    movimientoReclasificacionDTO.Codigo,
                    movimientoReclasificacionDTO.MesContableId
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

        private MovimientoReclasificacionDTO cargarMovimientoReclasificacionDTO(EntregaCatalogoViewModel model)
        {

            MovimientoReclasificacionDTO movimientoReclasificacionDTO;
            try
            {

                movimientoReclasificacionDTO = model.MovimientoReclasificacionDTO;

                if (model.EditAction == ViewModelEditAction.New)
                {
                    PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

                    movimientoReclasificacionDTO.PeriodoEmpresaSedeDTO = periodoEmpresaSedeDTO;
                    movimientoReclasificacionDTO.PeriodoEmpresaSedeId = periodoEmpresaSedeDTO.PeriodoEmpresaSedeId;

                    string mesContable = string.Format(StringFormatConstant.DATE_MONTH, movimientoReclasificacionDTO.Fecha);

                    MesContableDTO mesContableDTO = mesContableProxy.ObtenerPorCodigo(new MesContableDTO() { Codigo = mesContable });
                    movimientoReclasificacionDTO.MesContableId = mesContableDTO.Id;

                    EstadoMovimientoReclasificacionDTO estadoMovimientoReclasificacionDTO = estadoMovimientoReclasificacionProxy.ObtenerPorCodigo(new EstadoMovimientoReclasificacionDTO() { Codigo = EstadoMovimientoReclasificacionConstante.MOVIMIENTO });
                    movimientoReclasificacionDTO.EstadoMovimientoReclasificacionId = estadoMovimientoReclasificacionDTO.Id;

                }
                else
                {
                    movimientoReclasificacionDTO = movimientoReclasificacionProxy.GetById(new MovimientoReclasificacionDTO()
                    {
                        MovimientoReclasificacionId
                       = movimientoReclasificacionDTO.Id
                    });

                    model.MovimientoReclasificacionDTO.PeriodoEmpresaSedeId = movimientoReclasificacionDTO.PeriodoEmpresaSedeId;
                    movimientoReclasificacionDTO = model.MovimientoReclasificacionDTO;
                }

                if (string.IsNullOrEmpty(movimientoReclasificacionDTO.Observacion))
                    movimientoReclasificacionDTO.Observacion = string.Empty;

                movimientoReclasificacionDTO.Estado = EstadoConstante.ACTIVO;
                AuditDTO(movimientoReclasificacionDTO);

            }
            catch (Exception)
            {
                throw;
            }

            return movimientoReclasificacionDTO;
        }
        [HttpPost("Eliminar")]
        public ActionResult Eliminar([FromBody] EntregaCatalogoViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                model.MovimientoReclasificacionDTO.DTOState = DTOState.Modified;

                AuditDTO(model.MovimientoReclasificacionDTO);

                movimientoReclasificacionProxy.Delete(model.MovimientoReclasificacionDTO);

                resultResponse.Message = SR.GetString("IV_movimiento_reclasificacion_anulado", model.MovimientoReclasificacionDTO.Codigo);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                resultResponse.Message = e.Message;
                resultResponse.Success = false;
            }

            Logger.InfoFinish();

            return Json(resultResponse, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }*/

    }

}