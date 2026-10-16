using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.MG.Models
{
    public class ProductoViewModel : ViewModel
    {
        public ProductoViewModel()
        {
            Title = "Productos y Servicios";
        }

        public GrupoProductoDTO GrupoProductoDTO { get; set; }
        public ClaseProductoDTO ClaseProductoDTO { get; set; }
        public FamiliaProductoDTO FamiliaProductoDTO { get; set; }
        public ProductoDTO ProductoDTO { get; set; }
        public ProductoPresentacionVentaDTO ProductoPresentacionVentaDTO { get; set; }
        public ProductoSubProcesoProduccionDTO ProductoSubProcesoProduccionDTO { get; set; }
        public ProductoDetalleDTO ProductoDetalleDTO { get; set; }

        public List<SelectListItem> TipoProductoList { get; set; }
        public List<SelectListItem> GrupoProductoList { get; set; }
        public List<SelectListItem> ClaseProductoList { get; set; }
        public List<SelectListItem> FamiliaProductoList { get; set; }
        public List<SelectListItem> UnidadMedidaList { get; set; }
        public List<SelectListItem> TipoIgvList { get; set; }
        public List<SelectListItem> EstadoProductoList { get; set; }
        public List<SelectListItem> EstadoComercialProductoList { get; set; }

        public string UnidadMedidaString { get; set; }
    }
}
