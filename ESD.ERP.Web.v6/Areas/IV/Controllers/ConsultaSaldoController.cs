using ESD.Core;
using ESD.Core.Logging;
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
   public class ConsultaSaldoController : AuthorizedController
   {
      private readonly IAlmacenProxy almacenProxy;
      private readonly IMovimientoAlmacenSaldoProxy movimientoAlmacenSaldoProxy;
      private readonly IMesContableProxy mesContableProxy;
      
      public ConsultaSaldoController(
         IAlmacenProxy almacenProxy,
         IMovimientoAlmacenSaldoProxy movimientoAlmacenSaldoProxy,
         IMesContableProxy mesContableProxy
         )
      {
         this.almacenProxy = almacenProxy;
         this.movimientoAlmacenSaldoProxy = movimientoAlmacenSaldoProxy;
         this.mesContableProxy = mesContableProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         ConsultaSaldoViewModel consultaSaldoViewModel = new();

         cargarListas(consultaSaldoViewModel);
         cargarGridModel(consultaSaldoViewModel);

         return PartialView(consultaSaldoViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         ConsultaSaldoViewModel model = JsonSerializer.Deserialize<ConsultaSaldoViewModel>(jsonModel);

         model.MovimientoAlmacenSaldoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.MovimientoAlmacenSaldoDTO.OrderBy = "p.producto";

         model.MovimientoAlmacenSaldoDTO.PeriodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(model.ViewModelState.CID);

         try
         {
            string result = movimientoAlmacenSaldoProxy.ListarSaldoMultiAlmacenGrid(model.MovimientoAlmacenSaldoDTO);

            object jsonData = JsonSerializer.Deserialize(result, typeof(object));

            Logger.InfoFinish();
            return Json(jsonData, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }
      }
      private void cargarGridModel(ConsultaSaldoViewModel model)
      {
         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

         List<AlmacenDTO> almacenDTOList = almacenProxy.Search(new AlmacenDTO()
         {
            EmpresaSedeDTO = new EmpresaSedeDTO()
            {
               EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId,
            },
            EmpresaSedeId = periodoEmpresaSedeDTO.EmpresaSedeId,
            Estado = EstadoConstante.ACTIVO
         });

         string colName = ",Código,Descripción,Cod.Referencial,U.M.,Saldo,Reserva";

         foreach (AlmacenDTO almacenDTO in almacenDTOList)
            colName += string.Concat(SeparadorConstante.COMA, almacenDTO.Abreviatura);

         string colModel = "["
            + "{\"name\":\"MovimientoAlmacenSaldoId\",\"hidden\":true},"
            + "{\"name\":\"Codigo\",\"width\":\"100\",\"align\":\"left\"},"
            + "{\"name\":\"Descripcion\",\"width\":\"400\",\"align\":\"left\"},"
            + "{\"name\":\"CodigoReferencial\",\"width\":\"120\",\"align\":\"left\"},"
            + "{\"name\":\"UnidadMedidaNombre\",\"width\":\"80\",\"align\":\"left\"},"
            + "{\"name\":\"CantidadSaldo\",\"width\":\"65\",\"align\":\"right\",\"editable\":false,\"formatter\":\"number\",\"sorttype\":\"number\"},"
            + "{\"name\":\"CantidadReserva\",\"width\":\"65\",\"align\":\"right\",\"editable\":false,\"formatter\":\"number\",\"sorttype\":\"number\"}";

         foreach (AlmacenDTO almacenDTO in almacenDTOList)
         {
            colModel += ",{\"name\":\"" + almacenDTO.Id.ToString()
               + "\",\"width\":\"70\",\"align\":\"right\",\"editable\":false,\"formatter\":\"number\",\"sorttype\":\"number\"}";
         }

         colModel += "]";

         model.GridColNames = colName;
         model.GridColModel = colModel;
      }
      private void cargarListas(ConsultaSaldoViewModel model)
      {
         List<MesContableDTO> mesContableDTOList = mesContableProxy.Search(new MesContableDTO()
         {
            OrderBy = "mc.mes_contable",
            NotMesContableList = new List<string>() { MesContableConstante.APERTURA, MesContableConstante.CIERRE },
            Estado = EstadoConstante.ACTIVO,
         });
         model.MesContableList = WebHelper.ToSelectListItem(mesContableDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         MesContableDTO mesContableDTO = mesContableDTOList.Where(x => x.Codigo == string.Format(StringFormatConstant.DATE_MONTH, DateTime.Now)).FirstOrDefault();

         WebHelper.SelectedListItemSetValue(model.MesContableList, mesContableDTO.MesContableId);
         model.MesContableId = mesContableDTO.MesContableId.ToString();
      }

   }
}