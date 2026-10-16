using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("orden_p_p_ent")]
	public class OrdenProduccionProductoEntrega : BaseEntity
	{
		public override int EntityId => OrdenProduccionProductoEntregaId;
		[Key]
		[Column("orden_p_p_ent_id", Order = 1)]
		public int OrdenProduccionProductoEntregaId { get; set; }

		[Column("orden_p_prod_id")]
		public int OrdenProduccionProductoId { get; set; }

		[Column("cantidad")]
		public decimal Cantidad { get; set; }

		[Column("entrega_a")]
		public string EntregadoA { get; set; }

		[Column("fecha")]
		public DateTime Fecha { get; set; }

		[Column("observa")]
		public string Observacion { get; set; }

		[Column("personal_id")]
		public int PersonalId { get; set; }

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

		public OrdenProduccionProducto OrdenProduccionProducto { get; set; }
		public Personal Personal { get; set; }
	}
}
