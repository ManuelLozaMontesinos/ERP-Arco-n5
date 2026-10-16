using System;
using System.Collections.Generic;
using ESD.Core;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class NotaCreditoDTO : DTOBase
	{
		public override int Id => NotaCreditoId;
		public int NotaCreditoId { get; set; }
		public int PeriodoEmpresaSedeId { get; set; }
		public string NumeroOperacion { get; set; }
		public string SerieNumero
		{
			get
			{
				if (string.IsNullOrEmpty(Numero))
					return NumeroOperacion;

				if (string.IsNullOrEmpty(Numero.Trim()))
					return NumeroOperacion;

				return string.Concat(Serie, SeparadorConstante.GUION, Numero);
			}
		}
		public string Serie { get; set; }
		public string Numero { get; set; }
		public DateTime Fecha { get; set; }
		public int TipoNotaCreditoId { get; set; }
		public int ComprobanteId { get; set; }
		public int VendedorId { get; set; }
		public string Observacion { get; set; }
		public int ImpuestoTasaId { get; set; }
		public decimal MontoBruto { get; set; }
		public decimal MontoNeto { get; set; }
		public decimal MontoIgv { get; set; }
		public decimal MontoIsc { get; set; }
		public decimal MontoTotal { get; set; }
		public string EsContingencia { get; set; }
		public string Emitido { get; set; }
		public int? ComprobanteElectronicoId { get; set; }
		public string CodigoHash { get; set; }
		public int EstadoComprobanteId { get; set; }
		public int? EstadoSunatId { get; set; }
		public int? CajaId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public PeriodoEmpresaSedeDTO PeriodoEmpresaSedeDTO { get; set; }
		public TipoNotaCreditoDTO TipoNotaCreditoDTO { get; set; }
		public ComprobanteDTO ComprobanteDTO { get; set; }
		public VendedorDTO VendedorDTO { get; set; }
		public ImpuestoTasaDTO ImpuestoTasaDTO { get; set; }
		public EstadoComprobanteDTO EstadoComprobanteDTO { get; set; }
		public EstadoSunatDTO EstadoSunatDTO { get; set; }
		public CajaDTO CajaDTO { get; set; }

		public DateTime? FechaDesde { get; set; }
		public DateTime? FechaHasta { get; set; }
		public List<int> ComprobanteElectronicoIdList { get; set; }
		public List<string> TipoNotaCreditoList { get; set; }
	}
}
