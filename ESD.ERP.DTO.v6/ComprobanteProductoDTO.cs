using System;
using System.Collections.Generic;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class ComprobanteProductoDTO : DTOBase
	{
		public override int Id => ComprobanteProductoId;
		public int ComprobanteProductoId { get; set; }
		public int ComprobanteId { get; set; }
		public int ProductoId { get; set; }
		public string Descripcion { get; set; }
		public decimal Cantidad { get; set; }
		public int? UnidadMedidaId { get; set; }
		public decimal CantidadPresentacion { get; set; }
		public decimal CantidadEquivalente { get; set; }
		public decimal ValorUnitarioLista { get; set; }
		public decimal PrecioUnitarioLista { get; set; }
		public decimal ValorUnitario { get; set; }
		public decimal PrecioUnitario { get; set; }
		public string CantidadBloqueada { get; set; }
		public string IncluyeDescuentoListaPrecio { get; set; }
		public string PermiteDescuento { get; set; }
		public string SolicitaAutorizacionDescuento { get; set; }
		public decimal PrecioAutorizado { get; set; }
		public int? RegistroReferencialId { get; set; }
		public decimal MontoBruto { get; set; }
		public int TipoDescuentoId { get; set; }
		public decimal DescuentoValor { get; set; }
		public decimal PorcentajeDescuento { get; set; }
		public decimal MontoDescuento1 { get; set; }
		public decimal MontoDescuento2 { get; set; }
		public decimal MontoNeto { get; set; }
		public decimal MontoIgv { get; set; }
		public decimal MontoIsc { get; set; }
		public decimal MontoTotal { get; set; }
		public int TipoDetraccionId { get; set; }
		public decimal PorcentajeDetraccion { get; set; }
		public decimal MontoDetraccion { get; set; }
		public int TipoAfectacionIgvId { get; set; }
		public int? TipoIscId { get; set; }
		public decimal CantidadNotaCredito { get; set; }
		public int? RegistroCanjeId { get; set; }
		public int? RegistroComboId { get; set; }
		public decimal CantidadEntregada { get; set; }
		public DateTime? FechaDespacho { get; set; }
		public string ObservacionDespacho { get; set; }
		public string EntregaDespacho { get; set; }
		public int? PersonalId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public ComprobanteDTO ComprobanteDTO { get; set; }
		public ProductoDTO ProductoDTO { get; set; }
		public UnidadMedidaDTO UnidadMedidaDTO { get; set; }
		public TipoDetraccionDTO TipoDetraccionDTO { get; set; }
		public TipoAfectacionIgvDTO TipoAfectacionIgvDTO { get; set; }
		public TipoIscDTO TipoIscDTO { get; set; }
		public PersonalDTO PersonalDTO { get; set; }
		public TipoDescuentoDTO TipoDescuentoDTO { get; set; }
		public List<int> CotizacionProductoIdList { get; set; }
		public List<int> ComprobanteProductoIdList { get; set; }
		public string ConSaldoNotaCredito { get; set; }
		public string NullRegistroCanjeId { get; set; }
		public string NullRegistroComboId { get; set; }
	}
}
