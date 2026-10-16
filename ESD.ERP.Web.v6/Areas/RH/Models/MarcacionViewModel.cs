using ESD.Core.Web;
using ESD.ERP.DTO;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.RH.Models
{
   public class MarcacionViewModel : ViewModel
   {
      public MarcacionViewModel() {
         Title = "Marcaciones";
      }

      public PersonalMarcacionDTO PersonalMarcacionDTO { get; set; }
      public PersonalMarcacionHoraDTO PersonalMarcacionHoraDTO { get; set; }
      public List<PersonalMarcacionHoraDTO> PersonalMarcacionHoraDTOList { get; set; }
      public RelojMarcacionDTO RelojMarcacionDTO { get; set; }
      public string GridColNames { get; set; }
      public string GridColModel { get; set; }
      public string HoraList { get; set; }
      public string RelojString { get; set; }
   }
}
