using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
   public class UsuarioHorarioDTO : DTOBase
   {
      public override int Id => UsuarioHorarioId;
		public int UsuarioHorarioId { get; set; }
		public int UsuarioId { get; set; }
		public int DiaId { get; set; }
		public string HoraInicio { get; set; }
		public string HoraFin { get; set; }
		public string HoraExtension { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }

		public UsuarioDTO UsuarioDTO { get; set; }
		public DiaDTO DiaDTO { get; set; }

		public string TieneHorarioAsignado { get; set; }

	}
}
