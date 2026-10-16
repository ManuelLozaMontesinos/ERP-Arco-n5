using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("tipo_cambio")]
	public class TipoCambio : BaseEntity
	{
		public override int EntityId => TipoCambioId;
		[Key]
		[Column("tipo_cambio_id", Order = 1)]
		public int TipoCambioId { get; set; }

		[Column("tipo_cambio")]
		public string Codigo { get; set; }

		[Column("nombre")]
		public string Nombre { get; set; }

		[Column("origen_registro_id")]
		public int OrigenRegistroId { get; set; }

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

		public OrigenRegistro OrigenRegistro { get; set; }
	}
}
