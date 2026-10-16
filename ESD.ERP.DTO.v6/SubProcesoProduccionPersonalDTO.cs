using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class SubProcesoProduccionPersonalDTO : DTOBase
	{
		public override int Id => SubProcesoProduccionPersonalId;
		public int SubProcesoProduccionPersonalId { get; set; }
		public int SubProcesoProduccionId { get; set; }
		public int PersonalId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificaId { get; set; }
		public DateTime? FechaModifica { get; set; }
		public ProcesoProduccionDTO ProcesoProduccionDTO { get; set; }
		public PersonalDTO PersonalDTO { get; set; }
	}
}
