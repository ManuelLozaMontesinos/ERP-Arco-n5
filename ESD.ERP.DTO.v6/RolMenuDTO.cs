using ESD.Core.Service.DTO;
using System.Collections.Generic;

namespace ESD.ERP.DTO
{
	public class RolMenuDTO : DTOBase
	{
		public override int Id => RolMenuId;
		public int RolMenuId { get; set; }
		public int RolId { get; set; }
		public int MenuId { get; set; }
		public string Lectura { get; set; }
		public string Escritura { get; set; }
		public RolDTO RolDTO { get; set; }
		public MenuDTO MenuDTO { get; set; }
		public List<string> RolCodigoList { get; set; }
	}
}
