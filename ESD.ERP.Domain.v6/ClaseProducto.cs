using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("clase_producto")]
	public class ClaseProducto : BaseEntity
	{
		public override int EntityId => ClaseProductoId;
		[Key]
		[Column("clase_producto_id", Order = 1)]
		public int ClaseProductoId { get; set; }

		[Column("grupo_producto_id")]
		public int GrupoProductoId { get; set; }

		[Column("clase_producto")]
		public string Codigo { get; set; }

		[Column("nombre")]
		public string Nombre { get; set; }

		[Column("correla")]
		public short Correlativo { get; set; }

		[Column("tipo_detrac_id")]
      public int TipoDetraccionId { get; set; }

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

		public GrupoProducto GrupoProducto { get; set; }
		public TipoDetraccion TipoDetraccion { get; set; }
		public int NotClaseProductoId { get; set; }
		public int Periodo { get; set; }
	}
}
