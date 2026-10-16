using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("mov_alm_prod")]
	public class MovimientoAlmacenProducto : BaseEntity
	{
		public override int EntityId => MovimientoAlmacenProductoId;
		[Key]
		[Column("mov_alm_prod_id", Order = 1)]
		public int MovimientoAlmacenProductoId { get; set; }

		[Column("mov_almacen_id")]
		public int MovimientoAlmacenId { get; set; }

		[Column("producto_id")]
		public int ProductoId { get; set; }

		[Column("descripcion")]
		public string Descripcion { get; set; }

		[Column("cant_docu")]
		public decimal CantidadDocumento { get; set; }

		[Column("peso_n_docu")]
		public decimal PesoNetoDocumento { get; set; }

		[Column("peso_b_docu")]
		public decimal PesoBrutoDocumento { get; set; }

		[Column("cantidad")]
		public decimal Cantidad { get; set; }

		[Column("peso_neto")]
		public decimal PesoNeto { get; set; }

		[Column("peso_bruto")]
		public decimal PesoBruto { get; set; }

		[Column("valor_unitario")]
		public decimal ValorUnitario { get; set; }

		[Column("produ_recla_id")]
		public int? ProductoReclasificacionId { get; set; }

		[Column("cant_recla")]
		public decimal CantidadReclasificacion { get; set; }

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

		public MovimientoAlmacen MovimientoAlmacen { get; set; }
		public Producto Producto { get; set; }
		public Usuario Usuario { get; set; }
		public Producto ProductoReclasificacion { get; set; }

		public string ConCantidadReclasificacion { get; set; }
	}
}
