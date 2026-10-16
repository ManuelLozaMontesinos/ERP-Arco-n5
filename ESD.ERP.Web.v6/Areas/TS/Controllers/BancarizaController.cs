using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service.DTO;
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
   public class BancarizaController : AuthorizedController
   {
      private readonly IPeriodoEmpresaSedeProxy periodoEmpresaSedeProxy;
      private readonly IComprobanteProxy comprobanteProxy;
      private readonly IComprobanteCobranzaProxy comprobanteCobranzaProxy;
      private readonly IParametroSistemaProxy parametroSistemaProxy;
      private readonly IListaPrecioProxy listaPrecioProxy;
      private readonly ITipoCondicionPagoProxy tipoCondicionPagoProxy;
      private readonly IMedioPagoProxy medioPagoProxy;
      private readonly IEmpresaCuentaBancariaProxy empresaCuentaBancariaProxy;

      public BancarizaController(
         IPeriodoEmpresaSedeProxy periodoEmpresaSedeProxy,
         IComprobanteProxy comprobanteProxy,
         IComprobanteCobranzaProxy comprobanteCobranzaProxy,
         IParametroSistemaProxy parametroSistemaProxy,
         IListaPrecioProxy listaPrecioProxy,
         ITipoCondicionPagoProxy tipoCondicionPagoProxy,
         IMedioPagoProxy medioPagoProxy,
         IEmpresaCuentaBancariaProxy empresaCuentaBancariaProxy
         )
      {
         this.periodoEmpresaSedeProxy = periodoEmpresaSedeProxy;
         this.comprobanteProxy = comprobanteProxy;
         this.comprobanteCobranzaProxy = comprobanteCobranzaProxy;
         this.parametroSistemaProxy = parametroSistemaProxy;
         this.listaPrecioProxy = listaPrecioProxy;
         this.tipoCondicionPagoProxy = tipoCondicionPagoProxy;
         this.medioPagoProxy = medioPagoProxy;
         this.empresaCuentaBancariaProxy = empresaCuentaBancariaProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         BancarizaViewModel detraccionViewModel = new BancarizaViewModel();

         cargarListaPeriodoEmpresaSede(detraccionViewModel);

         detraccionViewModel.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { MenuItemType = MenuItemType.Edit }
         };

         return PartialView(detraccionViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         BancarizaViewModel model = JsonSerializer.Deserialize<BancarizaViewModel>(jsonModel);

         ParametroSistemaDTO parametroSistemaDTO = parametroSistemaProxy.SingleSearch(new ParametroSistemaDTO()
         {
            Codigo = ParametroSistemaConstante.VENTAS_BANCARIZACION
         });

         model.ComprobanteDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.ComprobanteDTO.OrderBy = "c.fecha desc,c.numero_opera desc";

         model.ComprobanteDTO.TipoDocumentoComercialList = new List<string>() {
            TipoDocumentoComercialConstante.FACTURA,TipoDocumentoComercialConstante.BOLETA_VENTA
         };
         model.ComprobanteDTO.MontoTotalMayorIgual = parametroSistemaDTO.ValorNumerico;
         model.ComprobanteDTO.EstadoComprobanteCodigoList = new List<string>() { EstadoComprobanteConstante.EMITIDO };
         model.ComprobanteDTO.Estado = EstadoConstante.ACTIVO;

         try
         {
            List<ComprobanteDTO> comprobanteDTOList = comprobanteProxy.Search(model.ComprobanteDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = comprobanteDTOList.Count,
               rows = from f in comprobanteDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.ComprobanteId,
                            TipoDocumentoComercialNombre = f.TipoDocumentoComercialDTO.Abreviatura,
                            f.SerieNumero,
                            Fecha = f.Fecha.ToShortDateString(),
                            f.ProveedorDTO.RazonSocial,
                            f.ProveedorDTO.NumeroDocumentoIdentidad,
                            Moneda = f.MonedaDTO.Simbolo,
                            f.MontoTotal,
                            f.Bancarizado,
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
      private void cargarListaPeriodoEmpresaSede(BancarizaViewModel model)
      {

         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

         List<PeriodoEmpresaSedeDTO> periodoEmpresaSedeDTOList = periodoEmpresaSedeProxy.Search(new PeriodoEmpresaSedeDTO() { 
            PeriodoId = periodoEmpresaSedeDTO.PeriodoId,
            EmpresaSedeDTO = periodoEmpresaSedeDTO.EmpresaSedeDTO,
            Estado = EstadoConstante.ACTIVO 
         });
         model.PeriodoEmpresaSedeList = WebHelper.ToSelectListItem(periodoEmpresaSedeDTOList, x => x.Id.ToString(), x => x.EmpresaSedeDTO.Nombre,
            SelectListFirstOptionType.Select);

         WebHelper.SelectedListItemSetValue(model.PeriodoEmpresaSedeList, periodoEmpresaSedeDTO.PeriodoEmpresaSedeId);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] BancarizaViewModel model)
      {

         Logger.InfoStart();

         BancarizaViewModel bancarizaViewModel = new BancarizaViewModel
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(bancarizaViewModel, model.ComprobanteDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, bancarizaViewModel);
      }
      private void cargarDataParaEditar(BancarizaViewModel model, int id)
      {

         ComprobanteDTO comprobanteDTO = comprobanteProxy.GetById(new ComprobanteDTO() { ComprobanteId = id });

         model.ComprobanteDTO = comprobanteDTO;
         cargarListas(model);

         WebHelper.SelectedListItemSetValue(model.ListaPrecioList, comprobanteDTO.ListaPrecioId);
         WebHelper.SelectedListItemSetValue(model.TipoCondicionPagoList, comprobanteDTO.TipoCondicionPagoId);
         WebHelper.SelectedListItemSetValue(model.MedioPagoList, comprobanteDTO.MedioPagoId);
      }
      private void cargarListas(BancarizaViewModel model)
      {
         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

         List<ListaPrecioDTO> listaPrecioDTOList = obtenerListaPrecio(periodoEmpresaSedeDTO, model.ComprobanteDTO.MonedaId);

         model.ListaPrecioList = WebHelper.ToSelectListItem(listaPrecioDTOList, x => x.Id.ToString(), x => x.NombreMoneda,
            SelectListFirstOptionType.Select);

         TipoCondicionPagoDTO tipoCondicionPagoDTO = new TipoCondicionPagoDTO() { Estado = EstadoConstante.ACTIVO };
         List<TipoCondicionPagoDTO> tipoCondicionPagoDTOList = tipoCondicionPagoProxy.Search(tipoCondicionPagoDTO);
         model.TipoCondicionPagoList = WebHelper.ToSelectListItem(tipoCondicionPagoDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<MedioPagoDTO> medioPagoDTOList = medioPagoProxy.Search(new MedioPagoDTO()
         {
            EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId,
            Estado = EstadoConstante.ACTIVO
         });
         model.MedioPagoList = WebHelper.ToSelectListItem(medioPagoDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<EmpresaCuentaBancariaDTO> empresaCuentaBancariaDTOList = empresaCuentaBancariaProxy.Search(new EmpresaCuentaBancariaDTO()
         {
            EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId,
            MonedaId = model.ComprobanteDTO.MonedaId,
            Estado = EstadoConstante.ACTIVO
         });
         model.EmpresaCuentaBancariaString = string.Join(SeparadorConstante.PUNTO_COMA, empresaCuentaBancariaDTOList.Select(x => x.Id.ToString()
            + SeparadorConstante.DOS_PUNTOS + x.BancoNumero));
      }
      private List<ListaPrecioDTO> obtenerListaPrecio(PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO, int monedaId)
      {
         List<ListaPrecioDTO> listaPrecioDTOList = listaPrecioProxy.Search(new ListaPrecioDTO()
         {
            EmpresaSedeDTO = periodoEmpresaSedeDTO.EmpresaSedeDTO,
            EmpresaSedeId = periodoEmpresaSedeDTO.EmpresaSedeId,
            MonedaId = monedaId,
            Estado = EstadoConstante.ACTIVO
         });

         return listaPrecioDTOList;
      }
      [HttpPost("ListarComprobanteCobranza")]
      public ActionResult ListarComprobanteCobranza(string jsonModel)
      {

         Logger.InfoStart();

         BancarizaViewModel model = JsonSerializer.Deserialize<BancarizaViewModel>(jsonModel);

         model.ComprobanteCobranzaDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.ComprobanteCobranzaDTO.OrderBy = "cc.fecha";

         try
         {
            List<ComprobanteCobranzaDTO> comprobanteCobranzaDTOList = comprobanteCobranzaProxy.Search(model.ComprobanteCobranzaDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = comprobanteCobranzaDTOList.Count,
               rows = from f in comprobanteCobranzaDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.ComprobanteCobranzaId,
                            f.ComprobanteId,
                            Fecha = f.Fecha.ToShortDateString(),
                            f.NumeroOperacion,
                            f.Monto,
                            f.EmpresaCuentaBancariaId,
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
      [HttpPost("GrabarComprobanteCobranza")]
      public ActionResult GrabarComprobanteCobranza([FromBody] BancarizaViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            ComprobanteCobranzaDTO comprobanteCobranzaDTO = model.ComprobanteCobranzaDTO;
            comprobanteCobranzaDTO.Estado = EstadoConstante.ACTIVO;
            AuditDTO(comprobanteCobranzaDTO);

            string messageResourceKey = string.Empty;
            switch (comprobanteCobranzaDTO.DTOState)
            {
               case DTOState.Added:
                  comprobanteCobranzaProxy.Insert(comprobanteCobranzaDTO);
                  messageResourceKey = "TS_comprobante_cobranza_agregada";
                  break;

               case DTOState.Modified:
                  comprobanteCobranzaProxy.Update(comprobanteCobranzaDTO);
                  messageResourceKey = "TS_comprobante_cobranza_actualizada";
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
      [HttpPost("EliminarComprobanteCobranza")]
      public ActionResult EliminarComprobanteCobranza([FromBody] BancarizaViewModel model)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.ComprobanteCobranzaDTO.DTOState = DTOState.Deleted;

            comprobanteCobranzaProxy.Delete(model.ComprobanteCobranzaDTO);

            resultResponse.Message = SR.GetString("TS_comprobante_cobranza_anulada");

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
