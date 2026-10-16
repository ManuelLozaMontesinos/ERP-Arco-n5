namespace ESD.ERP.Web.Areas.FE.Models.DTO
{
   public class ResumenItemDTO
   {
      public int DocumentoElectronicoId { get; set; }
      public string TipoDocumento { get; set; }
      public string Serie { get; set; }
      public string Correlativo { get; set; }
      public string TipoMoneda { get; set; }
      public string ClienteTipoDocumentoIdentidad { get; set; }
      public string ClienteNumeroDocumentoIdentidad { get; set; }
      public string ClienteRazonSocial { get; set; }
      public decimal PorcentajeIGV { get; set; }
      public decimal TotalGravado { get; set; }
      public decimal TotalInafecto { get; set; }
      public decimal TotalExonerado { get; set; }
      public decimal TotalOtrosCargos { get; set; }
      public decimal TotalIGV { get; set; }
      public decimal TotalISC { get; set; }
      public decimal TotalOtrosTributos { get; set; }
      public decimal Total { get; set; }
      public decimal TotalVentaGratuita { get; set; }
      public string TipoDocumentoModifica { get; set; }
      public string SerieModifica { get; set; }
      public string CorrelativoModifica { get; set; }
      public string RegimenPercepcion { get; set; }
      public decimal BaseImponiblePercepcion { get; set; }
      public decimal MontoPercepcion { get; set; }
      public decimal MontoTotalPercepcion { get; set; }
      public string NumeroFinalTicket { get; set; }
      public string Estado { get; set; }
   }
}
