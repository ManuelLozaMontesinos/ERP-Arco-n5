using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("orden_pro")]
	public class OrdenProduccion : BaseEntity
	{
		public override int EntityId => OrdenProduccionId;
		[Key]
		[Column("orden_pro_id", Order = 1)]
		public int OrdenProduccionId { get; set; }

		[Column("periodo_emp_sede_id")]
		public int PeriodoEmpresaSedeId { get; set; }

		[Column("orden_pro")]
		public string Codigo { get; set; }

		[Column("cotiza_id")]
		public int? CotizacionId { get; set; }

		[Column("almacen_id")]
		public int AlmacenId { get; set; }

		[Column("fecha")]
		public DateTime Fecha { get; set; }

		[Column("proveedor_id")]
		public int ProveedorId { get; set; }

		[Column("observa")]
		public string Observacion { get; set; }

		[Column("ruta_prod_id")]
		public int RutaProduccionId { get; set; }

		[Column("fecha_ent")]
		public DateTime? FechaEntrega { get; set; }

		[Column("estado_orden_p_id")]
		public int EstadoOrdenProduccionId { get; set; }

		[Column("incluye_insu")]
		public string IncluyeInsumo { get; set; }
		[Column("estado_despacho_id")]
		public int? EstadoDespachoId { get; set; }

		[Column("personal_id")]
		public int? PersonalId { get; set; }
		[Column("fecha_desp")]
		public DateTime? FechaDespacho { get; set; }
		[Column("observa_desp")]
		public string ObservacionDespacho { get; set; }
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

		[Column("modificaciones")]
		public int? Modificaciones { get; set; }

		[Column("estado_plano_mel_id")]
		public int? EstadoPlanoMelId { get; set; }

        [Column("estado_plano_mdf_id")]
        public int? EstadoPlanoMdfId { get; set; }

		[Column("comentarios")]
		public string? Comentarios { get; set; }

		[Column("es_parcial")]
		public string? EsParcial { get; set; }

		[Column("colores")]
		public string? Colores { get; set; }

        public PeriodoEmpresaSede PeriodoEmpresaSede { get; set; }
		public Cotizacion Cotizacion { get; set; }
		public Almacen Almacen { get; set; }
		public Proveedor Proveedor { get; set; }
		public RutaProduccion RutaProduccion { get; set; }
		public EstadoOrdenProduccion EstadoOrdenProduccion { get; set; }
		public EstadoDespacho EstadoDespacho { get; set; }
		public Personal Personal { get; set; }
		public EstadoOrdenProduccion EstadoPlanoMel { get; set; }
        public EstadoOrdenProduccion EstadoPlanoMdf { get; set; }


        public DateTime? FechaDesde { get; set; }
		public DateTime? FechaHasta { get; set; }
		public List<string> EstadoOrdenProduccionList { get; set; }

	}
}
