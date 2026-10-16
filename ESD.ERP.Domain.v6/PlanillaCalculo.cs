using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;
using ESD.ERP.Types;

namespace ESD.ERP.Domain
{
	[Table("plan_calculo")]
	public class PlanillaCalculo : BaseEntity
	{
		public override int EntityId => PlanillaCalculoId;
		[Key]
		[Column("plan_calculo_id", Order = 1)]
		public int PlanillaCalculoId { get; set; }

		[Column("plan_proce_id")]
		public int PlanillaProcesoId { get; set; }

		[Column("personal_id")]
		public int PersonalId { get; set; }

		[Column("concep_remu_id")]
		public int ConceptoRemunerativoId { get; set; }

		[Column("horas")]
		public decimal Horas { get; set; }

		[Column("monto_sub")]
		public decimal MontoSubTotal { get; set; }

		[Column("monto")]
		public decimal Monto { get; set; }

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

		public List<VariableProcesoType> VariableProcesoTypeList { get; set; }
	}
}
