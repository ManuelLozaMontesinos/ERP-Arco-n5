using System;
using System.Collections.Generic;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class CotizacionProductoDTO : DTOBase
	{
		public override int Id => CotizacionProductoId;
		public int CotizacionProductoId { get; set; }
		public int CotizacionId { get; set; }
		public int ProductoId { get; set; }
		public string Descripcion { get; set; }
		public decimal Cantidad { get; set; }
		public int? UnidadMedidaId { get; set; }
		public decimal CantidadPresentacion { get; set; }
		public decimal CantidadEquivalente { get; set; }
		public decimal CantidadComprobante { get; set; }
		public decimal CantidadPendiente {
         get { return Cantidad - CantidadComprobante; } 
		}
		public decimal ValorUnitarioLista { get; set; }
		public decimal PrecioUnitarioLista { get; set; }
		public decimal ValorUnitario { get; set; }
		public decimal PrecioUnitario { get; set; }
		public string CantidadBloqueada { get; set; }
		public string IncluyeDescuentoListaPrecio { get; set; }
		public string PermiteDescuento { get; set; }
		public decimal PrecioAutorizado { get; set; }
		public string SinSaldo { get; set; }
		public string SolicitaAutorizacionDescuento { get; set; }
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
		public decimal PesoNeto { get; set; }
		public int TipoAfectacionIgvId { get; set; }
		public int? TipoIscId { get; set; }
		public int? RegistroComboId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public decimal CantidadParcial { get; set; }

        public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public CotizacionDTO CotizacionDTO { get; set; }
		public ProductoDTO ProductoDTO { get; set; }
        public UnidadMedidaDTO UnidadMedidaDTO { get; set; }
		public TipoAfectacionIgvDTO TipoAfectacionIgvDTO { get; set; }
		public TipoIscDTO TipoIscDTO { get; set; }
		public decimal CortesGratis {  get; set; }
        public TipoDescuentoDTO TipoDescuentoDTO { get; set; }
		public CortesGratisDTO CortesGratisDTO { get; set; }
		public string ConCantidadPendiente { get; set; }
		public string NullRegistroComboId { get; set; }

    }
}