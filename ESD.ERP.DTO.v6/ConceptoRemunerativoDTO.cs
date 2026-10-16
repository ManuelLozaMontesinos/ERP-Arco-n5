using System;
using System.Collections.Generic;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class ConceptoRemunerativoDTO : DTOBase
	{
		public override int Id => ConceptoRemunerativoId;
		public int ConceptoRemunerativoId { get; set; }
		public string Codigo { get; set; }
		public int EmpresaId { get; set; }
		public string Nombre { get; set; }
		public string NombreBoleta { get; set; }
		public string EsRegularizacion { get; set; }
		public int TipoConceptoRemunerativoId { get; set; }
		public int? IngresoTributoDescuentoId { get; set; }
		public string UsaFormula { get; set; }
		public string FormulaHora { get; set; }
		public string Formula { get; set; }
		public string Condicion { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public EmpresaDTO EmpresaDTO { get; set; }
		public TipoConceptoRemunerativoDTO TipoConceptoRemunerativoDTO { get; set; }
		public IngresoTributoDescuentoDTO IngresoTributoDescuentoDTO { get; set; }

		public List<ConceptoBaseImponibleDTO> ConceptoBaseImponibleDTOList { get; set; }
	}
}
