using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class SubProcesoProduccionDTO : DTOBase
	{
		public override int Id => SubProcesoProduccionId;
		public int SubProcesoProduccionId { get; set; }
		public int ProcesoProduccionId { get; set; }
		public string Nombre { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificaId { get; set; }
		public DateTime? FechaModifica { get; set; }
		public ProcesoProduccionDTO ProcesoProduccionDTO { get; set; }
	}
}
