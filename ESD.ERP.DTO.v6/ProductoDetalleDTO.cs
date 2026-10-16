using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class ProductoDetalleDTO : DTOBase
	{
		public override int Id => ProductoDetalleId;
		public int ProductoDetalleId { get; set; }
		public int ProductoId { get; set; }
		public string Nombre { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public ProductoDTO ProductoDTO { get; set; }
	}
}
