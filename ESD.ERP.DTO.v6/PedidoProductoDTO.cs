using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class PedidoProductoDTO : DTOBase
	{
		public override int Id => PedidoProdId;
		public int PedidoProdId { get; set; }
		public int PedidoId { get; set; }
		public int ProductoId { get; set; }
		public string Descripcion { get; set; }
		public decimal Cantidad { get; set; }
		public int? UnidadMedidaId { get; set; }
		public decimal CantidadPresentacion { get; set; }
		public decimal CantidadEquivalente { get; set; }
		public decimal CantidadComprobante { get; set; }
		public decimal ValorUnitarioLista { get; set; }
		public decimal PrecioUnitarioLista { get; set; }
		public decimal ValorUnitario { get; set; }
		public decimal PrecioUnitario { get; set; }
		public string IncDctoLp { get; set; }
		public string SoliAutoDcto { get; set; }
		public string TieneSaldo { get; set; }
		public decimal MontoBruto { get; set; }
		public decimal MontoDescuento1 { get; set; }
		public decimal MontoDescuento2 { get; set; }
		public decimal MontoNeto { get; set; }
		public decimal MontoIgv { get; set; }
		public decimal MontoIsc { get; set; }
		public decimal MontoTotal { get; set; }
		public int TipoAfectacionIgvId { get; set; }
		public int? TipoIscId { get; set; }
		public int? AlmacenId { get; set; }
		public string Reclasifica { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public PedidoDTO PedidoDTO { get; set; }
		public ProductoDTO ProductoDTO { get; set; }
		public UnidadMedidaDTO UnidadMedidaDTO { get; set; }
		public TipoAfectacionIgvDTO TipoAfectacionIgvDTO { get; set; }
		public TipoIscDTO TipoIscDTO { get; set; }
	}
}
