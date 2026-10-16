using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.PD.Models
{
    public class AsigProdViewModel : ViewModel
    {
        public AsigProdViewModel()
        {
            Title = "Asignacion de Procesos";
        }

        public OrdenProduccionDTO OrdenProduccionDTO { get; set; }
        public OrdenProduccionProductoDTO OrdenProduccionProductoDTO { get; set; }
        public OrdenProduccionSubProcesoDTO OrdenProduccionSubProcesoDTO { get; set; }
        public SubProcesoProduccionDTO SubProcesoProduccionDTO { get; set; } 
        public CotizacionDTO CotizacionDTO { get; set; }
        public PersonalMaquinaDTO PersonalMaquinaDTO { get; set; }
        public List<SelectListItem> AlmacenList { get; set; }
        public string MaquinaList { get; set; }
        public string Vendedor { get; set; }
        public int OrdenProduccionId { get; set; }
        public List<OrdenProduccionSubProcesoDTO> OrdenProduccionSubProcesoDTOList { get; set; }
        public DateTime Fecha {  get; set; }

    }
}