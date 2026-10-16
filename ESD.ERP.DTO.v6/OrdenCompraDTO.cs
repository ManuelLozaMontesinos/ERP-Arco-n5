using System;
using System.Collections.Generic;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class OrdenCompraDTO : DTOBase
	{
		public override int Id => OrdenCompraId;
		public int OrdenCompraId { get; set; }
		public int PeriodoEmpresaSedeId { get; set; }
		public string Codigo { get; set; }
		public DateTime Fecha { get; set; }
		public int TipoProductoId { get; set; }
		public int? PedidoCompraId { get; set; }
		public int ProveedorId { get; set; }
		public DateTime? FechaEntrega { get; set; }
		public int MonedaId { get; set; }
		public decimal TipoCambio { get; set; }
		public int ImpuestoTasaId { get; set; }
		public decimal MontoBruto { get; set; }
		public decimal MontoImpuesto { get; set; }
		public decimal MontoTotal { get; set; }
		public string PlazoEntrega { get; set; }
		public string LugarEntrega { get; set; }
		public int CondicionPagoId { get; set; }
		public string Observacion { get; set; }
		public string PermitePagoParcial { get; set; }
		public string EsModificacion { get; set; }
		public int? EstadoAprobacionId { get; set; }
		public DateTime? FechaAprobacion { get; set; }
		public int? EstadoOrdenCompraAnteriorId { get; set; }
		public int EstadoOrdenCompraId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public PeriodoEmpresaSedeDTO PeriodoEmpresaSedeDTO { get; set; }
		public TipoProductoDTO TipoProductoDTO { get; set; }
		public ProveedorDTO ProveedorDTO { get; set; }
		public MonedaDTO MonedaDTO { get; set; }
		public ImpuestoTasaDTO ImpuestoTasaDTO { get; set; }
		public CondicionPagoDTO CondicionPagoDTO { get; set; }
		public EstadoOrdenCompraDTO EstadoOrdenCompraDTO { get; set; }
		public PedidoCompraDTO PedidoCompraDTO { get; set; }
		public EstadoAprobacionDTO EstadoAprobacionDTO { get; set; }

		public DateTime? FechaDesde { get; set; }
		public DateTime? FechaHasta { get; set; }
		public List<string> EstadoOrdenCompraList { get; set; }
	}
}
