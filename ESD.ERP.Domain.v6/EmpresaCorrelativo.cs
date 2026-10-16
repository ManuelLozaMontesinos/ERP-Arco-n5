using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("empresa_corre")]
	public class EmpresaCorrelativo : BaseEntity
	{
		public override int EntityId => EmpresaCorrelativoId;
		[Key]
		[Column("empresa_corre_id", Order = 1)]
		public int EmpresaCorrelativoId { get; set; }

		[Column("empresa_id")]
		public int EmpresaId { get; set; }

		[Column("tabla_id")]
		public int TablaId { get; set; }

		[Column("correlativo")]
		public int Correlativo { get; set; }

		[Column("usuario_creacion_id")]
		public int UsuarioCreacionId { get; set; }

		[Column("fecha_creacion")]
		public DateTime FechaCreacion { get; set; }

		[Column("usuario_modifica_id")]
		public int? UsuarioModificacionId { get; set; }

		[Column("fecha_modifica")]
		public DateTime? FechaModificacion { get; set; }

		public Empresa Empresa { get; set; }
		public Tabla Tabla { get; set; }
	}
}
