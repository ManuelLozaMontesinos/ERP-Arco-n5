namespace ESD.ERP.Types
{
   public class AlmacenVolumenProductoType
	{
		public int AlmacenVolumenProductoId { get; set; }
		public int EmpresaSedeId { get; set; }
		public int ProductoId { get; set; }
		public decimal StockMinimo { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }

	}
}