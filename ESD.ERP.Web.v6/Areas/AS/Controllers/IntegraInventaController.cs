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
   public class IntegraInventaController : AuthorizedController
   {
      private readonly IProcesoIntegracionProxy procesoIntegracionProxy;
      private readonly IParametroSistemaProxy parametroSistemaProxy;

      public IntegraInventaController(
         IProcesoIntegracionProxy procesoIntegracionProxy,
         IParametroSistemaProxy parametroSistemaProxy
         )
      {
         this.procesoIntegracionProxy = procesoIntegracionProxy;
         this.parametroSistemaProxy = parametroSistemaProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         IntegraInventaViewModel integraInventaViewModel = new();

         ProcesoIntegracionDTO procesoIntegracionDTO = new()
         {
            OrderBy = "pi.proce_integra",
            ModuloDTO = new ModuloDTO() { Codigo = ModuloConstante.INVENTARIOS }
         };

         List<ProcesoIntegracionDTO> procesoIntegracionDTOList = procesoIntegracionProxy.Search(procesoIntegracionDTO);
         integraInventaViewModel.ProcesoIntegracionDTOList = procesoIntegracionDTOList;

         integraInventaViewModel.MenuItemList = new List<MenuItem>() {
            new MenuItem() { Key="todos", Content="Ejecutar Todos"},
            new MenuItem() { Key="ejecutar", Content="Ejecutar Seleccionados"}};

         ParametroSistemaDTO parametroSistemaDTO = parametroSistemaProxy.SingleSearch(new ParametroSistemaDTO() { Codigo = ParametroSistemaConstante.ALERTA_STOCK_MINIMO });

         integraInventaViewModel.ParametroSistema01 = parametroSistemaDTO.ValorCadena;

         return PartialView(integraInventaViewModel);
      }
      [HttpPost("EjecutarProceso")]
      public ActionResult EjecutarProceso([FromBody]IntegraInventaViewModel model)
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
      [HttpPost("GrabarParametroSistema01")]
      public ActionResult GrabarParametroSistema01([FromBody] IntegraInventaViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            ParametroSistemaDTO parametroSistemaDTO = parametroSistemaProxy.SingleSearch(new ParametroSistemaDTO() { Codigo = ParametroSistemaConstante.ALERTA_STOCK_MINIMO });

            parametroSistemaDTO.ValorCadena = model.ParametroSistema01;

            parametroSistemaProxy.Update(parametroSistemaDTO);

            resultResponse.Message = SR.GetString("AS_proceso_integracion_correo_actualizado");
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