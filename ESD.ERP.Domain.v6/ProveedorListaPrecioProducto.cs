using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("prove_l_p_p")]
	public class ProveedorListaPrecioProducto : BaseEntity
	{
		public override int EntityId => ProveedorListaPrecioProductoId;
		[Key]
		[Column("prove_l_p_p_id", Order = 1)]
		public int ProveedorListaPrecioProductoId { get; set; }

		[Column("prove_lista_p_id")]
		public int ProveedorListaPrecioId { get; set; }

		[Column("producto_id")]
		public int ProductoId { get; set; }

		[Column("valor_u_ant")]
		public decimal ValorUnitarioAnterior { get; set; }

		[Column("precio_u_ant")]
		public decimal PrecioUnitarioAnterior { get; set; }

		[Column("fecha_ant")]
		public DateTime? FechaAnterior { get; set; }

		[Column("valor_unitario")]
		public decimal ValorUnitario { get; set; }

		[Column("precio_unitario")]
		public decimal PrecioUnitario { get; set; }

		[Column("fecha_vig")]
		public DateTime FechaVigente { get; set; }

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

		public ProveedorListaPrecio ProveedorListaPrecio { get; set; }
		public Producto Producto { get; set; }
		public List<int> ProductoIdList { get; set; }
	}
}
