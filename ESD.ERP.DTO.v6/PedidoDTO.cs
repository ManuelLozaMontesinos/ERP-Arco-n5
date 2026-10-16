using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class PedidoDTO : DTOBase
	{
		public override int Id => PedidoId;
		public int PedidoId { get; set; }
		public int PeriodoEmpresaSedeId { get; set; }
		public string Codigo { get; set; }
		public DateTime Fecha { get; set; }
		public int ProveedorId { get; set; }
		public int VendedorId { get; set; }
		public int MonedaId { get; set; }
		public decimal MontoTipoCambio { get; set; }
		public int ListaPrecioId { get; set; }
		public int AlmacenId { get; set; }
		public string Observacion { get; set; }
		public int ImpuestoTasaId { get; set; }
		public decimal MontoBruto { get; set; }
		public decimal MontoNeto { get; set; }
		public decimal MontoIgv { get; set; }
		public decimal MontoIsc { get; set; }
		public decimal MontoTotal { get; set; }
		public decimal MontoAnticipo { get; set; }
		public int CantAprobReq { get; set; }
		public string SoliAutoDcto { get; set; }
		public int? UsuarioId { get; set; }
		public DateTime? FechaAutoDcto { get; set; }
		public string ExoAutoAde { get; set; }
		public string SoliAutoAde { get; set; }
		public int? UsuAutoAdeId { get; set; }
		public DateTime? FechaAutoAde { get; set; }
		public string EsPedido { get; set; }
		public string TieneComproAnti { get; set; }
		public int EstadoPedidoId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public PeriodoEmpresaSedeDTO PeriodoEmpresaSedeDTO { get; set; }
		public ProveedorDTO ProveedorDTO { get; set; }
		public VendedorDTO VendedorDTO { get; set; }
		public MonedaDTO MonedaDTO { get; set; }
		public ListaPrecioDTO ListaPrecioDTO { get; set; }
		public ImpuestoTasaDTO ImpuestoTasaDTO { get; set; }
		public UsuarioDTO UsuarioDTO { get; set; }
		public UsuarioDTO UsuarioAutorizaDTO { get; set; }
		public EstadoPedidoDTO EstadoPedidoDTO { get; set; }
	}
}
