using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class RutaSubProcesoProduccionDTO : DTOBase
	{
		public override int Id =>RutaSubProcesoProduccionId;
		public int RutaSubProcesoProduccionId { get; set; }
		public int RutaProduccionId { get; set; }
		public int SubProcesoProduccionId { get; set; }
		public int Orden { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public RutaProduccionDTO RutaProduccionDTO { get; set; }
		public SubProcesoProduccionDTO SubProcesoProduccionDTO { get; set; }
	}
}
