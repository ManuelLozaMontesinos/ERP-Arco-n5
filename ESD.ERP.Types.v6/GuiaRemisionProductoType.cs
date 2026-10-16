namespace ESD.ERP.Types
{
	public class GuiaRemisionProductoType
	{
		public int GuiaRemisionProductoId { get; set; }
		public int GuiaRemisionId { get; set; }
		public int MovimientoAlmacenProductoId { get; set; }
		public string CodigoProducto { get; set; }
		public string Descripcion { get; set; }
		public decimal Cantidad { get; set; }
		public decimal PesoNeto { get; set; }
		public decimal PesoBruto { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
	}
}
