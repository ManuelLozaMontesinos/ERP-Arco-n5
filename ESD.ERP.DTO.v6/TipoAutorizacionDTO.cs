using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class TipoAutorizacionDTO : DTOBase
	{
		public override int Id => TipoAutorizacionId;
		public int TipoAutorizacionId { get; set; }
		public string Codigo { get; set; }
		public string Nombre { get; set; }
		public string UsaDetalle { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
	}
}
