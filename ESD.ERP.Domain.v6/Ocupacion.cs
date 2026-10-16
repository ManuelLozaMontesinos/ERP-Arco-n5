using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("ocupacion")]
	public class Ocupacion : BaseEntity
	{
		public override int EntityId => OcupacionId;
		[Key]
		[Column("ocupacion_id", Order = 1)]
		public int OcupacionId { get; set; }

		[Column("ocupacion")]
		public string Codigo { get; set; }

		[Column("nombre")]
		public string Nombre { get; set; }

		[Column("es_ejecutivo")]
		public string EsEjecutivo { get; set; }

		[Column("es_empleado")]
		public string EsEmpleado { get; set; }

		[Column("es_obrero")]
		public string EsObrero { get; set; }

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
