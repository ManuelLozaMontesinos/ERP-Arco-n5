using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service.DTO;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.AS.Models;
using ESD.ERP.Web.Areas.VT.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using ESD.ERP.WebApiProxy.Impl;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.VT.Controllers
{
   [Area("VT")]
   [Route("[controller]")]
   public class VendedorController : AuthorizedController
   {

      private readonly IVendedorProxy vendedorProxy;
      private readonly IVendedorSerieProxy vendedorSerieProxy;
      private readonly IListaPrecioProxy listaPrecioProxy;
      private readonly IListaPrecioVendedorProxy listaPrecioVendedorProxy;
      private readonly IMonedaProxy monedaProxy;

      public VendedorController(
           IVendedorProxy vendedorProxy,
           IVendedorSerieProxy vendedorSerieProxy,        
           IListaPrecioProxy listaPrecioProxy,
           IListaPrecioVendedorProxy listaPrecioVendedorProxy,
           IMonedaProxy monedaProxy
      )
        {
           this.vendedorProxy = vendedorProxy;
           this.vendedorSerieProxy = vendedorSerieProxy;
           this.listaPrecioProxy = listaPrecioProxy;
           this.listaPrecioVendedorProxy = listaPrecioVendedorProxy;
           this.monedaProxy = monedaProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         VendedorViewModel vendedorViewModel = new();

         MenuBarTagHelper menuBarTagHelper = new();
         vendedorViewModel.MenuItemList = menuBarTagHelper.CreateMenuStyleList(MenuItemStyle.CRUD);

         vendedorViewModel.MenuItemList.Add(new MenuItem() { Key = "activarVendedor", Content = "Activar" });

         return PartialView(vendedorViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         VendedorViewModel model = JsonSerializer.Deserialize<VendedorViewModel>(jsonModel);

         model.VendedorDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.VendedorDTO.OrderBy = "p.razon_social";

         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

         model.VendedorDTO.ProveedorDTO.EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId;

         try
         {
            List<VendedorDTO> proveedorDTOList = vendedorProxy.Search(model.VendedorDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = proveedorDTOList.Count,    
               rows = from f in proveedorDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.VendedorId,
                            f.Codigo,
                            f.ProveedorDTO.RazonSocial,
                            f.ProveedorDTO.NumeroDocumentoIdentidad,
                            f.Abreviatura,
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
      public ActionResult Crear([FromBody] VendedorViewModel model)
      {
         Logger.InfoStart();

         VendedorViewModel vendedorViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(vendedorViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, vendedorViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] VendedorViewModel model)
      {

         Logger.InfoStart();

         VendedorViewModel vendedorViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(vendedorViewModel, model.VendedorDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, vendedorViewModel);
      }
      private void cargarDataParaEditar(VendedorViewModel model, int id)
      {

         VendedorDTO vendedorDTO;
         if (model.EditAction == ViewModelEditAction.New)
         {
            vendedorDTO = new VendedorDTO
            {
               ProveedorDTO = new ProveedorDTO(),
               Estado = EstadoConstante.ACTIVO
            };
         }
         else
         {
            vendedorDTO = vendedorProxy.GetById(new VendedorDTO() { VendedorId = id });
         }

         model.VendedorDTO = vendedorDTO;

         cargarListas(model);

         MenuBarTagHelper menuBarTagHelper = new();

         model.MenuVendedorSerieList = menuBarTagHelper.CreateMenuStyleList(MenuItemStyle.NewDelete);
         model.MenuVendedorSerieList.Add(new MenuItem() { Key = "activarVS", Content = "Activar", Disabled = true }); 
         model.MenuListaPrecioVendedorList = menuBarTagHelper.CreateMenuStyleList(MenuItemStyle.NewDelete);
         model.MenuListaPrecioVendedorList.Add(new MenuItem() { Key = "activarLP", Content = "Activar", Disabled = true });
        }

      private void cargarListas(VendedorViewModel model)
      {

            List<MonedaDTO> monedaDTOList = monedaProxy.Search(new MonedaDTO() { Estado = EstadoConstante.ACTIVO });
            model.MonedaList = WebHelper.ToSelectListItem(monedaDTOList, x => x.Id.ToString(), x => x.Simbolo,
               SelectListFirstOptionType.Select);
      }

      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody] VendedorViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            VendedorDTO vendedorDTO = cargarVendedorDTO(model);

            int newId = model.VendedorDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = vendedorProxy.Insert(vendedorDTO);
                  messageResourceKey = "VT_vendedor_agregado";
                  break;

               case ViewModelEditAction.Edit:
                  vendedorProxy.Update(vendedorDTO);
                  messageResourceKey = "VT_vendedor_actualizado";
                  break;
            }

            vendedorDTO = vendedorProxy.GetById(new VendedorDTO() { VendedorId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, vendedorDTO.ProveedorDTO.RazonSocial);
            resultResponse.Data = new
            {
               vendedorDTO.Id,
               vendedorDTO.Codigo
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
      private VendedorDTO cargarVendedorDTO(VendedorViewModel model)
      {

         VendedorDTO vendedorDTO;
         try
         {

            vendedorDTO = model.VendedorDTO;

            if (model.EditAction == ViewModelEditAction.New)
            {
               PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);
               vendedorDTO.ProveedorDTO = new ProveedorDTO() { EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId };
            }

            vendedorDTO.Estado = EstadoConstante.ACTIVO;
            AuditDTO(vendedorDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return vendedorDTO;
      }
      [HttpPost("Eliminar")]
      public ActionResult Eliminar([FromBody] VendedorViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.VendedorDTO.DTOState = DTOState.Modified;

            AuditDTO(model.VendedorDTO);

            vendedorProxy.Delete(model.VendedorDTO);

            resultResponse.Message = SR.GetString("VT_vendedor_anulado", model.VendedorDTO.ProveedorDTO.RazonSocial);
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
      [HttpPost("ListarListaVendedor")]
      public ActionResult ListarListaVendedor(string jsonModel)
      {
            Logger.InfoStart();

            VendedorViewModel model = JsonSerializer.Deserialize<VendedorViewModel>(jsonModel);

            try
            {
                List<VendedorSerieDTO> vendedorSerieDTOList = vendedorSerieProxy.Search(model.VendedorSerieDTO);

                int index = 1;
                var jsonData = new
                {
                    total = 1,
                    page = 1,
                    records = vendedorSerieDTOList.Count,
                    rows = from f in vendedorSerieDTOList.AsEnumerable()
                           select new
                           {
                               id = index++,
                               cell = new
                               {
                                   f.VendedorSerieId,
                                   f.VendedorId,
                                   EmpresaSedeNombre = f.DocumentoComercialSerieDTO.EmpresaSedeDTO.Nombre,
                                   f.DocumentoComercialSerieId,
                                   TipoDocumentoComercialNombre = f.DocumentoComercialSerieDTO.TipoDocumentoComercialDTO.Nombre,
                                   f.DocumentoComercialSerieDTO.Serie,
                                   f.DocumentoComercialSerieDTO.Correlativo,
                                   f.Estado,
                                   DTOState = DTOState.Unchanged
                               }
                           }
                };

                Logger.InfoFinish();
                return Json(jsonData, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch(Exception e){
                Logger.Error(e);
                throw;
            }
      }
      [HttpPost("ListarVendedorSerie")]
      public ActionResult ListarVendedorSerie(string jsonModel)
      {

         Logger.InfoStart();

         VendedorViewModel model = JsonSerializer.Deserialize<VendedorViewModel>(jsonModel);

         model.VendedorSerieDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.VendedorSerieDTO.OrderBy = "es.nombre,tdc.tipo_doc_com";

         try
         {
            List<VendedorSerieDTO> vendedorSerieDTOList = vendedorSerieProxy.Search(model.VendedorSerieDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = vendedorSerieDTOList.Count,
               rows = from f in vendedorSerieDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.VendedorSerieId,
                            f.VendedorId,
                            EmpresaSedeNombre = f.DocumentoComercialSerieDTO.EmpresaSedeDTO.Nombre,
                            f.DocumentoComercialSerieId,
                            TipoDocumentoComercialNombre = f.DocumentoComercialSerieDTO.TipoDocumentoComercialDTO.Nombre,
                            AlmacenId = f.DocumentoComercialSerieDTO.AlmacenDTO.AlmacenId,
                            AlmacenNombre = f.DocumentoComercialSerieDTO.AlmacenDTO.Nombre,
                             f.DocumentoComercialSerieDTO.Serie,
                            f.DocumentoComercialSerieDTO.Correlativo,
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
                
      [HttpPost("ListarListaPrecioVendedor")]
      public ActionResult ListarListaPrecioVendedor(string jsonModel)
      {

        Logger.InfoStart();

        VendedorViewModel model = JsonSerializer.Deserialize<VendedorViewModel>(jsonModel);

        model.ListaPrecioVendedorDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
        model.ListaPrecioVendedorDTO.OrderBy = "lp.nombre";

        PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

        model.ListaPrecioVendedorDTO.ListaPrecioDTO.EmpresaSedeDTO = periodoEmpresaSedeDTO.EmpresaSedeDTO;

        try
        {
            List<ListaPrecioVendedorDTO> listaPrecioVendedorDTO = listaPrecioVendedorProxy.Search(model.ListaPrecioVendedorDTO);

            int index = 1;
            var jsonData = new
                {
                    total = 1,
                    page = 1,
                    records = listaPrecioVendedorDTO.Count,
                    rows = from f in listaPrecioVendedorDTO.AsEnumerable()
                        select new
                        {
                            id = index++,
                            cell = new
                            {
                            f.ListaPrecioVendedorId,
                            f.VendedorId,
                            f.ListaPrecioId,
                            f.VendedorDTO.Abreviatura,
                            f.ListaPrecioDTO.Nombre,
                            f.ListaPrecioDTO.MonedaDTO.MonedaId,
                            MonedaNombre = f.ListaPrecioDTO.MonedaDTO.Nombre,
                            f.ListaPrecioDTO.EmpresaSedeDTO.EmpresaSedeId,
                            EmpresaSede = f.ListaPrecioDTO.EmpresaSedeDTO.Nombre,
                            f.ListaPrecioDTO.MonedaDTO?.Simbolo,
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

      [HttpPost("ListaPrecioListPopup")]
      public ActionResult ListaPrecioListPopup([FromBody] VendedorViewModel model)
      {
        return PartialView(model);
      }

      [HttpPost("ListaListaPrecio")]
      public ActionResult ListaListaPrecio(string jsonModel)
      {

            Logger.InfoStart();

            ListaPrecioViewModel model = JsonSerializer.Deserialize<ListaPrecioViewModel>(jsonModel);

            model.ListaPrecioDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
            model.ListaPrecioDTO.OrderBy = "es.empresa_sede_id, m.moneda_id";

            model.ListaPrecioDTO.EmpresaSedeDTO = new EmpresaSedeDTO()
            {
                EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID)
            };

            try
            {
                List<ListaPrecioDTO> listaPrecioDTOList = listaPrecioProxy.Search(model.ListaPrecioDTO);

                int index = 1;
                var jsonData = new
                {
                    total = 1,
                    page = 1,
                    records = listaPrecioDTOList.Count,
                    rows = from f in listaPrecioDTOList.AsEnumerable()
                           select new
                           {
                               id = index++,
                               cell = new
                               {
                                   f.ListaPrecioId,
                                   EmpresaSedeNombre = f.EmpresaSedeDTO.Nombre,
                                   f.Nombre,
                                   MonedaNombre = f.MonedaDTO.Nombre,

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

      [HttpPost("GrabarVendedorSerie")]
      public ActionResult GrabarVendedorSerie([FromBody] VendedorViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            VendedorSerieDTO vendedorSerieDTO = cargarVendedorSerieDTO(model);

            string messageResourceKey = "VT_documento_comercial_serie_agregado";

            vendedorSerieProxy.ActualizarListaDocumentoComercialSerieId(vendedorSerieDTO);

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
      private VendedorSerieDTO cargarVendedorSerieDTO(VendedorViewModel model)
      {

         VendedorSerieDTO vendedorSerieDTO;
         try
         {

            vendedorSerieDTO = model.VendedorSerieDTO;
            vendedorSerieDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(vendedorSerieDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return vendedorSerieDTO;
      }

        [HttpPost("GrabarListaVendedor")]
        public ActionResult GrabarListaVendedor([FromBody] VendedorViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                ListaPrecioVendedorDTO listaPrecioVendedorDTO = cargarListaPrecioVendedorDTO(model);
                                
                listaPrecioVendedorProxy.ActualizarListaPrecioVendedorId(listaPrecioVendedorDTO);

                string messageResourceKey = "VT_documento_comercial_serie_agregado";

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
        private ListaPrecioVendedorDTO cargarListaPrecioVendedorDTO(VendedorViewModel model)
        {

            ListaPrecioVendedorDTO listaPrecioVendedorDTO;
            try
            {

                listaPrecioVendedorDTO = model.ListaPrecioVendedorDTO;
                listaPrecioVendedorDTO.Estado = EstadoConstante.ACTIVO;

                AuditDTO(listaPrecioVendedorDTO);
            }

            catch (Exception)
            {
                throw;
            }

            return listaPrecioVendedorDTO;
        }

      [HttpPost("EliminarVendedorSerie")]
      public ActionResult EliminarVendedorSerie([FromBody] VendedorViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

            try
            {
            model.VendedorSerieDTO.DTOState = DTOState.Modified;
            AuditDTO(model.VendedorSerieDTO);

            vendedorSerieProxy.Delete(model.VendedorSerieDTO);

            resultResponse.Message = SR.GetString("VT_documento_comercial_serie_anulado");
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

      [HttpPost("ActivarVendedorSerie")]
      public ActionResult ActivarVendedorSerie([FromBody] VendedorViewModel model)
      {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                model.VendedorSerieDTO.DTOState = DTOState.Modified;
                AuditDTO(model.VendedorSerieDTO);

                vendedorSerieProxy.Update(model.VendedorSerieDTO);
                
                resultResponse.Message = "Documento activado correctamente";
                
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


        [HttpPost("EliminarListaPrecioVendedor")]
        public ActionResult EliminarListaPrecioVendedor([FromBody] VendedorViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                model.ListaPrecioVendedorDTO.DTOState = DTOState.Modified;
                AuditDTO(model.ListaPrecioVendedorDTO);

                listaPrecioVendedorProxy.Delete(model.ListaPrecioVendedorDTO);

                resultResponse.Message = "La lista de precio fue anulada";
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

        [HttpPost("ActivarListaPrecioVendedor")]
        public ActionResult ActivarListaPrecioVendedor([FromBody] VendedorViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                model.ListaPrecioVendedorDTO.DTOState = DTOState.Modified;
                AuditDTO(model.ListaPrecioVendedorDTO);

                listaPrecioVendedorProxy.Update(model.ListaPrecioVendedorDTO);

                resultResponse.Message = "Documento activado correctamente";

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
        [HttpPost("ActivarVendedor")]
        public ActionResult ActivarVendedor([FromBody] VendedorViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                VendedorDTO vendedorDTO = vendedorProxy.GetById(new VendedorDTO() {VendedorId = model.VendedorDTO.Id});
                vendedorDTO.Estado = EstadoConstante.ACTIVO;

                vendedorProxy.Update(vendedorDTO);

                resultResponse.Message = "VendedorActivado";
            }
            catch(Exception e)
            {
                Logger.Error(e);
                resultResponse.Message = e.Message;
                resultResponse.Success = false;
            }

            Logger.InfoFinish();

            return Json(resultResponse, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

      [HttpPost("ListPopup")]
      public ActionResult ListPopup([FromBody] VendedorViewModel model)
      {
         return PartialView(model);
      }
    }
}