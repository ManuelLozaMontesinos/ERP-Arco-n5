using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("notifi_push")]
	public class NotificacionPush : BaseEntity
	{
		public override int EntityId => NotificacionPushId;
		[Key]
		[Column("notifi_push_id", Order = 1)]
		public int NotificacionPushId { get; set; }

		[Column("fecha")]
		public DateTime? Fecha { get; set; }
		[Column("fecha_reg")]
		public DateTime FechaRegistro { get; set; }
		[Column("titulo")]
		public string Titulo { get; set; }

		[Column("descripcion")]
		public string Descripcion { get; set; }

		[Column("lectura")]
		public string Lectura { get; set; }
		[Column("usuario_id")]
		public int UsuarioId { get; set; }

	}
}
