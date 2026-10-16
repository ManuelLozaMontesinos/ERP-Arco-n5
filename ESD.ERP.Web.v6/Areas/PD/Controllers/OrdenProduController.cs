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

namespace ESD.ERP.Web.Areas.PD.Controllers
{
   [Area("PD")]
   [Route("[controller]")]
   public class OrdenProduController : AuthorizedController
   {
      private readonly IOrdenProduccionProxy ordenProduccionProxy;
      private readonly IOrdenProduccionProductoProxy ordenProduccionProductoProxy;
      private readonly IOrdenProduccionSubProcesoProxy ordenProduccionSubProcesoProxy;
      private readonly IEstadoOrdenProduccionProxy estadoOrdenProduccionProxy;
      private readonly IProveedorProxy proveedorProxy;
      private readonly IAlmacenProxy almacenProxy;
      private readonly IVendedorProxy vendedorProxy;
      private readonly IMaquinaProxy maquinaProxy;
      private readonly AppHelper appHelper;

      public OrdenProduController(
         IOrdenProduccionProxy ordenProduccionProxy,
         IOrdenProduccionProductoProxy ordenProduccionProductoProxy,
         IEstadoOrdenProduccionProxy estadoOrdenProduccionProxy,
         IOrdenProduccionSubProcesoProxy ordenProduccionSubProcesoProxy,
         IProveedorProxy proveedorProxy,
         IAlmacenProxy almacenProxy,
         IVendedorProxy vendedorProxy,
         IMaquinaProxy maquinaProxy,
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
         this.appHelper = appHelper;
      }

      [Route("[action]")]
      public IActionResult Index()
      {
         OrdenProduViewModel ordenProduViewModel = new();

         
         ordenProduViewModel.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { MenuItemType = MenuItemType.Edit },
            new MenuItem() { MenuItemType = MenuItemType.Preview },
            new MenuItem() { MenuItemType = MenuItemType.Delete},
         };

         if (UsuarioEnRol(new List<string>() { RolConstante.SYS_ADMIN, RolConstante.TI, "00021" }))
         {
            ordenProduViewModel.MenuItemList.Add(new MenuItem() { Key = "planos", Content = "Cambiar Estado", Disabled = false});
         }
         if(UsuarioEnRol(new List<String>() { RolConstante.SYS_ADMIN, RolConstante.TI}))
         {
            ordenProduViewModel.MenuItemList.Add(new MenuItem() { Key = "verAnulado", Content = "verAnulado", Disabled = false });
         }

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
                ordenProduViewModel.Vendedor = vendedorDTO.ProveedorDTO.RazonSocial;
         }

            return PartialView(ordenProduViewModel);

      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         OrdenProduViewModel model = JsonSerializer.Deserialize<OrdenProduViewModel>(jsonModel);

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
                            f.CotizacionId, // Se añadio CotizaId para el metodo Delete
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
      public ActionResult Crear([FromBody] OrdenProduViewModel model)
      {
         Logger.InfoStart();

         OrdenProduViewModel ordenProduViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true,
         };

         try
         {
            cargarDataParaEditar(ordenProduViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, ordenProduViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] OrdenProduViewModel model)
      {

         Logger.InfoStart();

         OrdenProduViewModel ordenProduViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(ordenProduViewModel, model.OrdenProduccionDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, ordenProduViewModel);
      }
      private void cargarDataParaEditar(OrdenProduViewModel model, int id)
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

            EstadoOrdenProduccionDTO estadoOrdenProduccionDTO = estadoOrdenProduccionProxy.ObtenerPorCodigo(new EstadoOrdenProduccionDTO() { 
               Codigo = EstadoOrdenProduccionConstante.PENDIENTE});
            ordenProduccionDTO.EstadoOrdenProduccionId = estadoOrdenProduccionDTO.Id;
            ordenProduccionDTO.EstadoOrdenProduccionDTO = estadoOrdenProduccionDTO;
         }
         else
         {
            ordenProduccionDTO = ordenProduccionProxy.GetById(new OrdenProduccionDTO() { OrdenProduccionId  =  id });
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
      private void cargarListas(OrdenProduViewModel model)
      {
         cargarListasCompartidas(model);
      }
      private void cargarListasCompartidas(OrdenProduViewModel model)
      {
         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);


         List<MaquinaDTO> maquinaDTOList = maquinaProxy.
         Search(new MaquinaDTO()
         {
             /*AlmacenDTO = new AlmacenDTO() { EmpresaSedeId  = periodoEmpresaSedeDTO.EmpresaSedeId},*/
             Estado = EstadoConstante.ACTIVO
         });
         model.Maquina = string.Join(SeparadorConstante.PUNTO_COMA, maquinaDTOList.Select(x => x.Id.ToString() + SeparadorConstante.DOS_PUNTOS + x.Nombre));

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
      public ActionResult Grabar([FromBody] OrdenProduViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            OrdenProduccionDTO ordenProduccionDTO = cargarOrdenProduccionDTO(model);

            int newId = model.OrdenProduccionDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = ordenProduccionProxy.Insert(ordenProduccionDTO);
                  messageResourceKey = "PD_orden_produccion_agregada";
                  break;

               case ViewModelEditAction.Edit:
                  ordenProduccionProxy.Update(ordenProduccionDTO);
                  messageResourceKey = "PD_orden_produccion_actualizada";
                  break;
            }

            ordenProduccionDTO = ordenProduccionProxy.GetById(new OrdenProduccionDTO() { OrdenProduccionId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, ordenProduccionDTO.Codigo);
            resultResponse.Data = new
            {
               ordenProduccionDTO.Id,
               ordenProduccionDTO.Codigo 
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
      private OrdenProduccionDTO cargarOrdenProduccionDTO(OrdenProduViewModel model)
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
               ordenProduccionDTO = ordenProduccionProxy.GetById(new OrdenProduccionDTO() { OrdenProduccionId = model.OrdenProduccionDTO.Id});

               PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);
               model.OrdenProduccionDTO.PeriodoEmpresaSedeId = ordenProduccionDTO.PeriodoEmpresaSedeId;
               model.OrdenProduccionDTO.AlmacenId = ordenProduccionDTO.AlmacenId;
               model.OrdenProduccionDTO.EstadoOrdenProduccionDTO = ordenProduccionDTO.EstadoOrdenProduccionDTO;
               model.OrdenProduccionDTO.EstadoOrdenProduccionId = ordenProduccionDTO.EstadoOrdenProduccionId;  

               ordenProduccionDTO = model.OrdenProduccionDTO;
            }

            ordenProduccionDTO.Estado = EstadoConstante.ACTIVO;
            AuditDTO(ordenProduccionDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return ordenProduccionDTO;
      }
      [HttpPost("Eliminar")]
      public ActionResult Eliminar([FromBody] OrdenProduViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.OrdenProduccionDTO.DTOState = DTOState.Modified;

            AuditDTO(model.OrdenProduccionDTO);
            ordenProduccionProxy.Delete(model.OrdenProduccionDTO);

            OrdenProduccionDTO ordenProduccionDTO = ordenProduccionProxy.GetById(model.OrdenProduccionDTO);

            resultResponse.Message = SR.GetString("PD_orden_produccion_anulada", ordenProduccionDTO.Codigo);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            resultResponse.Message = e.Message;
            resultResponse.Success = false;
            resultResponse.Success = false;
         }

         Logger.InfoFinish();

         return Json(resultResponse, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
      }
      [HttpPost("Solicitar")]
      public ActionResult Solicitar([FromBody] OrdenProduViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            OrdenProduccionDTO ordenProduccionDTO = cargarOrdenProduccionDTO(model);

            int newId = model.OrdenProduccionDTO.Id;
            string messageResourceKey = "PD_orden_produccion_aprobacion_solicitada";

            //ordenProduccionProxy.SolicitarAutorizacion(ordenProduccionDTO);

            ordenProduccionDTO = ordenProduccionProxy.GetById(new OrdenProduccionDTO() { OrdenProduccionId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, ordenProduccionDTO.Codigo);
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
      [HttpPost("ListarOrdenProduccionProducto")]
      public ActionResult ListarOrdenProduccionProducto(string jsonModel)
      {

         Logger.InfoStart();

         OrdenProduViewModel model = JsonSerializer.Deserialize<OrdenProduViewModel>(jsonModel);

         model.OrdenProduccionProductoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.OrdenProduccionProductoDTO.OrderBy = "tp.tipo_producto,p.producto";

         try
         {
            List<OrdenProduccionProductoDTO> ordenProduccionProductoDTOList = ordenProduccionProductoProxy.Search(model.OrdenProduccionProductoDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = ordenProduccionProductoDTOList.Count,
               rows = from f in ordenProduccionProductoDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.OrdenProduccionProductoId,
                            f.OrdenProduccionId,
                            f.ProductoId,
                            f.ProductoDTO.Codigo,
                            ProductoNombre = f.ProductoDTO.Descripcion,
                            TipoProducto = f.ProductoDTO.FamiliaProductoDTO.ClaseProductoDTO.GrupoProductoDTO.TipoProductoDTO.Codigo,
                            f.ProductoDTO.UnidadMedidaDTO.UnidadMedidaId,
                            UnidadMedidaNombre = f.ProductoDTO.UnidadMedidaDTO.Abreviatura,
                            f.Descripcion,
                            f.Cantidad,
                            Insumo = f?.EstadoDespachoDTO?.Nombre,
                            f?.ObservacionDespacho,
                            Personal = f?.PersonalDTO?.ProveedorDTO?.RazonSocial,
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
      [HttpPost("GrabarOrdenProduccionProducto")]
      public ActionResult GrabarOrdenProduccionProducto([FromBody] OrdenProduViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            OrdenProduccionProductoDTO ordenProduccionProductoDTO = cargarOrdenProduccionProductoDTO(model);

            int newId = model.OrdenProduccionProductoDTO.Id;
            string messageResourceKey = string.Empty;
            switch (ordenProduccionProductoDTO.DTOState)
            {
               case DTOState.Added:
                  newId = ordenProduccionProductoProxy.Insert(ordenProduccionProductoDTO);
                  messageResourceKey = "IV_producto_agregado";
                  break;

               case DTOState.Modified:
                  ordenProduccionProductoProxy.Update(ordenProduccionProductoDTO);
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
      private OrdenProduccionProductoDTO cargarOrdenProduccionProductoDTO(OrdenProduViewModel model)
      {

         OrdenProduccionProductoDTO ordenProduccionProductoDTO;
         try
         {

            ordenProduccionProductoDTO = model.OrdenProduccionProductoDTO;

            AuditDTO(ordenProduccionProductoDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return ordenProduccionProductoDTO;
      }
      [HttpPost("EliminarOrdenProduccionProducto")]
      public ActionResult EliminarOrdenProduccionProducto([FromBody] OrdenProduViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.OrdenProduccionProductoDTO.DTOState = DTOState.Modified;
            AuditDTO(model.OrdenProduccionProductoDTO);

            OrdenProduccionProductoDTO ordenProduccionProductoDTO = ordenProduccionProductoProxy.GetById(model.OrdenProduccionProductoDTO);

            ordenProduccionProductoProxy.Delete(ordenProduccionProductoDTO);

            resultResponse.Message = SR.GetString("VT_producto_anulado");
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
      public FileContentResult ReportTo(string format, string cid, string ordenProduccionId)
      {
         Logger.InfoStart();

         byte[] reportData = obtenerReporteData(ordenProduccionId);

         OrdenProduccionDTO ordenProduccionDTO = ordenProduccionProxy.GetById(new OrdenProduccionDTO() { OrdenProduccionId = Convert.ToInt32(ordenProduccionId) });

         return appHelper.ReportToPdf(reportData, string.Concat("OP", ordenProduccionDTO.Codigo));
      }
      private byte[] obtenerReporteData(string ordenProduccionId)
      {
         Dictionary<string, string> parameters = new()
         {
            { "ordenProduccionId", ordenProduccionId }
         };

         byte[] reportData = appHelper.ExecuteReport("PD_OrdenProduccion", parameters, ReportFormatConstant.PDF);

         return reportData;
      }
      [HttpPost("ListarOrdenProduccionSubProceso")]
      public ActionResult ListarOrdenProduccionSubProceso(string jsonModel)
      {

         Logger.InfoStart();

         OrdenProduViewModel model = JsonSerializer.Deserialize<OrdenProduViewModel>(jsonModel);

         model.OrdenProduccionSubProcesoDTO.Estado = EstadoConstante.ACTIVO;
         model.OrdenProduccionSubProcesoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.OrdenProduccionSubProcesoDTO.OrderBy = "opsp.orden";

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
                            f.SubProcesoProduccionId,
                            SubProcesoProduccionNombre= string.Concat(f.SubProcesoProduccionDTO.ProcesoProduccionDTO.Nombre,SeparadorConstante.BARRA_OBLICUA,
                            f.SubProcesoProduccionDTO.Nombre),
                            f?.AlmacenId,
                            AlmacenNombre = f?.AlmacenDTO?.Nombre,
                            ObservacionSubProceso = f.Observacion,
                            FechaInicio = f.FechaInicio.HasValue ? string.Format(StringFormatConstant.DATE_TIME, f.FechaInicio.Value) : string.Empty,
                            FechaFin = f.FechaFin.HasValue ? string.Format(StringFormatConstant.DATE_TIME, f.FechaFin.Value) : string.Empty,
                            f?.MaquinaId,
                            f?.MaquinaDTO?.Nombre,
                            f?.MaquinaRealId,
                            MRNombre = f?.MaquinaRealDTO?.Nombre,
                            f.PersonalId,
                            f?.PersonalDTO?.ProveedorDTO?.RazonSocial,
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
      [HttpPost("GenararSubProcesosProduccion")]
      public ActionResult GenararSubProcesosProduccion([FromBody] OrdenProduViewModel model)
      {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                int newId = model.OrdenProduccionSubProcesoDTO.Id;
                string messageResourseKey = string.Empty;
                
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
        [HttpPost("CambiarEstadoEditPopup")]
        public IActionResult CambiarEstadoEditPopup([FromBody] OrdenProduViewModel model)
        {

            OrdenProduccionDTO ordenProduccionDTO = ordenProduccionProxy.GetById(new OrdenProduccionDTO() { OrdenProduccionId = model.OrdenProduccionDTO.OrdenProduccionId});
            
            List<EstadoOrdenProduccionDTO> estadoOrdenProduccionDTOList = estadoOrdenProduccionProxy.Search(new EstadoOrdenProduccionDTO()
            {
                Codigo = EstadoOrdenProduccionConstante.CULMINADA,
                Estado = EstadoConstante.ACTIVO
            });
            
            model.EstadoOrdenProduccionMelList = WebHelper.ToSelectListItem(estadoOrdenProduccionDTOList, x => x.Id.ToString(), x => x.Nombre,
                SelectListFirstOptionType.Select);
            model.EstadoOrdenProduccionMdfList = WebHelper.ToSelectListItem(estadoOrdenProduccionDTOList, x => x.Id.ToString(), x => x.Nombre,
                SelectListFirstOptionType.Select);
            

            return PartialView(model);
        }
        [HttpPost("CambiarEstado")]
        public ActionResult CambiarEstado([FromBody] OrdenProduViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                AuditDTO(model.OrdenProduccionDTO);
                 
                ordenProduccionProxy.CambiarEstado(model.OrdenProduccionDTO);

                resultResponse.Message = SR.GetString("VT_cotizacion_estado_actualizado");
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
      public ActionResult GrabarOrdenProduccionSubProceso([FromBody] OrdenProduViewModel model)
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
      private OrdenProduccionSubProcesoDTO cargarOrdenProduccionSubProcesoDTO(OrdenProduViewModel model)
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

               //model.OrdenProduccionSubProcesoDTO.Observacion = ordenProduccionSubProcesoDTO.Observacion;

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