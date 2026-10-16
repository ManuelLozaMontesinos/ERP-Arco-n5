using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("situ_espe_trab")]
	public class SituacionEspecialTrabajador : BaseEntity
	{
		public override int EntityId => SituacionEspecialTrabajadorId;
		[Key]
		[Column("situ_espe_trab_id", Order = 1)]
		public int SituacionEspecialTrabajadorId { get; set; }

		[Column("situ_espe_trab")]
		public string Codigo { get; set; }

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

	}
}
