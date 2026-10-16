using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("reloj")]
	public class Reloj : BaseEntity
	{
		public override int EntityId => RelojId;
		[Key]
		[Column("reloj_id", Order = 1)]
		public int RelojId { get; set; }

		[Column("empresa_id")]
		public int EmpresaId { get; set; }

		[Column("nombre")]
		public string Nombre { get; set; }

		[Column("ruta_archivo")]
		public string RutaArchivo { get; set; }

		[Column("es_central")]
		public string EsCentral { get; set; }

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
	}
}
