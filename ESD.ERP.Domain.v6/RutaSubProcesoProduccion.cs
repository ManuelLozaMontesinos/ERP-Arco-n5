using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("ruta_p_s_p_p")]
	public class RutaSubProcesoProduccion : BaseEntity
	{
		public override int EntityId => RutaSubProcesoProduccionId;
		[Key]
		[Column("ruta_p_s_p_p_id", Order = 1)]
		public int RutaSubProcesoProduccionId { get; set; }

		[Column("ruta_prod_id")]
		public int RutaProduccionId { get; set; }

		[Column("sub_proce_p_id")]
		public int SubProcesoProduccionId { get; set; }
		[Column("orden")]
		public int Orden { get; set; }
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

		public RutaProduccion RutaProduccion { get; set; }
		public SubProcesoProduccion SubProcesoProduccion { get; set; }
	}
}
