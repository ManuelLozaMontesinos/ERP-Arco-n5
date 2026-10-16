using ESD.Core.Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.CM.Models
{
   public class PuntoPedidoViewModel : ViewModel
   {
      public PuntoPedidoViewModel() {
         Title = "Punto de Pedido - Consulta";
      }

      public List<SelectListItem> PeriodoEmpresaSedeList { get; set; }
      public string GridColNames { get; set; }
      public string GridColModel { get; set; }
   }
}