using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("proce_integra")]
	public class ProcesoIntegracion : BaseEntity
	{
		public override int EntityId => ProcesoIntegracionId;
		[Key]
		[Column("proce_integra_id", Order = 1)]
		public int ProcesoIntegracionId { get; set; }

		[Column("modulo_id")]
		public int ModuloId { get; set; }

		[Column("proce_integra")]
		public string Codigo { get; set; }

		[Column("descripcion")]
		public string Descripcion { get; set; }

		[Column("nombre")]
		public string Nombre { get; set; }

		[Column("tipo_pro_integra_id")]
		public int TipoProcesoIntegracionId { get; set; }

		[Column("estado")]
		public string Estado { get; set; }

		public Modulo Modulo { get; set; }
		public TipoProcesoIntegracion TipoProcesoIntegracion { get; set; }
	}
}
