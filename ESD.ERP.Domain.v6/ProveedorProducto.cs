using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("prove_prod")]
	public class ProveedorProducto : BaseEntity
	{
		public override int EntityId => ProveedorProductoId;
		[Key]
		[Column("prove_prod_id", Order = 1)]
		public int ProveedorProductoId { get; set; }

		[Column("proveedor_id")]
		public int ProveedorId { get; set; }

		[Column("producto_id")]
		public int ProductoId { get; set; }

		[Column("cod_referencial")]
		public string CodigoReferencial { get; set; }

		[Column("peso_neto")]
		public decimal PesoNeto { get; set; }

		[Column("peso_bruto")]
		public decimal PesoBruto { get; set; }

		[Column("master_pack")]
		public int MasterPack { get; set; }

		[Column("volumen")]
		public decimal Volumen { get; set; }

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
		public Producto Producto { get; set; }
	}
}
