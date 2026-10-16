using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("caja_perso")]
	public class CajaPersonal : BaseEntity
	{
		public override int EntityId => CajaPersonalId;
		[Key]
		[Column("caja_perso_id", Order = 1)]
		public int CajaPersonalId { get; set; }

		[Column("caja_id", Order = 1)]
		public int CajaId { get; set; }

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

		public Caja Caja { get; set; }
		public Personal Personal { get; set; }
	}
}
