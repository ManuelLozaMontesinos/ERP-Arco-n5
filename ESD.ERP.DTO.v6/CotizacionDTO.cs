using System;
using System.Collections.Generic;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class CotizacionDTO : DTOBase
	{
		public override int Id => CotizacionId;
		public int CotizacionId { get; set; }
		public int PeriodoEmpresaSedeId { get; set; }
		public string Codigo { get; set; }
		public DateTime Fecha { get; set; }
		public int ProveedorId { get; set; }
		public int VendedorId { get; set; }
		public int MonedaId { get; set; }
		public decimal MontoTipoCambio { get; set; }
		public int ListaPrecioId { get; set; }
		public int TipoCondicionPagoId { get; set; }
		public string Observacion { get; set; }
		public int ImpuestoTasaId { get; set; }
		public decimal MontoBruto { get; set; }
		public decimal PorcentajeDescuento { get; set; }
		public decimal MontoDescuento { get; set; }
		public decimal MontoDescuentoItem { get; set; }
		public decimal MontoNeto { get; set; }
		public decimal MontoIgv { get; set; }
		public decimal MontoIsc { get; set; }
		public decimal MontoAnticipo { get; set; }
		public decimal MontoTotal { get; set; }
		public decimal MontoSaldo { 
			get { return MontoTotal - MontoAnticipo; }
			}
		public int? AlmacenId { get; set; }
		public int? MesContableId { get; set; }
		public string EsPedido { get; set; }
		public string EnviadoProduccion { get; set; }
		public int EstadoCotizacionId { get; set; }
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
		public TipoCondicionPagoDTO TipoCondicionPagoDTO { get; set; }
		public ImpuestoTasaDTO ImpuestoTasaDTO { get; set; }
		public EstadoCotizacionDTO EstadoCotizacionDTO { get; set; }
		public AlmacenDTO AlmacenDTO { get; set; }
		public DateTime? FechaDesde { get; set; }
		public DateTime? FechaHasta { get; set; }
		public int TipoDocumentoComercialId { get; set; }
		public string SerieComprobante { get; set; }
		public string SerieEsContingencia { get; set; }
		public int? AlmacenComprobanteId { get; set; }
		public DateTime FechaComprobante { get; set; }
		public int ProveedorComprobanteId { get; set; }
		public List<string> EstadoCotizacionList { get; set; }
		public decimal CortesGratis { get; set; }

		public List<CotizacionProductoDTO> CotizacionProductoDTOList { get; set; }
	}
}