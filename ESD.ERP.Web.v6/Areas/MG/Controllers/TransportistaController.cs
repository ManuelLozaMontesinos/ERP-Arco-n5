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
   public class TransportistaController : AuthorizedController
   {
      private readonly ITransportistaProxy transportistaProxy;
      private readonly ITransportistaUnidadProxy transportistaUnidadProxy;
      private readonly ITransportistaConductorProxy transportistaConductorProxy;
      private readonly ITipoDocumentoIdentidadProxy tipoDocumentoIdentidadProxy;

      public TransportistaController(
         ITransportistaProxy transportistaProxy,
         ITransportistaUnidadProxy transportistaUnidadProxy,
         ITransportistaConductorProxy transportistaConductorProxy,
         ITipoDocumentoIdentidadProxy tipoDocumentoIdentidadProxy
         )
      {
         this.transportistaProxy = transportistaProxy;
         this.transportistaUnidadProxy = transportistaUnidadProxy;
         this.transportistaConductorProxy = transportistaConductorProxy;
         this.tipoDocumentoIdentidadProxy = tipoDocumentoIdentidadProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         TransportistaViewModel transportistaViewModel = new();

         return PartialView(transportistaViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         TransportistaViewModel model = JsonSerializer.Deserialize<TransportistaViewModel>(jsonModel);

         model.TransportistaDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.TransportistaDTO.OrderBy = "t.numero_ruc";
         model.TransportistaDTO.EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID);

         try
         {
            List<TransportistaDTO> transportistaDTOList = transportistaProxy.Search(model.TransportistaDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = transportistaDTOList.Count,
               rows = from f in transportistaDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.TransportistaId,
                            f.NumeroRuc,
                            f.RazonSocial,
                            f.NumeroInscripcionVehicular,
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
      public ActionResult Crear([FromBody] TransportistaViewModel model)
      {
         Logger.InfoStart();

         TransportistaViewModel transportistaViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(transportistaViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, transportistaViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] TransportistaViewModel model)
      {

         Logger.InfoStart();

         TransportistaViewModel transportistaViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(transportistaViewModel, model.TransportistaDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, transportistaViewModel);
      }
      private void cargarDataParaEditar(TransportistaViewModel model, int id)
      {

         TransportistaDTO transportistaDTO = null;
         if (model.EditAction == ViewModelEditAction.New)
         {
            transportistaDTO = new TransportistaDTO();
         }
         else
         {
            transportistaDTO = transportistaProxy.GetById(new TransportistaDTO() { TransportistaId = id });
         }
         
         cargarListas(model);

         model.TransportistaDTO = transportistaDTO;
      }
      private void cargarListas(TransportistaViewModel model)
      {

         List<TipoDocumentoIdentidadDTO> tipoDocumentoIdentidadDTOList = tipoDocumentoIdentidadProxy.Search(new TipoDocumentoIdentidadDTO()
         {
            Estado = EstadoConstante.ACTIVO
         });
         model.TipoDocumentoIdentidadString = string.Join(SeparadorConstante.PUNTO_COMA, tipoDocumentoIdentidadDTOList.Select(x => x.Id.ToString()
         + SeparadorConstante.DOS_PUNTOS + x.Nombre));
      }
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody] TransportistaViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            TransportistaDTO transportistaDTO = cargarTransportistaDTO(model);

            int newId = model.TransportistaDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = transportistaProxy.Insert(transportistaDTO);
                  messageResourceKey = "MG_transportista_agregado";
                  break;

               case ViewModelEditAction.Edit:
                  transportistaProxy.Update(transportistaDTO);
                  messageResourceKey = "MG_transportista_actualizado";
                  break;
            }

            transportistaDTO = transportistaProxy.GetById(new TransportistaDTO() { TransportistaId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, transportistaDTO.RazonSocial);
            resultResponse.Data = new
            {
               transportistaDTO.Id
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
      private TransportistaDTO cargarTransportistaDTO(TransportistaViewModel model)
      {

         TransportistaDTO transportistaDTO;
         try
         {

            if (model.EditAction == ViewModelEditAction.New)
            {
               PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

               transportistaDTO = model.TransportistaDTO;
               transportistaDTO.EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId;
            }
            else
            {
               transportistaDTO = transportistaProxy.GetById(new TransportistaDTO() 
                  { TransportistaId = model.TransportistaDTO.Id });

               transportistaDTO = model.TransportistaDTO;
            }

            transportistaDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(transportistaDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return transportistaDTO;
      }
      [HttpGet("Eliminar")]
      public JsonResult Eliminar(int id)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            TransportistaDTO transportistaDTO = transportistaProxy.GetById(new TransportistaDTO() { TransportistaId = id });

            AuditDTO(transportistaDTO);

            transportistaProxy.Delete(transportistaDTO);

            resultResponse.Message = SR.GetString("MG_transportista_anulado", transportistaDTO.RazonSocial);
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
      [HttpPost("ListarTransportistaUnidad")]
      public ActionResult ListarTransportistaUnidad(string jsonModel)
      {

         Logger.InfoStart();

         TransportistaViewModel model = JsonSerializer.Deserialize<TransportistaViewModel>(jsonModel);

         model.TransportistaUnidadDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.TransportistaUnidadDTO.OrderBy = "tu.marca";

         try
         {
            List<TransportistaUnidadDTO> transportistaUnidadDTOList = transportistaUnidadProxy.Search(model.TransportistaUnidadDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = transportistaUnidadDTOList.Count,
               rows = from f in transportistaUnidadDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.TransportistaUnidadId,
                            f.TransportistaId,
                            f.Marca,
                            f.Modelo,
                            f.Placa,
                            f.Placa2,
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
      [HttpPost("GrabarTransportistaUnidad")]
      public ActionResult GrabarTransportistaUnidad([FromBody] TransportistaViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            TransportistaUnidadDTO transportistaUnidadDTO = cargarTransportistaUnidadDTO(model);

            int newId = model.TransportistaUnidadDTO.Id;
            string messageResourceKey = string.Empty;
            switch (transportistaUnidadDTO.DTOState)
            {
               case DTOState.Added:
                  newId = transportistaUnidadProxy.Insert(transportistaUnidadDTO);
                  messageResourceKey = "MG_transportista_unidad_agregado";
                  break;

               case DTOState.Modified:
                  transportistaUnidadProxy.Update(transportistaUnidadDTO);
                  messageResourceKey = "MG_transportista_unidad_actualizado";
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
      private TransportistaUnidadDTO cargarTransportistaUnidadDTO(TransportistaViewModel model)
      {

         TransportistaUnidadDTO transportistaUnidadDTO;
         try
         {

            if (model.TransportistaUnidadDTO.DTOState == DTOState.Added)
            {
               transportistaUnidadDTO = model.TransportistaUnidadDTO;
            }
            else
            {
               transportistaUnidadDTO = transportistaUnidadProxy.GetById(new TransportistaUnidadDTO() { TransportistaUnidadId = model.TransportistaUnidadDTO.Id });
               transportistaUnidadDTO = model.TransportistaUnidadDTO;
            }

            transportistaUnidadDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(transportistaUnidadDTO);
         }
         catch (Exception)
         {
            throw;
         }

         return transportistaUnidadDTO;
      }
      [HttpPost("EliminarTransportistaUnidad")]
      public ActionResult EliminarTransportistaUnidad([FromBody] TransportistaViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.TransportistaUnidadDTO.DTOState = DTOState.Modified;
            AuditDTO(model.TransportistaUnidadDTO);

            transportistaUnidadProxy.Delete(model.TransportistaUnidadDTO);

            resultResponse.Message = SR.GetString("MG_transportista_unidad_anulado");
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
      [HttpPost("ListarTransportistaConductor")]
      public ActionResult ListarTransportistaConductor(string jsonModel)
      {

         Logger.InfoStart();

         TransportistaViewModel model = JsonSerializer.Deserialize<TransportistaViewModel>(jsonModel);

         model.TransportistaConductorDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.TransportistaConductorDTO.OrderBy = "tc.apellido_pat,tc.apellido_mat";

         try
         {
            List<TransportistaConductorDTO> transportistaConductorDTOList = transportistaConductorProxy.Search(model.TransportistaConductorDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = transportistaConductorDTOList.Count,
               rows = from f in transportistaConductorDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.TransportistaConductorId,
                            f.TransportistaId,
                            f.ApellidoPaterno,
                            f.ApellidoMaterno,
                            f.Nombres,
                            f.TipoDocumentoIdentidadId,
                            f.NumeroDocumentoIdentidad,
                            f.NumeroLicencia,
                            f.Telefono,
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
      [HttpPost("GrabarTransportistaConductor")]
      public ActionResult GrabarTransportistaConductor([FromBody] TransportistaViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            TransportistaConductorDTO transportistaConductorDTO = cargarTransportistaConductorDTO(model);

            int newId = model.TransportistaConductorDTO.Id;
            string messageResourceKey = string.Empty;
            switch (transportistaConductorDTO.DTOState)
            {
               case DTOState.Added:
                  newId = transportistaConductorProxy.Insert(transportistaConductorDTO);
                  messageResourceKey = "MG_transportista_conductor_agregado";
                  break;

               case DTOState.Modified:
                  transportistaConductorProxy.Update(transportistaConductorDTO);
                  messageResourceKey = "MG_transportista_conductor_actualizado";
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
      private TransportistaConductorDTO cargarTransportistaConductorDTO(TransportistaViewModel model)
      {

         TransportistaConductorDTO transportistaConductorDTO;
         try
         {

            if (model.TransportistaConductorDTO.DTOState == DTOState.Added)
            {
               transportistaConductorDTO = model.TransportistaConductorDTO;
            }
            else
            {
               transportistaConductorDTO = transportistaConductorProxy.GetById(new TransportistaConductorDTO() { TransportistaConductorId = model.TransportistaConductorDTO.Id });
               transportistaConductorDTO = model.TransportistaConductorDTO;
            }

            transportistaConductorDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(transportistaConductorDTO);
         }
         catch (Exception)
         {
            throw;
         }

         return transportistaConductorDTO;
      }
      [HttpPost("EliminarTransportistaConductor")]
      public ActionResult EliminarTransportistaConductor([FromBody] TransportistaViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.TransportistaConductorDTO.DTOState = DTOState.Modified;
            AuditDTO(model.TransportistaConductorDTO);

            transportistaConductorProxy.Delete(model.TransportistaConductorDTO);

            resultResponse.Message = SR.GetString("MG_transportista_conductor_anulado");
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
