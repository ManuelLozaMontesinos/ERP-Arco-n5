using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class GuiaRemisionProductoDTO : DTOBase
	{
		public override int Id => GuiaRemisionProductoId;
		public int GuiaRemisionProductoId { get; set; }
		public int GuiaRemisionId { get; set; }
		public int? MovimientoAlmacenProductoId { get; set; }
		public string CodigoProducto { get; set; }
		public string Descripcion { get; set; }
		public decimal Cantidad { get; set; }
		public decimal PesoNeto { get; set; }
		public decimal PesoBruto { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public GuiaRemisionDTO GuiaRemisionDTO { get; set; }
		public MovimientoAlmacenProductoDTO MovimientoAlmacenProductoDTO { get; set; }
		public ProductoDTO ProductoDTO { get; set; }
	}
}
