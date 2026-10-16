using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("directiva_usuario")]
	public class DirectivaUsuario : BaseEntity
	{
		public override int EntityId => DirectivaUsuarioId;
		[Key]
		[Column("directiva_usuario_id", Order = 1)]
		public int DirectivaUsuarioId { get; set; }

		[Column("directiva_regla_id")]
		public int DirectivaReglaId { get; set; }

		[Column("usuario_id")]
		public int UsuarioId { get; set; }

		[Column("valor")]
		public int Valor { get; set; }

		[Column("codigo")]
		public string Codigo { get; set; }

		[Column("descripcion")]
		public string Descripcion { get; set; }

		[Column("usuario_creacion_id")]
		public int UsuarioCreacionId { get; set; }

		[Column("fecha_creacion")]
		public DateTime FechaCreacion { get; set; }

		[Column("usuario_modifica_id")]
		public int? UsuarioModificacionId { get; set; }

		[Column("fecha_modifica")]
		public DateTime? FechaModificacion { get; set; }

		public DirectivaRegla DirectivaRegla { get; set; }
		public Usuario Usuario { get; set; }
	}
}
