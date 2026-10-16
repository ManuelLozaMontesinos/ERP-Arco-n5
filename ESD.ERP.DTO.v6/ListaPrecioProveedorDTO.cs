using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class ListaPrecioProveedorDTO : DTOBase
	{
		public override int Id => ListaPrecioProveedorId;
		public int ListaPrecioProveedorId { get; set; }
		public int ListaPrecioId { get; set; }
		public int GrupoProveedorId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public ListaPrecioDTO ListaPrecioDTO { get; set; }
		public GrupoProveedorDTO GrupoProveedorDTO { get; set; }
		public int ProveedorId { get; set; }
	}
}
