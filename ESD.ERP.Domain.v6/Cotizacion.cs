using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("cotiza")]
	public class Cotizacion : BaseEntity
	{
		public override int EntityId => CotizacionId;
		[Key]
		[Column("cotiza_id", Order = 1)]
		public int CotizacionId { get; set; }

		[Column("periodo_emp_sede_id")]
		public int PeriodoEmpresaSedeId { get; set; }

		[Column("cotiza")]
		public string Codigo { get; set; }

		[Column("fecha")]
		public DateTime Fecha { get; set; }

		[Column("proveedor_id")]
		public int ProveedorId { get; set; }

		[Column("vendedor_id")]
		public int VendedorId { get; set; }

		[Column("moneda_id")]
		public int MonedaId { get; set; }

		[Column("monto_tc")]
		public decimal MontoTipoCambio { get; set; }

		[Column("lista_precio_id")]
		public int ListaPrecioId { get; set; }

		[Column("tipo_c_pago_id")]
		public int TipoCondicionPagoId { get; set; }

		[Column("observa")]
		public string Observacion { get; set; }

		[Column("impuesto_tasa_id")]
		public int ImpuestoTasaId { get; set; }

		[Column("monto_bruto")]
		public decimal MontoBruto { get; set; }

		[Column("porc_dcto")]
		public decimal PorcentajeDescuento { get; set; }

		[Column("monto_dcto")]
		public decimal MontoDescuento { get; set; }

		[Column("monto_dcto_item")]
		public decimal MontoDescuentoItem { get; set; }

		[Column("monto_neto")]
		public decimal MontoNeto { get; set; }

		[Column("monto_igv")]
		public decimal MontoIgv { get; set; }

		[Column("monto_isc")]
		public decimal MontoIsc { get; set; }
		[Column("monto_anticipo")]
		public decimal MontoAnticipo { get; set; }

		[Column("monto_total")]
		public decimal MontoTotal { get; set; }

		[Column("almacen_id")]
		public int? AlmacenId { get; set; }

		[Column("mes_contable_id")]
		public int? MesContableId { get; set; }

		[Column("es_pedido")]
		public string EsPedido { get; set; }

		[Column("envia_prod")]
		public string EnviadoProduccion { get; set; }

		[Column("estado_cotiza_id")]
		public int EstadoCotizacionId { get; set; }

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
		public Proveedor Proveedor { get; set; }
		public Vendedor Vendedor { get; set; }
		public Moneda Moneda { get; set; }
		public ListaPrecio ListaPrecio { get; set; }
		public TipoCondicionPago TipoCondicionPago { get; set; }
		public ImpuestoTasa ImpuestoTasa { get; set; }
		public Almacen Almacen { get; set; }
		public EstadoCotizacion EstadoCotizacion { get; set; }
		public DateTime? FechaDesde { get; set; }
		public DateTime? FechaHasta { get; set; }
		public List<string> EstadoCotizacionList { get; set; }
	}
}
