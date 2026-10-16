using ESD.Core.Web;
using ESD.ERP.DTO;

namespace ESD.ERP.Web.Areas.MG.Models
{
   public class CentroCostoViewModel : ViewModel
   {
      public CentroCostoViewModel()
      {
         Title = "Centro de Costos";
      }

      public CentroCostoDTO CentroCostoDTO { get; set; }
   }
}