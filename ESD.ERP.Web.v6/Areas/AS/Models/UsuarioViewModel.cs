using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.AS.Models
{
   public class UsuarioViewModel : ViewModel
   {
      public UsuarioViewModel() {
         Title = "Usuario";
      }

      public UsuarioDTO UsuarioDTO { get; set; }
      public RolUsuarioDTO RolUsuarioDTO { get; set; }
      public DirectivaUsuarioDTO DirectivaUsuarioDTO { get; set; }
      public UsuarioHorarioDTO UsuarioHorarioDTO { get; set; }
      public bool ContrasenaAnteriorVisible { get; set; } = false;
      public string ContrasenaAnterior { get; set; }
      public string Contrasena { get; set; }
      public string ContrasenaConfirmada { get; set; }
      public string CambiarContrasenaProximo { get; set; }
      public List<SelectListItem> DirectivaReglaList { get; set; }
      public List<UsuarioHorarioDTO> UsuarioHorarioDTOList { get; set; }
      public string HoraInicio { get; set; }
      public string HoraFin { get; set; }
   }
}
