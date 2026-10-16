using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("reloj_marca")]
	public class RelojMarcacion : BaseEntity
	{
		public override int EntityId => RelojMarcacionId;
		[Key]
		[Column("reloj_marca_id", Order = 1)]
		public int RelojMarcacionId { get; set; }

		[Column("reloj_id")]
		public int RelojId { get; set; }

		[Column("personal_id")]
		public int PersonalId { get; set; }

		[Column("fecha")]
		public DateTime Fecha { get; set; }

		[Column("fecha_orig")]
		public DateTime? FechaOriginal { get; set; }

		[Column("fecha_final")]
		public DateTime FechaFinal { get; set; }

		[Column("observa")]
		public string Observacion { get; set; }

		[Column("estado")]
		public string Estado { get; set; }

		public Personal Personal { get; set; }
		public Reloj Reloj { get; set; }
	}
}
