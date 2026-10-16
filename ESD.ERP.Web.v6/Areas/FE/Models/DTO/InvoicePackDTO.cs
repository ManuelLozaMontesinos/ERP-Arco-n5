using System;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.FE.Models.DTO
{
   public class InvoicePackDTO
   {

      public InvoicePackDTO()
      {
         this.Items = new List<int>();
      }

      public int TipoComprobante { get; set; }
      public DateTime Fecha { get; set; }
      public string ProveedorRUC { get; set; }
      public List<int> Items { get; set; }
   }
}
