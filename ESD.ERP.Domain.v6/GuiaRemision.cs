using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("guia_remision")]
	public class GuiaRemision : BaseEntity
	{
		public override int EntityId => GuiaRemisionId;
		[Key]
		[Column("guia_remision_id", Order = 1)]
		public int GuiaRemisionId { get; set; }

		[Column("periodo_emp_sede_id")]
		public int PeriodoEmpresaSedeId { get; set; }

		[Column("tipo_doc_com_id")]
		public int TipoDocumentoComercialId { get; set; }

		[Column("mov_almacen_id")]
		public int? MovimientoAlmacenId { get; set; }

		[Column("serie")]
		public string Serie { get; set; }

		[Column("numero")]
		public string Numero { get; set; }

		[Column("fecha")]
		public DateTime Fecha { get; set; }

		[Column("fec_ini_tra")]
		public DateTime FechaInicioTraslado { get; set; }

		[Column("costo_flete")]
		public decimal CostoFlete { get; set; }

		[Column("es_interno")]
		public string EsInterno { get; set; }

		[Column("proveedor_id")]
		public int ProveedorId { get; set; }

		[Column("distrito_id")]
		public int? DistritoId { get; set; }

		[Column("punto_partida")]
		public string PuntoPartida { get; set; }

		[Column("cod_sunat_par")]
		public string CodigoSunatPartida { get; set; }

		[Column("tipo_d_com_ref_id")]
		public int? TipoDocumentoComercialReferenciaId { get; set; }

		[Column("serie_d_ref")]
		public string SerieDocumentoReferencia { get; set; }

		[Column("numero_d_ref")]
		public string NumeroDocumentoReferencia { get; set; }

		[Column("proveedor2_id")]
		public int ProveedorDestinoId { get; set; }

		[Column("distrito_llegada_id")]
		public int? DistritoLlegadaId { get; set; }

		[Column("punto_llegada")]
		public string PuntoLlegada { get; set; }

		[Column("cod_sunat_lle")]
		public string CodigoSunatLlegada { get; set; }

		[Column("transp_unid_id")]
		public int? TransportistaUnidadId { get; set; }

		[Column("transp_condu_id")]
		public int? TransportistaConductorId { get; set; }

		[Column("motivo_trasla_id")]
		public int? MotivoTrasladoId { get; set; }

		[Column("es_venta")]
		public string EsVenta { get; set; }

		[Column("impreso")]
		public string Impreso { get; set; }

		[Column("peso_b_total")]
		public decimal PesoBrutoTotal { get; set; }

		[Column("compro_elec_id")]
		public int? ComprobanteElectronicoId { get; set; }

		[Column("es_modifica")]
		public string EsModificacion { get; set; }

		[Column("estado_aproba_id")]
		public int? EstadoAprobacionId { get; set; }

		[Column("estado_g_r_ant_id")]
		public int? EstadoGuiaRemisionAnteriorId { get; set; }

		[Column("estado_guia_r_id")]
		public int EstadoGuiaRemisionId { get; set; }

		[Column("observa")]
		public string Observacion { get; set; }

		[Column("obs_anula")]
		public string ObservacionAnulacion { get; set; }

		[Column("estado_sunat_id")]
		public int? EstadoSunatId { get; set; }

		[Column("estado")]
		public string Estado { get; set; }

		[Column("usuario_creacion_id")]
		public int UsuarioCreacionId { get; set; }

		[Column("fecha_creacion")]
		public DateTime FechaCreacion { get; set; }

		[Column("usuario_modifica_id")]
		public int? UsuarioModificacionId { get; set; }

		[Column("fecha_modifica")]
		public DateTime? FechaModificacion { get; set; }

		public PeriodoEmpresaSede PeriodoEmpresaSede { get; set; }
		public TipoDocumentoComercial TipoDocumentoComercial { get; set; }
		public MovimientoAlmacen MovimientoAlmacen { get; set; }
		public Proveedor Proveedor { get; set; }
		public Proveedor ProveedorDestino { get; set; }
		public TransportistaUnidad TransportistaUnidad { get; set; }
		public TransportistaConductor TransportistaConductor { get; set; }
		public MotivoTraslado MotivoTraslado { get; set; }
		public EstadoGuiaRemision EstadoGuiaRemision { get; set; }
		public Usuario Usuario { get; set; }
		public EstadoAprobacion EstadoAprobacion { get; set; }
		public Distrito Distrito { get; set; }
		public Distrito DistritoLlegada { get; set; }
		public EstadoSunat EstadoSunat { get; set; }
		public TipoDocumentoComercial TipoDocumentoComercialReferencia { get; set; }
		public DateTime? FechaDesde { get; set; }
		public DateTime? FechaHasta { get; set; }
	}
}
