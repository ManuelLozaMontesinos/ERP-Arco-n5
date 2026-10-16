using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.MK.Models
{
    public class MerchandisingViewModel : ViewModel
    {
        public MerchandisingViewModel()
        {
            Title = "Merchandising";
        }

        public MovimientoAlmacenDTO MovimientoAlmacenDTO { get; set; }
        public AlmacenTransaccionDTO AlmacenTransaccionDTO { get; set; }
        public MovimientoAlmacenProductoDTO MovimientoAlmacenProductoDTO { get; set; }

        public List<SelectListItem> AlmacenList { get; set; }
        public List<SelectListItem> TransaccionAlmacenList { get; set; }
        public List<SelectListItem> AlmacenTransaccionList { get; set; }
        public TransaccionAlmacenDTO TransaccionAlmacenDTO { get; set; }
        public List<SelectListItem> TipoMovimientoAlmacenList { get; set; }
        public List<SelectListItem> TipoDocumentoComercialList { get; set; }
        public string MovimientoConfiguracion { get; set; }
        public string RegRestringido { get; set; }
    }
}