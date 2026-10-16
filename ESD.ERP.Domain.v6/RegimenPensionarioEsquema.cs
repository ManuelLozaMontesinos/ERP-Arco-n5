using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("reg_pens_esq")]
	public class RegimenPensionarioEsquema : BaseEntity
	{
		public override int EntityId => RegimenPensionarioEsquemaId;
		[Key]
		[Column("reg_pens_esq_id", Order = 1)]
		public int RegimenPensionarioEsquemaId { get; set; }

		[Column("reg_pension_id")]
		public int RegimenPensionarioId { get; set; }

		[Column("tipo_esque_pen_id")]
		public int TipoEsquemaPensionId { get; set; }

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

		public RegimenPensionario RegimenPensionario { get; set; }
		public TipoEsquemaPension TipoEsquemaPension { get; set; }
	}
}
