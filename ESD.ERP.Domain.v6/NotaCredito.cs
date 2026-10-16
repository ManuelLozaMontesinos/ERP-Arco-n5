using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("nota_credito")]
	public class NotaCredito : BaseEntity
	{
		public override int EntityId => NotaCreditoId;
		[Key]
		[Column("nota_credito_id", Order = 1)]
		public int NotaCreditoId { get; set; }

		[Column("periodo_emp_sede_id")]
		public int PeriodoEmpresaSedeId { get; set; }

		[Column("numero_opera")]
		public string NumeroOperacion { get; set; }

		[Column("serie")]
		public string Serie { get; set; }

		[Column("numero")]
		public string Numero { get; set; }

		[Column("fecha")]
		public DateTime Fecha { get; set; }

		[Column("tipo_nota_c_id")]
		public int TipoNotaCreditoId { get; set; }

		[Column("comprobante_id")]
		public int ComprobanteId { get; set; }

		[Column("vendedor_id")]
		public int VendedorId { get; set; }

		[Column("observa")]
		public string Observacion { get; set; }

		[Column("impuesto_tasa_id")]
		public int ImpuestoTasaId { get; set; }

		[Column("monto_bruto")]
		public decimal MontoBruto { get; set; }

		[Column("monto_neto")]
		public decimal MontoNeto { get; set; }

		[Column("monto_igv")]
		public decimal MontoIgv { get; set; }

		[Column("monto_isc")]
		public decimal MontoIsc { get; set; }

		[Column("monto_total")]
		public decimal MontoTotal { get; set; }

		[Column("es_contingencia")]
		public string EsContingencia { get; set; }

		[Column("emitido")]
		public string Emitido { get; set; }

		[Column("compro_elec_id")]
		public int? ComprobanteElectronicoId { get; set; }

		[Column("codigo_hash")]
		public string CodigoHash { get; set; }

		[Column("estado_compro_id")]
		public int EstadoComprobanteId { get; set; }

		[Column("estado_sunat_id")]
		public int? EstadoSunatId { get; set; }

		[Column("caja_id")]
		public int? CajaId { get; set; }

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
		public TipoNotaCredito TipoNotaCredito { get; set; }
		public Comprobante Comprobante { get; set; }
		public Vendedor Vendedor { get; set; }
		public ImpuestoTasa ImpuestoTasa { get; set; }
		public EstadoComprobante EstadoComprobante { get; set; }
		public EstadoSunat EstadoSunat { get; set; }
		public Caja Caja { get; set; }

		public DateTime? FechaDesde { get; set; }
		public DateTime? FechaHasta { get; set; }

		public List<int> ComprobanteElectronicoIdList { get; set; }
		public List<string> TipoNotaCreditoList { get; set; }
	}
}
