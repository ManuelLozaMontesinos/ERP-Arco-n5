using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class TipoVariableCalculoDTO : DTOBase
	{
		public override int Id => TipoVariableCalculoId;
		public int TipoVariableCalculoId { get; set; }
		public string Codigo { get; set; }
		public string Nombre { get; set; }
		public string Abreviatura { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificaId { get; set; }
		public DateTime? FechaModifica { get; set; }
	}
}
