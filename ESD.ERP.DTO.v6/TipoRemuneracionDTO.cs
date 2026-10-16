using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class TipoRemuneracionDTO : DTOBase
	{
		public override int Id => TipoRemuneracionId;
		public int TipoRemuneracionId { get; set; }
		public string Nombre { get; set; }
		public int VariableCalculoId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public VariableCalculoDTO VariableCalculoDTO { get; set; }
	}
}
