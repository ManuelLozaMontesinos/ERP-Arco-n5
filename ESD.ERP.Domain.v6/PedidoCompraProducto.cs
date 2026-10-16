using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("pedido_c_prod")]
	public class PedidoCompraProducto : BaseEntity
	{
		public override int EntityId => PedidoCompraProductoId;
		[Key]
		[Column("pedido_c_prod_id", Order = 1)]
		public int PedidoCompraProductoId { get; set; }

		[Column("pedido_co_id")]
		public int PedidoCompraId { get; set; }

		[Column("producto_id")]
		public int? ProductoId { get; set; }

		[Column("descripcion")]
		public string Descripcion { get; set; }

		[Column("cantidad")]
		public decimal Cantidad { get; set; }

		[Column("peso_neto")]
		public decimal PesoNeto { get; set; }

		[Column("peso_bruto")]
		public decimal PesoBruto { get; set; }

		[Column("stock_prove")]
		public string StockEnProveedor { get; set; }

		[Column("es_adicional")]
		public string EsAdicional { get; set; }

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

		public PedidoCompra PedidoCompra { get; set; }
		public Producto Producto { get; set; }
	}
}
