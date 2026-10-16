using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("tipo_afecta_igv")]
	public class TipoAfectacionIgv : BaseEntity
	{
		public override int EntityId => TipoAfectacionIgvId;
		[Key]
		[Column("tipo_afecta_igv_id", Order = 1)]
		public int TipoAfectacionIgvId { get; set; }

		[Column("tipo_igv_id")]
		public int TipoIgvId { get; set; }

		[Column("tipo_afecta_igv")]
		public string Codigo { get; set; }

		[Column("nombre")]
		public string Nombre { get; set; }

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

		public TipoIgv TipoIgv { get; set; }
	}
}
