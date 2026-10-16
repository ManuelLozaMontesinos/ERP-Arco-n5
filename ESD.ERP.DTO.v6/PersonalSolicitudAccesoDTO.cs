using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{

	public class PersonalSolicitudAccesoDTO : DTOBase
	{
		public override int Id => PersonalSolicitudAccesoId;
		public int PersonalSolicitudAccesoId { get; set; }
		public int PersonalId { get; set; }
		public DateTime Fecha { get; set; }
		public DateTime FechaRegistro { get; set; }
		public string HoraSolicitada { get; set; }
		public int? PersonalAutorizaId { get; set; }
		public string HoraAutorizada { get; set; }
		public DateTime? FechaAutoriza { get; set; }
		public int EstadoAprobacionId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }

		public PersonalDTO PersonalDTO { get; set; }
		public PersonalDTO PersonalAutorizaDTO { get; set; }
		public EstadoAprobacionDTO EstadoAprobacionDTO { get; set; }

		public DateTime? FechaDesde { get; set; }
		public DateTime? FechaHasta { get; set; }

	}
}
