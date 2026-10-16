using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("sub_p_p_perso")]
	public class SubProcesoProduccionPersonal : BaseEntity
	{
		public override int EntityId => SubProcesoProduccionId;
		[Key]
		[Column("sub_p_p_perso_id", Order = 1)]
		public int SubProcesoProduccionPersonalId { get; set; }

		[Column("sub_proce_p_id")]
		public int SubProcesoProduccionId { get; set; }

		[Column("personal_id")]
		public int PersonalId { get; set; }

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
		public Personal Personal { get; set; }
	}
}
