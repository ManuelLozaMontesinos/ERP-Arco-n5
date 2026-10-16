using ESD.Core.Service.DTO;
using System.Collections.Generic;

namespace ESD.ERP.DTO
{
	public class RolUsuarioDTO : DTOBase
	{
		public override int Id => RolUsuarioId;
		public int RolUsuarioId { get; set; }
		public int UsuarioId { get; set; }
		public int RolId { get; set; }
		public UsuarioDTO UsuarioDTO { get; set; }
		public RolDTO RolDTO { get; set; }
		public List<string> RolList { get; set; }
	}
}
