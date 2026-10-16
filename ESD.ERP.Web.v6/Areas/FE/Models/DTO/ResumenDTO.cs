using System;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.FE.Models.DTO
{
   public class ResumenDTO
   {
      public ResumenDTO()
      {
         this.Items = new List<ResumenItemDTO>();
      }
      public string SignedBy { get; set; }
      public string ID { get; set; }
      public int CorrelativoEnvio { get; set; }
      public DateTime FechaResumen { get; set; }
      public DateTime FechaComprobante { get; set; }
      public string ProveedorNombre { get; set; }
      public string ProveedorTipoDocumento { get; set; }
      public string ProveedorRUC { get; set; }
      public string NumeroTicket { get; set; }
      public string MotivoContingencia { get; set; }

      public List<ResumenItemDTO> Items { get; set; }
   }
}
