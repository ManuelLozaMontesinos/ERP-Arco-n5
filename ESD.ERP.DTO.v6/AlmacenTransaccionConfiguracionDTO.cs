using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class AlmacenTransaccionConfiguracionDTO : DTOBase
	{
		public override int Id => AlmacenTransaccionConfiguracionId;
		public int AlmacenTransaccionConfiguracionId { get; set; }
		public int AlmacenTransaccionId { get; set; }
		public int AtributoConfiguracionId { get; set; }
		public string Visible { get; set; }
		public string Requerido { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public AlmacenTransaccionDTO AlmacenTransaccionDTO { get; set; }
		public AtributoConfiguracionDTO AtributoConfiguracionDTO { get; set; }
	}
}
