using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("motivo_trasla")]
	public class MotivoTraslado : BaseEntity
	{
		public override int EntityId => MotivoTrasladoId;
		[Key]
		[Column("motivo_trasla_id", Order = 1)]
		public int MotivoTrasladoId { get; set; }

		[Column("motivo_trasla")]
		public string Codigo { get; set; }

		[Column("nombre")]
		public string Nombre { get; set; }

		[Column("codigo_sunat")]
		public string CodigoSunat { get; set; }

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

	}
}
