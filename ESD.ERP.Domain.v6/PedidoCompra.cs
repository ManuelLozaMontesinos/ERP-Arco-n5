using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("pedido_co")]
	public class PedidoCompra : BaseEntity
	{
		public override int EntityId => PedidoCompraId;
		[Key]
		[Column("pedido_co_id", Order = 1)]
		public int PedidoCompraId { get; set; }

		[Column("periodo_emp_sede_id")]
		public int PeriodoEmpresaSedeId { get; set; }

		[Column("pedido_co")]
		public string Codigo { get; set; }

		[Column("fecha")]
		public DateTime Fecha { get; set; }

		[Column("tipo_producto_id")]
		public int TipoProductoId { get; set; }

		[Column("proveedor_id")]
		public int ProveedorId { get; set; }

		[Column("es_modifica")]
		public string EsModificacion { get; set; }

		[Column("estado_aproba_id")]
		public int? EstadoAprobacionId { get; set; }

		[Column("fecha_aproba")]
		public DateTime? FechaAprobacion { get; set; }

		[Column("observa")]
		public string Observacion { get; set; }

		[Column("estado_p_c_ant_id")]
		public int? EstadoPedidoCompraAnteriorId { get; set; }

		[Column("estado_pedido_co_id")]
		public int EstadoPedidoCompraId { get; set; }

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
		public EstadoAprobacion EstadoAprobacion { get; set; }
		public EstadoPedidoCompra EstadoPedidoCompra { get; set; }
		public DateTime? FechaDesde { get; set; }
		public DateTime? FechaHasta { get; set; }
	}
}
