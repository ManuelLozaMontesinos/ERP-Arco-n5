using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("comprob_traza")]
	public class ComprobanteTraza : BaseEntity
	{
		public override int EntityId => ComprobanteTrazaId;
		[Key]
		[Column("comprob_traza_id", Order = 1)]
		public int ComprobanteTrazaId { get; set; }

		[Column("comprobante_id")]
		public int ComprobanteId { get; set; }

		[Column("tipo_acc_audit_id")]
		public int TipoAccionAuditoriaId { get; set; }

		[Column("fecha")]
		public DateTime Fecha { get; set; }

		[Column("descripcion")]
		public string Descripcion { get; set; }

		[Column("usuario_id")]
		public int UsuarioId { get; set; }

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

		public Comprobante Comprobante { get; set; }
		public TipoAccionAuditoria TipoAccionAuditoria { get; set; }
		public Usuario Usuario { get; set; }

	}
}
