using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("produ_p_vta")]
	public class ProductoPresentacionVenta : BaseEntity
	{
		public override int EntityId => ProductoPresentacionVentaId;
		[Key]
		[Column("produ_p_vta_id", Order = 1)]
		public int ProductoPresentacionVentaId { get; set; }

		[Column("producto_id")]
		public int ProductoId { get; set; }

		[Column("unidad_medida_id")]
		public int UnidadMedidaId { get; set; }

		[Column("cant_equiv")]
		public decimal CantidadEquivalente { get; set; }

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

		public Producto Producto { get; set; }
		public UnidadMedida UnidadMedida { get; set; }
	}
}
