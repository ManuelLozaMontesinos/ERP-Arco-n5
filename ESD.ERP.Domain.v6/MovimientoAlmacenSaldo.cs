using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("mov_alm_saldo")]
	public class MovimientoAlmacenSaldo : BaseEntity
	{
		public override int EntityId => MovimientoAlmacenSaldoId;
		[Key]
		[Column("mov_alm_saldo_id", Order = 1)]
		public int MovimientoAlmacenSaldoId { get; set; }

		[Column("periodo_emp_sede_id")]
		public int PeriodoEmpresaSedeId { get; set; }

		[Column("almacen_id")]
		public int AlmacenId { get; set; }

		[Column("producto_id")]
		public int ProductoId { get; set; }

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
		public Almacen Almacen { get; set; }
		public Producto Producto { get; set; }
		public MesContable MesContable { get; set; }
	}
}
