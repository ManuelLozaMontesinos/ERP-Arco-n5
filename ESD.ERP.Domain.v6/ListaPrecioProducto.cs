using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("lista_p_prod")]
	public class ListaPrecioProducto : BaseEntity
	{
		public override int EntityId => ListaPrecioProductoId;
		[Key]
		[Column("lista_p_prod_id", Order = 1)]
		public int ListaPrecioProductoId { get; set; }

		[Column("lista_precio_id")]
		public int ListaPrecioId { get; set; }

		[Column("producto_id")]
		public int ProductoId { get; set; }

		[Column("valor_unitario")]
		public decimal ValorUnitario { get; set; }

		[Column("precio_unitario")]
		public decimal PrecioUnitario { get; set; }

		[Column("permite_dcto")]
		public string PermiteDescuento { get; set; }

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

		public ListaPrecio ListaPrecio { get; set; }
		public Producto Producto { get; set; }
		public TipoDescuento TipoDescuento { get; set; }
		public int MesContableId { get; set; }
		public int PeriodoId { get; set; }
	}
}
