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
   public class IntegraCapitalController : AuthorizedController
   {
      private readonly IProcesoIntegracionProxy procesoIntegracionProxy;

      public IntegraCapitalController(
         IProcesoIntegracionProxy procesoIntegracionProxy
         )
      {
         this.procesoIntegracionProxy = procesoIntegracionProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         IntegraCapitalViewModel integraCapitalViewModel = new();

         ProcesoIntegracionDTO procesoIntegracionDTO = new()
         {
            ModuloDTO = new ModuloDTO() { Codigo = ModuloConstante.CAPITAL_HUMANO }
         };

         List<ProcesoIntegracionDTO> procesoIntegracionDTOList = procesoIntegracionProxy.Search(procesoIntegracionDTO);
         integraCapitalViewModel.ProcesoIntegracionDTOList = procesoIntegracionDTOList;

         integraCapitalViewModel.MenuItemList = new List<MenuItem>() {
            new MenuItem() { Key="todos", Content="Ejecutar Todos"},
            new MenuItem() { Key="ejecutar", Content="Ejecutar Seleccionados"}};

         return PartialView(integraCapitalViewModel);
      }
      [HttpPost("EjecutarProceso")]
      public ActionResult EjecutarProceso([FromBody]IntegraCapitalViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            int empresaId = ObtenerEmpresaId(model.ViewModelState.CID);
            ProcesoIntegracionDTO procesoIntegracionDTO = procesoIntegracionProxy.GetById(model.ProcesoIntegracionDTO);

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