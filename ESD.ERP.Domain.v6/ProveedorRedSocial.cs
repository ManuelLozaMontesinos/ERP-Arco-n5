using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("prove_r_s")]
	public class ProveedorRedSocial : BaseEntity
	{
		public override int EntityId => ProveedorRedSocialId;
		[Key]
		[Column("prove_r_s_id", Order = 1)]
		public int ProveedorRedSocialId { get; set; }

		[Column("prove_id")]
		public int ProveedorId { get; set; }

		[Column("red_social_id")]
		public int RedSocialId { get; set; }

		[Column("url")]
		public string Url { get; set; }

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

		public Proveedor Proveedor { get; set; }
		public RedSocial RedSocial { get; set; }
	}
}
