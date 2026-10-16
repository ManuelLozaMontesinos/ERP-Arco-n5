using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class VariableCalculoDTO : DTOBase
	{
		public override int Id => VariableCalculoId;
		public int VariableCalculoId { get; set; }
		public string Codigo { get; set; }
		public string Nombre { get; set; }
		public int TipoVariableCalculoId { get; set; }
		public string Condicion { get; set; }
		public int? TipoDatoId { get; set; }
		public decimal ValorNumericoVerdad { get; set; }
		public decimal ValorNumericoFalso { get; set; }
		public string ValorCadenaVerdad { get; set; }
		public string ValorCadenaFalso { get; set; }
		public string TablaOrigen { get; set; }
		public string TablaCondicion { get; set; }
		public string TablaAgrupacion { get; set; }
		public string EsSubconsulta { get; set; }
		public string SubConsultaAlias { get; set; }
		public string SubConsultaTipo { get; set; }
		public string SubConsultaColumna { get; set; }
		public string SubConsultaEnlace { get; set; }
		public string AlcanceFormulaHora { get; set; }
		public string AlcanceFormula { get; set; }
		public string AlcanceCondicion { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public TipoVariableCalculoDTO TipoVariableCalculoDTO { get; set; }
		public TipoDatoDTO TipoDatoDTO { get; set; }

		public int NotVariableCalculoId { get; set; }
	}
}
