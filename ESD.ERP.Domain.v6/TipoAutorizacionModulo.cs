using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("tipo_auto_modulo")]
	public class TipoAutorizacionModulo : BaseEntity
	{
		public override int EntityId => TipoAutorizacionModuloId;
		[Key]
		[Column("tipo_auto_modulo_id", Order = 1)]
		public int TipoAutorizacionModuloId { get; set; }

		[Column("opcion_modulo_id")]
		public int OpcionModuloId { get; set; }

		[Column("tipo_autoriza_id")]
		public int TipoAutorizacionId { get; set; }

		[Column("usuario_creacion_id")]
		public int UsuarioCreacionId { get; set; }

		[Column("fecha_creacion")]
		public DateTime FechaCreacion { get; set; }

		[Column("usuario_modifica_id")]
		public int? UsuarioModificacionId { get; set; }

		[Column("fecha_modifica")]
		public DateTime? FechaModificacion { get; set; }

		public OpcionModulo OpcionModulo { get; set; }
		public TipoAutorizacion TipoAutorizacion { get; set; }
	}
}
