using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("orden_p_prod")]
	public class OrdenProduccionProducto : BaseEntity
	{
		public override int EntityId => OrdenProduccionProductoId;
		[Key]
		[Column("orden_p_prod_id", Order = 1)]
		public int OrdenProduccionProductoId { get; set; }

		[Column("orden_pro_id")]
		public int OrdenProduccionId { get; set; }

		[Column("producto_id")]
		public int ProductoId { get; set; }

		[Column("descripcion")]
		public string Descripcion { get; set; }

		[Column("cantidad")]
		public decimal Cantidad { get; set; }

		[Column("cant_entrega")]
		public decimal CantidadEntregada { get; set; }

		[Column("fecha_entre")]
		public DateTime? FechaEntrega { get; set; }
		
		[Column("entrega_a")]
		public string EntregadoA { get; set; }

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

        [Column("estado_despacho_id")]
		public int? EstadoDespachoId { get; set; }

		[Column("personal_despacho_id")]
		public int? PersonalDespachoId { get; set; }

        [Column("observa_desp")]
        public string? ObservacionDespacho { get; set; }

        [Column("fecha_despacho")]
		public DateTime? FechaDespacho { get; set; }

        public OrdenProduccion OrdenProduccion { get; set; }
		public Producto Producto { get; set; }
        public EstadoDespacho EstadoDespacho { get; set; }
        public Personal Personal { get; set; }


    }
}
