using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service.DTO;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.IV.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.IV.Controllers
{
   [Area("IV")]
   [Route("[controller]")]
   public class TransaccionController : AuthorizedController
   {

      private readonly ITransaccionAlmacenProxy transaccionAlmacenProxy;
      private readonly ITipoMovimientoAlmacenProxy tipoMovimientoAlmacenProxy;
      private readonly IOrigenRegistroProxy origenRegistroProxy;
      private readonly ITipoOperacionProxy tipoOperacionProxy;

      public TransaccionController(
         ITransaccionAlmacenProxy transaccionAlmacenProxy,
         ITipoMovimientoAlmacenProxy tipoMovimientoAlmacenProxy,
         IOrigenRegistroProxy origenRegistroProxy,
         ITipoOperacionProxy tipoOperacionProxy
         )
      {
         this.transaccionAlmacenProxy = transaccionAlmacenProxy;
         this.tipoMovimientoAlmacenProxy = tipoMovimientoAlmacenProxy;
         this.origenRegistroProxy = origenRegistroProxy;
         this.tipoOperacionProxy = tipoOperacionProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         TransaccionViewModel transaccionViewModel = new();

         cargarListas(transaccionViewModel);

         return PartialView(transaccionViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         TransaccionViewModel model = JsonSerializer.Deserialize<TransaccionViewModel>(jsonModel);

         model.TransaccionAlmacenDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.TransaccionAlmacenDTO.OrderBy = "ta.nombre";

         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

         model.TransaccionAlmacenDTO.EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId;

         try
         {
            List<TransaccionAlmacenDTO> transaccionAlmacenDTOList = transaccionAlmacenProxy.Search(model.TransaccionAlmacenDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = transaccionAlmacenDTOList.Count,
               rows = from f in transaccionAlmacenDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.TransaccionAlmacenId,
                            f.Codigo,
                            f.Nombre,
                            TipoMovimientoAlmacenNombre = f?.TipoMovimientoAlmacenDTO?.Nombre,
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
      public ActionResult Crear([FromBody] TransaccionViewModel model)
      {
         Logger.InfoStart();

         TransaccionViewModel transaccionViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(transaccionViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, transaccionViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] TransaccionViewModel model)
      {

         Logger.InfoStart();

         TransaccionViewModel transaccionViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(transaccionViewModel, model.TransaccionAlmacenDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, transaccionViewModel);
      }
      private void cargarDataParaEditar(TransaccionViewModel model, int id)
      {

         TransaccionAlmacenDTO transaccionAlmacenDTO;
         if (model.EditAction == ViewModelEditAction.New)
         {
            transaccionAlmacenDTO = new TransaccionAlmacenDTO();
         }
         else
         {
            transaccionAlmacenDTO = transaccionAlmacenProxy.GetById(new TransaccionAlmacenDTO() { TransaccionAlmacenId = id });
         }

         transaccionAlmacenDTO.EsCompra = transaccionAlmacenDTO.EsCompra == SiNoConstante.S ? EstadoConstante.ACTIVO : EstadoConstante.INACTIVO;

         model.TransaccionAlmacenDTO = transaccionAlmacenDTO;

         cargarListas(model);

         WebHelper.SelectedListItemSetValue(model.TipoMovimientoAlmacenList, transaccionAlmacenDTO.TipoMovimientoAlmacenId);
         WebHelper.SelectedListItemSetValue(model.TipoOperacionList, transaccionAlmacenDTO.TipoOperacionId);
      }
      private void cargarListas(TransaccionViewModel model)
      {
         List<TipoMovimientoAlmacenDTO> tipoMovimientoAlmacenDTOList = tipoMovimientoAlmacenProxy.Search(new TipoMovimientoAlmacenDTO() { Estado = EstadoConstante.ACTIVO });
         model.TipoMovimientoAlmacenList = WebHelper.ToSelectListItem(tipoMovimientoAlmacenDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Blank);

         List<TipoOperacionDTO> tipoOperacionDTOList = tipoOperacionProxy.Search(new TipoOperacionDTO() { Estado = EstadoConstante.ACTIVO });
         model.TipoOperacionList = WebHelper.ToSelectListItem(tipoOperacionDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Blank);
      }
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody] TransaccionViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            TransaccionAlmacenDTO transaccionAlmacenDTO = cargarTransaccionAlmacenDTO(model);

            int newId = model.TransaccionAlmacenDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = transaccionAlmacenProxy.Insert(transaccionAlmacenDTO);
                  messageResourceKey = "IV_transaccion_almacen_agregada";
                  break;

               case ViewModelEditAction.Edit:
                  transaccionAlmacenProxy.Update(transaccionAlmacenDTO);
                  messageResourceKey = "IV_transaccion_almacen_actualizada";
                  break;
            }

            transaccionAlmacenDTO = transaccionAlmacenProxy.GetById(new TransaccionAlmacenDTO() { TransaccionAlmacenId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, transaccionAlmacenDTO.Nombre);
            resultResponse.Data = new
            {
               transaccionAlmacenDTO.Id,
               transaccionAlmacenDTO.Codigo
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
      private TransaccionAlmacenDTO cargarTransaccionAlmacenDTO(TransaccionViewModel model)
      {

         TransaccionAlmacenDTO transaccionAlmacenDTO;
         try
         {

            transaccionAlmacenDTO = model.TransaccionAlmacenDTO;

            if (model.EditAction == ViewModelEditAction.New)
            {
               PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);
               transaccionAlmacenDTO.EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId;

               OrigenRegistroDTO origenRegistroDTO = origenRegistroProxy
                  .ObtenerPorCodigo(new OrigenRegistroDTO() { Codigo = OrigenRegistroConstante.USUARIO });
               transaccionAlmacenDTO.OrigenRegistroId = origenRegistroDTO.Id;
            }

            if (string.IsNullOrEmpty(transaccionAlmacenDTO.EsConfigurable))
               transaccionAlmacenDTO.EsConfigurable = SiNoConstante.S;

            if (string.IsNullOrEmpty(transaccionAlmacenDTO.TieneMantenimiento))
               transaccionAlmacenDTO.TieneMantenimiento = SiNoConstante.S;

            transaccionAlmacenDTO.Estado = EstadoConstante.ACTIVO;
            AuditDTO(transaccionAlmacenDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return transaccionAlmacenDTO;
      }
      [HttpPost("Eliminar")]
      public ActionResult Eliminar([FromBody] TransaccionViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.TransaccionAlmacenDTO.DTOState = DTOState.Modified;

            AuditDTO(model.TransaccionAlmacenDTO);

            transaccionAlmacenProxy.Delete(model.TransaccionAlmacenDTO);

            resultResponse.Message = SR.GetString("IV_transaccion_almacen_anulada", model.TransaccionAlmacenDTO.Nombre);
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