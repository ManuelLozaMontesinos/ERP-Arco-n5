using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("plan_p_hora")]
	public class PlanillaProcesoHora : BaseEntity
	{
		public override int EntityId => PlanillaProcesoHoraId;
		[Key]
		[Column("plan_p_hora_id", Order = 1)]
		public int PlanillaProcesoHoraId { get; set; }

		[Column("plan_proce_id")]
		public int PlanillaProcesoId { get; set; }

		[Column("hora_rem_id")]
		public int HoraRemunerativaId { get; set; }

		[Column("personal_id")]
		public int PersonalId { get; set; }

		[Column("cantidad")]
		public decimal Cantidad { get; set; }

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
		public HoraRemunerativa HoraRemunerativa { get; set; }
		public Personal Personal { get; set; }
		public List<int> PersonalIdList { get; set; }
	}
}
