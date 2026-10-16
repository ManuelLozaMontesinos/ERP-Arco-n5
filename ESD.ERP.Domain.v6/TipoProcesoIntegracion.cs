using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("tipo_pro_integra")]
	public class TipoProcesoIntegracion : BaseEntity
	{
		public override int EntityId => TipoProcesoIntegracionId;
		[Key]
		[Column("tipo_pro_integra_id", Order = 1)]
		public int TipoProcesoIntegracionId { get; set; }

		[Column("tipo_pro_integra")]
		public string Codigo { get; set; }

		[Column("nombre")]
		public string Nombre { get; set; }

		[Column("estado")]
		public string Estado { get; set; }

	}
}
