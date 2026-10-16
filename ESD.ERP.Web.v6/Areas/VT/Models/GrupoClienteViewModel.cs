using ESD.Core.Web;
using ESD.ERP.DTO;

namespace ESD.ERP.Web.Areas.VT.Models
{
   public class GrupoClienteViewModel : ViewModel
   {
      public GrupoClienteViewModel()
      {
         Title = "Grupo de Clientes";
      }

      public GrupoProveedorDTO GrupoProveedorDTO { get; set; }
      public GrupoProveedorItemDTO GrupoProveedorItemDTO { get; set; }
   }
}