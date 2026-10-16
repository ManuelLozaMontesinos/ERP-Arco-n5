using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class RelojMarcacionDTO : DTOBase
	{
		public override int Id => RelojMarcacionId;
		public int RelojMarcacionId { get; set; }
		public int RelojId { get; set; }
		public int PersonalId { get; set; }
		public DateTime Fecha { get; set; }
		public DateTime? FechaOriginal { get; set; }
		public DateTime FechaFinal { get; set; }
		public string Observacion { get; set; }
		public string Estado { get; set; }
		public PersonalDTO PersonalDTO { get; set; }
		public RelojDTO RelojDTO  { get; set; }
	}
}
