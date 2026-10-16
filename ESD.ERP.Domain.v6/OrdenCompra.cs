using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("orden_co")]
	public class OrdenCompra : BaseEntity
	{
		public override int EntityId => OrdenCompraId;
		[Key]
		[Column("orden_co_id", Order = 1)]
		public int OrdenCompraId { get; set; }

		[Column("periodo_emp_sede_id")]
		public int PeriodoEmpresaSedeId { get; set; }

		[Column("orden_co")]
		public string Codigo { get; set; }

		[Column("fecha")]
		public DateTime Fecha { get; set; }

		[Column("tipo_producto_id")]
		public int TipoProductoId { get; set; }

		[Column("pedido_co_id")]
		public int? PedidoCompraId { get; set; }

		[Column("proveedor_id")]
		public int ProveedorId { get; set; }

		[Column("fecha_entrega")]
		public DateTime? FechaEntrega { get; set; }

		[Column("moneda_id")]
		public int MonedaId { get; set; }

		[Column("tipo_cambio")]
		public decimal TipoCambio { get; set; }

		[Column("impuesto_tasa_id")]
		public int ImpuestoTasaId { get; set; }

		[Column("monto_bruto")]
		public decimal MontoBruto { get; set; }

		[Column("monto_impuesto")]
		public decimal MontoImpuesto { get; set; }

		[Column("monto_total")]
		public decimal MontoTotal { get; set; }

		[Column("plazo_entrega")]
		public string PlazoEntrega { get; set; }

		[Column("lugar_entrega")]
		public string LugarEntrega { get; set; }

		[Column("cond_pago_id")]
		public int CondicionPagoId { get; set; }

		[Column("observa")]
		public string Observacion { get; set; }

		[Column("permite_pago_p")]
		public string PermitePagoParcial { get; set; }

		[Column("es_modifica")]
		public string EsModificacion { get; set; }

		[Column("estado_aproba_id")]
		public int? EstadoAprobacionId { get; set; }

		[Column("fecha_aproba")]
		public DateTime? FechaAprobacion { get; set; }

		[Column("estado_oc_ant_id")]
		public int? EstadoOrdenCompraAnteriorId { get; set; }

		[Column("estado_orden_co_id")]
		public int EstadoOrdenCompraId { get; set; }

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
		public TipoProducto TipoProducto { get; set; }
		public Proveedor Proveedor { get; set; }
		public Moneda Moneda { get; set; }
		public ImpuestoTasa ImpuestoTasa { get; set; }
		public EstadoOrdenCompra EstadoOrdenCompra { get; set; }
		public CondicionPago CondicionPago { get; set;}
		public PedidoCompra PedidoCompra { get; set; }
		public EstadoAprobacion EstadoAprobacion { get; set; }
		public DateTime? FechaDesde { get; set; }
		public DateTime? FechaHasta { get; set; }
		public List<string> EstadoOrdenCompraList { get; set; }
	}
}