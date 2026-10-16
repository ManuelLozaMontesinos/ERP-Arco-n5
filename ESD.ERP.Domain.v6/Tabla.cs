using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("tabla")]
	public class Tabla : BaseEntity
	{
		public override int EntityId => TablaId;
		[Key]
		[Column("tabla_id", Order = 1)]
		public int TablaId { get; set; }

		[Column("tabla")]
		public string Codigo { get; set; }

		[Column("nombre")]
		public string Nombre { get; set; }

		[Column("corre_por_emp")]
		public string CorrelativoPorEmpresa { get; set; }

		[Column("corre_por_emp_sede")]
		public string CorrelativoPorEmpresaSede { get; set; }

		[Column("correlativo")]
		public int Correlativo { get; set; }

		[Column("prefijo")]
		public string Prefijo { get; set; }

		[Column("formato_corre")]
		public string FormatoCorrelativo { get; set; }

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
