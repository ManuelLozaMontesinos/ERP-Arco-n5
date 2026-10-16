using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.TS.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.TS.Controllers
{
   [Area("TS")]
   [Route("[controller]")]
   public class CuentaBancoController : AuthorizedController
   {
      private readonly IEmpresaCuentaBancariaProxy empresaCuentaBancariaProxy;
      private readonly IBancoProxy bancoProxy;
      private readonly IMonedaProxy monedaProxy;

      public CuentaBancoController(
         IEmpresaCuentaBancariaProxy empresaCuentaBancariaProxy,
         IBancoProxy bancoProxy,
         IMonedaProxy monedaProxy
         )
      {
         this.empresaCuentaBancariaProxy = empresaCuentaBancariaProxy;
         this.bancoProxy = bancoProxy;
         this.monedaProxy = monedaProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         CuentaBancoViewModel cuentaBancoViewModel = new();

         cargarListaBanco(cuentaBancoViewModel);

         return PartialView(cuentaBancoViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         CuentaBancoViewModel model = JsonSerializer.Deserialize<CuentaBancoViewModel>(jsonModel);

         model.EmpresaCuentaBancariaDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.EmpresaCuentaBancariaDTO.OrderBy = "b.nombre";

         model.EmpresaCuentaBancariaDTO.EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID);

         try
         {
            List<EmpresaCuentaBancariaDTO> empresaCuentaBancariaDTOList = empresaCuentaBancariaProxy.Search(model.EmpresaCuentaBancariaDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = empresaCuentaBancariaDTOList.Count,
               rows = from f in empresaCuentaBancariaDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.EmpresaCuentaBancariaId,
                            BancoNombre = f.BancoDTO.Nombre,
                            f.Nombre,
                            MonedaNombre = f.MonedaDTO.Nombre,
                            f.Cuenta,
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
      private void cargarListaBanco(CuentaBancoViewModel model)
      {

         List<BancoDTO> bancoDTOList = bancoProxy.Search(new BancoDTO() { Estado = EstadoConstante.ACTIVO });

         model.BancoList = WebHelper.ToSelectListItem(bancoDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);
      }
      private void cargarListaMoneda(CuentaBancoViewModel model)
      {

         List<MonedaDTO> monedaDTOList = monedaProxy.Search(new MonedaDTO() { Estado = EstadoConstante.ACTIVO });

         model.MonedaList = WebHelper.ToSelectListItem(monedaDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);
      }
      [HttpPost("Crear")]
      public ActionResult Crear([FromBody] CuentaBancoViewModel model)
      {
         Logger.InfoStart();

         CuentaBancoViewModel cuentaBancoViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(cuentaBancoViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, cuentaBancoViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] CuentaBancoViewModel model)
      {

         Logger.InfoStart();

         CuentaBancoViewModel cuentaBancoViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(cuentaBancoViewModel, model.EmpresaCuentaBancariaDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, cuentaBancoViewModel);
      }
      private void cargarDataParaEditar(CuentaBancoViewModel model, int id)
      {

         if (model.EditAction == ViewModelEditAction.New)
         {
            model.EmpresaCuentaBancariaDTO = new EmpresaCuentaBancariaDTO();
         }
         else
         {
            model.EmpresaCuentaBancariaDTO = empresaCuentaBancariaProxy.GetById(new EmpresaCuentaBancariaDTO() { EmpresaCuentaBancariaId = id });
         }

         cargarListaBanco(model);
         cargarListaMoneda(model);

         WebHelper.SelectedListItemSetValue(model.BancoList, model.EmpresaCuentaBancariaDTO.BancoId);
         WebHelper.SelectedListItemSetValue(model.MonedaList, model.EmpresaCuentaBancariaDTO.MonedaId);
      }
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody] CuentaBancoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            EmpresaCuentaBancariaDTO empresaCuentaBancariaDTO = cargarEmpresaCuentaBancariaDTO(model);

            int newId = model.EmpresaCuentaBancariaDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = empresaCuentaBancariaProxy.Insert(empresaCuentaBancariaDTO);
                  messageResourceKey = "TS_cuenta_bancaria_agregada";
                  break;

               case ViewModelEditAction.Edit:
                  empresaCuentaBancariaProxy.Update(empresaCuentaBancariaDTO);
                  messageResourceKey = "TS_cuenta_bancaria_actualizada";
                  break;
            }

            empresaCuentaBancariaDTO = empresaCuentaBancariaProxy.GetById(new EmpresaCuentaBancariaDTO() { EmpresaCuentaBancariaId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, empresaCuentaBancariaDTO.Nombre);
            resultResponse.Data = new
            {
               empresaCuentaBancariaDTO.Id
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
      private EmpresaCuentaBancariaDTO cargarEmpresaCuentaBancariaDTO(CuentaBancoViewModel model)
      {

         EmpresaCuentaBancariaDTO empresaCuentaBancariaDTO;
         try
         {

            if (model.EditAction == ViewModelEditAction.New)
            {
               empresaCuentaBancariaDTO = model.EmpresaCuentaBancariaDTO;
               empresaCuentaBancariaDTO.EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID);
            }
            else
            {
               empresaCuentaBancariaDTO = empresaCuentaBancariaProxy.GetById(new EmpresaCuentaBancariaDTO() { EmpresaCuentaBancariaId = model.EmpresaCuentaBancariaDTO.Id });
               empresaCuentaBancariaDTO = model.EmpresaCuentaBancariaDTO;
            }

            empresaCuentaBancariaDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(empresaCuentaBancariaDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return empresaCuentaBancariaDTO;
      }
      [HttpGet("Eliminar")]
      public JsonResult Eliminar(int id)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            EmpresaCuentaBancariaDTO empresaCuentaBancariaDTO = empresaCuentaBancariaProxy.GetById(new EmpresaCuentaBancariaDTO() { EmpresaCuentaBancariaId = id });

            AuditDTO(empresaCuentaBancariaDTO);

            empresaCuentaBancariaProxy.Delete(empresaCuentaBancariaDTO);

            resultResponse.Message = SR.GetString("TS_cuenta_bancaria_anulada", empresaCuentaBancariaDTO.Nombre);
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

   }
}
