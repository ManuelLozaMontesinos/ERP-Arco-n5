using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("alm_trans_config")]
	public class AlmacenTransaccionConfiguracion : BaseEntity
	{
		public override int EntityId => AlmacenTransaccionConfiguracionId;
		[Key]
		[Column("alm_trans_config_id", Order = 1)]
		public int AlmacenTransaccionConfiguracionId { get; set; }

		[Column("alm_transa_id")]
		public int AlmacenTransaccionId { get; set; }

		[Column("atrib_config_id")]
		public int AtributoConfiguracionId { get; set; }

		[Column("visible")]
		public string Visible { get; set; }

		[Column("requerido")]
		public string Requerido { get; set; }

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

		public AlmacenTransaccion AlmacenTransaccion { get; set; }
		public AtributoConfiguracion AtributoConfiguracion { get; set; }
	}
}
