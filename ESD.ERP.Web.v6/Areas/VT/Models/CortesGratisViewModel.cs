using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.VT.Models
{
    public class CortesGratisViewModel : ViewModel
    {
        public CortesGratisViewModel()
        {
            Title = "Cortes Gratis";
        }

        public CortesGratisDTO CortesGratisDTO { get; set; }
        public EmpresaSedeDTO EmpresaSedeDTO { get; set; }
        public ProductoDTO ProductoDTO { get; set; }
        public FamiliaProductoDTO FamiliaProductoDTO { get; set; }
        public ClaseProductoDTO ClaseProductoDTO { get; set; }

        public List<SelectListItem> FamiliaProductoList;

        public List<SelectListItem> ClaseProductoList;
        public List<MenuItem> MenuProductoList { get; set; }

    }
}
