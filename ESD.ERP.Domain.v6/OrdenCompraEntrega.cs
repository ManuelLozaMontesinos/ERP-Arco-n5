using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("orden_c_ent")]
	public class OrdenCompraEntrega : BaseEntity
	{
		public override int EntityId => OrdenCompraEntregaId;
		[Key]
		[Column("orden_c_ent_id", Order = 1)]
		public int OrdenCompraEntregaId { get; set; }

		[Column("orden_c_prod_id")]
		public int OrdenCompraProductoId { get; set; }

		[Column("cantidad")]
		public decimal Cantidad { get; set; }

		[Column("fecha")]
		public DateTime? Fecha { get; set; }

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

		public OrdenCompraProducto OrdenCompraProducto { get; set; }
	}
}
