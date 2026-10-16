using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("plan_p_regu")]
	public class PlanillaProcesoRegularizacion : BaseEntity
	{
		public override int EntityId => PlanillaProcesoRegularizacionId;
		[Key]
		[Column("plan_p_regu_id", Order = 1)]
		public int PlanillaProcesoRegularizacionId { get; set; }

		[Column("plan_proce_id")]
		public int PlanillaProcesoId { get; set; }

		[Column("personal_id")]
		public int PersonalId { get; set; }

		[Column("concep_remu_id")]
		public int ConceptoRemunerativoId { get; set; }

		[Column("cant_hora")]
		public decimal CantidadHora { get; set; }

		[Column("monto")]
		public decimal Monto { get; set; }

		[Column("observa")]
		public string Observacion { get; set; }

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

		public PlanillaProceso PlanillaProceso { get; set; }
		public Personal Personal { get; set; }
		public ConceptoRemunerativo ConceptoRemunerativo { get; set; }
	}
}
