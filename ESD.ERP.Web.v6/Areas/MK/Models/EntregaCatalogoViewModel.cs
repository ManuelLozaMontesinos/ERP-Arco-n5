using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.VT.Models
{
    public class EntregaCatalogoViewModel : ViewModel
    {
        public EntregaCatalogoViewModel()
        {
            Title = "Entrega de Catalogos";
        }

        public CotizacionDTO CotizacionDTO { get; set; }
        public CotizacionProductoDTO CotizacionProductoDTO { get; set; }
        public CalculadorVentaItemDTO CalculadorVentaItemDTO { get; set; }
        public decimal PorcentajeFraccionPU { get; set; }
        public ComprobanteDTO ComprobanteDTO { get; set; }
        public CotizacionAutorizacionDTO CotizacionAutorizacionDTO { get; set; }
        public List<CotizacionAutorizacionDTO> CotizacionAutorizacionDTOList { get; set; }
        public List<CotizacionProductoDTO> CotizacionProductoDTOList { get; set; }
        public AnticipoClienteDTO AnticipoClienteDTO { get; set; }
        public OrdenProduccionDTO OrdenProduccionDTO { get; set; }
        public OrdenProduccionSubProcesoDTO OrdenProduccionSubProcesoDTO { get; set; }

        public List<SelectListItem> EstadoCotizacionList { get; set; }
        public List<SelectListItem> MonedaList { get; set; }
        public List<SelectListItem> ListaPrecioList { get; set; }
        public List<SelectListItem> TipoCondicionPagoList { get; set; }
        public List<SelectListItem> AlmacenList { get; set; }
        public string AutorizacionPendiente { get; set; }
        public int ListaPrecioProveedorId { get; set; }
        public string IgnorarIncrementoFraccion { get; set; }
        public List<MenuItem> MenuAnticipoList { get; set; }
        public int OrdenProduccionId { get; set; }
        public string FileId { get; set; }
        public string ServidorImpresion { get; set; }
        public string DataUrl { get; set; }
        public string Vendedor { get; set; }
        public string TipoDescuentoString { get; set; }
        public string AutorizacionProduccion { get; set; }
    }
}
