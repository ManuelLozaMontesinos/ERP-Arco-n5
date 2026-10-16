using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("rol_menu")]
	public class RolMenu : BaseEntity
	{
		public override int EntityId => RolMenuId;
		[Key]
		[Column("rol_menu_id", Order = 1)]
		public int RolMenuId { get; set; }

		[Column("rol_id")]
		public int RolId { get; set; }

		[Column("menu_id")]
		public int MenuId { get; set; }

		[Column("lectura")]
		public string Lectura { get; set; }

		[Column("escritura")]
		public string Escritura { get; set; }

		public Rol Rol { get; set; }
		public Menu Menu { get; set; }
		public List<RolMenu> RolMenuList { get; set; }
		public List<string> RolCodigoList { get; set; }
	}
}
