using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class PersonalPlanillaDTO : DTOBase
	{
		public override int Id => PersonalPlanillaId;
		public int PersonalPlanillaId { get; set; }
		public int PersonalId { get; set; }
		public int PlanillaId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public PersonalDTO PersonalDTO { get; set; }
		public PlanillaDTO PlanillaDTO { get; set; }
	}
}
