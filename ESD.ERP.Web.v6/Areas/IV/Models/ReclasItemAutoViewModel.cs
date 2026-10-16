using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.IV.Models
{
    public class ReclasItemAutoViewModel : ViewModel
    {
        public ReclasItemAutoViewModel()
        {
            Title = "Reclasificación de Items - Autorizacion";
        }
        public MovimientoReclasificacionDTO MovimientoReclasificacionDTO { get; set; }
        public MovimientoAlmacenDTO MovimientoAlmacenDTO { get; set; }
        public MovimientoAlmacenProductoDTO MovimientoAlmacenProductoDTO { get; set; }
        public List<SelectListItem> AlmacenList { get; set; }
    }
}
