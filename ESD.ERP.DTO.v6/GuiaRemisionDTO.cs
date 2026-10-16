using System;
using ESD.Core;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class GuiaRemisionDTO : DTOBase
	{
		public override int Id => GuiaRemisionId;
		public int GuiaRemisionId { get; set; }
		public int PeriodoEmpresaSedeId { get; set; }
		public int TipoDocumentoComercialId { get; set; }
		public int? MovimientoAlmacenId { get; set; }
		public string Serie { get; set; }
		public string Numero { get; set; }
		public string SerieNumero
		{
			get
			{
				return string.Concat(Serie, SeparadorConstante.GUION, Numero);
			}
		}
		public DateTime Fecha { get; set; }
		public DateTime FechaInicioTraslado { get; set; }
		public decimal CostoFlete { get; set; }
		public string EsInterno { get; set; }
		public int ProveedorId { get; set; }
		public int? DistritoId { get; set; }
		public string PuntoPartida { get; set; }
		public string CodigoSunatPartida { get; set; }
		public int? TipoDocumentoComercialReferenciaId { get; set; }
		public string SerieDocumentoReferencia { get; set; }
		public string NumeroDocumentoReferencia { get; set; }
		public int ProveedorDestinoId { get; set; }
		public int? DistritoLlegadaId { get; set; }
		public string PuntoLlegada { get; set; }
		public string CodigoSunatLlegada { get; set; }
		public int? TransportistaUnidadId { get; set; }
		public int? TransportistaConductorId { get; set; }
		public int? MotivoTrasladoId { get; set; }
		public string EsVenta { get; set; }
		public string Impreso { get; set; }
		public decimal PesoBrutoTotal { get; set; }
		public int? ComprobanteElectronicoId { get; set; }
		public string EsModificacion { get; set; }
		public int? EstadoAprobacionId { get; set; }
		public int? EstadoGuiaRemisionAnteriorId { get; set; }
		public int EstadoGuiaRemisionId { get; set; }
		public string Observacion { get; set; }
		public string ObservacionAnulacion { get; set; }
		public int? EstadoSunatId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public PeriodoEmpresaSedeDTO PeriodoEmpresaSedeDTO { get; set; }
		public TipoDocumentoComercialDTO TipoDocumentoComercialDTO { get; set; }
		public MovimientoAlmacenDTO MovimientoAlmacenDTO { get; set; }
		public ProveedorDTO ProveedorDTO { get; set; }
		public ProveedorDTO ProveedorDestinoDTO { get; set; }
		public TransportistaUnidadDTO TransportistaUnidadDTO { get; set; }
		public TransportistaConductorDTO TransportistaConductorDTO { get; set; }
		public MotivoTrasladoDTO MotivoTrasladoDTO { get; set; }
		public UsuarioDTO UsuarioDTO { get; set; }
		public EstadoGuiaRemisionDTO EstadoGuiaRemisionDTO { get; set; }
		public EstadoAprobacionDTO EstadoAprobacionDTO { get; set; }
		public DistritoDTO DistritoDTO { get; set; }
		public DistritoDTO DistritoLlegadaDTO { get; set; }
		public EstadoSunatDTO EstadoSunatDTO { get; set; }
		public TipoDocumentoComercialDTO TipoDocumentoComercialReferenciaDTO { get; set; }
		public DateTime? FechaDesde { get; set; }
		public DateTime? FechaHasta { get; set; }
	}
}
