using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("alm_vol_prod")]
	public class AlmacenVolumenProducto : BaseEntity
	{
		public override int EntityId => AlmacenVolumenProductoId;
		[Key]
		[Column("alm_vol_prod_id", Order = 1)]
		public int AlmacenVolumenProductoId { get; set; }

		[Column("empresa_sede_id")]
		public int EmpresaSedeId { get; set; }

		[Column("producto_id")]
		public int ProductoId { get; set; }

		[Column("stock_min")]
		public decimal StockMinimo { get; set; }

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

		public EmpresaSede EmpresaSede { get; set; }
		public Producto Producto { get; set; }
		public Proveedor Proveedor { get; set; }

		public int PeriodoEmpresaSedeId { get; set; }
	}
}
