using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class ProveedorOportunidadDTO : DTOBase
	{
		public override int Id => ProveedorOportunidadId;
		public int ProveedorOportunidadId { get; set; }
		public int ProveedorId { get; set; }
		public int OportunidadVentaId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public ProveedorDTO ProveedorDTO { get; set; }
		public OportunidadVentaDTO OportunidadVentaDTO { get; set; }
	}
}
