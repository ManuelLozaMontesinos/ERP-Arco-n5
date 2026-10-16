using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class PedidoCompraProductoDTO : DTOBase
	{
		public override int Id => PedidoCompraProductoId;
		public int PedidoCompraProductoId { get; set; }
		public int PedidoCompraId { get; set; }
		public int? ProductoId { get; set; }
		public string Descripcion { get; set; }
		public decimal Cantidad { get; set; }
		public decimal PesoNeto { get; set; }
		public decimal PesoBruto { get; set; }
		public string StockEnProveedor { get; set; }
		public string EsAdicional { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificaId { get; set; }
		public DateTime? FechaModifica { get; set; }
		public PedidoCompraDTO PedidoCompraDTO { get; set; }
		public ProductoDTO ProductoDTO { get; set; }
	}
}
