using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("concep_base_im")]
	public class ConceptoBaseImponible : BaseEntity
	{
		public override int EntityId => ConceptoBaseImponibleId;
		[Key]
		[Column("concep_base_im_id", Order = 1)]
		public int ConceptoBaseImponibleId { get; set; }

		[Column("tipo_base_impo_id")]
		public int TipoBaseImponibleId { get; set; }

		[Column("concep_remu_id")]
		public int ConceptoRemunerativoId { get; set; }

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

		public TipoBaseImponible TipoBaseImponible { get; set; }
		public ConceptoRemunerativo ConceptoRemunerativo { get; set; }
	}
}
