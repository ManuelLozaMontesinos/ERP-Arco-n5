using ESD.ERP.DTO;

namespace ESD.ERP.Web.Model
{
   public class LoginViewModel
   {
      public string UserName { get; set; }
      public string OldPassword { get; set; }
      public string Password { get; set; }
      public string ConfirmPassword { get; set; }
      public UsuarioDTO UsuarioDTO { get; set; }
      public int UsuarioId { get; set; }
      public string HoraSolicitada { get; set; }
   }
}