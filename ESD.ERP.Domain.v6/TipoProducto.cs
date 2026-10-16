using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("tipo_producto")]
	public class TipoProducto : BaseEntity
	{
		public override int EntityId => TipoProductoId;
		[Key]
		[Column("tipo_producto_id", Order = 1)]
		public int TipoProductoId { get; set; }

		[Column("empresa_id")]
		public int EmpresaId { get; set; }

		[Column("tipo_producto")]
		public string Codigo { get; set; }

		[Column("nombre")]
		public string Nombre { get; set; }

		[Column("correla")]
		public short Correlativo { get; set; }

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
