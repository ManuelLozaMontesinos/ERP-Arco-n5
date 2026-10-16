using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("grupo_producto")]
	public class GrupoProducto : BaseEntity
	{
		public override int EntityId => GrupoProductoId;
		[Key]
		[Column("grupo_producto_id", Order = 1)]
		public int GrupoProductoId { get; set; }

		[Column("tipo_producto_id")]
		public int TipoProductoId { get; set; }

		[Column("grupo_producto")]
		public string Codigo { get; set; }

		[Column("nombre")]
		public string Nombre { get; set; }

		[Column("correla")]
		public short Correlativo { get; set; }

		[Column("usar_inc_fracc")]
		public string UsarIncrementoFraccion { get; set; }

		[Column("es_combo")]
		public string EsCombo { get; set; }
		[Column("es_insumo")]
		public string EsInsumo { get; set; }
		[Column("estado")]
		public string Estado { get; set; }

		[Column("fecha_alta")]
		public DateTime FechaAlta { get; set; }

		[Column("fecha_baja")]
		public DateTime? FechaBaja { get; set; }

		[Column("usuario_creacion_id")]
		public int UsuarioCreacionId { get; set; }

		[Column("fecha_creacion")]
		public DateTime FechaCreacion { get; set; }

		[Column("usuario_modifica_id")]
		public int? UsuarioModificacionId { get; set; }

		[Column("fecha_modifica")]
		public DateTime? FechaModificacion { get; set; }

		public TipoProducto TipoProducto { get; set; }
		public int NotGrupoProductoId { get; set; }
		public int Periodo { get; set; }
	}
}
