using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("reg_pens_comi")]
	public class RegimenPensionarioComision : BaseEntity
	{
		public override int EntityId => RegimenPensionarioComisionId;
		[Key]
		[Column("reg_pens_comi_id", Order = 1)]
		public int RegimenPensionarioComisionId { get; set; }

		[Column("reg_pens_esq_id")]
		public int RegimenPensionarioEsquemaId { get; set; }

		[Column("tipo_apo_comi_id")]
		public int TipoAporteComisionId { get; set; }

		[Column("porcentaje")]
		public decimal Porcentaje { get; set; }

		[Column("fecha_alta")]
		public DateTime FechaAlta { get; set; }

		[Column("fecha_baja")]
		public DateTime? FechaBaja { get; set; }

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

		public RegimenPensionarioEsquema RegimenPensionarioEsquema { get; set; }
		public TipoAporteComision TipoAporteComision { get; set; }
		public string FechaBajaNull { get; set; }
	}
}
