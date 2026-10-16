using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service.DTO;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.PD.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.PD.Controllers
{
   [Area("PD")]
   [Route("[controller]")]
   public class DespachoController : AuthorizedController
   {
      private readonly IComprobanteProxy comprobanteProxy;
      private readonly IComprobanteProductoProxy comprobanteProductoProxy;
      private readonly IComprobanteProductoEntregaProxy comprobanteProductoEntregaProxy;
      private readonly IEstadoDespachoProxy estadoDespachoProxy;

      public DespachoController(
         IComprobanteProxy comprobanteProxy,
         IComprobanteProductoProxy comprobanteProductoProxy,
         IComprobanteProductoEntregaProxy comprobanteProductoEntregaProxy,
         IEstadoDespachoProxy estadoDespachoProxy
         )
      {
         this.comprobanteProxy = comprobanteProxy;
         this.comprobanteProductoProxy = comprobanteProductoProxy;
         this.comprobanteProductoEntregaProxy = comprobanteProductoEntregaProxy;
         this.estadoDespachoProxy = estadoDespachoProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         DespachoViewModel despachoViewModel = new()
         {
            MenuItemList = new List<MenuItem>
            {
               new MenuItem() { MenuItemType = MenuItemType.Edit },
               new MenuItem() { Key="cerrar", Content="Cerrar Despacho" }
            },
            ComprobanteDTO = new ComprobanteDTO()
            {
               FechaDesde = DateTime.Today.AddDays(-30),
               FechaHasta = DateTime.Today
            }
         };

         cargarListaEstadoDespacho(despachoViewModel);

         return PartialView(despachoViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         DespachoViewModel model = JsonSerializer.Deserialize<DespachoViewModel>(jsonModel);

         model.ComprobanteDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.ComprobanteDTO.TipoComprobanteId = 1;
         model.ComprobanteDTO.OrderBy = "c.fecha desc,c.numero_opera desc";

         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

         model.ComprobanteDTO.PeriodoEmpresaSedeDTO = new PeriodoEmpresaSedeDTO
         {
            EmpresaSedeId = periodoEmpresaSedeDTO.EmpresaSedeId,
            PeriodoEmpresaSedeId = periodoEmpresaSedeDTO.PeriodoEmpresaSedeId
         };
         model.ComprobanteDTO.EstadoComprobanteCodigoList = new List<string>() { EstadoComprobanteConstante.EMITIDO };

         try
         {
            List<ComprobanteDTO> comprobanteDTOList = comprobanteProxy.BuscarDespacho(model.ComprobanteDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = comprobanteDTOList.Count,
               rows = from f in comprobanteDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.ComprobanteId,
                            TipoDocumentoComercialNombre = f.TipoDocumentoComercialDTO.Abreviatura,
                            f.SerieNumero,
                            Fecha = f.Fecha.ToShortDateString(),
                            f.ProveedorDTO.RazonSocial,
                            f.ProveedorDTO.NumeroDocumentoIdentidad,
                            f.CotizacionPedido,
                            f.ColorLista,
                            EstadoDespacho = f?.EstadoDespachoDTO?.Codigo,
                            EstadoDespachoNombre = f?.EstadoDespachoDTO?.Nombre,
                            PersonalDespacho = obtenerPersonalDespacho(f),
                            FechaDespacho = f.FechaDespacho.HasValue ? string.Format(StringFormatConstant.DATE_TIME, f.FechaDespacho.Value) : string.Empty,
                            f.ObservacionDespacho,
                            f.EntregaDespacho,
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
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] DespachoViewModel model)
      {

         Logger.InfoStart();

         DespachoViewModel despachoViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(despachoViewModel, model.ComprobanteDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, despachoViewModel);
      }
      private void cargarDataParaEditar(DespachoViewModel model, int id)
      {

         model.ComprobanteDTO = comprobanteProxy.GetById(new ComprobanteDTO() { ComprobanteId = id });

         model.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { Key="cerrar", Content="Cerrar Despacho" },
            new MenuItem() { MenuItemType = MenuItemType.Exit }
         };

         model.MenuItemList2 = new List<MenuItem>
         {
            new MenuItem() { Key="seleccion", Content="Despachar Seleccionados" },
            new MenuItem() { Key="parcial", Content="Despacho Parcial" },
         };
      }
      private void cargarListaEstadoDespacho(DespachoViewModel model)
      {
         List<EstadoDespachoDTO> estadoDespachoDTOList = estadoDespachoProxy.Search(new EstadoDespachoDTO()
         {
            Estado = EstadoConstante.ACTIVO
         });
         model.EstadoDespachoList = WebHelper.ToSelectListItem(estadoDespachoDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);
      }
      [HttpPost("ListarComprobanteProducto")]
      public ActionResult ListarComprobanteProducto(string jsonModel)
      {

         Logger.InfoStart();

         DespachoViewModel model = JsonSerializer.Deserialize<DespachoViewModel>(jsonModel);

         model.ComprobanteProductoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.ComprobanteProductoDTO.OrderBy = "tp.tipo_producto,p.producto";
         model.ComprobanteProductoDTO.NullRegistroComboId = SiNoConstante.S;

         try
         {
            List<ComprobanteProductoDTO> comprobanteProductoDTOList = comprobanteProductoProxy.Search(model.ComprobanteProductoDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = comprobanteProductoDTOList.Count,
               rows = from f in comprobanteProductoDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.ComprobanteProductoId,
                            f.ProductoDTO.Codigo,
                            ProductoNombre = f.ProductoDTO.Descripcion,
                            f.ProductoDTO.FamiliaProductoDTO.ClaseProductoDTO.GrupoProductoDTO.EsCombo,
                            TipoProducto = f.ProductoDTO.FamiliaProductoDTO.ClaseProductoDTO.GrupoProductoDTO.TipoProductoDTO.Codigo,
                            UnidadMedidaNombre = f.UnidadMedidaDTO == null ? f.ProductoDTO.UnidadMedidaDTO.Abreviatura : f.UnidadMedidaDTO.Abreviatura,
                            f.Descripcion,
                            f.Cantidad,
                            f.CantidadPresentacion,
                            f.CantidadEquivalente,
                            CantidadVenta = f.CantidadPresentacion == decimal.Zero ? f.Cantidad : f.CantidadPresentacion,
                            f.CantidadEntregada,
                            FechaDespacho = f.FechaDespacho.HasValue ? string.Format(StringFormatConstant.DATE_TIME, f.FechaDespacho.Value) : string.Empty,
                            f.ObservacionDespacho,
                            f.PersonalId,
                            PersonalDespacho = obtenerPersonalDespacho(f),
                            f.EntregaDespacho,
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
      private string obtenerPersonalDespacho(ComprobanteDTO dto)
      {
         if (dto.PersonalId.HasValue)
            return dto?.PersonalDTO?.ProveedorDTO?.RazonSocial;

         if (dto.EstadoDespachoId.HasValue)
            return PersonalConstante.VARIOS_TEXTO;

         return string.Empty;
      }
      private string obtenerPersonalDespacho(ComprobanteProductoDTO dto)
      {
         if (dto.PersonalId.HasValue)
            return dto?.PersonalDTO?.ProveedorDTO?.RazonSocial;

         if (dto.CantidadEntregada > decimal.Zero)
            return PersonalConstante.VARIOS_TEXTO;

         return string.Empty;
      }
      [HttpPost("CerrarPopup")]
      public ActionResult CerrarPopup([FromBody] DespachoViewModel model)
      {

         try
         {
            if (model == null)
            {
               model = new DespachoViewModel
               {
                  ComprobanteDTO = new ComprobanteDTO()
               };
            }
            model.ModalEditId = GetModalEditPanelId();
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         return PartialView(model);
      }
      [HttpPost("Cerrar")]
      public ActionResult Cerrar([FromBody] DespachoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            AuditDTO(model.ComprobanteDTO);

            comprobanteProxy.CerrarDespacho(model.ComprobanteDTO);

            ComprobanteDTO comprobanteDTO = comprobanteProxy.GetById(new ComprobanteDTO() { ComprobanteId = model.ComprobanteDTO.Id });

            resultResponse.Message = SR.GetString("VT_comprobante_atendido");
            resultResponse.Data = new
            {
               EstadoDespacho = comprobanteDTO?.EstadoDespachoDTO?.Codigo,
               comprobanteDTO.ObservacionDespacho
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
      [HttpPost("AgregarDespacho")]
      public ActionResult AgregarDespacho([FromBody] DespachoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            AuditDTO(model.ComprobanteProductoDTO);

            comprobanteProductoProxy.AgregarComprobanteProductoEntrega(model.ComprobanteProductoDTO);

            resultResponse.Message = SR.GetString("PD_comprobante_producto_entrega_agregada");
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
      [HttpPost("CantidadParcialEditPopup")]
      public ActionResult CantidadParcialEditPopup([FromBody] DespachoViewModel model)
      {

         model.ModalEditId = GetModalEditPanelId();

         return PartialView(model);
      }
      [HttpPost("ListarComprobanteProductoEntrega")]
      public ActionResult ListarComprobanteProductoEntrega(string jsonModel)
      {

         Logger.InfoStart();

         DespachoViewModel model = JsonSerializer.Deserialize<DespachoViewModel>(jsonModel);

         model.ComprobanteProductoEntregaDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.ComprobanteProductoEntregaDTO.OrderBy = "cpe.fecha desc";

         try
         {
            List<ComprobanteProductoEntregaDTO> comprobanteProductoEntregaDTOList = comprobanteProductoEntregaProxy.Search(model.ComprobanteProductoEntregaDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = comprobanteProductoEntregaDTOList.Count,
               rows = from f in comprobanteProductoEntregaDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.ComprobanteProductoEntregaId,
                            f.Cantidad,
                            Fecha = string.Format(StringFormatConstant.DATE_TIME, f.Fecha),
                            f.EntregaDespacho,
                            f.Observacion,
                            f.PersonalId,
                            PersonalNombre = f.PersonalDTO.ProveedorDTO.RazonSocial,
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
      [HttpPost("GrabarComprobanteProductoEntrega")]
      public ActionResult GrabarComprobanteProductoEntrega([FromBody] DespachoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            ComprobanteProductoEntregaDTO comprobanteProductoEntregaDTO = cargarComprobanteProductoEntregaDTO(model);

            int newId = model.ComprobanteProductoEntregaDTO.Id;
            string messageResourceKey = string.Empty;
            switch (comprobanteProductoEntregaDTO.DTOState)
            {
               case DTOState.Added:
                  newId = comprobanteProductoEntregaProxy.Insert(comprobanteProductoEntregaDTO);
                  messageResourceKey = "PD_comprobante_producto_entrega_agregada";
                  break;

               case DTOState.Modified:
                  comprobanteProductoEntregaProxy.Update(comprobanteProductoEntregaDTO);
                  messageResourceKey = "PD_comprobante_producto_entrega_actualizada";
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
      private ComprobanteProductoEntregaDTO cargarComprobanteProductoEntregaDTO(DespachoViewModel model)
      {

         try
         {
            model.ComprobanteProductoEntregaDTO.Fecha = DateTime.Now;

            if (string.IsNullOrEmpty(model.ComprobanteProductoEntregaDTO.Observacion))
               model.ComprobanteProductoEntregaDTO.Observacion = string.Empty;

            AuditDTO(model.ComprobanteProductoEntregaDTO);
         }
         catch (Exception)
         {
            throw;
         }

         return model.ComprobanteProductoEntregaDTO;
      }
      [HttpPost("EliminarDetalle")]
      public ActionResult EliminarComprobanteProductoEntrega([FromBody] DespachoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.ComprobanteProductoEntregaDTO.DTOState = DTOState.Modified;
            AuditDTO(model.ComprobanteProductoEntregaDTO);

            comprobanteProductoEntregaProxy.Delete(model.ComprobanteProductoEntregaDTO);

            resultResponse.Message = SR.GetString("PD_comprobante_producto_entrega_eliminada");
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
      [HttpGet("ObtenerComprobanteEstadoDespacho")]
      public JsonResult ObtenerComprobanteEstadoDespacho(int id)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {
            ComprobanteDTO comprobanteDTO = comprobanteProxy.GetById(new ComprobanteDTO() { ComprobanteId = id });

            resultResponse.Data = new
            {
               EstadoDespacho = comprobanteDTO?.EstadoDespachoDTO?.Codigo,
               comprobanteDTO.ObservacionDespacho
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
