using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.RH.Models
{
   public class AccesoExtViewModel : ViewModel
   {
      public AccesoExtViewModel() {
         Title = "Acceso Extemp. al Sistema - Autorizaciones";
      }
      public PersonalSolicitudAccesoDTO PersonalSolicitudAccesoDTO { get; set; }
      public List<SelectListItem> EstadoAprobacionList { get; set; }
   }
}
