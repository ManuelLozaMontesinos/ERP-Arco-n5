using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.AS.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.AS.Controllers
{
   [Area("AS")]
   [Route("[controller]")]
   public class DocuComSerieController : AuthorizedController
   {
      private readonly IEmpresaSedeProxy empresaSedeProxy;
      private readonly ITipoDocumentoComercialProxy tipoDocumentoComercialProxy;
      private readonly IDocumentoComercialSerieProxy documentoComercialSerieProxy;
      private readonly IAlmacenProxy almacenProxy;
      public DocuComSerieController(
         IEmpresaSedeProxy empresaSedeProxy,
         ITipoDocumentoComercialProxy tipoDocumentoComercialProxy,
         IDocumentoComercialSerieProxy documentoComercialSerieProxy,
         IAlmacenProxy almacenProxy
         )
      {
         this.empresaSedeProxy = empresaSedeProxy;
         this.tipoDocumentoComercialProxy = tipoDocumentoComercialProxy;
         this.documentoComercialSerieProxy = documentoComercialSerieProxy;
         this.almacenProxy = almacenProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         DocuComSerieViewModel docuComSerieViewModel = new();

         cargarListasCompartidas(docuComSerieViewModel);

         return PartialView(docuComSerieViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         DocuComSerieViewModel model = JsonSerializer.Deserialize<DocuComSerieViewModel>(jsonModel);

         model.DocumentoComercialSerieDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.DocumentoComercialSerieDTO.OrderBy = "es.empresa_sede,tdc.tipo_doc_com";

         if (model.DocumentoComercialSerieDTO.EmpresaSedeDTO == null)
            model.DocumentoComercialSerieDTO.EmpresaSedeDTO = new EmpresaSedeDTO();

         model.DocumentoComercialSerieDTO.EmpresaSedeDTO.EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID);

         try
         {
            List<DocumentoComercialSerieDTO> documentoComercialSerieDTOList = documentoComercialSerieProxy.Search(model.DocumentoComercialSerieDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = documentoComercialSerieDTOList.Count,
               rows = from f in documentoComercialSerieDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.DocumentoComercialSerieId,
                            EmpresaSedeNombre = f.EmpresaSedeDTO.Nombre,
                            TipoDocumentoComercialNombre = f.TipoDocumentoComercialDTO.Nombre,
                            f.TipoDocumentoComercialDTO.EsElectronico,
                            f.Serie,
                            f.Correlativo,
                            f.Formato,
                            AlmacenNombre = f.AlmacenDTO != null ? f.AlmacenDTO.Nombre : string.Empty,
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
      public ActionResult Crear([FromBody] DocuComSerieViewModel model)
      {
         Logger.InfoStart();

         DocuComSerieViewModel docuComSerieViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(docuComSerieViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, docuComSerieViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] DocuComSerieViewModel model)
      {

         Logger.InfoStart();

         DocuComSerieViewModel docuComSerieViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(docuComSerieViewModel, model.DocumentoComercialSerieDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, docuComSerieViewModel);
      }
      private void cargarDataParaEditar(DocuComSerieViewModel model, int id)
      {

         DocumentoComercialSerieDTO documentoComercialSerieDTO = null;
         if (model.EditAction == ViewModelEditAction.New)
         {
            documentoComercialSerieDTO = new DocumentoComercialSerieDTO() {
               TipoDocumentoComercialDTO = new TipoDocumentoComercialDTO()
            };
         }
         else
         {
            documentoComercialSerieDTO = documentoComercialSerieProxy.GetById(new DocumentoComercialSerieDTO() { DocumentoComercialSerieId = id });
         }

         documentoComercialSerieDTO.EsContingencia = documentoComercialSerieDTO.EsContingencia == SiNoConstante.S ? EstadoConstante.ACTIVO : EstadoConstante.INACTIVO;
         documentoComercialSerieDTO.EnvioEnLinea = documentoComercialSerieDTO.EnvioEnLinea == SiNoConstante.S ? EstadoConstante.ACTIVO : EstadoConstante.INACTIVO;

         model.DocumentoComercialSerieDTO = documentoComercialSerieDTO;

         cargarListas(model);

         WebHelper.SelectedListItemSetValue(model.EmpresaSedeList, documentoComercialSerieDTO.EmpresaSedeId);
         WebHelper.SelectedListItemSetValue(model.TipoDocumentoComercialList, documentoComercialSerieDTO.TipoDocumentoComercialId);
         WebHelper.SelectedListItemSetValue(model.TipoDocumentoComercialRelacionadoList, documentoComercialSerieDTO.TipoDocumentoComercialRelacionadoId);
         WebHelper.SelectedListItemSetValue(model.AlmacenList, documentoComercialSerieDTO.AlmacenId);
      }
      private void cargarListas(DocuComSerieViewModel model)
      {

         cargarListasCompartidas(model);

         if (!model.DocumentoComercialSerieDTO.AlmacenId.HasValue)
            model.AlmacenList = WebHelper.ToSelectListItem(SelectListFirstOptionType.Empty);
         else
         {
            PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);
            List<AlmacenDTO> almacenDTOList = almacenProxy.Search(new AlmacenDTO() { 
               EmpresaSedeDTO = periodoEmpresaSedeDTO.EmpresaSedeDTO,
               EmpresaSedeId = periodoEmpresaSedeDTO.EmpresaSedeId,
               EsVenta = SiNoConstante.S,
               Estado = EstadoConstante.ACTIVO
            });

            model.AlmacenList = WebHelper.ToSelectListItem(almacenDTOList, x => x.Id.ToString(), x => x.Nombre,SelectListFirstOptionType.Select);
         }
      }
      private void cargarListasCompartidas(DocuComSerieViewModel model)
      {
         List<EmpresaSedeDTO> empresaSedeDTOList = empresaSedeProxy.
         Search(new EmpresaSedeDTO()
         {
            EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID),
            Estado = EstadoConstante.ACTIVO,
         });
         model.EmpresaSedeList = WebHelper.ToSelectListItem(empresaSedeDTOList, x => x.Id.ToString(), x => x.Nombre, SelectListFirstOptionType.Select);

         List<TipoDocumentoComercialDTO> tipoDocumentoComercialDTOList = tipoDocumentoComercialProxy.
         Search(new TipoDocumentoComercialDTO()
         {
            Estado = EstadoConstante.ACTIVO,
         });
         model.TipoDocumentoComercialList = WebHelper.ToSelectListItem(tipoDocumentoComercialDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         model.TipoDocumentoComercialRelacionadoList = WebHelper.ToSelectListItem(tipoDocumentoComercialDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Blank);

         if (model.SelectedItemList == null)
            return;

         SelectedItem selectedItem = model.SelectedItemList.Where(x => x.Name == "EmpresaSedeId").FirstOrDefault();
         if (selectedItem != null)
         {
            if (selectedItem.SelectedItemType == SelectedItemType.Value)
               WebHelper.SelectedListItemSetValue(model.EmpresaSedeList, Convert.ToInt32(selectedItem.Value));
         }

         selectedItem = model.SelectedItemList.Where(x => x.Name == "TipoDocumentoComercial").FirstOrDefault();
         if (selectedItem != null)
         {
            if (selectedItem.SelectedItemType == SelectedItemType.Value)
            {
               TipoDocumentoComercialDTO tipoDocumentoComercialDTO = tipoDocumentoComercialDTOList
                  .Where(x => x.Codigo == selectedItem.Value).FirstOrDefault();
               if (tipoDocumentoComercialDTO != null)
                  WebHelper.SelectedListItemSetValue(model.TipoDocumentoComercialList, tipoDocumentoComercialDTO.Id);
            }
         }
      }
      [HttpGet("ObtenerTipoDocumentoComercial")]
      public JsonResult ObtenerTipoDocumentoComercial(int id)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {
            TipoDocumentoComercialDTO tipoDocumentoComercialDTO = tipoDocumentoComercialProxy.GetById(new TipoDocumentoComercialDTO() { TipoDocumentoComercialId = id });

            resultResponse.Data = new
            {
               tipoDocumentoComercialDTO.EsElectronico,
               tipoDocumentoComercialDTO.EsComprobantePago
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
      [HttpPost("ListarAlmacen")]
      public JsonResult ListarAlmacen([FromBody] DocuComSerieViewModel model)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {

            PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);
            List<AlmacenDTO> almacenDTOList = almacenProxy.Search(new AlmacenDTO()
            {
               EmpresaSedeDTO = periodoEmpresaSedeDTO.EmpresaSedeDTO,
               EmpresaSedeId = model.DocumentoComercialSerieDTO.EmpresaSedeId,
               EsVenta = SiNoConstante.S,
               Estado = EstadoConstante.ACTIVO
            });

            resultResponse.Data = new
            {
               List = WebHelper.ToSelectListItem(almacenDTOList, x => x.Id.ToString(), x => x.Nombre,
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
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody] DocuComSerieViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            DocumentoComercialSerieDTO documentoComercialSerieDTO = cargarDocumentoComercialSerieDTO(model);

            int newId = model.DocumentoComercialSerieDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = documentoComercialSerieProxy.Insert(documentoComercialSerieDTO);
                  messageResourceKey = "VT_documento_comercial_agregado";
                  break;

               case ViewModelEditAction.Edit:
                  documentoComercialSerieProxy.Update(documentoComercialSerieDTO);
                  messageResourceKey = "VT_documento_comercial_actualizado";
                  break;
            }

            documentoComercialSerieDTO = documentoComercialSerieProxy.GetById(new DocumentoComercialSerieDTO() { DocumentoComercialSerieId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, documentoComercialSerieDTO.TipoDocumentoComercialDTO.Nombre);
            resultResponse.Data = new
            {
               documentoComercialSerieDTO.Id
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
      private DocumentoComercialSerieDTO cargarDocumentoComercialSerieDTO(DocuComSerieViewModel model)
      {

         DocumentoComercialSerieDTO documentoComercialSerieDTO;
         try
         {

            if (model.EditAction == ViewModelEditAction.New)
            {
               documentoComercialSerieDTO = model.DocumentoComercialSerieDTO;
            }
            else
            {
               documentoComercialSerieDTO = documentoComercialSerieProxy.GetById(new DocumentoComercialSerieDTO() 
                  { DocumentoComercialSerieId = model.DocumentoComercialSerieDTO.Id });

               documentoComercialSerieDTO = model.DocumentoComercialSerieDTO;
            }

            documentoComercialSerieDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(documentoComercialSerieDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return documentoComercialSerieDTO;
      }
      [HttpPost("Eliminar")]
      public JsonResult Eliminar([FromBody] DocuComSerieViewModel model)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            DocumentoComercialSerieDTO documentoComercialSerieDTO = documentoComercialSerieProxy
               .GetById(model.DocumentoComercialSerieDTO);

            AuditDTO(documentoComercialSerieDTO);

            documentoComercialSerieProxy.Delete(documentoComercialSerieDTO);

            resultResponse.Message = SR.GetString("VT_documento_comercial_anulado");
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
      [Route("ListPopup")]
      public IActionResult ListPopup([FromBody] DocuComSerieViewModel model)
      {
         if(model == null)
            model = new DocuComSerieViewModel();

         cargarListasCompartidas(model);

         return PartialView(model);
      }
   }
}
