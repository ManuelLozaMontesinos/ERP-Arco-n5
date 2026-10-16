using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class PlanillaProcesoRegularizacionDTO : DTOBase
	{
		public override int Id => PlanillaProcesoRegularizacionId;
		public int PlanillaProcesoRegularizacionId { get; set; }
		public int PlanillaProcesoId { get; set; }
		public int PersonalId { get; set; }
		public int ConceptoRemunerativoId { get; set; }
		public decimal CantidadHora { get; set; }
		public decimal Monto { get; set; }
		public string Observacion { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public PlanillaProcesoDTO PlanillaProcesoDTO { get; set; }
		public PersonalDTO PersonalDTO { get; set; }
		public ConceptoRemunerativoDTO ConceptoRemunerativoDTO { get; set; }
	}
}
