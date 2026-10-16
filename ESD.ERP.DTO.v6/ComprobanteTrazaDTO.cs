using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class ComprobanteTrazaDTO : DTOBase
	{
		public override int Id => ComprobanteTrazaId;
		public int ComprobanteTrazaId { get; set; }
		public int ComprobanteId { get; set; }
		public int TipoAccionAuditoriaId { get; set; }
		public DateTime Fecha { get; set; }
		public string Descripcion { get; set; }
		public int UsuarioId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }

		public ComprobanteDTO ComprobanteDTO { get; set; }
		public TipoAccionAuditoriaDTO TipoAccionAuditoriaDTO { get; set; }
		public UsuarioDTO UsuarioDTO { get; set; }
	}
}
