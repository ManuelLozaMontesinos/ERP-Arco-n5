using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("vendedor")]
	public class Vendedor : BaseEntity
	{
		public override int EntityId => VendedorId;
		[Key]
		[Column("vendedor_id", Order = 1)]
		public int VendedorId { get; set; }

		[Column("vendedor")]
		public string Codigo { get; set; }

		[Column("proveedor_id")]
		public int ProveedorId { get; set; }

		[Column("abrev")]
		public string Abreviatura { get; set; }

		[Column("servidor_imp")]
		public string ServidorImpresion { get; set; }

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

		public Proveedor Proveedor { get; set; }
	}
}
