using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class MenuDTO : DTOBase
	{
		public override int Id => MenuId;
		public int MenuId { get; set; }
		public string Nombre { get; set; }
		public string Area { get; set; }
		public string Url { get; set; }
		public string Icono { get; set; }
		public int? MenuPadreId { get; set; }
		public int Orden { get; set; }
		public string Estado { get; set; }
	}
}
