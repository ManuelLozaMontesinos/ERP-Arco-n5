using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class ListaPrecioProveedorProductoDTO : DTOBase
	{
		public override int Id => ListaPrecioProveedorProductoId;
		public int ListaPrecioProveedorProductoId { get; set; }
		public int ListaPrecioProveedorId { get; set; }
		public int ListaPrecioProductoId { get; set; }
		public decimal PorcentajePrecioBase { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public ListaPrecioProveedorDTO ListaPrecioProveedorDTO { get; set; }
		public ListaPrecioProductoDTO ListaPrecioProductoDTO { get; set; }
	}
}
