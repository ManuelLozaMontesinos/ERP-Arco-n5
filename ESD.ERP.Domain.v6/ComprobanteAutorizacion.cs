using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("comprob_autoriza")]
	public class ComprobanteAutorizacion : BaseEntity
	{
		public override int EntityId => ComprobanteAutorizacionId;
		[Key]
		[Column("comprob_autoriza_id", Order = 1)]
		public int ComprobanteAutorizacionId { get; set; }

		[Column("tipo_autoriza_id")]
		public int TipoAutorizacionId { get; set; }

		[Column("comprobante_id")]
		public int ComprobanteId { get; set; }

		[Column("usuario_id")]
		public int? UsuarioId { get; set; }

		[Column("fecha_auto")]
		public DateTime? FechaAutorizacion { get; set; }

		[Column("usuario_creacion_id")]
		public int UsuarioCreacionId { get; set; }

		[Column("estado")]
		public string Estado { get; set; }

		[Column("estado_aproba_id")]
		public int EstadoAprobacionId { get; set; }

		[Column("fecha_creacion")]
		public DateTime FechaCreacion { get; set; }

		[Column("usuario_modifica_id")]
		public int? UsuarioModificacionId { get; set; }

		[Column("fecha_modifica")]
		public DateTime? FechaModificacion { get; set; }

		public TipoAutorizacion TipoAutorizacion { get; set; }
		public Comprobante Comprobante { get; set; }
		public Usuario Usuario { get; set; }
		public List<int> TipoAutorizacionIdList { get; set; }
		public EstadoAprobacion EstadoAprobacion { get; set; }
	}
}
