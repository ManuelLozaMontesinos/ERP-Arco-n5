using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.VT.Models
{
   public class ComprobanteViewModel : ViewModel
   {
      public ComprobanteViewModel() {
         Title = "Facturacion";
      }

      public ComprobanteDTO ComprobanteDTO { get; set; }
      public ComprobanteProductoDTO ComprobanteProductoDTO { get; set; }
      public CalculadorVentaItemDTO CalculadorVentaItemDTO { get; set; }
      public decimal PorcentajeFraccionPU { get; set; }
      public ComprobanteAutorizacionDTO ComprobanteAutorizacionDTO { get; set; }
      public List<ComprobanteAutorizacionDTO> ComprobanteAutorizacionDTOList { get; set; }
      public List<ComprobanteProductoDTO> ComprobanteProductoDTOList { get; set; }
      public GuiaRemisionDTO GuiaRemisionDTO { get; set; }
      public AnticipoClienteDTO AnticipoClienteDTO { get; set; }
      public ComprobanteTrazaDTO ComprobanteTrazaDTO { get; set; }

      public List<SelectListItem> TipoDocumentoComercialList { get; set; }
      public List<SelectListItem> SerieList { get; set; }
      public List<SelectListItem> EstadoComprobanteList { get; set; }
      public List<SelectListItem> MonedaList { get; set; }
      public List<SelectListItem> ListaPrecioList { get; set; }
      public List<SelectListItem> TipoCondicionPagoList { get; set; }
      public List<SelectListItem> MedioPagoList { get; set; }
      public List<SelectListItem> PeriodoEmpresaSedeList { get; set; }

      public string AutorizacionPendiente { get; set; }
      public string TieneGuia { get; set; }
      public int ListaPrecioProveedorId { get; set; }
      public string IgnorarIncrementoFraccion { get; set; }
      public string FileId { get; set; }
      public string ServidorImpresion { get; set; }
      public string DataUrl { get; set; }

      public string EsObsequio { get; set; }
      public string Vendedor { get; set; }
      public string TieneAnticipo { get; set; }
      public string TipoDescuentoString { get; set; }
   }
}
