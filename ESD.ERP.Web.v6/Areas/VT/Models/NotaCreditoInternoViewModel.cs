using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.VT.Models
{
    public class NotaCreditoInternoViewModel : ViewModel
    {
        public NotaCreditoInternoViewModel()
        {
            Title = "Nota de Crédito Interno";
        }

        public NotaCreditoDTO NotaCreditoDTO { get; set; }
        public NotaCreditoProductoDTO NotaCreditoProductoDTO { get; set; }
        public CalculadorVentaItemDTO CalculadorVentaItemDTO { get; set; }
        public ComprobanteDTO ComprobanteDTO { get; set; }

        public List<SelectListItem> TipoNotaCreditoList { get; set; }
        public List<SelectListItem> EstadoComprobanteList { get; set; }
        public List<SelectListItem> SerieList { get; set; }

    }
}