using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("guia_r_prod")]
	public class GuiaRemisionProducto : BaseEntity
	{
		public override int EntityId => GuiaRemisionProductoId;
		[Key]
		[Column("guia_r_prod_id", Order = 1)]
		public int GuiaRemisionProductoId { get; set; }

		[Column("guia_remision_id")]
		public int GuiaRemisionId { get; set; }

		[Column("mov_alm_prod_id")]
		public int? MovimientoAlmacenProductoId { get; set; }

		[Column("cod_produ")]
		public string CodigoProducto { get; set; }

		[Column("descripcion")]
		public string Descripcion { get; set; }

		[Column("cantidad")]
		public decimal Cantidad { get; set; }

		[Column("peso_neto")]
		public decimal PesoNeto { get; set; }

		[Column("peso_bruto")]
		public decimal PesoBruto { get; set; }

		[Column("estado")]
		public string Estado { get; set; }

		[Column("usuario_creacion_id")]
		public int UsuarioCreacionId { get; set; }

		[Column("fecha_creacion")]
		public DateTime FechaCreacion { get; set; }

		[Column("usuario_modifica_id")]
		public int? UsuarioModificaId { get; set; }

		[Column("fecha_modifica")]
		public DateTime? FechaModifica { get; set; }

		public GuiaRemision GuiaRemision { get; set; }
		public MovimientoAlmacenProducto MovimientoAlmacenProducto { get; set; }
		public Producto Producto { get; set; }
	}
}
