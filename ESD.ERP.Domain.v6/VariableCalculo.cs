using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("var_calc")]
	public class VariableCalculo : BaseEntity
	{
		public override int EntityId => VariableCalculoId;
		[Key]
		[Column("var_calc_id", Order = 1)]
		public int VariableCalculoId { get; set; }

		[Column("var_calc")]
		public string Codigo { get; set; }

		[Column("nombre")]
		public string Nombre { get; set; }

		[Column("tipo_v_calc_id")]
		public int TipoVariableCalculoId { get; set; }

		[Column("condicion")]
		public string Condicion { get; set; }

		[Column("tipo_dato_id")]
		public int? TipoDatoId { get; set; }

		[Column("valor_n_v")]
		public decimal ValorNumericoVerdad { get; set; }

		[Column("valor_n_f")]
		public decimal ValorNumericoFalso { get; set; }

		[Column("valor_c_v")]
		public string ValorCadenaVerdad { get; set; }

		[Column("valor_c_f")]
		public string ValorCadenaFalso { get; set; }

		[Column("tabla_orig")]
		public string TablaOrigen { get; set; }

		[Column("tabla_condi")]
		public string TablaCondicion { get; set; }

		[Column("tabla_agrupa")]
		public string TablaAgrupacion { get; set; }

		[Column("es_subconsu")]
		public string EsSubconsulta { get; set; }

		[Column("subconsu_alias")]
		public string SubConsultaAlias { get; set; }

		[Column("subconsu_tipo")]
		public string SubConsultaTipo { get; set; }

		[Column("subconsu_column")]
		public string SubConsultaColumna { get; set; }

		[Column("subconsu_enlace")]
		public string SubConsultaEnlace { get; set; }

		[Column("alcance_for_h")]
		public string AlcanceFormulaHora { get; set; }

		[Column("alcance_formu")]
		public string AlcanceFormula { get; set; }

		[Column("alcance_condi")]
		public string AlcanceCondicion { get; set; }

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

		public TipoVariableCalculo TipoVariableCalculo { get; set; }
		public TipoDato TipoDato { get; set; }

		public int NotVariableCalculoId { get; set; }
		public List<string> VariableCalculoList { get; set; }
	}
}
