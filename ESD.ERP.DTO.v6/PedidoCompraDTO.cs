using System;
using System.Collections.Generic;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class PedidoCompraDTO : DTOBase
	{
		public override int Id => PedidoCompraId;
		public int PedidoCompraId { get; set; }
		public int PeriodoEmpresaSedeId { get; set; }
		public string Codigo { get; set; }
		public DateTime Fecha { get; set; }
		public int TipoProductoId { get; set; }
		public int ProveedorId { get; set; }
		public string EsModificacion { get; set; }
		public int? EstadoAprobacionId { get; set; }
		public DateTime? FechaAprobacion { get; set; }
		public string Observacion { get; set; }
		public int EstadoPedidoCompraId { get; set; }
		public int? EstadoOrdenCompraAnteriorId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public PeriodoEmpresaSedeDTO PeriodoEmpresaSedeDTO { get; set; }
		public TipoProductoDTO TipoProductoDTO { get; set; }
		public ProveedorDTO ProveedorDTO { get; set; }
		public EstadoAprobacionDTO EstadoAprobacionDTO { get; set; }
		public EstadoPedidoCompraDTO EstadoPedidoCompraDTO { get; set; }
		public DateTime? FechaDesde { get; set; }
		public DateTime? FechaHasta { get; set; }
		public List<PedidoCompraProductoDTO> PedidoCompraProductoDTOList{ get; set; }
	}
}
