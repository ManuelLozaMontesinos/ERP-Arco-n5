using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.FE.Models.DTO
{
   public class ResponseDTO
   {
      public int TipoComprobante { get; set; }
      public string Serie { get; set; }
      public string Numero { get; set; }
      public string HashCode { get; set; }
      public bool Success { get; set; }
      public string Error { get; set; }
      public string EstadoComprobante { get; set; }
      public int ComprobanteElectronicoId { get; set; }
      public string SunatCDR { get; set; }
      public string SunatNota { get; set; }
      public string SunatCodigo { get; set; }
      public string SunatDescripcion { get; set; }
      public string SunatNumeroTicket { get; set; }
      public List<int> ComprobanteElectronicoList { get; set; }
   }
}