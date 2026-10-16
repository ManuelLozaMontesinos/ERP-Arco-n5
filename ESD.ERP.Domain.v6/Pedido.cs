using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("pedido")]
	public class Pedido : BaseEntity
	{
		public override int EntityId => PedidoId;
		[Key]
		[Column("pedido_id", Order = 1)]
		public int PedidoId { get; set; }

		[Column("periodo_emp_sede_id")]
		public int PeriodoEmpresaSedeId { get; set; }

		[Column("pedido")]
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

		[Column("almacen_id")]
		public int AlmacenId { get; set; }

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

		[Column("monto_anticipo")]
		public decimal MontoAnticipo { get; set; }

		[Column("cant_aprob_req")]
		public int CantAprobReq { get; set; }

		[Column("soli_auto_dcto")]
		public string SoliAutoDcto { get; set; }

		[Column("usuario_id")]
		public int? UsuarioId { get; set; }

		[Column("fecha_auto_dcto")]
		public DateTime? FechaAutoDcto { get; set; }

		[Column("exo_auto_ade")]
		public string ExoAutoAde { get; set; }

		[Column("soli_auto_ade")]
		public string SoliAutoAde { get; set; }

		[Column("usu_auto_ade_id")]
		public int? UsuAutoAdeId { get; set; }

		[Column("fecha_auto_ade")]
		public DateTime? FechaAutoAde { get; set; }

		[Column("es_pedido")]
		public string EsPedido { get; set; }

		[Column("tiene_compro_anti")]
		public string TieneComproAnti { get; set; }

		[Column("estado_pedido_id")]
		public int EstadoPedidoId { get; set; }

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
		public ImpuestoTasa ImpuestoTasa { get; set; }
		public Usuario Usuario { get; set; }
		public Usuario UsuarioAutoriza { get; set; }
		public EstadoPedido EstadoPedido { get; set; }
	}
}
