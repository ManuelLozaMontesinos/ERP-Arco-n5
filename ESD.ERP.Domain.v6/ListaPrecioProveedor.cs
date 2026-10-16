using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("lista_p_prove")]
	public class ListaPrecioProveedor : BaseEntity
	{
		public override int EntityId => ListaPrecioProveedorId;
		[Key]
		[Column("lista_p_prove_id", Order = 1)]
		public int ListaPrecioProveedorId { get; set; }

		[Column("lista_precio_id")]
		public int ListaPrecioId { get; set; }

		[Column("grupo_prove_id")]
		public int GrupoProveedorId { get; set; }

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

		public ListaPrecio ListaPrecio { get; set; }
		public GrupoProveedor GrupoProveedor { get; set; }
		public int ProveedorId { get; set; }
	}
}
