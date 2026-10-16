using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("moneda")]
	public class Moneda : BaseEntity
	{
		public override int EntityId => MonedaId;
		[Key]
		[Column("moneda_id", Order = 1)]
		public int MonedaId { get; set; }

		[Column("moneda")]
		public string Codigo { get; set; }

		[Column("nombre")]
		public string Nombre { get; set; }

		[Column("simbolo")]
		public string Simbolo { get; set; }

		[Column("es_local")]
		public string EsLocal { get; set; }

		[Column("codigo_iso")]
		public string CodigoIso { get; set; }

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
