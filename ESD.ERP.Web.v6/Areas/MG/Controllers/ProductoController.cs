using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service.DTO;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.MG.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.MG.Controllers
{
    [Area("MG")]
    [Route("[controller]")]
    [CheckAndHandleFileResult]
    public class ProductoController : AuthorizedController
    {
        private readonly ITipoProductoProxy tipoProductoProxy;
        private readonly IGrupoProductoProxy grupoProductoProxy;
        private readonly IClaseProductoProxy claseProductoProxy;
        private readonly IFamiliaProductoProxy familiaProductoProxy;
        private readonly IProductoProxy productoProxy;
        private readonly IUnidadMedidaProxy unidadMedidaProxy;
        private readonly ITipoIgvProxy tipoIgvProxy;
        private readonly IEstadoProductoProxy estadoProductoProxy;
        private readonly IEstadoComercialProductoProxy estadoComercialProductoProxy;
        private readonly IProductoPresentacionVentaProxy productoPresentacionVentaProxy;
        private readonly IProductoSubProcesoProduccionProxy productoSubProcesoProduccionProxy;
        private readonly IProductoDetalleProxy productoDetalleProxy;
        private readonly AppHelper appHelper;

        public ProductoController(
           ITipoProductoProxy tipoProductoProxy,
           IGrupoProductoProxy grupoProductoProxy,
           IClaseProductoProxy claseProductoProxy,
           IFamiliaProductoProxy familiaProductoProxy,
           IProductoProxy productoProxy,
           IUnidadMedidaProxy unidadMedidaProxy,
           ITipoIgvProxy tipoIgvProxy,
           IEstadoProductoProxy estadoProductoProxy,
           IEstadoComercialProductoProxy estadoComercialProductoProxy,
           IProductoPresentacionVentaProxy productoPresentacionVentaProxy,
           IProductoSubProcesoProduccionProxy productoSubProcesoProduccionProxy,
           IProductoDetalleProxy productoDetalleProxy,
           AppHelper appHelper
           )
        {
            this.tipoProductoProxy = tipoProductoProxy;
            this.grupoProductoProxy = grupoProductoProxy;
            this.claseProductoProxy = claseProductoProxy;
            this.familiaProductoProxy = familiaProductoProxy;
            this.productoProxy = productoProxy;
            this.unidadMedidaProxy = unidadMedidaProxy;
            this.tipoIgvProxy = tipoIgvProxy;
            this.estadoProductoProxy = estadoProductoProxy;
            this.estadoComercialProductoProxy = estadoComercialProductoProxy;
            this.productoPresentacionVentaProxy = productoPresentacionVentaProxy;
            this.productoSubProcesoProduccionProxy = productoSubProcesoProduccionProxy;
            this.productoDetalleProxy = productoDetalleProxy;
            this.appHelper = appHelper;
        }
        [Route("[action]")]
        public IActionResult Index()
        {
            ProductoViewModel productoViewModel = new();

            FamiliaProductoDTO familiaProductoDTO = new()
            {
                ClaseProductoDTO = new ClaseProductoDTO()
                {
                    GrupoProductoDTO = new GrupoProductoDTO
                    {
                        TipoProductoDTO = new TipoProductoDTO()
                    }
                }
            };
            productoViewModel.ProductoDTO = new ProductoDTO() { FamiliaProductoDTO = familiaProductoDTO };

            cargarListasCompartidas(productoViewModel);

            return PartialView(productoViewModel);
        }
        [HttpPost("ListarBandeja")]
        public ActionResult ListarBandeja(string jsonModel)
        {

            Logger.InfoStart();

            ProductoViewModel model = JsonSerializer.Deserialize<ProductoViewModel>(jsonModel);

            model.ProductoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
            model.ProductoDTO.OrderBy = "p.descripcion";

            if (model.ProductoDTO.FamiliaProductoDTO == null)
            {

                FamiliaProductoDTO familiaProductoDTO = new()
                {
                    ClaseProductoDTO = new ClaseProductoDTO()
                    {
                        GrupoProductoDTO = new GrupoProductoDTO
                        {
                            TipoProductoDTO = new TipoProductoDTO()
                        }
                    }
                };
                model.ProductoDTO.FamiliaProductoDTO = familiaProductoDTO;
            }

            model.ProductoDTO.FamiliaProductoDTO.ClaseProductoDTO.GrupoProductoDTO.TipoProductoDTO
               .EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID);

            PeriodoDTO periodoDTO = ObtenerPeriodoDTO(model.ViewModelState.CID);
            model.ProductoDTO.Periodo = periodoDTO.Codigo;

            try
            {
                List<ProductoDTO> productoDTOList = productoProxy.Search(model.ProductoDTO);

                int index = 1;
                var jsonData = new
                {
                    total = 1,
                    page = 1,
                    records = productoDTOList.Count,
                    rows = from f in productoDTOList.AsEnumerable()
                           select new
                           {
                               id = index++,
                               cell = new
                               {
                                   f.ProductoId,
                                   f.Codigo,
                                   f.Descripcion,
                                   f.CodigoReferencial,
                                   UnidadMedidaNombre = f.UnidadMedidaDTO.Nombre,
                                   f.TipoIgvId,
                                   TipoIgv = f.TipoIgvDTO.Codigo,
                                   FamiliaNombre = f.FamiliaProductoDTO.Nombre,
                                   f.FamiliaProductoDTO.ClaseProductoDTO.GrupoProductoDTO.EsCombo,
                                   f.PesoNeto,
                                   f.MasterPack,
                                   ColorNombre = f?.ColorDTO?.Nombre,
                                   EatadoProducto = f.EstadoProductoDTO.Nombre,
                                   EatadoComercialProducto = f.EstadoComercialProductoDTO.Nombre,
                                   FechaBaja = f.FechaBaja != null ? f.FechaBaja.Value.ToShortDateString() : string.Empty,
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
        [HttpGet("ListarGrupoProducto")]
        public JsonResult ListarGrupoProducto(int id)
        {

            Logger.InfoStart();
            ResultResponse resultResponse = new();

            try
            {

                List<GrupoProductoDTO> grupoProductoDTOList = obtenerListaGrupoProducto(id);
                resultResponse.Data = WebHelper.ToSelectListItem(grupoProductoDTOList, x => x.Id.ToString(), x => x.Nombre,
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
        private List<GrupoProductoDTO> obtenerListaGrupoProducto(int tipoProductoId)
        {
            GrupoProductoDTO grupoProductoDTO = new()
            {
                TipoProductoId = tipoProductoId,
                EsCombo = SiNoConstante.N,
                Estado = EstadoConstante.ACTIVO
            };

            PeriodoDTO periodoDTO = ObtenerPeriodoDTO();
            grupoProductoDTO.Periodo = periodoDTO.Codigo;

            List<GrupoProductoDTO> grupoProductoDTOList = grupoProductoProxy.Search(grupoProductoDTO);


            return grupoProductoDTOList;
        }
        [HttpGet("ListarClaseProducto")]
        public JsonResult ListarClaseProducto(int id)
        {

            Logger.InfoStart();
            ResultResponse resultResponse = new();

            try
            {

                List<ClaseProductoDTO> claseProductoDTOList = obtenerListaClaseProducto(id);
                resultResponse.Data = WebHelper.ToSelectListItem(claseProductoDTOList, x => x.Id.ToString(), x => x.Nombre,
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
        private List<ClaseProductoDTO> obtenerListaClaseProducto(int grupoProductoId)
        {
            ClaseProductoDTO claseProductoDTO = new()
            {
                GrupoProductoId = grupoProductoId,
                Estado = EstadoConstante.ACTIVO
            };

            PeriodoDTO periodoDTO = ObtenerPeriodoDTO();
            claseProductoDTO.Periodo = periodoDTO.Codigo;

            List<ClaseProductoDTO> claseProductoDTOList = claseProductoProxy.Search(claseProductoDTO);


            return claseProductoDTOList;
        }
        [HttpGet("ListarFamiliaProducto")]
        public JsonResult ListarFamiliaProducto(int id)
        {

            Logger.InfoStart();
            ResultResponse resultResponse = new();

            try
            {

                List<FamiliaProductoDTO> familiaProductoDTOList = obtenerListaFamiliaProducto(id);
                resultResponse.Data = WebHelper.ToSelectListItem(familiaProductoDTOList, x => x.Id.ToString(), x => x.Nombre,
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
        private List<FamiliaProductoDTO> obtenerListaFamiliaProducto(int claseProductoId)
        {
            FamiliaProductoDTO familiaProductoDTO = new()
            {
                ClaseProductoId = claseProductoId,
                Estado = EstadoConstante.ACTIVO
            };

            PeriodoDTO periodoDTO = ObtenerPeriodoDTO();
            familiaProductoDTO.Periodo = periodoDTO.Codigo;

            List<FamiliaProductoDTO> familiaProductoDTOList = familiaProductoProxy.Search(familiaProductoDTO);


            return familiaProductoDTOList;
        }
        [HttpPost("Crear")]
        public ActionResult Crear([FromBody] ProductoViewModel model)
        {
            Logger.InfoStart();

            ProductoViewModel productoViewModel = new()
            {
                EditAction = ViewModelEditAction.New,
                IsNew = true
            };

            try
            {
                cargarDataParaEditar(productoViewModel, 0);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw;
            }

            Logger.InfoFinish();

            return PartialView(PartialViewNameConstant.CRUD, productoViewModel);
        }
        [HttpPost("Editar")]
        public ActionResult Editar([FromBody] ProductoViewModel model)
        {

            Logger.InfoStart();

            ProductoViewModel productoViewModel = new()
            {
                EditAction = ViewModelEditAction.Edit
            };

            try
            {
                cargarDataParaEditar(productoViewModel, model.ProductoDTO.Id);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw;
            }

            Logger.InfoFinish();

            return PartialView(PartialViewNameConstant.CRUD, productoViewModel);
        }
        private void cargarDataParaEditar(ProductoViewModel model, int id)
        {

            ProductoDTO productoDTO = null;
            if (model.EditAction == ViewModelEditAction.New)
            {

                productoDTO = new ProductoDTO()
                {
                    FechaAlta = DateTime.Today
                };

                FamiliaProductoDTO familiaProductoDTO = new()
                {
                    ClaseProductoDTO = new ClaseProductoDTO()
                    {
                        GrupoProductoDTO = new GrupoProductoDTO
                        {
                            TipoProductoDTO = new TipoProductoDTO()
                        }
                    }
                };
                productoDTO.FamiliaProductoDTO = familiaProductoDTO;

                PeriodoDTO periodoDTO = ObtenerPeriodoDTO(model.ViewModelState.CID);
                productoDTO.Periodo = periodoDTO.Codigo;

                UnidadMedidaDTO unidadMedidaDTO = unidadMedidaProxy.ObtenerPorCodigo(new UnidadMedidaDTO()
                { Codigo = UnidadMedidaConstante.DEFAULT });
                productoDTO.UnidadMedidaId = unidadMedidaDTO.Id;

                TipoIgvDTO tipoIgvDTO = tipoIgvProxy.ObtenerPorCodigo(new TipoIgvDTO()
                { Codigo = TipoIgvConstante.DEFAULT });
                productoDTO.TipoIgvId = tipoIgvDTO.Id;

                EstadoProductoDTO estadoProductoDTO = estadoProductoProxy.ObtenerPorCodigo(new EstadoProductoDTO()
                { Codigo = EstadoProductoConstante.ACTIVO });
                productoDTO.EstadoProductoId = estadoProductoDTO.Id;

                EstadoComercialProductoDTO estadoComercialProductoDTO = estadoComercialProductoProxy.ObtenerPorCodigo(new EstadoComercialProductoDTO()
                { Codigo = EstadoComercialProductoConstante.STOCK });
                productoDTO.EstadoComercialProductoId = estadoComercialProductoDTO.Id;
            }
            else
            {
                productoDTO = productoProxy.GetById(new ProductoDTO() { ProductoId = id });

                productoDTO.EsPerecible = productoDTO.EsPerecible == SiNoConstante.N ? EstadoConstante.INACTIVO : EstadoConstante.ACTIVO;
                productoDTO.DetalleRequerido = productoDTO.DetalleRequerido == SiNoConstante.N ? EstadoConstante.INACTIVO : EstadoConstante.ACTIVO;
            }

            model.ProductoDTO = productoDTO;

            cargarListas(model);

            WebHelper.SelectedListItemSetValue(model.TipoProductoList, productoDTO.FamiliaProductoDTO.
               ClaseProductoDTO.GrupoProductoDTO.TipoProductoId);
            WebHelper.SelectedListItemSetValue(model.GrupoProductoList, productoDTO.FamiliaProductoDTO.
               ClaseProductoDTO.GrupoProductoId);
            WebHelper.SelectedListItemSetValue(model.ClaseProductoList, productoDTO.FamiliaProductoDTO.
               ClaseProductoId);
            WebHelper.SelectedListItemSetValue(model.FamiliaProductoList, productoDTO.FamiliaProductoId);
            WebHelper.SelectedListItemSetValue(model.UnidadMedidaList, productoDTO.UnidadMedidaId);
            WebHelper.SelectedListItemSetValue(model.TipoIgvList, productoDTO.TipoIgvId);
            WebHelper.SelectedListItemSetValue(model.EstadoProductoList, productoDTO.EstadoProductoId);
            WebHelper.SelectedListItemSetValue(model.EstadoComercialProductoList, productoDTO.EstadoComercialProductoId);
        }
        private void cargarListas(ProductoViewModel model)
        {
            cargarListasCompartidas(model);

            List<UnidadMedidaDTO> unidadMedidaDTOList = unidadMedidaProxy.
            Search(new UnidadMedidaDTO()
            {
                EmpresaId = ObtenerEmpresaId(),
                Estado = EstadoConstante.ACTIVO,
            });
            model.UnidadMedidaList = WebHelper.ToSelectListItem(unidadMedidaDTOList, x => x.Id.ToString(), x => x.Nombre, SelectListFirstOptionType.Select);
            model.UnidadMedidaString = string.Join(SeparadorConstante.PUNTO_COMA, unidadMedidaDTOList.Select(x => x.Id.ToString() + SeparadorConstante.DOS_PUNTOS + x.Nombre));

            List<TipoIgvDTO> tipoIgvDTOList = tipoIgvProxy.
            Search(new TipoIgvDTO()
            {
                Estado = EstadoConstante.ACTIVO,
            });
            model.TipoIgvList = WebHelper.ToSelectListItem(tipoIgvDTOList, x => x.Id.ToString(), x => x.Nombre, SelectListFirstOptionType.Select);

            List<EstadoProductoDTO> estadoProductoDTOList = estadoProductoProxy.
            Search(new EstadoProductoDTO()
            {
                Estado = EstadoConstante.ACTIVO,
            });
            model.EstadoProductoList = WebHelper.ToSelectListItem(estadoProductoDTOList, x => x.Id.ToString(), x => x.Nombre, SelectListFirstOptionType.Select);

            List<EstadoComercialProductoDTO> estadoComercialProductoDTOList = estadoComercialProductoProxy.
            Search(new EstadoComercialProductoDTO()
            {
                Estado = EstadoConstante.ACTIVO,
            });
            model.EstadoComercialProductoList = WebHelper.ToSelectListItem(estadoComercialProductoDTOList, x => x.Id.ToString(), x => x.Nombre, SelectListFirstOptionType.Select);
        }
        private void cargarListasCompartidas(ProductoViewModel model)
        {
            List<TipoProductoDTO> tipoProductoDTOList = tipoProductoProxy.
               Search(new TipoProductoDTO()
               {
                   EmpresaId = ObtenerEmpresaId(),
                   Estado = EstadoConstante.ACTIVO,
               });
            model.TipoProductoList = WebHelper.ToSelectListItem(tipoProductoDTOList, x => x.Id.ToString(), x => x.Nombre, SelectListFirstOptionType.Select);

            if (model.ProductoDTO.FamiliaProductoDTO.ClaseProductoDTO.GrupoProductoDTO.TipoProductoId == 0)
                model.GrupoProductoList = WebHelper.ToSelectListItem(SelectListFirstOptionType.Select);
            else
            {
                List<GrupoProductoDTO> grupoProductoDTOList = grupoProductoProxy.
                            Search(new GrupoProductoDTO()
                            {
                                TipoProductoId = model.ProductoDTO.FamiliaProductoDTO.ClaseProductoDTO.GrupoProductoDTO.TipoProductoId,
                                Estado = EstadoConstante.ACTIVO,
                            });
                model.GrupoProductoList = WebHelper.ToSelectListItem(grupoProductoDTOList, x => x.Id.ToString(), x => x.Nombre, SelectListFirstOptionType.Select);
            }

            if (model.ProductoDTO.FamiliaProductoDTO.ClaseProductoDTO.GrupoProductoId == 0)
                model.ClaseProductoList = WebHelper.ToSelectListItem(SelectListFirstOptionType.Select);
            else
            {
                List<ClaseProductoDTO> claseProductoDTOList = claseProductoProxy.
                            Search(new ClaseProductoDTO()
                            {
                                GrupoProductoId = model.ProductoDTO.FamiliaProductoDTO.ClaseProductoDTO.GrupoProductoId,
                                Estado = EstadoConstante.ACTIVO,
                            });
                model.ClaseProductoList = WebHelper.ToSelectListItem(claseProductoDTOList, x => x.Id.ToString(), x => x.Nombre, SelectListFirstOptionType.Select);
            }

            if (model.ProductoDTO.FamiliaProductoDTO.ClaseProductoId == 0)
                model.FamiliaProductoList = WebHelper.ToSelectListItem(SelectListFirstOptionType.Select);
            else
            {
                List<FamiliaProductoDTO> familiaProductoDTOList = familiaProductoProxy.
                            Search(new FamiliaProductoDTO()
                            {
                                ClaseProductoId = model.ProductoDTO.FamiliaProductoDTO.ClaseProductoId,
                                Estado = EstadoConstante.ACTIVO,
                            });
                model.FamiliaProductoList = WebHelper.ToSelectListItem(familiaProductoDTOList, x => x.Id.ToString(), x => x.Nombre, SelectListFirstOptionType.Select);
            }
        }
        [HttpPost("Eliminar")]
        public ActionResult Eliminar([FromBody] ProductoViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                model.ProductoDTO.DTOState = DTOState.Modified;
                model.ProductoDTO.FechaBaja = DateTime.Today;

                EstadoProductoDTO estadoProductoDTO = estadoProductoProxy.ObtenerPorCodigo(new EstadoProductoDTO() { Codigo = EstadoProductoConstante.INACTIVO });
                model.ProductoDTO.EstadoProductoId = estadoProductoDTO.Id;
                AuditDTO(model.ProductoDTO);

                productoProxy.Delete(model.ProductoDTO);

                resultResponse.Message = SR.GetString("MG_producto_anulado", model.ProductoDTO.Codigo);
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
        public FileContentResult ReportToExcel(string cid)
        {
            Dictionary<string, string> parameters = new();
            int empresaId = ObtenerEmpresaId(cid);
            parameters.Add("empresaId", empresaId.ToString());

            PeriodoDTO periodoDTO = ObtenerPeriodoDTO(cid);
            parameters.Add("periodo", periodoDTO.Codigo.ToString());

            parameters.Add("estado", EstadoConstante.ACTIVO);

            return appHelper.ReportToExcelData("sp_mgrpt_producto", parameters, "Productos");
        }

        [HttpGet("ReportToExcelAnulados")]
        public FileContentResult ReportToExcelAnulados(string cid)
        {
            Dictionary<string, string> parameters = new();
            int empresaId = ObtenerEmpresaId(cid);
            parameters.Add("empresaId", empresaId.ToString());

            PeriodoDTO periodoDTO = ObtenerPeriodoDTO(cid);
            parameters.Add("periodo", periodoDTO.Codigo.ToString());
 
            return appHelper.ReportToExcelData("sp_mgrpt_producto", parameters, "Productos");
        }

        [HttpPost("Grabar")]
        public ActionResult Grabar([FromBody] ProductoViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                ProductoDTO productoDTO = cargarProductoDTO(model);

                int newId = model.ProductoDTO.Id;
                string messageResourceKey = string.Empty;
                switch (model.EditAction)
                {
                    case ViewModelEditAction.New:
                        newId = productoProxy.Insert(productoDTO);
                        messageResourceKey = "MG_producto_agregado";
                        break;

                    case ViewModelEditAction.Edit:
                        productoProxy.Update(productoDTO);
                        messageResourceKey = "MG_producto_actualizado";
                        break;
                }

                productoDTO = productoProxy.GetById(new ProductoDTO() { ProductoId = newId });

                resultResponse.Message = SR.GetString(messageResourceKey, productoDTO.Codigo);
                resultResponse.Data = new
                {
                    productoDTO.Id,
                    productoDTO.Codigo
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
        private ProductoDTO cargarProductoDTO(ProductoViewModel model)
        {

            ProductoDTO productoDTO;
            try
            {

                if (model.EditAction == ViewModelEditAction.New)
                {
                    productoDTO = model.ProductoDTO;

                    PeriodoDTO periodoDTO = ObtenerPeriodoDTO(model.ViewModelState.CID);
                    productoDTO.Periodo = periodoDTO.Codigo;

                    productoDTO.FechaAlta = DateTime.Today;
                    productoDTO.TienePresentacionVenta = SiNoConstante.N;
                    productoDTO.EsOperacion = SiNoConstante.N;

                }
                else
                {
                    productoDTO = productoProxy.GetById(new ProductoDTO()
                    { ProductoId = model.ProductoDTO.Id });

                    productoDTO = model.ProductoDTO;
                }

                productoDTO = model.ProductoDTO;

                if (string.IsNullOrEmpty(productoDTO.CodigoReferencial))
                    productoDTO.CodigoReferencial = string.Empty;

                productoDTO.Estado = EstadoConstante.ACTIVO;

                AuditDTO(productoDTO);

            }
            catch (Exception)
            {
                throw;
            }

            return productoDTO;
        }
        [HttpPost("ListarPresentacionVenta")]
        public ActionResult ListarPresentacionVenta(string jsonModel)
        {

            Logger.InfoStart();

            ProductoViewModel model = JsonSerializer.Deserialize<ProductoViewModel>(jsonModel);

            model.ProductoPresentacionVentaDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
            model.ProductoPresentacionVentaDTO.OrderBy = "ppv.unidad_medida_id";


            try
            {
                List<ProductoPresentacionVentaDTO> productoPresentacionVentaDTOList = productoPresentacionVentaProxy.Search(model.ProductoPresentacionVentaDTO);

                int index = 1;
                var jsonData = new
                {
                    total = 1,
                    page = 1,
                    records = productoPresentacionVentaDTOList.Count,
                    rows = from f in productoPresentacionVentaDTOList.AsEnumerable()
                           select new
                           {
                               id = index++,
                               cell = new
                               {
                                   f.ProductoPresentacionVentaId,
                                   f.ProductoId,
                                   f.UnidadMedidaId,
                                   f.CantidadEquivalente,
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
        [HttpPost("GrabarPresentacionVenta")]
        public ActionResult GrabarPresentacionVenta([FromBody] ProductoViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                ProductoPresentacionVentaDTO productoPresentacionVentaDTO = cargarProductoPresentacionVentaDTO(model);

                int newId = model.ProductoPresentacionVentaDTO.Id;
                string messageResourceKey = string.Empty;
                switch (productoPresentacionVentaDTO.DTOState)
                {
                    case DTOState.Added:
                        newId = productoPresentacionVentaProxy.Insert(productoPresentacionVentaDTO);
                        messageResourceKey = "MG_producto_presentacion_agregada";
                        break;

                    case DTOState.Modified:
                        productoPresentacionVentaProxy.Update(productoPresentacionVentaDTO);
                        messageResourceKey = "MG_producto_presentacion_actualizada";
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
        private ProductoPresentacionVentaDTO cargarProductoPresentacionVentaDTO(ProductoViewModel model)
        {

            ProductoPresentacionVentaDTO productoPresentacionVentaDTO;
            try
            {

                productoPresentacionVentaDTO = model.ProductoPresentacionVentaDTO;

                productoPresentacionVentaDTO.Estado = EstadoConstante.ACTIVO;

                AuditDTO(productoPresentacionVentaDTO);

            }
            catch (Exception)
            {
                throw;
            }

            return productoPresentacionVentaDTO;
        }
        [HttpPost("EliminarPresentacionVenta")]
        public ActionResult EliminarPresentacionVenta([FromBody] ProductoViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                model.ProductoPresentacionVentaDTO.DTOState = DTOState.Modified;
                AuditDTO(model.ProductoPresentacionVentaDTO);

                productoPresentacionVentaProxy.Delete(model.ProductoPresentacionVentaDTO);

                resultResponse.Message = SR.GetString("MG_producto_presentacion_eliminada");
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
        public IActionResult ListPopup([FromBody] ProductoViewModel model)
        {

            if (model == null)
                model = new ProductoViewModel();

            if (model.ProductoDTO == null)
                model.ProductoDTO = new ProductoDTO();

            if (model.ProductoDTO.FamiliaProductoDTO == null)
                model.ProductoDTO.FamiliaProductoDTO = new FamiliaProductoDTO();

            if (model.ProductoDTO.FamiliaProductoDTO.ClaseProductoDTO == null)
                model.ProductoDTO.FamiliaProductoDTO.ClaseProductoDTO = new ClaseProductoDTO();

            if (model.ProductoDTO.FamiliaProductoDTO.ClaseProductoDTO.GrupoProductoDTO == null)
            {
                GrupoProductoDTO grupoProductoDTO = new()
                {
                    EsCombo = SiNoConstante.N
                };

                model.ProductoDTO.FamiliaProductoDTO.ClaseProductoDTO.GrupoProductoDTO = grupoProductoDTO;
            }

            int tipoProductoId = 0;

            if (model.ProductoDTO.FamiliaProductoDTO.ClaseProductoDTO.GrupoProductoDTO.TipoProductoDTO == null)
                model.ProductoDTO.FamiliaProductoDTO.ClaseProductoDTO.GrupoProductoDTO.TipoProductoDTO = new TipoProductoDTO();
            else
            {
                TipoProductoDTO tipoProductoDTO = model.ProductoDTO.FamiliaProductoDTO.ClaseProductoDTO.GrupoProductoDTO.TipoProductoDTO;
                if (!string.IsNullOrEmpty(tipoProductoDTO.Codigo))
                {
                    tipoProductoDTO = tipoProductoProxy.SingleSearch(new TipoProductoDTO()
                    {
                        EmpresaId = ObtenerEmpresaId(),
                        Codigo = tipoProductoDTO.Codigo
                    });
                    model.ProductoDTO.FamiliaProductoDTO.ClaseProductoDTO.GrupoProductoDTO.TipoProductoDTO = tipoProductoDTO;
                    model.ProductoDTO.FamiliaProductoDTO.ClaseProductoDTO.GrupoProductoDTO.TipoProductoId = tipoProductoDTO.Id;
                    tipoProductoId = tipoProductoDTO.Id;
                }
            }

            cargarListasCompartidas(model);

            WebHelper.SelectedListItemSetValue(model.TipoProductoList, tipoProductoId);

            ViewBag.ModalEditId = GetModalEditPanelId();
            return PartialView(model);
        }
        [HttpPost("ListarProductoSubProcesoProduccion")]
        public ActionResult ListarProductoSubProcesoProduccion(string jsonModel)
        {

            Logger.InfoStart();

            ProductoViewModel model = JsonSerializer.Deserialize<ProductoViewModel>(jsonModel);

            model.ProductoSubProcesoProduccionDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
            model.ProductoSubProcesoProduccionDTO.OrderBy = "pspp.orden";
            model.ProductoSubProcesoProduccionDTO.ProductoId = NumberUtil.WhenValueIsZero(model.ProductoSubProcesoProduccionDTO.ProductoId);

            try
            {
                List<ProductoSubProcesoProduccionDTO> productoSubProcesoProduccionDTOList = productoSubProcesoProduccionProxy.Search(model.ProductoSubProcesoProduccionDTO);

                int index = 1;
                var jsonData = new
                {
                    total = 1,
                    page = 1,
                    records = productoSubProcesoProduccionDTOList.Count,
                    rows = from f in productoSubProcesoProduccionDTOList.AsEnumerable()
                           select new
                           {
                               id = index++,
                               cell = new
                               {
                                   f.ProductoSubProcesoProduccionId,
                                   f.ProductoId,
                                   f.SubProcesoProduccionId,
                                   ProcesoNombre = f.SubProcesoProduccionDTO.ProcesoProduccionDTO.Nombre,
                                   f.SubProcesoProduccionDTO.Nombre,
                                   f.Orden,
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
        [HttpPost("GrabarProductoSubProcesoProduccion")]
        public ActionResult GrabarProductoSubProcesoProduccion([FromBody] ProductoViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                ProductoSubProcesoProduccionDTO productoSubProcesoProduccionDTO = cargarProductoSubProcesoProduccionDTO(model);

                int newId = model.ProductoSubProcesoProduccionDTO.Id;
                string messageResourceKey = string.Empty;
                switch (productoSubProcesoProduccionDTO.DTOState)
                {
                    case DTOState.Added:
                        newId = productoSubProcesoProduccionProxy.Insert(productoSubProcesoProduccionDTO);
                        messageResourceKey = "MG_producto_sub_proceso_agregado";
                        break;

                    case DTOState.Modified:
                        productoSubProcesoProduccionProxy.Update(productoSubProcesoProduccionDTO);
                        messageResourceKey = "MG_producto_sub_proceso_actualizado";
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
        private ProductoSubProcesoProduccionDTO cargarProductoSubProcesoProduccionDTO(ProductoViewModel model)
        {

            ProductoSubProcesoProduccionDTO productoSubProcesoProduccionDTO;
            try
            {

                productoSubProcesoProduccionDTO = model.ProductoSubProcesoProduccionDTO;

                productoSubProcesoProduccionDTO.Estado = EstadoConstante.ACTIVO;

                AuditDTO(productoSubProcesoProduccionDTO);

            }
            catch (Exception)
            {
                throw;
            }

            return productoSubProcesoProduccionDTO;
        }
        [HttpPost("EliminarProductoSubProcesoProduccion")]
        public ActionResult EliminarProductoSubProcesoProduccion([FromBody] ProductoViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                model.ProductoSubProcesoProduccionDTO.DTOState = DTOState.Modified;
                AuditDTO(model.ProductoSubProcesoProduccionDTO);

                productoSubProcesoProduccionProxy.Delete(model.ProductoSubProcesoProduccionDTO);

                resultResponse.Message = SR.GetString("MG_producto_sub_proceso_eliminado");
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
        [HttpPost("ListarDetalle")]
        public ActionResult ListarDetalle(string jsonModel)
        {

            Logger.InfoStart();

            ProductoViewModel model = JsonSerializer.Deserialize<ProductoViewModel>(jsonModel);

            model.ProductoDetalleDTO.PageSize = ObtenerMaximoRegistrosBusqueda();

            try
            {
                List<ProductoDetalleDTO> productoDetalleDTOList = productoDetalleProxy.Search(model.ProductoDetalleDTO);

                int index = 1;
                var jsonData = new
                {
                    total = 1,
                    page = 1,
                    records = productoDetalleDTOList.Count,
                    rows = from f in productoDetalleDTOList.AsEnumerable()
                           select new
                           {
                               id = index++,
                               cell = new
                               {
                                   f.ProductoDetalleId,
                                   f.ProductoId,
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
        }
        [HttpPost("GrabarDetalle")]
        public ActionResult GrabarDetalle([FromBody] ProductoViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                ProductoDetalleDTO productoDetalleDTO = cargarProductoDetalleDTO(model);

                int newId = model.ProductoDetalleDTO.Id;
                string messageResourceKey = string.Empty;
                switch (productoDetalleDTO.DTOState)
                {
                    case DTOState.Added:
                        newId = productoDetalleProxy.Insert(productoDetalleDTO);
                        messageResourceKey = "MG_producto_detalle_agregado";
                        break;

                    case DTOState.Modified:
                        productoDetalleProxy.Update(productoDetalleDTO);
                        messageResourceKey = "MG_producto_detalle_actualizado";
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
        private ProductoDetalleDTO cargarProductoDetalleDTO(ProductoViewModel model)
        {

            ProductoDetalleDTO productoDetalleDTO;
            try
            {

                productoDetalleDTO = model.ProductoDetalleDTO;

                productoDetalleDTO.Estado = EstadoConstante.ACTIVO;

                AuditDTO(productoDetalleDTO);

            }
            catch (Exception)
            {
                throw;
            }

            return productoDetalleDTO;
        }
        [HttpPost("EliminarDetalle")]
        public ActionResult EliminarDetalle([FromBody] ProductoViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                model.ProductoDetalleDTO.DTOState = DTOState.Modified;
                AuditDTO(model.ProductoDetalleDTO);

                productoDetalleProxy.Delete(model.ProductoDetalleDTO);

                resultResponse.Message = SR.GetString("MG_producto_detalle_eliminado");
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