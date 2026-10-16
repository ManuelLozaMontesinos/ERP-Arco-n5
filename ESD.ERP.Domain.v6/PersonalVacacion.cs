using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("personal_vaca")]
	public class PersonalVacacion : BaseEntity
	{
		public override int EntityId => PersonalVacacionId;
		[Key]
		[Column("personal_vaca_id", Order = 1)]
		public int PersonalVacacionId { get; set; }

		[Column("personal_id")]
		public int PersonalId { get; set; }

		[Column("fecha_inicio")]
		public DateTime? FechaInicio { get; set; }

		[Column("fecha_fin")]
		public DateTime? FechaFin { get; set; }

		[Column("dias_a_cuenta")]
		public int DiasACuenta { get; set; }

		[Column("estado_vaca_id")]
		public int EstadoVacacionId { get; set; }

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
		public EstadoVacacion EstadoVacacion { get; set; }
	}
}
