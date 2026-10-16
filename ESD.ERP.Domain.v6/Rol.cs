using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("rol")]
	public class Rol : BaseEntity
	{
		public override int EntityId => RolId;
		[Key]
		[Column("rol_id", Order = 1)]
		public int RolId { get; set; }

		[Column("empresa_id")]
		public int? EmpresaId { get; set; }

		[Column("rol")]
		public string Codigo { get; set; }

		[Column("nombre")]
		public string Nombre { get; set; }

		[Column("origen_registro_id")]
		public int OrigenRegistroId { get; set; }

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

		public OrigenRegistro OrigenRegistro { get; set; }
	}
}
