using System;

namespace ESD.ERP.Types
{
   public class ProveedorListaPrecioProductoType
	{
		public int ProveedorListaPrecioProductoId { get; set; }
		public int ProveedorListaPrecioId { get; set; }
		public int ProductoId { get; set; }
		public decimal ValorUnitarioAnterior { get; set; }
		public decimal PrecioUnitarioAnterior { get; set; }
		public DateTime? FechaAnterior { get; set; }
		public decimal ValorUnitario { get; set; }
		public decimal PrecioUnitario { get; set; }
		public DateTime FechaVigente { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }

	}
}