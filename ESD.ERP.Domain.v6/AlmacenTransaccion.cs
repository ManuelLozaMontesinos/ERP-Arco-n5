using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("alm_transa")]
	public class AlmacenTransaccion : BaseEntity
	{
		public override int EntityId => AlmacenTransaccionId;
		[Key]
		[Column("alm_transa_id", Order = 1)]
		public int AlmacenTransaccionId { get; set; }

		[Column("almacen_id")]
		public int AlmacenId { get; set; }

		[Column("trans_alma_id")]
		public int TransaccionAlmacenId { get; set; }

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

		public Almacen Almacen { get; set; }
		public TransaccionAlmacen TransaccionAlmacen { get; set; }
	}
}
