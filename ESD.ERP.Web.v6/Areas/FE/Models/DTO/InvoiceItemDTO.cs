namespace ESD.ERP.Web.Areas.FE.Models.DTO
{
   public class InvoiceItemDTO
   {
      public string Codigo { get; set; }
      public string Descripcion { get; set; }
      public string UnidadMedida { get; set; }
      public string Correlativo { get; set; }
      public decimal Cantidad { get; set; }
      public decimal ValorUnitario { get; set; }
      public decimal PrecioUnitario { get; set; }
      public string TipoIGV { get; set; }
      public string TipoAfectacion { get; set; }
      public decimal MontoBruto { get; set; }
      public decimal MontoNeto { get; set; }
      public decimal MontoIGV { get; set; }
      public decimal PorcentajeDescuento { get; set; }
      public decimal MontoDescuento { get; set; }
      public decimal MontoTotal { get; set; }
      public string TipoISC { get; set; }
      public decimal MontoISC { get; set; }
   }
}
