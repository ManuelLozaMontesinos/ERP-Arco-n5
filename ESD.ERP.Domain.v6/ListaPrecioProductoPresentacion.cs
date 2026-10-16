using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("lista_p_prod_pres")]
	public class ListaPrecioProductoPresentacion : BaseEntity
	{
		public override int EntityId => ListaPrecioProductoPresentacionId;
		[Key]
		[Column("lista_p_prod_pres_id", Order = 1)]
		public int ListaPrecioProductoPresentacionId { get; set; }

		[Column("lista_p_prod_id")]
		public int ListaPrecioProductoId { get; set; }

		[Column("produ_p_vta_id")]
		public int ProductoPresentacionVentaId { get; set; }

		[Column("porc_precio_base")]
		public decimal PorcentajePrecioBase { get; set; }

		[Column("num_dec_redo")]
		public int NumeroDecimalesRedondeo { get; set; }

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

		public ListaPrecioProducto ListaPrecioProducto { get; set; }
		public ProductoPresentacionVenta ProductoPresentacionVenta { get; set; }
	}
}
