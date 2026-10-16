using System;
using System.Collections.Generic;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class OrdenProduccionDTO : DTOBase
	{
		public override int Id => OrdenProduccionId;
		public int OrdenProduccionId { get; set; }
		public int PeriodoEmpresaSedeId { get; set; }
		public string Codigo { get; set; }
		public int? CotizacionId { get; set; }
		public int AlmacenId { get; set; }
		public DateTime Fecha { get; set; }
		public int ProveedorId { get; set; }
		public string Observacion { get; set; }
		public int RutaProduccionId { get; set; }
		public DateTime? FechaEntrega { get; set; }
		public DateTime? HoraEntrega { get; set; }
		public DateTime? DiaEntrega { get; set; }
		public int EstadoOrdenProduccionId { get; set; }
		public string IncluyeInsumo { get; set; }
		public int? EstadoDespachoId { get; set; }
		public int? PersonalId { get; set; }
		public DateTime? FechaDespacho { get; set; }
		public string ObservacionDespacho { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public int? Modificaciones {  get; set; }
		public int? EstadoPlanoMelId { get; set; }
		public int? EstadoPlanoMdfId { get; set; }
        public string Comentarios { get; set; }
		public string EsParcial { get; set; }
		public string Colores { get; set; }

        public PeriodoEmpresaSedeDTO PeriodoEmpresaSedeDTO { get; set; }
		public CotizacionDTO CotizacionDTO { get; set; }
		public AlmacenDTO AlmacenDTO { get; set; }
		public ProveedorDTO ProveedorDTO { get; set; }
		public RutaProduccionDTO RutaProduccionDTO { get; set; }
		public EstadoOrdenProduccionDTO EstadoOrdenProduccionDTO { get; set; }
		public EstadoDespachoDTO EstadoDespachoDTO { get; set; }
		public PersonalDTO PersonalDTO { get; set; }
        public EstadoOrdenProduccionDTO EstadoPlanoMelDTO { get; set; }
        public EstadoOrdenProduccionDTO EstadoPlanoMdfDTO { get; set; }	


        public DateTime? FechaDesde { get; set; }
		public DateTime? FechaHasta { get; set; }
		public List<string> EstadoOrdenProduccionList { get; set; }
        public List<OrdenProduccionSubProcesoDTO> OrdenProduccionSubProcesoDTOList { get; set; }

    }
}
