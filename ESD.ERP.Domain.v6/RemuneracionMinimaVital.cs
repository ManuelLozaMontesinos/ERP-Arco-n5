using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("rem_min_vit")]
	public class RemuneracionMinimaVital : BaseEntity
	{
		public override int EntityId => RemuneracionMinimaVitalId;
		[Key]
		[Column("rem_min_vit_id", Order = 1)]
		public int RemuneracionMinimaVitalId { get; set; }

		[Column("monto")]
		public decimal Monto { get; set; }

		[Column("fecha_alta")]
		public DateTime FechaAlta { get; set; }

		[Column("fecha_baja")]
		public DateTime? FechaBaja { get; set; }

		[Column("estado")]
		public string Estado { get; set; }

		[Column("usuario_creacion_id")]
		public int UsuarioCreacionId { get; set; }

		[Column("fecha_creacion")]
		public DateTime FechaCreacion { get; set; }

		[Column("usuario_modifica_id")]
		public int? UsuarioModificaId { get; set; }

		[Column("fecha_modifica")]
		public DateTime? FechaModifica { get; set; }

		public string FechaBajaNull { get; set; }
	}
}
