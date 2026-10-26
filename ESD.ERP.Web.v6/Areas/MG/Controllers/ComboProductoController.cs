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
   public class ComboProductoController : AuthorizedController
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
      private readonly IComboProductoProxy comboProductoProxy;

      public ComboProductoController(
         ITipoProductoProxy tipoProductoProxy,
         IGrupoProductoProxy grupoProductoProxy,
         IClaseProductoProxy claseProductoProxy,
         IFamiliaProductoProxy familiaProductoProxy,
         IProductoProxy productoProxy,
         IUnidadMedidaProxy unidadMedidaProxy,
         ITipoIgvProxy tipoIgvProxy,
         IEstadoProductoProxy estadoProductoProxy,
         IEstadoComercialProductoProxy estadoComercialProductoProxy,
         IComboProductoProxy comboProductoProxy
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
         this.comboProductoProxy = comboProductoProxy;

      }
      [Route("[action]")]
      public IActionResult Index()
      {
         ComboProductoViewModel comboProductoViewModel = new();

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
         comboProductoViewModel.ProductoDTO = new ProductoDTO() { FamiliaProductoDTO = familiaProductoDTO };

         cargarListasCompartidas(comboProductoViewModel);

         return PartialView(comboProductoViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         ComboProductoViewModel model = JsonSerializer.Deserialize<ComboProductoViewModel>(jsonModel);

         model.ProductoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.ProductoDTO.OrderBy = "p.descripcion";

         if (model.ProductoDTO.FamiliaProductoDTO == null) {

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
                            UnidadMedida = f.UnidadMedidaDTO.Nombre,
                            FamiliaNombre = f.FamiliaProductoDTO.Nombre,
                            FechaBaja = f.FechaBaja != null ? f.FechaBaja.Value.ToShortDateString() : string.Empty,
                            EatadoProducto = f.EstadoProductoDTO.Nombre,
                            EatadoComercialProducto = f.EstadoComercialProductoDTO.Nombre,
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
            EsCombo = SiNoConstante.S,
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
      public ActionResult Crear([FromBody] ComboProductoViewModel model)
      {
         Logger.InfoStart();

         ComboProductoViewModel comboProductoViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(comboProductoViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, comboProductoViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] ComboProductoViewModel model)
      {

         Logger.InfoStart();

         ComboProductoViewModel comboProductoViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(comboProductoViewModel, model.ProductoDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, comboProductoViewModel);
      }
      private void cargarDataParaEditar(ComboProductoViewModel model, int id)
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
                     TipoProductoId = 1,
                     TipoProductoDTO = new TipoProductoDTO { TipoProductoId = 1 }
                  }
               }
            };
            productoDTO.FamiliaProductoDTO = familiaProductoDTO;

            UnidadMedidaDTO unidadMedidaDTO = unidadMedidaProxy.ObtenerPorCodigo(new UnidadMedidaDTO()
            { Codigo = UnidadMedidaConstante.DEFAULT });
            productoDTO.UnidadMedidaId = unidadMedidaDTO.Id;

            EstadoProductoDTO estadoProductoDTO = estadoProductoProxy.ObtenerPorCodigo(new EstadoProductoDTO()
            { Codigo = EstadoProductoConstante.ACTIVO });
            productoDTO.EstadoProductoId = estadoProductoDTO.Id;

            EstadoComercialProductoDTO estadoComercialProductoDTO = estadoComercialProductoProxy.ObtenerPorCodigo(new EstadoComercialProductoDTO()
            { Codigo = EstadoComercialProductoConstante.STOCK });
            productoDTO.EstadoComercialProductoId = estadoComercialProductoDTO.Id;

            PeriodoDTO periodoDTO = ObtenerPeriodoDTO(model.ViewModelState.CID);
            productoDTO.Periodo = periodoDTO.Codigo;
         }
         else
         {
            productoDTO = productoProxy.GetById(new ProductoDTO() { ProductoId = id });
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
         WebHelper.SelectedListItemSetValue(model.EstadoProductoList, productoDTO.EstadoProductoId);
         WebHelper.SelectedListItemSetValue(model.EstadoComercialProductoList, productoDTO.EstadoComercialProductoId);
      }
      private void cargarListas(ComboProductoViewModel model)
      {
         cargarListasCompartidas(model);

         List<UnidadMedidaDTO> unidadMedidaDTOList = unidadMedidaProxy.
            Search(new UnidadMedidaDTO()
            {
               EmpresaId = ObtenerEmpresaId(),
               Estado = EstadoConstante.ACTIVO,
            });
         model.UnidadMedidaList = WebHelper.ToSelectListItem(unidadMedidaDTOList, x => x.Id.ToString(), x => x.Nombre, SelectListFirstOptionType.Select);

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
      private void cargarListasCompartidas(ComboProductoViewModel model)
      {
         List<GrupoProductoDTO> grupoComboList = grupoProductoProxy.Search(new GrupoProductoDTO
         {
            EsCombo = SiNoConstante.S,
            Estado = EstadoConstante.ACTIVO
         });
         List<int> tipoIdsConCombos = grupoComboList?.Select(g => g.TipoProductoId).Distinct().ToList() ?? new List<int>();
         List<TipoProductoDTO> tipoProductoDTOList = tipoProductoProxy.
            Search(new TipoProductoDTO()
            {
               EmpresaId = ObtenerEmpresaId(),
               Estado = EstadoConstante.ACTIVO,
            });
         if (tipoIdsConCombos.Count > 0)
         {
            tipoProductoDTOList = tipoProductoDTOList.Where(tp => tipoIdsConCombos.Contains(tp.Id)).ToList();
         }

         model.TipoProductoList = WebHelper.ToSelectListItem(tipoProductoDTOList, x => x.Id.ToString(), x => x.Nombre, SelectListFirstOptionType.Select);

         if (model.ProductoDTO.FamiliaProductoDTO.ClaseProductoDTO.GrupoProductoDTO.TipoProductoId == 0)
            model.GrupoProductoList = WebHelper.ToSelectListItem(SelectListFirstOptionType.Select);
         else {
            List<GrupoProductoDTO> grupoProductoDTOList = grupoProductoProxy.
                        Search(new GrupoProductoDTO()
                        {
                           TipoProductoId = model.ProductoDTO.FamiliaProductoDTO.ClaseProductoDTO.GrupoProductoDTO.TipoProductoId,
                           EsCombo = SiNoConstante.S,
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
      public ActionResult Eliminar([FromBody] ComboProductoViewModel model)
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
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody] ComboProductoViewModel model)
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
      private ProductoDTO cargarProductoDTO(ComboProductoViewModel model)
      {

         ProductoDTO productoDTO;
         try
         {

            if (model.EditAction == ViewModelEditAction.New)
            {
               productoDTO = model.ProductoDTO;

               PeriodoDTO periodoDTO = ObtenerPeriodoDTO(model.ViewModelState.CID);
               productoDTO.Periodo = periodoDTO.Codigo;

               TipoIgvDTO tipoIgvDTO = tipoIgvProxy.ObtenerPorCodigo(new TipoIgvDTO()
               { Codigo = TipoIgvConstante.DEFAULT });
               productoDTO.TipoIgvId = tipoIgvDTO.Id;

               productoDTO.FechaAlta = DateTime.Today;
               productoDTO.TienePresentacionVenta = SiNoConstante.N;
               productoDTO.EsOperacion = SiNoConstante.N;
               productoDTO.CodigoReferencial = string.Empty;

            }
            else
            {
               productoDTO = productoProxy.GetById(new ProductoDTO()
               { ProductoId = model.ProductoDTO.Id });

               model.ProductoDTO.TipoIgvId = productoDTO.TipoIgvId;

               productoDTO = model.ProductoDTO;
            }

            productoDTO = model.ProductoDTO;

            if (string.IsNullOrEmpty(productoDTO.CodigoReferencial))
               productoDTO.CodigoReferencial = string.Empty;
            if (string.IsNullOrEmpty(productoDTO.EsPerecible))
               productoDTO.EsPerecible = SiNoConstante.N;
            if (string.IsNullOrEmpty(productoDTO.DetalleRequerido))
               productoDTO.DetalleRequerido = SiNoConstante.N;

            productoDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(productoDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return productoDTO;
      }
      [HttpPost("ListarComboProducto")]
      public ActionResult ListarComboProducto(string jsonModel)
      {

         Logger.InfoStart();

         ComboProductoViewModel model = JsonSerializer.Deserialize<ComboProductoViewModel>(jsonModel);

         model.ComboProductoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.ComboProductoDTO.OrderBy = "pi.descripcion";


         try
         {
            List<ComboProductoDTO> comboProductoDTOList = comboProductoProxy.Search(model.ComboProductoDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = comboProductoDTOList.Count,
               rows = from f in comboProductoDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.ComboProductoId,
                            f.ProductoId,
                            f.ProductoIncluidoId,
                            f.ProductoIncluidoDTO.Codigo,
                            f.ProductoIncluidoDTO.Descripcion,
                            UnidadMedidaNombre = f.ProductoIncluidoDTO.UnidadMedidaDTO.Abreviatura,
                            f.Cantidad,
                            f.PorcentajePrecio,
                            f.NumeroDecimalesRedondeo,
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
      [HttpPost("GrabarComboProducto")]
      public ActionResult GrabarComboProducto([FromBody] ComboProductoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            ComboProductoDTO comboProductoDTO = cargarComboProductoDTO(model);

            int newId = model.ComboProductoDTO.Id;
            string messageResourceKey = string.Empty;
            switch (comboProductoDTO.DTOState)
            {
               case DTOState.Added:
                  newId = comboProductoProxy.Insert(comboProductoDTO);
                  messageResourceKey = "MG_combo_producto_agregado";
                  break;

               case DTOState.Modified:
                  comboProductoProxy.Update(comboProductoDTO);
                  messageResourceKey = "MG_combo_producto_actualizado";
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
      private ComboProductoDTO cargarComboProductoDTO(ComboProductoViewModel model)
      {

         ComboProductoDTO comboProductoDTO;
         try
         {

            comboProductoDTO = model.ComboProductoDTO;

            comboProductoDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(comboProductoDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return comboProductoDTO;
      }
      [HttpPost("EliminarComboProducto")]
      public ActionResult EliminarComboProducto([FromBody] ComboProductoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.ComboProductoDTO.DTOState = DTOState.Modified;
            AuditDTO(model.ComboProductoDTO);

            comboProductoProxy.Delete(model.ComboProductoDTO);

            resultResponse.Message = SR.GetString("MG_combo_producto_eliminado");
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
