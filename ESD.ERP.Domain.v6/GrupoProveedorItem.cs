using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("grupo_p_item")]
	public class GrupoProveedorItem : BaseEntity
	{
		public override int EntityId => GrupoProveedorItemId;
		[Key]
		[Column("grupo_p_item_id", Order = 1)]
		public int GrupoProveedorItemId { get; set; }

		[Column("grupo_prove_id")]
		public int GrupoProveedorId { get; set; }

		[Column("proveedor_id")]
		public int ProveedorId { get; set; }

		[Column("estado")]
		public string Estado { get; set; }

		[Column("usuario_creacion_id")]
		public int UsuarioCreacionId { get; set; }

		[Column("fecha_creacion")]
		public DateTime FechaCreacion { get; set; }

		[Column("usuario_modifica_id")]
		public int? UsuarioModificaId { get; set; }

		[Column("fecha_modifica")]
		public DateTime? FechaModifica { get; set; }

		public GrupoProveedor GrupoProveedor { get; set; }
		public Proveedor Proveedor { get; set; }
	}
}
