using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("modulo")]
	public class Modulo : BaseEntity
	{
		public override int EntityId => ModuloId;
		[Key]
		[Column("modulo_id", Order = 1)]
		public int ModuloId { get; set; }

		[Column("modulo")]
		public string Codigo { get; set; }

		[Column("nombre")]
		public string Nombre { get; set; }

		[Column("estado")]
		public string Estado { get; set; }

	}
}
