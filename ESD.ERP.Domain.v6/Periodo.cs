using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("periodo")]
	public class Periodo : BaseEntity
	{
		public override int EntityId => PeriodoId;
		[Key]
		[Column("periodo_id", Order = 1)]
		public int PeriodoId { get; set; }

		[Column("periodo")]
		public int Codigo { get; set; }

		[Column("habi_proc_bat")]
		public string HabilitarProcesoBatch { get; set; }

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

		public int NotPeriodoId { get; set; }
	}
}
