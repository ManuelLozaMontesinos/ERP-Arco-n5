using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.CM.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.CM.Controllers
{
   [Area("CM")]
   [Route("[controller]")]
   public class PuntoPedidoController : AuthorizedController
   {
      private readonly IPeriodoEmpresaSedeProxy periodoEmpresaSedeProxy;

      public PuntoPedidoController(
         IPeriodoEmpresaSedeProxy periodoEmpresaSedeProxy
         )
      {
         this.periodoEmpresaSedeProxy = periodoEmpresaSedeProxy;
      }

      [Route("[action]")]
      public IActionResult Index()
      {
         PuntoPedidoViewModel puntoPedidoViewModel = new();

         cargarListaPeriodoEmpresSede(puntoPedidoViewModel);
         cargarGridModel(puntoPedidoViewModel);

         return PartialView(puntoPedidoViewModel);
      }
      private void cargarListaPeriodoEmpresSede(PuntoPedidoViewModel model)
      {

         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO();
         List<PeriodoEmpresaSedeDTO> periodoEmpresaSedeDTOList = periodoEmpresaSedeProxy.Search(new PeriodoEmpresaSedeDTO()
         {
            EmpresaSedeDTO = periodoEmpresaSedeDTO.EmpresaSedeDTO,
            PeriodoDTO = periodoEmpresaSedeDTO.PeriodoDTO,
            PeriodoId = periodoEmpresaSedeDTO.PeriodoId,
            Estado = EstadoConstante.ACTIVO
         });

         model.PeriodoEmpresaSedeList = WebHelper.ToSelectListItem(periodoEmpresaSedeDTOList, x => x.Id.ToString(), x => x.EmpresaSedeDTO.Nombre,
            SelectListFirstOptionType.Select);

      }
      private void cargarGridModel(PuntoPedidoViewModel model)
      {
         string colName = ",Código,Descripción,U.M.,,Proveedor,,,,Stock Min.,Saldo";

         string colModel = "["
            + "{\"name\":\"ProductoId\",\"hidden\":true},"
            + "{\"name\":\"Codigo\",\"width\":\"80\",\"align\":\"left\"},"
            + "{\"name\":\"Descripcion\",\"width\":\"250\",\"align\":\"left\"},"
            + "{\"name\":\"UnidadMedidaNombre\",\"width\":\"45\",\"align\":\"left\"},"
            + "{\"name\":\"ProveedorId\",\"hidden\":true},"
            + "{\"name\":\"RazonSocial\",\"width\":\"200\",\"align\":\"left\"},"
            + "{\"name\":\"PesoNeto\",\"hidden\":true},"
            + "{\"name\":\"PesoBruto\",\"hidden\":true},"
            + "{\"name\":\"MasterPack\",\"hidden\":true},"
            + "{\"name\":\"StockMinimo\",\"width\":\"80\",\"align\":\"right\",\"editable\":false,\"formatter\":\"number\",\"sorttype\":\"number\"},"
            + "{\"name\":\"Saldo\",\"width\":\"80\",\"align\":\"right\",\"editable\":false,\"formatter\":\"number\",\"sorttype\":\"number\"}";

         colModel += "]";

         model.GridColNames = colName;
         model.GridColModel = colModel;
      }
   }
}