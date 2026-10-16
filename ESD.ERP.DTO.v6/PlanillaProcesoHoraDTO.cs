using System;
using System.Collections.Generic;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class PlanillaProcesoHoraDTO : DTOBase
	{
		public override int Id => PlanillaProcesoHoraId;
		public int PlanillaProcesoHoraId { get; set; }
		public int PlanillaProcesoId { get; set; }
		public int HoraRemunerativaId { get; set; }
		public int PersonalId { get; set; }
		public decimal Cantidad { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public PlanillaProcesoDTO PlanillaProcesoDTO { get; set; }
		public HoraRemunerativaDTO HoraRemunerativaDTO { get; set; }
		public PersonalDTO PersonalDTO { get; set; }
		public List<int> PersonalIdList { get; set; }
	}
}