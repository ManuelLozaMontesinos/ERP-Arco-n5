using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.TS.Models
{
   public class CuentaBancoViewModel : ViewModel
   {
      public CuentaBancoViewModel() {
         Title = "Cuentas Bancarias";
      }

      public EmpresaCuentaBancariaDTO EmpresaCuentaBancariaDTO { get; set; }

      public List<SelectListItem> BancoList { get; set; }
      public List<SelectListItem> MonedaList { get; set; }
   }
}