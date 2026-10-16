using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("perso_plan")]
	public class PersonalPlanilla : BaseEntity
	{
		public override int EntityId => PersonalPlanillaId;
		[Key]
		[Column("perso_plan_id", Order = 1)]
		public int PersonalPlanillaId { get; set; }

		[Column("personal_id")]
		public int PersonalId { get; set; }

		[Column("planilla_id")]
		public int PlanillaId { get; set; }

		[Column("estado")]
		public string Estado { get; set; }

		[Column("usuario_creacion_id")]
		public int UsuarioCreacionId { get; set; }

		[Column("fecha_creacion")]
		public DateTime FechaCreacion { get; set; }

		[Column("usuario_modifica_id")]
		public int? UsuarioModificaId { get; set; }

		[Column("fecha_modifica")]
		public DateTime? FechaModifica { get; set; }

		public Personal Personal { get; set; }
		public Planilla Planilla { get; set; }
	}
}
