using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("perso_sol_acc")]
	public class PersonalSolicitudAcceso : BaseEntity
	{
		public override int EntityId => PersonalSolicitudAccesoId;
		[Key]
		[Column("perso_sol_acc_id", Order = 1)]
		public int PersonalSolicitudAccesoId { get; set; }

		[Column("personal_id")]
		public int PersonalId { get; set; }

		[Column("fecha")]
		public DateTime Fecha { get; set; }

		[Column("fecha_registro")]
		public DateTime FechaRegistro { get; set; }

		[Column("hora_sol")]
		public string HoraSolicitada { get; set; }

		[Column("perso_auto_id")]
		public int? PersonalAutorizaId { get; set; }

		[Column("hora_auto")]
		public string HoraAutorizada { get; set; }

		[Column("fecha_auto")]
		public DateTime? FechaAutoriza { get; set; }

		[Column("estado_aproba_id")]
		public int EstadoAprobacionId { get; set; }

		[Column("estado")]
		public string Estado { get; set; }

		[Column("usuario_creacion_id")]
		public int UsuarioCreacionId { get; set; }

		[Column("fecha_creacion")]
		public DateTime FechaCreacion { get; set; }

		[Column("usuario_modifica_id")]
		public int? UsuarioModificacionId { get; set; }

		[Column("fecha_modifica")]
		public DateTime? FechaModificacion { get; set; }

		public Personal Personal { get; set; }
		public Personal PersonalAutoriza { get; set; }
		public EstadoAprobacion EstadoAprobacion { get; set; }

		public DateTime? FechaDesde { get; set; }
		public DateTime? FechaHasta { get; set; }
	}
}
