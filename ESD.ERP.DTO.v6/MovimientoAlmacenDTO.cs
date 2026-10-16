using System;
using System.Collections.Generic;
using ESD.Core;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class MovimientoAlmacenDTO : DTOBase
	{
		public override int Id => MovimientoAlmacenId;
		public int MovimientoAlmacenId { get; set; }
		public int PeriodoEmpresaSedeId { get; set; }
		public int AlmacenId { get; set; }
		public int TransaccionAlmacenId { get; set; }
		public int TipoMovimientoAlmacenId { get; set; }
		public string Codigo { get; set; }
		public int? ProveedorId { get; set; }
		public int? AlmacenDestinoId { get; set; }
		public int? MovimientoAlmacenReferencialId { get; set; }
		public DateTime Fecha { get; set; }
		public int MesContableId { get; set; }
		public int? OrdenCompraId { get; set; }
		public string NumeroOrdenCompra { get; set; }
		public string NumeroGuiaRemision { get; set; }
		public int? ComprobanteId { get; set; }
		public string DocumentoSerieNumeroReferencial
      {
         get
         {
				return string.Concat(
					TipoDocumentoComercialDTO != null ? TipoDocumentoComercialDTO.Abreviatura : string.Empty, 
					!string.IsNullOrEmpty(SerieDocumento) ? string.Concat(
						(TipoDocumentoComercialDTO != null ? SeparadorConstante.DOS_PUNTOS : string.Empty),SerieDocumento) : string.Empty,
					!string.IsNullOrEmpty(NumeroDocumento) ? string.Concat(
						(!string.IsNullOrEmpty(SerieDocumento) ? SeparadorConstante.GUION : string.Empty), NumeroDocumento) : string.Empty
				);
			}
      }
		public int? TipoDocumentoComercialId { get; set; }
		public string SerieDocumento { get; set; }
		public string NumeroDocumento { get; set; }
		public string TieneGuiaRemision { get; set; }
		public string GuiaRemisionEnModificacion { get; set; }
		public int? PersonalId { get; set; }
		public int? CentroCostoId { get; set; }
		public int? EstadoAprobacionId { get; set; }
		public int? PersonalAutorizaId { get; set; }
		public DateTime? FechaAutorizacion { get; set; }
		public int? MovimientoReclasificacionId { get; set; }
		public int OrdenReclasificacion { get; set; }
		public string AfectaSaldo { get; set; }
		public string Observacion { get; set; }
		public int EstadoMovimientoAlmacenId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public string EsEditable { get; set; }

        public PeriodoEmpresaSedeDTO PeriodoEmpresaSedeDTO { get; set; }
		public AlmacenDTO AlmacenDTO { get; set; }
		public TransaccionAlmacenDTO TransaccionAlmacenDTO { get; set; }
		public TipoMovimientoAlmacenDTO TipoMovimientoAlmacenDTO { get; set; }
		public ProveedorDTO ProveedorDTO { get; set; }
		public AlmacenDTO AlmacenDestinoDTO { get; set; }
		public MesContableDTO MesContableDTO { get; set; }
		public PersonalDTO PersonalDTO { get; set; }
		public CentroCostoDTO CentroCostoDTO { get; set; }
		public EstadoAprobacionDTO EstadoAprobacionDTO { get; set; }
		public PersonalDTO PersonalAutorizaDTO { get; set; }
		public TipoDocumentoComercialDTO TipoDocumentoComercialDTO { get; set; }
		public EstadoMovimientoAlmacenDTO EstadoMovimientoAlmacenDTO { get; set; }
		public UsuarioDTO UsuarioDTO { get; set; }
		public DateTime? FechaDesde { get; set; }
		public DateTime? FechaHasta { get; set; }
		public List<string> EstadoMovimientoAlmacenList { get; set; }
		public List<string> TransaccionAlmacenList { get; set; }
		public List<string> NotTransaccionAlmacenList { get; set; }
	}
}
