using ESD.Core.Web;
using ESD.ERP.DTO;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.AS.Models
{
   public class RolViewModel : ViewModel
   {
      public RolViewModel() {
         Title = "Rol";
      }

      public RolDTO RolDTO { get; set; }
      public RolUsuarioDTO RolUsuarioDTO { get; set; }
      public List<RolMenuDTO> RolMenuDTOList { get; set; }
   }
}