using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("mov_almacen")]
	public class MovimientoAlmacen : BaseEntity
	{
		public override int EntityId => MovimientoAlmacenId;
		[Key]
		[Column("mov_almacen_id", Order = 1)]
		public int MovimientoAlmacenId { get; set; }

		[Column("periodo_emp_sede_id")]
		public int PeriodoEmpresaSedeId { get; set; }

		[Column("almacen_id")]
		public int AlmacenId { get; set; }

		[Column("trans_alma_id")]
		public int TransaccionAlmacenId { get; set; }

		[Column("tipo_mov_alma_id")]
		public int TipoMovimientoAlmacenId { get; set; }

		[Column("mov_almacen")]
		public string Codigo { get; set; }

		[Column("proveedor_id")]
		public int? ProveedorId { get; set; }

		[Column("almacen_destino_id")]
		public int? AlmacenDestinoId { get; set; }
		[Column("mov_alma_ref_id")]
		public int? MovimientoAlmacenReferencialId { get; set; }
		[Column("fecha")]
		public DateTime Fecha { get; set; }

		[Column("mes_contable_id")]
		public int MesContableId { get; set; }

		[Column("orden_cs_id")]
		public int? OrdenCompraId { get; set; }

		[Column("orden_cs")]
		public string NumeroOrdenCompra { get; set; }

		[Column("nro_guia_rem")]
		public string NumeroGuiaRemision { get; set; }

		[Column("comprobante_id")]
		public int? ComprobanteId { get; set; }

		[Column("tipo_doc_com_id")]
		public int? TipoDocumentoComercialId { get; set; }

		[Column("serie_doc")]
		public string SerieDocumento { get; set; }

		[Column("numero_doc")]
		public string NumeroDocumento { get; set; }

		[Column("tiene_guia_rem")]
		public string TieneGuiaRemision { get; set; }
		[Column("g_r_en_modi")]
		public string GuiaRemisionEnModificacion { get; set; }

		[Column("personal_id")]
		public int? PersonalId { get; set; }

		[Column("centro_costo_id")]
		public int? CentroCostoId { get; set; }

		[Column("estado_aproba_id")]
		public int? EstadoAprobacionId { get; set; }

		[Column("perso_auto_id")]
		public int? PersonalAutorizaId { get; set; }

		[Column("fecha_auto")]
		public DateTime? FechaAutorizacion { get; set; }

		[Column("mov_reclasi_id")]
		public int? MovimientoReclasificacionId { get; set; }

		[Column("orden_reclasi")]
		public int OrdenReclasificacion { get; set; }

		[Column("afecta_saldo")]
		public string AfectaSaldo { get; set; }

		[Column("observacion")]
		public string Observacion { get; set; }

		[Column("estado_mov_alm_id")]
		public int EstadoMovimientoAlmacenId { get; set; }

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

		[Column("es_editable")]
		public string EsEditable { get; set;}

		public PeriodoEmpresaSede PeriodoEmpresaSede { get; set; }
		public Almacen Almacen { get; set; }
		public TransaccionAlmacen TransaccionAlmacen { get; set; }
		public TipoMovimientoAlmacen TipoMovimientoAlmacen { get; set; }
		public Proveedor Proveedor { get; set; }
		public Almacen AlmacenDestino { get; set; }
		public MesContable MesContable { get; set; }
		public Personal Personal { get; set; }
		public CentroCosto CentroCosto { get; set; }
		public EstadoAprobacion EstadoAprobacion { get; set; }
		public Personal PersonalAutoriza{ get; set; }
		public TipoDocumentoComercial TipoDocumentoComercial { get; set; }
		public EstadoMovimientoAlmacen EstadoMovimientoAlmacen { get; set; }
		public Usuario Usuario { get; set; }
		public DateTime? FechaDesde { get; set; }
		public DateTime? FechaHasta { get; set; }
		public List<string> EstadoMovimientoAlmacenList { get; set; }
		public List<string> TransaccionAlmacenList { get; set; }
		public List<string> NotTransaccionAlmacenList { get; set; }
	}
}
