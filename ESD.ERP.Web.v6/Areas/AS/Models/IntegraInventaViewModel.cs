using ESD.Core.Web;
using ESD.ERP.DTO;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.AS.Models
{
   public class IntegraInventaViewModel : ViewModel
   {
      public IntegraInventaViewModel() {
         Title = "Integracion Inventarios";
      }

      public ModuloDTO ModuloDTO { get; set; }
      public ProcesoIntegracionDTO ProcesoIntegracionDTO { get; set; }
      public List<ProcesoIntegracionDTO> ProcesoIntegracionDTOList { get; set; }
      public string ParametroSistema01 { get; set; }
   }
}
