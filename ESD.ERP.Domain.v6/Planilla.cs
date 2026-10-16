using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("planilla")]
	public class Planilla : BaseEntity
	{
		public override int EntityId => PlanillaId;
		[Key]
		[Column("planilla_id", Order = 1)]
		public int PlanillaId { get; set; }

		[Column("empresa_id")]
		public int EmpresaId { get; set; }

		[Column("nombre")]
		public string Nombre { get; set; }

		[Column("proc_anual")]
		public short ProcesoAnual { get; set; }

		[Column("tipo_trabaja_id")]
		public int TipoTrabajadorId { get; set; }

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

		public Empresa Empresa { get; set; }
		public TipoTrabajador TipoTrabajador { get; set; }
	}
}
