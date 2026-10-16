using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("menu")]
	public class Menu : BaseEntity
	{
		public override int EntityId => MenuId;
		[Key]
		[Column("menu_id", Order = 1)]
		public int MenuId { get; set; }

		[Column("nombre")]
		public string Nombre { get; set; }

		[Column("area")]
		public string Area { get; set; }

		[Column("url")]
		public string Url { get; set; }

		[Column("icono")]
		public string Icono { get; set; }

		[Column("menu_padre_id")]
		public int? MenuPadreId { get; set; }

		[Column("orden")]
		public int Orden { get; set; }

		[Column("estado")]
		public string Estado { get; set; }

	}
}
