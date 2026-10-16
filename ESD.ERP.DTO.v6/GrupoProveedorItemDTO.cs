using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class GrupoProveedorItemDTO : DTOBase
	{
		public override int Id => GrupoProveedorItemId;
		public int GrupoProveedorItemId { get; set; }
		public int GrupoProveedorId { get; set; }
		public int ProveedorId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificaId { get; set; }
		public DateTime? FechaModifica { get; set; }
		public GrupoProveedorDTO GrupoProveedorDTO { get; set; }
		public ProveedorDTO ProveedorDTO { get; set; }
	}
}
