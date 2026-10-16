using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("usuario_horario")]
	public class UsuarioHorario : BaseEntity
	{
		public override int EntityId => UsuarioHorarioId;
		[Key]
		[Column("usuario_horario_id", Order = 1)]
		public int UsuarioHorarioId { get; set; }

		[Column("usuario_id")]
		public int UsuarioId { get; set; }

		[Column("dia_id")]
		public int DiaId { get; set; }

		[Column("hora_ini")]
		public string HoraInicio { get; set; }

		[Column("hora_fin")]
		public string HoraFin { get; set; }
		[Column("hora_ext")]
		public string HoraExtension { get; set; }

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

		public Usuario Usuario { get; set; }
		public Dia Dia { get; set; }

		public string TieneHorarioAsignado { get; set; }

	}
}
