using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("comprob_p_ent")]
	public class ComprobanteProductoEntrega : BaseEntity
	{
		public override int EntityId => ComprobanteProductoEntregaId;
		[Key]
		[Column("comprob_p_ent_id", Order = 1)]
		public int ComprobanteProductoEntregaId { get; set; }

		[Column("comprob_prod_id")]
		public int ComprobanteProductoId { get; set; }

		[Column("cantidad")]
		public decimal Cantidad { get; set; }

		[Column("entrega_desp")]
		public string EntregaDespacho { get; set; }

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

		public ComprobanteProducto ComprobanteProducto { get; set; }
		public Personal Personal { get; set; }
	}
}
