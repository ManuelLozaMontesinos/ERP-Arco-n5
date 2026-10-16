using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("tipo_nat_juri")]
	public class TipoNaturalezaJuridica : BaseEntity
	{
		public override int EntityId => TipoNaturalezaJuridicaId;
		[Key]
		[Column("tipo_nat_juri_id", Order = 1)]
		public int TipoNaturalezaJuridicaId { get; set; }

		[Column("tipo_nat_juri")]
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

	}
}
