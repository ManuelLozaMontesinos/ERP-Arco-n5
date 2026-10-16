using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("orden_c_comp")]
	public class OrdenCompraComprobante : BaseEntity
	{
		public override int EntityId => OrdenCompraComprobanteId;
		[Key]
		[Column("orden_c_comp_id", Order = 1)]
		public int OrdenCompraComprobanteId { get; set; }

		[Column("orden_co_id")]
		public int OrdenCompraId { get; set; }

		[Column("tipo_doc_com_id")]
		public int TipoDocumentoComercialId { get; set; }

		[Column("serie")]
		public string Serie { get; set; }

		[Column("numero")]
		public string Numero { get; set; }

		[Column("fecha")]
		public DateTime Fecha { get; set; }

		[Column("fecha_vcto")]
		public DateTime? FechaVencimiento { get; set; }

		[Column("monto_bruto")]
		public decimal MontoBruto { get; set; }

		[Column("monto_impuesto")]
		public decimal MontoImpuesto { get; set; }

		[Column("monto_total")]
		public decimal MontoTotal { get; set; }

		[Column("cond_pago_id")]
		public int? CondicionPagoId { get; set; }

		[Column("reg_refe_id")]
		public int? RegistroReferencialId { get; set; }

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

		public OrdenCompra OrdenCompra { get; set; }
		public TipoDocumentoComercial TipoDocumentoComercial { get; set; }
		public CondicionPago CondicionPago { get; set; }
	}
}
