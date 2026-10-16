using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class FeriadoDTO : DTOBase
	{
		public override int Id => FeriadoId;
		public int FeriadoId { get; set; }
		public DateTime Fecha { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
	}
}
