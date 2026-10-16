using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class RutaProduccionDTO : DTOBase
	{
		public override int Id => RutaProduccionId;
		public int RutaProduccionId { get; set; }
		public string Nombre { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
	}
}
