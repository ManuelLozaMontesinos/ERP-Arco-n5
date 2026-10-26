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
    public class FamiliaProductoController : AuthorizedController
    {
        private readonly ITipoProductoProxy tipoProductoProxy;
        private readonly IGrupoProductoProxy grupoProductoProxy;
        private readonly IClaseProductoProxy claseProductoProxy;
        private readonly IFamiliaProductoProxy familiaProductoProxy;
        private readonly ITipoDetraccionProxy tipoDetraccionProxy;

        public FamiliaProductoController(
           ITipoProductoProxy tipoProductoProxy,
           IGrupoProductoProxy grupoProductoProxy,
           IClaseProductoProxy claseProductoProxy,
           IFamiliaProductoProxy familiaProductoProxy,
           ITipoDetraccionProxy tipoDetraccionProxy
           )
        {
            this.tipoProductoProxy = tipoProductoProxy;
            this.grupoProductoProxy = grupoProductoProxy;
            this.claseProductoProxy = claseProductoProxy;
            this.familiaProductoProxy = familiaProductoProxy;
            this.tipoDetraccionProxy = tipoDetraccionProxy;
        }
        [Route("[action]")]
        public IActionResult Index()
        {
            FamiliaProductoViewModel familiaProductoViewModel = new();

            cargarListasCompartidas(familiaProductoViewModel);

            return PartialView(familiaProductoViewModel);
        }
        [HttpPost("ListarBandeja")]
        public ActionResult ListarBandeja(string jsonModel)
        {

            Logger.InfoStart();

            FamiliaProductoViewModel model = JsonSerializer.Deserialize<FamiliaProductoViewModel>(jsonModel);

            model.GrupoProductoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
            model.GrupoProductoDTO.OrderBy = "gp.grupo_producto";

            if (model.GrupoProductoDTO.TipoProductoDTO == null)
                model.GrupoProductoDTO.TipoProductoDTO = new TipoProductoDTO();

            model.GrupoProductoDTO.TipoProductoDTO.EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID);

            PeriodoDTO periodoDTO = ObtenerPeriodoDTO(model.ViewModelState.CID);
            model.GrupoProductoDTO.Periodo = periodoDTO.Codigo;

            try
            {
                List<GrupoProductoDTO> grupoProductoDTOList = grupoProductoProxy.Search(model.GrupoProductoDTO);

                int index = 1;
                var jsonData = new
                {
                    total = 1,
                    page = 1,
                    records = grupoProductoDTOList.Count,
                    rows = from f in grupoProductoDTOList.AsEnumerable()
                           select new
                           {
                               id = index++,
                               cell = new
                               {
                                   f.GrupoProductoId,
                                   TipoProducto = f.TipoProductoDTO.Codigo,
                                   f.Codigo,
                                   f.Nombre,
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
        [HttpPost("Crear")]
        public ActionResult Crear([FromBody] FamiliaProductoViewModel model)
        {
            Logger.InfoStart();

            FamiliaProductoViewModel familiaProductoViewModel = new()
            {
                EditAction = ViewModelEditAction.New,
                IsNew = true
            };

            try
            {
                cargarDataParaEditar(familiaProductoViewModel, 0);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw;
            }

            Logger.InfoFinish();

            return PartialView(PartialViewNameConstant.CRUD, familiaProductoViewModel);
        }
        [HttpPost("Editar")]
        public ActionResult Editar([FromBody] FamiliaProductoViewModel model)
        {

            Logger.InfoStart();

            FamiliaProductoViewModel familiaProductoViewModel = new()
            {
                EditAction = ViewModelEditAction.Edit
            };

            try
            {
                cargarDataParaEditar(familiaProductoViewModel, model.GrupoProductoDTO.Id);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw;
            }

            Logger.InfoFinish();

            return PartialView(PartialViewNameConstant.CRUD, familiaProductoViewModel);
        }
        private void cargarDataParaEditar(FamiliaProductoViewModel model, int id)
        {

            GrupoProductoDTO grupoProductoDTO = null;
            if (model.EditAction == ViewModelEditAction.New)
            {

                grupoProductoDTO = new GrupoProductoDTO()
                {
                    FechaAlta = DateTime.Today
                };
                PeriodoDTO periodoDTO = ObtenerPeriodoDTO(model.ViewModelState.CID);
                grupoProductoDTO.Periodo = periodoDTO.Codigo;
            }
            else
            {
                grupoProductoDTO = grupoProductoProxy.GetById(new GrupoProductoDTO() { GrupoProductoId = id });
            }

            grupoProductoDTO.UsarIncrementoFraccion = grupoProductoDTO.UsarIncrementoFraccion == SiNoConstante.S ? EstadoConstante.ACTIVO : EstadoConstante.INACTIVO;
            grupoProductoDTO.EsCombo = grupoProductoDTO.EsCombo == SiNoConstante.S ? EstadoConstante.ACTIVO : EstadoConstante.INACTIVO;
            grupoProductoDTO.EsInsumo = grupoProductoDTO.EsInsumo == SiNoConstante.S ? EstadoConstante.ACTIVO : EstadoConstante.INACTIVO;

            model.GrupoProductoDTO = grupoProductoDTO;

            cargarListas(model);

            WebHelper.SelectedListItemSetValue(model.TipoProductoList, grupoProductoDTO.TipoProductoId);
        }
        private void cargarListas(FamiliaProductoViewModel model)
        {
            cargarListasCompartidas(model);

            List<TipoDetraccionDTO> tipoDetraccionDTOList = tipoDetraccionProxy.Search(new TipoDetraccionDTO() { Estado = EstadoConstante.ACTIVO });
            model.TipoDetraccionString = string.Join(SeparadorConstante.PUNTO_COMA, tipoDetraccionDTOList.Select(x => x.Id.ToString() + SeparadorConstante.DOS_PUNTOS + x.Nombre));
        }
        private void cargarListasCompartidas(FamiliaProductoViewModel model)
        {
            List<TipoProductoDTO> tipoproductoDTOList = tipoProductoProxy.
               Search(new TipoProductoDTO()
               {
                   EmpresaId = ObtenerEmpresaId(),
                   Estado = EstadoConstante.ACTIVO,
               });
            model.TipoProductoList = WebHelper.ToSelectListItem(tipoproductoDTOList, x => x.Id.ToString(), x => x.Nombre, SelectListFirstOptionType.Select);
        }
        [HttpPost("Grabar")]
        public ActionResult Grabar([FromBody] FamiliaProductoViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                GrupoProductoDTO grupoProductoDTO = cargarGrupoProductoDTO(model);

                int newId = model.GrupoProductoDTO.Id;
                string messageResourceKey = string.Empty;
                switch (model.EditAction)
                {
                    case ViewModelEditAction.New:
                        newId = grupoProductoProxy.Insert(grupoProductoDTO);
                        messageResourceKey = "MG_grupo_producto_agregado";
                        break;

                    case ViewModelEditAction.Edit:
                        grupoProductoProxy.Update(grupoProductoDTO);
                        messageResourceKey = "MG_grupo_producto_actualizado";
                        break;
                }

                grupoProductoDTO = grupoProductoProxy.GetById(new GrupoProductoDTO() { GrupoProductoId = newId });

                resultResponse.Message = SR.GetString(messageResourceKey, grupoProductoDTO.Codigo);
                resultResponse.Data = new
                {
                    grupoProductoDTO.Id,
                    grupoProductoDTO.Codigo
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
        private GrupoProductoDTO cargarGrupoProductoDTO(FamiliaProductoViewModel model)
        {

            GrupoProductoDTO grupoProductoDTO;
            try
            {

                if (model.EditAction == ViewModelEditAction.New)
                {
                    grupoProductoDTO = model.GrupoProductoDTO;

                    TipoProductoDTO tipoProductoDTO = tipoProductoProxy.GetById(new TipoProductoDTO() { TipoProductoId = model.GrupoProductoDTO.TipoProductoId });
                    grupoProductoDTO.TipoProductoDTO = tipoProductoDTO;

                    PeriodoDTO periodoDTO = ObtenerPeriodoDTO(model.ViewModelState.CID);
                    grupoProductoDTO.Periodo = periodoDTO.Codigo;

                    grupoProductoDTO.FechaAlta = DateTime.Today;
                }
                else
                {
                    grupoProductoDTO = grupoProductoProxy.GetById(new GrupoProductoDTO()
                    { GrupoProductoId = model.GrupoProductoDTO.Id });

                    grupoProductoDTO = model.GrupoProductoDTO;
                }

                grupoProductoDTO = model.GrupoProductoDTO;

                if (grupoProductoDTO.TipoProductoId == 2)
                {
                    grupoProductoDTO.EsCombo = SiNoConstante.N;
                    grupoProductoDTO.EsInsumo = SiNoConstante.N;
                    grupoProductoDTO.UsarIncrementoFraccion = SiNoConstante.N;
                }

                grupoProductoDTO.Estado = EstadoConstante.ACTIVO;

                AuditDTO(grupoProductoDTO);

            }
            catch (Exception)
            {
                throw;
            }

            return grupoProductoDTO;
        }
        [HttpPost("Eliminar")]
        public ActionResult Eliminar([FromBody] FamiliaProductoViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                model.GrupoProductoDTO.DTOState = DTOState.Modified;
                model.GrupoProductoDTO.FechaBaja = DateTime.Today;
                AuditDTO(model.GrupoProductoDTO);

                grupoProductoProxy.Delete(model.GrupoProductoDTO);

                resultResponse.Message = SR.GetString("MG_grupo_producto_anulado");
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
        [HttpPost("ListarClaseProducto")]
        public ActionResult ListarClaseProducto(string jsonModel)
        {

            Logger.InfoStart();

            FamiliaProductoViewModel model = JsonSerializer.Deserialize<FamiliaProductoViewModel>(jsonModel);

            model.ClaseProductoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
            model.ClaseProductoDTO.OrderBy = "cp.fecha_alta desc";

            PeriodoDTO periodoDTO = ObtenerPeriodoDTO(model.ViewModelState.CID);
            model.ClaseProductoDTO.Periodo = periodoDTO.Codigo;

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
                                   f.GrupoProductoId,
                                   f.Codigo,
                                   f.Nombre,
                                   f.TipoDetraccionId,
                                   f.TipoDetraccionDTO.Tasa,
                                   FechaBaja = f.FechaBaja != null ? f.FechaBaja.Value.ToShortDateString() : string.Empty,
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
        [HttpGet("ObtenerTipoDetraccion")]
        public JsonResult ObtenerTipoDetraccion(int id)
        {

            Logger.InfoStart();
            ResultResponse resultResponse = new();

            try
            {
                TipoDetraccionDTO tipoDetraccionDTO = tipoDetraccionProxy.
                   GetById(new TipoDetraccionDTO() { TipoDetraccionId = id });

                resultResponse.Data = new
                {
                    tipoDetraccionDTO.Codigo,
                    tipoDetraccionDTO.Tasa
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
        [HttpPost("GrabarClaseProducto")]
        public ActionResult GrabarClaseProducto([FromBody] FamiliaProductoViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                ClaseProductoDTO claseProductoDTO = cargarClaseProductoDTO(model);

                int newId = model.ClaseProductoDTO.Id;
                string messageResourceKey = string.Empty;
                switch (claseProductoDTO.DTOState)
                {
                    case DTOState.Added:
                        newId = claseProductoProxy.Insert(claseProductoDTO);
                        messageResourceKey = "MG_clase_producto_agregada";
                        break;

                    case DTOState.Modified:
                        claseProductoProxy.Update(claseProductoDTO);
                        messageResourceKey = "MG_clase_producto_actualizada";
                        break;
                }

                claseProductoDTO = claseProductoProxy.GetById(new ClaseProductoDTO() { ClaseProductoId = newId });

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
        private ClaseProductoDTO cargarClaseProductoDTO(FamiliaProductoViewModel model)
        {

            ClaseProductoDTO claseProductoDTO;
            try
            {

                claseProductoDTO = model.ClaseProductoDTO;

                claseProductoDTO.Estado = EstadoConstante.ACTIVO;
                claseProductoDTO.FechaAlta = DateTime.Today;

                PeriodoDTO periodoDTO = ObtenerPeriodoDTO(model.ViewModelState.CID);
                claseProductoDTO.Periodo = periodoDTO.Codigo;

                AuditDTO(claseProductoDTO);

            }
            catch (Exception)
            {
                throw;
            }

            return claseProductoDTO;
        }
        [HttpPost("EliminarClaseProducto")]
        public ActionResult EliminarClaseProducto([FromBody] FamiliaProductoViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                model.ClaseProductoDTO.DTOState = DTOState.Modified;
                model.ClaseProductoDTO.FechaBaja = DateTime.Today;
                AuditDTO(model.ClaseProductoDTO);

                claseProductoProxy.Delete(model.ClaseProductoDTO);

                resultResponse.Message = SR.GetString("MG_clase_producto_anulada");
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
        [HttpPost("ListarFamiliaProducto")]
        public ActionResult ListarFamiliaProducto(string jsonModel)
        {

            Logger.InfoStart();

            FamiliaProductoViewModel model = JsonSerializer.Deserialize<FamiliaProductoViewModel>(jsonModel);

            model.FamiliaProductoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
            model.FamiliaProductoDTO.OrderBy = "fp.fecha_alta desc";

            PeriodoDTO periodoDTO = ObtenerPeriodoDTO(model.ViewModelState.CID);
            model.FamiliaProductoDTO.Periodo = periodoDTO.Codigo;

            try
            {
                List<FamiliaProductoDTO> claseProductoDTOList = familiaProductoProxy.Search(model.FamiliaProductoDTO);

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
                                   f.FamiliaProductoId,
                                   f.ClaseProductoId,
                                   f.Codigo,
                                   f.Nombre,
                                   FechaBaja = f.FechaBaja != null ? f.FechaBaja.Value.ToShortDateString() : string.Empty,
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
        [HttpPost("GrabarFamiliaProducto")]
        public ActionResult GrabarFamiliaProducto([FromBody] FamiliaProductoViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                FamiliaProductoDTO familiaProductoDTO = cargarFamiliaProductoDTO(model);

                int newId = model.FamiliaProductoDTO.Id;
                string messageResourceKey = string.Empty;
                switch (familiaProductoDTO.DTOState)
                {
                    case DTOState.Added:
                        newId = familiaProductoProxy.Insert(familiaProductoDTO);
                        messageResourceKey = "MG_familia_producto_agregada";
                        break;

                    case DTOState.Modified:
                        familiaProductoProxy.Update(familiaProductoDTO);
                        messageResourceKey = "MG_familia_producto_actualizada";
                        break;
                }

                familiaProductoDTO = familiaProductoProxy.GetById(new FamiliaProductoDTO() { FamiliaProductoId = newId });

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
        private FamiliaProductoDTO cargarFamiliaProductoDTO(FamiliaProductoViewModel model)
        {

            FamiliaProductoDTO familiaProductoDTO;
            try
            {

                familiaProductoDTO = model.FamiliaProductoDTO;

                familiaProductoDTO.Estado = EstadoConstante.ACTIVO;
                familiaProductoDTO.FechaAlta = DateTime.Today;

                PeriodoDTO periodoDTO = ObtenerPeriodoDTO(model.ViewModelState.CID);
                familiaProductoDTO.Periodo = periodoDTO.Codigo;

                AuditDTO(familiaProductoDTO);

            }
            catch (Exception)
            {
                throw;
            }

            return familiaProductoDTO;
        }
        [HttpPost("EliminarFamiliaProducto")]
        public ActionResult EliminarFamiliaProducto([FromBody] FamiliaProductoViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                model.FamiliaProductoDTO.DTOState = DTOState.Modified;
                model.FamiliaProductoDTO.FechaBaja = DateTime.Today;
                AuditDTO(model.FamiliaProductoDTO);

                familiaProductoProxy.Delete(model.FamiliaProductoDTO);

                resultResponse.Message = SR.GetString("MG_familia_producto_anulada");
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
