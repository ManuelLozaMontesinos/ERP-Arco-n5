using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class PlanillaConceptoRemunerativoDTO : DTOBase
	{
		public override int Id => PlanillaConceptoRemunerativoId;
		public int PlanillaConceptoRemunerativoId { get; set; }
		public int PlanillaId { get; set; }
		public int ConceptoRemunerativoId { get; set; }
		public short Orden { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public PlanillaDTO PlanillaDTO { get; set; }
		public ConceptoRemunerativoDTO ConceptoRemunerativoDTO { get; set; }
	}
}
