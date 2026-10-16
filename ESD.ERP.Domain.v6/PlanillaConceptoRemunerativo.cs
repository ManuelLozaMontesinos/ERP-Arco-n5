using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("plan_con_rem")]
	public class PlanillaConceptoRemunerativo : BaseEntity
	{
		public override int EntityId => PlanillaConceptoRemunerativoId;
		[Key]
		[Column("plan_con_rem_id", Order = 1)]
		public int PlanillaConceptoRemunerativoId { get; set; }

		[Column("planilla_id")]
		public int PlanillaId { get; set; }

		[Column("concep_remu_id")]
		public int ConceptoRemunerativoId { get; set; }

		[Column("orden")]
		public short Orden { get; set; }

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

		public Planilla Planilla { get; set; }
		public ConceptoRemunerativo ConceptoRemunerativo { get; set; }
	}
}
