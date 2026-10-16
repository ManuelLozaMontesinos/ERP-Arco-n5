using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service.DTO;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.VT.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using ESD.ERP.WebApiProxy.Impl;
using Microsoft.AspNetCore.Mvc;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace ESD.ERP.Web.Areas.VT.Controllers
{
    [Area("VT")]
    [Route("CortesGratis")]
    public class CortesGratisController : AuthorizedController
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
        private readonly IVendedorSerieProxy vendedorSerieProxy;
        private readonly IListaPrecioProveedorProxy listaPrecioProveedorProxy;
        private readonly IGrupoProveedorItemProxy grupoProveedorItemProxy;
        private readonly IMesContableProxy mesContableProxy;
        private readonly ITipoCambioDiaProxy tipoCambioDiaProxy;
        private readonly IMovimientoAlmacenSaldoProxy movimientoAlmacenSaldoProxy;
        private readonly ITipoDescuentoProxy tipoDescuentoProxy;
        private readonly IProductoDetalleProxy productoDetalleProxy;
        private readonly IProductoProxy productoProxy;
        private readonly IClaseProductoProxy claseProductoProxy;
        private readonly IFamiliaProductoProxy familiaProductoProxy;
        private readonly ICortesGratisProxy cortesGratisProxy;
        private readonly AppHelper appHelper;

        public CortesGratisController(
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
           IVendedorSerieProxy vendedorSerieProxy,
           IListaPrecioProveedorProxy listaPrecioProveedorProxy,
           IGrupoProveedorItemProxy grupoProveedorItemProxy,
           IMesContableProxy mesContableProxy,
           ITipoCambioDiaProxy tipoCambioDiaProxy,
           IMovimientoAlmacenSaldoProxy movimientoAlmacenSaldoProxy,
           ITipoDescuentoProxy tipoDescuentoProxy,
           IProductoDetalleProxy productoDetalleProxy,
           IClaseProductoProxy claseProductoProxy,
           IFamiliaProductoProxy familiaProductoProxy,
           IProductoProxy productoProxy,
           ICortesGratisProxy cortesGratisProxy,
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
            this.vendedorSerieProxy = vendedorSerieProxy;
            this.listaPrecioProveedorProxy = listaPrecioProveedorProxy;
            this.grupoProveedorItemProxy = grupoProveedorItemProxy;
            this.mesContableProxy = mesContableProxy;
            this.tipoCambioDiaProxy = tipoCambioDiaProxy;
            this.movimientoAlmacenSaldoProxy = movimientoAlmacenSaldoProxy;
            this.tipoDescuentoProxy = tipoDescuentoProxy;
            this.productoDetalleProxy = productoDetalleProxy;
            this.productoProxy = productoProxy;
            this.claseProductoProxy = claseProductoProxy;
            this.familiaProductoProxy = familiaProductoProxy;
            this.cortesGratisProxy = cortesGratisProxy;
            this.appHelper = appHelper;
        }

        [Route("[action]")]
        public IActionResult Index()
        {
            CortesGratisViewModel cortesGratisViewModel = new();

            cargarListasCompartidas(cortesGratisViewModel);

            MenuBarTagHelper menuBarTagHelper = new();
            cortesGratisViewModel.MenuItemList = menuBarTagHelper.CreateMenuStyleList(MenuItemStyle.Exit);

            return PartialView(cortesGratisViewModel);
        }
        private void cargarListasCompartidas(CortesGratisViewModel model)
        {
            PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);
            model.EmpresaSedeDTO = periodoEmpresaSedeDTO.EmpresaSedeDTO;
        }
        private void cargarListas(CortesGratisViewModel model)
        {
            List<ClaseProductoDTO> claseProductoDTOList = claseProductoProxy.Search(new ClaseProductoDTO()
            {
                GrupoProductoId = 5,
                ClaseProductoId = model.ClaseProductoDTO.Id,
                Codigo = model.ClaseProductoDTO.Codigo,
                Estado = EstadoConstante.ACTIVO
            });
            model.ClaseProductoList = WebHelper.ToSelectListItem(claseProductoDTOList, x => x.Id.ToString(), x => x.Nombre, SelectListFirstOptionType.Select);
            List<FamiliaProductoDTO> familiaProductoListDTO = familiaProductoProxy.Search(new FamiliaProductoDTO()
            {
                ClaseProductoId = model.ClaseProductoDTO.Id,
                Estado = EstadoConstante.ACTIVO,
            });
            model.FamiliaProductoList = WebHelper.ToSelectListItem(familiaProductoListDTO, x => x.Id.ToString(), x => x.Nombre, SelectListFirstOptionType.Select);

            cargarListasCompartidas(model);

        }
        private void cargarDataParaEditar(CortesGratisViewModel model, int id)
        {

            ClaseProductoDTO claseProductoDTO = null;
            if (model.EditAction == ViewModelEditAction.New)
            {
                claseProductoDTO = claseProductoProxy.GetById(new ClaseProductoDTO() { ClaseProductoId = id });
            }
            else
            {
                claseProductoDTO = claseProductoProxy.GetById(new ClaseProductoDTO() { ClaseProductoId = id });
            }
            model.ClaseProductoDTO = claseProductoDTO;

            MenuBarTagHelper menuBarTagHelper = new();
            model.MenuProductoList = menuBarTagHelper.CreateMenuStyleList(MenuItemStyle.EditAndSaveCancel);

            cargarListas(model);    
        } 

        [HttpPost("ListarBandejaClase")]
        public ActionResult ListarBandejaClase(string jsonModel)
        {

            Logger.InfoStart();

            CortesGratisViewModel model = JsonSerializer.Deserialize<CortesGratisViewModel>(jsonModel);

            model.ClaseProductoDTO.GrupoProductoId = 5;
            model.ClaseProductoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
            model.ClaseProductoDTO.OrderBy = "cp.nombre";

            try
            {
                List<ClaseProductoDTO> claseProductoDTOList = claseProductoProxy.Search(model.ClaseProductoDTO);

                int index = 1;
                var jsonData = new
                {
                    total = 1,
                    page = 1,
                    records = claseProductoDTOList.Count,
                    rows = from f in claseProductoDTOList.AsEnumerable()
                           select new
                           {
                               id = index++,
                               cell = new
                               {
                                   f.ClaseProductoId,
                                   f.Nombre,
                                   f.Codigo,
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
        [HttpPost("ListarProducto")]
        public ActionResult ListarProducto(string jsonModel)
        {
            Logger.InfoStart();

            CortesGratisViewModel model = JsonSerializer.Deserialize<CortesGratisViewModel>(jsonModel);
            
            model.CortesGratisDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
            model.CortesGratisDTO.OrderBy = "p.descripcion";
            
            try
            {
                    List<CortesGratisDTO> cortesGratisDTOList = cortesGratisProxy.Search(model.CortesGratisDTO);

                int index = 1;
                var jsonData = new
                {
                    total = 1,
                    page = 1,
                    records = cortesGratisDTOList.Count,
                    rows = from f in cortesGratisDTOList.AsEnumerable()
                           select new
                           {
                               id = index++,
                               cell = new
                               {
                                   f.CortesGratisId,
                                   f.ProductoId,
                                   f.ProductoDTO.Descripcion,
                                   Codigo = f.ProductoDTO.Codigo,
                                   CortesGratis = f.Cantidad,
                                   f.Estado,
                                   f.DTOState
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
        [HttpPost("Editar")]
        public ActionResult Editar([FromBody] CortesGratisViewModel model)
        {

            Logger.InfoStart();

            CortesGratisViewModel cortesGratisViewModel = new()
            {
                EditAction = ViewModelEditAction.Edit
            };

            try
            {
                cargarDataParaEditar(cortesGratisViewModel, model.ClaseProductoDTO.Id);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw;
            }

            Logger.InfoFinish();

            return PartialView(PartialViewNameConstant.CRUD, cortesGratisViewModel);
        }

        [HttpPost("GuardarCortes")]
        public ActionResult GuardarCortes([FromBody] CortesGratisViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {

                CortesGratisDTO cortesGratisDTO = model.CortesGratisDTO;
                cortesGratisDTO.Estado = EstadoConstante.ACTIVO;

                cortesGratisProxy.Update(cortesGratisDTO);

                resultResponse.Message = "Cortes gratis actualizado";
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
