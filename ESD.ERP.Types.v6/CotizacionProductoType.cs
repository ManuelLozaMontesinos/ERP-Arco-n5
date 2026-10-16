namespace ESD.ERP.Types
{
   public class CotizacionProductoType
   {
      public int CotizacionProductoId { get; set; }
      public int CotizacionId { get; set; }
      public int ProductoId { get; set; }
      public decimal Cantidad { get; set; }
      public decimal ValorUnitario { get; set; }
      public decimal PrecioUnitario { get; set; }
      public decimal PrecioAutorizado { get; set; }
      public string CantidadBloqueada { get; set; }
      public string SolicitaAutorizacionDescuento { get; set; }
      public decimal MontoBruto { get; set; }
      public decimal MontoDescuento1 { get; set; }
      public decimal MontoDescuento2 { get; set; }
      public decimal MontoNeto { get; set; }
      public decimal MontoIgv { get; set; }
      public decimal MontoIsc { get; set; }
      public decimal MontoTotal { get; set; }
      public string Estado { get; set; }
      public int UsuarioCreacionId { get; set; }
   }
}
