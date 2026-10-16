using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("unidad_medida")]
	public class UnidadMedida : BaseEntity
	{
		public override int EntityId => UnidadMedidaId;
		[Key]
		[Column("unidad_medida_id", Order = 1)]
		public int UnidadMedidaId { get; set; }

		[Column("empresa_id")]
		public int EmpresaId { get; set; }

		[Column("unidad_medida")]
		public string Codigo { get; set; }

		[Column("nombre")]
		public string Nombre { get; set; }

		[Column("abrev")]
		public string Abreviatura { get; set; }

		[Column("codigo_iso")]
		public string CodigoISO { get; set; }

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
