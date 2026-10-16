using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("sub_proce_p")]
	public class SubProcesoProduccion : BaseEntity
	{
		public override int EntityId => SubProcesoProduccionId;
		[Key]
		[Column("sub_proce_p_id", Order = 1)]
		public int SubProcesoProduccionId { get; set; }

		[Column("proce_prod_id")]
		public int ProcesoProduccionId { get; set; }

		[Column("nombre")]
		public string Nombre { get; set; }

		[Column("estado")]
		public string Estado { get; set; }

		[Column("usuario_creacion_id")]
		public int UsuarioCreacionId { get; set; }

		[Column("fecha_creacion")]
		public DateTime FechaCreacion { get; set; }

		[Column("usuario_modifica_id")]
		public int? UsuarioModificacionId { get; set; }

		[Column("fecha_modifica")]
		public DateTime? FechaModificacion { get; set; }

		public ProcesoProduccion ProcesoProduccion { get; set; }
	}
}
