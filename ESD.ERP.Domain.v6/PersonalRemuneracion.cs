using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("personal_rem")]
	public class PersonalRemuneracion : BaseEntity
	{
		public override int EntityId => PersonalRemuneracionId;
		[Key]
		[Column("personal_rem_id", Order = 1)]
		public int PersonalRemuneracionId { get; set; }

		[Column("personal_id")]
		public int PersonalId { get; set; }

		[Column("tipo_remunera_id")]
		public int TipoRemuneracionId { get; set; }
		
		[Column("moneda_id")]
		public int MonedaId { get; set; }

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
		public int? UsuarioModificacionId { get; set; }

		[Column("fecha_modifica")]
		public DateTime? FechaModificacion { get; set; }

		public Personal Personal { get; set; }
		public Moneda Moneda { get; set; }
		public TipoRemuneracion TipoRemuneracion { get; set; }
		public string FechaBajaNull { get; set; }
	}
}
