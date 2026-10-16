using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class ProcesoIntegracionDTO : DTOBase
	{
		public override int Id => ProcesoIntegracionId;
		public int ProcesoIntegracionId { get; set; }
		public int ModuloId { get; set; }
		public string Codigo { get; set; }
		public string Descripcion { get; set; }
		public string Nombre { get; set; }
		public int TipoProcesoIntegracionId { get; set; }
		public string Estado { get; set; }
		public ModuloDTO ModuloDTO { get; set; }
		public TipoProcesoIntegracionDTO TipoProcesoIntegracionDTO { get; set; }
	}
}
