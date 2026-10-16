using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class ProveedorRedSocialDTO : DTOBase
	{
		public override int Id => ProveedorRedSocialId;
		public int ProveedorRedSocialId { get; set; }
		public int ProveedorId { get; set; }
		public int RedSocialId { get; set; }
		public string Url { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public ProveedorDTO ProveedorDTO { get; set; }
		public RedSocialDTO RedSocialDTO { get; set; }
	}
}
