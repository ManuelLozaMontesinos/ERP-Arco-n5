using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	public class CuadroCosto : BaseEntity
	{
		public override int EntityId => CuadroCostoId;
		[Key]
		[Column("cuadro_costo_id", Order = 1)]
		public int CuadroCostoId { get; set; }

		[Column("periodo_emp_sede_id")]
		public int PeriodoEmpresaSedeId { get; set; }

		[Column("lista_precio_id")]
		public int ListaPrecioId { get; set; }

		[Column("mes_contable_id")]
		public int MesContableId { get; set; }

		[Column("producto_id")]
		public int ProductoId { get; set; }

		[Column("valor_prom")]
		public decimal ValorPromedio { get; set; }

		[Column("valor_repo")]
		public decimal ValorReposicion { get; set; }

		[Column("fecha_ult_com")]
		public DateTime? FechaUltimaCompra { get; set; }

		[Column("valor_ult_com")]
		public decimal ValorUltimaCompra { get; set; }

		[Column("precio_unitario")]
		public decimal PrecioUnitario { get; set; }

		[Column("margen_a")]
		public decimal MargenA { get; set; }

		[Column("margen_b")]
		public decimal MargenB { get; set; }

		public PeriodoEmpresaSede PeriodoEmpresaSede { get; set; }
		public Producto Producto { get; set; }
	}
}
