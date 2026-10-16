namespace ESD.ERP.Types
{
   public class ListaPrecioProductoType
	{
		public int ListaPrecioProductoId { get; set; }
		public int ListaPrecioId { get; set; }
		public int ProductoId { get; set; }
		public decimal ValorUnitario { get; set; }
		public decimal PrecioUnitario { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }

	}
}