using System;
using System.Collections.Generic;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class NotaCreditoProductoDTO : DTOBase
	{
		public override int Id => NotaCreditoProductoId;
		public int NotaCreditoProductoId { get; set; }
		public int NotaCreditoId { get; set; }
		public int? ComprobanteProductoId { get; set; }
		public int? ProductoId { get; set; }
		public string Descripcion { get; set; }
		public decimal Cantidad { get; set; }
		public decimal PrecioUnitario { get; set; }
		public decimal MontoBruto { get; set; }
		public decimal MontoDescuento1 { get; set; }
		public decimal MontoDescuento2 { get; set; }
		public decimal MontoNeto { get; set; }
		public decimal MontoIgv { get; set; }
		public decimal MontoIsc { get; set; }
		public decimal MontoTotal { get; set; }
		public int TipoAfectacionIgvId { get; set; }
		public int? TipoIscId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificaId { get; set; }
		public DateTime? FechaModifica { get; set; }
		public NotaCreditoDTO NotaCreditoDTO { get; set; }
		public ComprobanteProductoDTO ComprobanteProductoDTO { get; set; }
		public ProductoDTO ProductoDTO { get; set; }
		public TipoAfectacionIgvDTO TipoAfectacionIgvDTO { get; set; }
		public TipoIscDTO TipoIscDTO { get; set; }
		public List<int> ComprobanteProductoIdList { get; set; }
	}
}
