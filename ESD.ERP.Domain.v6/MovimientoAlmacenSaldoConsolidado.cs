using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("mov_alm_s_con")]
	public class MovimientoAlmacenSaldoConsolidado : BaseEntity
	{
		public override int EntityId => MovimientoAlmacenSaldoConsolidadoId;
		[Key]
		[Column("mov_alm_s_con_id", Order = 1)]
		public int MovimientoAlmacenSaldoConsolidadoId { get; set; }

		[Column("periodo_emp_sede_id")]
		public int PeriodoEmpresaSedeId { get; set; }

		[Column("producto_id")]
		public int ProductoId { get; set; }

		[Column("almacen_id")]
		public int AlmacenId { get; set; }

		[Column("mes_contable_id")]
		public int MesContableId { get; set; }

		[Column("cant_ant")]
		public decimal CantidadAnterior { get; set; }

		[Column("valor_unit_ant")]
		public decimal ValorUnitarioAnterior { get; set; }

		[Column("cant_ing")]
		public decimal CantidadIngreso { get; set; }

		[Column("cant_sal")]
		public decimal CantidadSalida { get; set; }

		[Column("cant_reserva")]
		public decimal CantidadReserva { get; set; }

		[Column("valor_unit")]
		public decimal ValorUnitario { get; set; }

		public PeriodoEmpresaSede PeriodoEmpresaSede { get; set; }
		public Producto Producto { get; set; }
		public MesContable MesContable { get; set; }
	}
}
