using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("concep_remu")]
	public class ConceptoRemunerativo : BaseEntity
	{
		public override int EntityId => ConceptoRemunerativoId;
		[Key]
		[Column("concep_remu_id", Order = 1)]
		public int ConceptoRemunerativoId { get; set; }

		[Column("concep_remu")]
		public string Codigo { get; set; }

		[Column("empresa_id")]
		public int EmpresaId { get; set; }

		[Column("nombre")]
		public string Nombre { get; set; }

		[Column("nombre_boleta")]
		public string NombreBoleta { get; set; }

		[Column("es_regula")]
		public string EsRegularizacion { get; set; }

		[Column("tipo_con_remu_id")]
		public int TipoConceptoRemunerativoId { get; set; }

		[Column("ing_trib_des_id")]
		public int? IngresoTributoDescuentoId { get; set; }

		[Column("usa_formula")]
		public string UsaFormula { get; set; }

		[Column("formula_hora")]
		public string FormulaHora { get; set; }

		[Column("formula")]
		public string Formula { get; set; }

		[Column("condicion")]
		public string Condicion { get; set; }

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

		public Empresa Empresa { get; set; }
		public TipoConceptoRemunerativo TipoConceptoRemunerativo { get; set; }
		public IngresoTributoDescuento IngresoTributoDescuento { get; set; }
	}
}
