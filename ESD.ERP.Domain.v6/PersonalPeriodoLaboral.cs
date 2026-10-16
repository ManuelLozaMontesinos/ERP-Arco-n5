using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("perso_per_lab")]
	public class PersonalPeriodoLaboral : BaseEntity
	{
		public override int EntityId => PersonalPeriodoLaboralId;
		[Key]
		[Column("perso_per_lab_id", Order = 1)]
		public int PersonalPeriodoLaboralId { get; set; }

		[Column("personal_id")]
		public int PersonalId { get; set; }

		[Column("fecha_ing")]
		public DateTime FechaIngreso { get; set; }

		[Column("fecha_alta")]
		public DateTime? FechaAlta { get; set; }

		[Column("fecha_cese")]
		public DateTime? FechaCese { get; set; }

		[Column("motivo_fin_peri_id")]
		public int? MotivoFinPeriodoId { get; set; }

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
		public MotivoFinPeriodo MotivoFinPeriodo { get; set; }
	}
}
