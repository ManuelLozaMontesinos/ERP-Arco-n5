namespace ESD.ERP.Types
{
   public class PedidoCompraProductoType
	{
		public int PedidoCompraProductoId { get; set; }
		public int PedidoCompraId { get; set; }
		public int ProductoId { get; set; }
		public decimal Cantidad { get; set; }
		public decimal PesoNeto { get; set; }
		public decimal PesoBruto { get; set; }
		public string StockEnProveedor { get; set; }
		public string EsAdicional { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
	}
}