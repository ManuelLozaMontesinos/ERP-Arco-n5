using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service;
using ESD.Core.Service.DTO;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.IV.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using ESD.ERP.WebApiProxy.Impl;
using Microsoft.AspNetCore.Mvc;
using NLog.Targets;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.IV.Controllers
{
    [Area("IV")]
    [Route("[controller]")]
    public class ReclasItemAutoController : AuthorizedController
    {
        private readonly IAlmacenProxy almacenProxy;
        private readonly IMovimientoReclasificacionProxy movimientoReclasificacionProxy;
        private readonly IMesContableProxy mesContableProxy;
        private readonly IEstadoMovimientoReclasificacionProxy estadoMovimientoReclasificacionProxy;
        private readonly IMovimientoAlmacenProductoProxy movimientoAlmacenProductoProxy;
        private readonly AppHelper appHelper;
        public ReclasItemAutoController(
           IAlmacenProxy almacenProxy,
           IMovimientoReclasificacionProxy movimientoReclasificacionProxy,
           IMesContableProxy mesContableProxy,
           IEstadoMovimientoReclasificacionProxy estadoMovimientoReclasificacionProxy,
           IMovimientoAlmacenProductoProxy movimientoAlmacenProductoProxy,
           AppHelper appHelper
           )
        {
            this.almacenProxy = almacenProxy;
            this.movimientoReclasificacionProxy = movimientoReclasificacionProxy;
            this.mesContableProxy = mesContableProxy;
            this.estadoMovimientoReclasificacionProxy = estadoMovimientoReclasificacionProxy;
            this.movimientoAlmacenProductoProxy = movimientoAlmacenProductoProxy;
            this.appHelper = appHelper;
        }
        [Route("[action]")]
        public IActionResult Index()
        {
            ReclasItemAutoViewModel reclasItemAutoViewModel = new();

            cargarListas(reclasItemAutoViewModel);

            return PartialView(reclasItemAutoViewModel);
        }
        [HttpPost("ListarBandeja")]
        public ActionResult ListarBandeja(string jsonModel)
        {

            Logger.InfoStart();

            ReclasItemAutoViewModel model = JsonSerializer.Deserialize<ReclasItemAutoViewModel>(jsonModel);

            model.MovimientoReclasificacionDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
            model.MovimientoReclasificacionDTO.OrderBy = "mr.fecha desc,mr.mov_reclasi";
            model.MovimientoReclasificacionDTO.PeriodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(model .ViewModelState.CID);
            model.MovimientoReclasificacionDTO.EstadoMovimientoReclasificacionDTO.Codigo = EstadoMovimientoReclasificacionConstante.PENDIENTE;

            try
            {
                List<MovimientoReclasificacionDTO> movimientoReclasificacionDTOList = movimientoReclasificacionProxy.Search(model.MovimientoReclasificacionDTO);

                int index = 1;
                var jsonData = new
                {
                    total = 1,
                    page = 1,
                    records = movimientoReclasificacionDTOList.Count,
                    rows = from f in movimientoReclasificacionDTOList.AsEnumerable()
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
                                   f.EstadoMovimientoReclasificacionId,
                                   EstadoMovimientoReclasificacion = f.EstadoMovimientoReclasificacionDTO.Codigo,
                                   EstadoMovimientoReclasificacionNombre = f.EstadoMovimientoReclasificacionDTO.Nombre,
                                   f.Estado,
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
        public ActionResult Crear([FromBody] ReclasItemAutoViewModel model)
        {
            Logger.InfoStart();

            ReclasItemAutoViewModel reclasItemAutoViewModel = new()
            {
                EditAction = ViewModelEditAction.New,
                IsNew = true,
            };

            try
            {
                cargarDataParaEditar(reclasItemAutoViewModel, 0);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw;
            }

            Logger.InfoFinish();

            return PartialView(PartialViewNameConstant.CRUD, reclasItemAutoViewModel);
        }

        [HttpPost("Grabar")]
        public ActionResult Grabar([FromBody] ReclasItemAutoViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                MovimientoReclasificacionDTO movimientoReclasificacionDTO = cargarMovimientoReclasificacionDTO(model);

                int newId = model.MovimientoReclasificacionDTO.Id;
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

        private MovimientoReclasificacionDTO cargarMovimientoReclasificacionDTO(ReclasItemAutoViewModel model)
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

                    EstadoMovimientoReclasificacionDTO estadoMovimientoReclasificacionDTO = estadoMovimientoReclasificacionProxy.ObtenerPorCodigo(new EstadoMovimientoReclasificacionDTO() { Codigo = EstadoMovimientoReclasificacionConstante.PENDIENTE });
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

        [HttpPost("Editar")]
        public ActionResult Editar([FromBody] ReclasItemAutoViewModel model)
        {

            Logger.InfoStart();

            ReclasItemAutoViewModel reclasItemAutoViewModel = new()
            {
                EditAction = ViewModelEditAction.Edit
            };

            try
            {
                cargarDataParaEditar(reclasItemAutoViewModel, model.MovimientoReclasificacionDTO.Id);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw;
            }

            Logger.InfoFinish();

            return PartialView(PartialViewNameConstant.CRUD, reclasItemAutoViewModel);
        }
        private void cargarDataParaEditar(ReclasItemAutoViewModel model, int id)
        {

            MovimientoReclasificacionDTO movimientoReclasificacionDTO;
            if (model.EditAction == ViewModelEditAction.New)
            {
                movimientoReclasificacionDTO = new MovimientoReclasificacionDTO()
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

            model.MovimientoReclasificacionDTO = movimientoReclasificacionDTO;

            cargarListas(model);

            WebHelper.SelectedListItemSetValue(model.AlmacenList, movimientoReclasificacionDTO.MovimientoAlmacenDTO.AlmacenId);

            model.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { Key="aprobar", Content="Aprobar" },
            new MenuItem() { Key="desaprobar", Content="Desaprobar" },
            new MenuItem() { MenuItemType = MenuItemType.Exit}
         };

        }
        private void cargarListas(ReclasItemAutoViewModel model)
        {
            PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

            List<AlmacenDTO> almacenDTOList = almacenProxy.Search(new AlmacenDTO()
            {
                EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId },
                EmpresaSedeId = periodoEmpresaSedeDTO.EmpresaSedeId,
                Estado = EstadoConstante.ACTIVO,

            });
            model.AlmacenList = WebHelper.ToSelectListItem(almacenDTOList, x => x.Id.ToString(), x => x.Nombre,
               SelectListFirstOptionType.Select);
        }

       [HttpPost("Aprobar")]

        public ActionResult Aprobar([FromBody] ReclasItemAutoViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                MovimientoReclasificacionDTO movimientoReclasificacionDTO = cargarMovimientoReclasificacionDTO(model);

                int newId = model.MovimientoReclasificacionDTO.Id;
                string messageResourceKey = "IV_movimiento_reclasificacion_aprobado";

                movimientoReclasificacionProxy.Aprobar(movimientoReclasificacionDTO);
                movimientoReclasificacionDTO = movimientoReclasificacionProxy.GetById(new MovimientoReclasificacionDTO() { MovimientoReclasificacionId = newId });

                resultResponse.Message = SR.GetString(messageResourceKey, movimientoReclasificacionDTO.Codigo);
                resultResponse.Data = new
                {
                    EstadoMovimientoReclasificacion = movimientoReclasificacionDTO.EstadoMovimientoReclasificacionDTO.Codigo
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

        [HttpPost("Desaprobar")]
        public ActionResult Desaprobar([FromBody] ReclasItemAutoViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();
            try
            {
                MovimientoReclasificacionDTO movimientoReclasificacionDTO = cargarMovimientoReclasificacionDTO(model);
                int newId = model.MovimientoReclasificacionDTO.Id;
                string messageResourceKey = "IV_movimiento_reclasificacion_desaprobado";

                movimientoReclasificacionProxy.Desaprobar(movimientoReclasificacionDTO);
                movimientoReclasificacionDTO = movimientoReclasificacionProxy.GetById(new MovimientoReclasificacionDTO() { MovimientoReclasificacionId = newId });

                resultResponse.Message = SR.GetString(messageResourceKey, movimientoReclasificacionDTO.Codigo);
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
