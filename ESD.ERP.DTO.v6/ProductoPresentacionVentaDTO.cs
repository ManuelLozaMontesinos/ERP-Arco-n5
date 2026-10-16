using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class ProductoPresentacionVentaDTO : DTOBase
	{
		public override int Id => ProductoPresentacionVentaId;
		public int ProductoPresentacionVentaId { get; set; }
		public int ProductoId { get; set; }
		public int UnidadMedidaId { get; set; }
		public decimal CantidadEquivalente { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public ProductoDTO ProductoDTO { get; set; }
		public UnidadMedidaDTO UnidadMedidaDTO { get; set; }
	}
}
