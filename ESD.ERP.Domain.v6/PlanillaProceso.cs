using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("plan_proce")]
	public class PlanillaProceso : BaseEntity
	{
		public override int EntityId => PlanillaProcesoId;
		[Key]
		[Column("plan_proce_id", Order = 1)]
		public int PlanillaProcesoId { get; set; }

		[Column("periodo_emp_sede_id")]
		public int PeriodoEmpresaSedeId { get; set; }

		[Column("planilla_id")]
		public int PlanillaId { get; set; }

		[Column("proc_anual")]
		public short ProcesoAnual { get; set; }

		[Column("mes_id")]
		public int MesId { get; set; }

		[Column("moneda_id")]
		public int MonedaId { get; set; }

		[Column("monto_tc")]
		public decimal MontoTipoCambio { get; set; }

		[Column("peri_remu_id")]
		public int PeriodicidadRemunerativaId { get; set; }

		[Column("fecha_desde")]
		public DateTime FechaDesde { get; set; }

		[Column("fecha_hasta")]
		public DateTime FechaHasta { get; set; }

		[Column("fecha_proceso")]
		public DateTime? FechaProceso { get; set; }

		[Column("estado_plan_pro_id")]
		public int EstadoPlanillaProcesoId { get; set; }

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

		public PeriodoEmpresaSede PeriodoEmpresaSede { get; set; }
		public Planilla Planilla { get; set; }
		public Mes Mes { get; set; }
		public Moneda Moneda { get; set; }
		public EstadoPlanillaProceso EstadoPlanillaProceso { get; set; }
		public PeriodicidadRemunerativa PeriodicidadRemunerativa { get; set; }
	}
}
