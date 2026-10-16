using ESD.Core.Logging;
using ESD.Core.Service;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.AS.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.AS.Controllers
{
   [Area("AS")]
   [Route("[controller]")]
   public class IntegraVentaController : AuthorizedController
   {
      private readonly IProcesoIntegracionProxy procesoIntegracionProxy;

      public IntegraVentaController(
         IProcesoIntegracionProxy procesoIntegracionProxy
         )
      {
         this.procesoIntegracionProxy = procesoIntegracionProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         IntegraVentaViewModel integraVentaViewModel = new();

         ProcesoIntegracionDTO procesoIntegracionDTO = new()
         {
            ModuloDTO = new ModuloDTO() { Codigo = ModuloConstante.VENTA }
         };

         List<ProcesoIntegracionDTO> procesoIntegracionDTOList = procesoIntegracionProxy.Search(procesoIntegracionDTO);
         integraVentaViewModel.ProcesoIntegracionDTOList = procesoIntegracionDTOList;

         integraVentaViewModel.MenuItemList = new List<MenuItem>() {
            new MenuItem() { Key="todos", Content="Ejecutar Todos"},
            new MenuItem() { Key="ejecutar", Content="Ejecutar Seleccionados"}};

         return PartialView(integraVentaViewModel);
      }
      [HttpPost("EjecutarProceso")]
      public ActionResult EjecutarProceso([FromBody]IntegraVentaViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {

            ProcesoIntegracionDTO procesoIntegracionDTO = procesoIntegracionProxy.GetById(model.ProcesoIntegracionDTO);

            switch (procesoIntegracionDTO.TipoProcesoIntegracionDTO.Codigo)
            {
               case TipoProcesoIntegracionConstante.JOB_BASE_DATOS:
                  procesoIntegracionProxy.EjecutarJob(procesoIntegracionDTO);
                  resultResponse.Message = SR.GetString("AS_proceso_integracion_ejecutado", procesoIntegracionDTO.Descripcion);
                  break;

               case TipoProcesoIntegracionConstante.SERVICIO_REST:
                  int empresaId = ObtenerEmpresaId(model.ViewModelState.CID);
                  var client = new RestClient(procesoIntegracionDTO.Nombre)
                  {
                     Timeout = -1
                  };
                  var request = new RestRequest(Method.POST);
                  request.AddHeader("Content-Type", "application/json");
                  request.AddParameter("application/json", empresaId.ToString(), ParameterType.RequestBody);
                  IRestResponse response = client.Execute(request);

                  ServiceResponse serviceResponse = JsonConvert.DeserializeObject<ServiceResponse>(response.Content);
                  resultResponse.Message = serviceResponse.Message;
                  break;
            }
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