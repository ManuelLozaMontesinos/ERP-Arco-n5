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
   public class ParametroSistemaController : AuthorizedController
   {
      private readonly IParametroSistemaProxy parametroSistemaProxy;

      public ParametroSistemaController(
         IParametroSistemaProxy parametroSistemaProxy
         )
      {
         this.parametroSistemaProxy = parametroSistemaProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         ParametroSistemaViewModel parametroSistemaViewModel = new();

         return PartialView(parametroSistemaViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         ParametroSistemaViewModel model = JsonSerializer.Deserialize<ParametroSistemaViewModel>(jsonModel);

         model.ParametroSistemaDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.ParametroSistemaDTO.OrderBy = "gps.nombre,ps.nombre";

         try
         {
            List<ParametroSistemaDTO> parametroSistemaDTOList = parametroSistemaProxy.Search(model.ParametroSistemaDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = parametroSistemaDTOList.Count,
               rows = from f in parametroSistemaDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.ParametroSistemaId,
                            f.Codigo,
                            f.Nombre,
                            GrupoParametroNombre = f.GrupoParametroSistemaDTO.Nombre,
                            Valor = crearValor(f),
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
      private string crearValor(ParametroSistemaDTO parametroSistemaDTO)
      {
         string valorString = string.Empty;

         switch (parametroSistemaDTO.TipoDatoDTO.Codigo)
         {
            case TipoDatoConstante.CADENA:
               valorString = parametroSistemaDTO.ValorCadena;
               break;

            case TipoDatoConstante.CADENA_OCULTA:
               valorString = new string('*',20);
               break;

            case TipoDatoConstante.ENTERO:
               valorString = Convert.ToInt32(parametroSistemaDTO.ValorNumerico).ToString();
               break;

            case TipoDatoConstante.DECIMAL:
               valorString = parametroSistemaDTO.ValorNumerico.ToString("N2");
               break;

            case TipoDatoConstante.RANGO_HORA:
               valorString = string.Concat(parametroSistemaDTO.RangoNumericoDesde.ToString("00.00").Replace(SeparadorConstante.PUNTO, SeparadorConstante.DOS_PUNTOS),
                  SeparadorConstante.ESPACIO,SeparadorConstante.GUION,
                  SeparadorConstante.ESPACIO, parametroSistemaDTO.RangoNumericoHasta.ToString("00.00").Replace(SeparadorConstante.PUNTO, SeparadorConstante.DOS_PUNTOS)
                  );
               break;
         }


         return valorString;
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] ParametroSistemaViewModel model)
      {

         Logger.InfoStart();

         ParametroSistemaViewModel parametroSistemaViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(parametroSistemaViewModel, model.ParametroSistemaDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, parametroSistemaViewModel);
      }
      private void cargarDataParaEditar(ParametroSistemaViewModel model, int id)
      {

         ParametroSistemaDTO parametroSistemaDTO = parametroSistemaProxy.GetById(new ParametroSistemaDTO() { ParametroSistemaId = id });

         switch (parametroSistemaDTO.TipoDatoDTO.Codigo)
         {
            case TipoDatoConstante.CADENA:
               model.ValorCadena = parametroSistemaDTO.ValorCadena;
               break;

            case TipoDatoConstante.CADENA_OCULTA:
               model.ValorCadenaOculta = parametroSistemaDTO.ValorCadena;
               break;

            case TipoDatoConstante.ENTERO:
               model.ValorCadenaEntero = Convert.ToInt32(parametroSistemaDTO.ValorNumerico);
               break;

            case TipoDatoConstante.DECIMAL:
               model.ValorCadenaDecimal = parametroSistemaDTO.ValorNumerico;
               break;

            case TipoDatoConstante.RANGO_HORA:
               model.ValorHoraDesde = parametroSistemaDTO.RangoNumericoDesde.ToString("N2");
               model.ValorHoraHasta = parametroSistemaDTO.RangoNumericoHasta.ToString("N2");
               break;
         }

         model.ParametroSistemaDTO = parametroSistemaDTO;
      }
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody] ParametroSistemaViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            ParametroSistemaDTO parametroSistemaDTO = cargarParametroSistemaDTO(model);

            int newId = model.ParametroSistemaDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = parametroSistemaProxy.Insert(parametroSistemaDTO);
                  messageResourceKey = "AS_parametro_sistema_actualizado";
                  break;

               case ViewModelEditAction.Edit:
                  parametroSistemaProxy.Update(parametroSistemaDTO);
                  messageResourceKey = "AS_parametro_sistema_actualizado";
                  break;
            }

            parametroSistemaDTO = parametroSistemaProxy.GetById(new ParametroSistemaDTO() { ParametroSistemaId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, parametroSistemaDTO.Nombre);
            resultResponse.Data = new
            {
               parametroSistemaDTO.Id
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
      private ParametroSistemaDTO cargarParametroSistemaDTO(ParametroSistemaViewModel model)
      {

         ParametroSistemaDTO parametroSistemaDTO = null;

         try
         {
            parametroSistemaDTO = parametroSistemaProxy.GetById(new ParametroSistemaDTO() { ParametroSistemaId = model.ParametroSistemaDTO.Id });

            switch (parametroSistemaDTO.TipoDatoDTO.Codigo)
            {
               case TipoDatoConstante.CADENA:
                  parametroSistemaDTO.ValorCadena = model.ValorCadena;
                  break;

               case TipoDatoConstante.CADENA_OCULTA:
                  parametroSistemaDTO.ValorCadena = model.ValorCadenaOculta;
                  break;

               case TipoDatoConstante.ENTERO:
                  parametroSistemaDTO.ValorNumerico = model.ValorCadenaEntero;
                  break;

               case TipoDatoConstante.DECIMAL:
                  parametroSistemaDTO.ValorNumerico = model.ValorCadenaDecimal;
                  break;

               case TipoDatoConstante.RANGO_HORA:
                  parametroSistemaDTO.RangoNumericoDesde = Convert.ToDecimal(model.ValorHoraDesde.Replace(SeparadorConstante.DOS_PUNTOS,SeparadorConstante.PUNTO));
                  parametroSistemaDTO.RangoNumericoHasta = Convert.ToDecimal(model.ValorHoraHasta.Replace(SeparadorConstante.DOS_PUNTOS, SeparadorConstante.PUNTO));
                  break;
            }

            parametroSistemaDTO.Estado = EstadoConstante.ACTIVO;

         }
         catch (Exception)
         {
            throw;
         }

         return parametroSistemaDTO;
      }
   }
}
