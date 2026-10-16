using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class TipoProcesoIntegracionDTO : DTOBase
	{
		public override int Id => TipoProcesoIntegracionId;
		public int TipoProcesoIntegracionId { get; set; }
		public string Codigo { get; set; }
		public string Nombre { get; set; }
		public string Estado { get; set; }
	}
}
