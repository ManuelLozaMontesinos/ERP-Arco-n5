using ESD.Core.Web;
using ESD.ERP.DTO;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.AS.Models
{
   public class IntegraCapitalViewModel : ViewModel
   {
      public IntegraCapitalViewModel() {
         Title = "Integracion Capital Humano";
      }

      public ModuloDTO ModuloDTO { get; set; }
      public ProcesoIntegracionDTO ProcesoIntegracionDTO { get; set; }
      public List<ProcesoIntegracionDTO> ProcesoIntegracionDTOList { get; set; }
   }
}
