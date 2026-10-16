using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.VT.Models
{
   public class CajaCobranzaViewModel : ViewModel
   {
      public CajaCobranzaViewModel() {
         Title = "Cobranza";
      }

      public ComprobanteDTO ComprobanteDTO { get; set; }
      public CalculadorVentaItemDTO CalculadorVentaItemDTO { get; set; }
      public ComprobanteProductoDTO ComprobanteProductoDTO { get; set; }
      public AnticipoClienteDTO AnticipoClienteDTO { get; set; }
      public NotaCreditoDTO NotaCreditoDTO { get; set; }

      public List<SelectListItem> EstadoComprobanteList { get; set; }
      public List<SelectListItem> TipoDocumentoComercialList { get; set; }
      public List<SelectListItem> MonedaList { get; set; }
      public List<SelectListItem> ListaPrecioList { get; set; }
      public List<SelectListItem> TipoCondicionPagoList { get; set; }
      public List<SelectListItem> MedioPagoList { get; set; }
      public List<SelectListItem> CajaList { get; set; }
      public decimal PorcentajeFraccionPU { get; set; }
      public string CopiaCliente { get; set; }
      public string CopiaAdministracion { get; set; }
      public string CopiaDespacho { get; set; }
      public string GridColNames { get; set; }
      public string GridColModel { get; set; }
   }
}
