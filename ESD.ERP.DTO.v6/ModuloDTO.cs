using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class ModuloDTO : DTOBase
	{
		public override int Id => ModuloId;
		public int ModuloId { get; set; }
		public string Codigo { get; set; }
		public string Nombre { get; set; }
		public string Estado { get; set; }
	}
}
