	using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("producto")]
	public class Producto : BaseEntity
	{
		public override int EntityId => ProductoId;
		[Key]
		[Column("producto_id", Order = 1)]
		public int ProductoId { get; set; }

		[Column("producto")]
		public string Codigo { get; set; }

		[Column("descripcion")]
		public string Descripcion { get; set; }

		[Column("cod_referencial")]
		public string CodigoReferencial { get; set; }

		[Column("familia_producto_id")]
		public int FamiliaProductoId { get; set; }

		[Column("unidad_medida_id")]
		public int UnidadMedidaId { get; set; }

		[Column("tipo_igv_id")]
		public int TipoIgvId { get; set; }

		[Column("peso_neto")]
		public decimal PesoNeto { get; set; }

		[Column("peso_bruto")]
		public decimal PesoBruto { get; set; }

		[Column("valor_repo")]
		public decimal ValorReposicion { get; set; }

		[Column("volumen")]
		public decimal Volumen { get; set; }

		[Column("tiene_pres_vta")]
		public string TienePresentacionVenta { get; set; }

		[Column("es_opera")]
		public string EsOperacion { get; set; }
		[Column("es_perecible")]
		public string EsPerecible { get; set; }
		[Column("detalle_req")]
		public string DetalleRequerido { get; set; }
		[Column("master_pack")]
		public int MasterPack { get; set; }

		[Column("colorId")]
		public int? ColorId { get; set; }

		[Column("estado_producto_id")]
		public int EstadoProductoId { get; set; }

		[Column("estado_com_prod_id")]
		public int EstadoComercialProductoId { get; set; }

		[Column("estado")]
		public string Estado { get; set; }

		[Column("fecha_alta")]
		public DateTime FechaAlta { get; set; }

		[Column("fecha_baja")]
		public DateTime? FechaBaja { get; set; }

		[Column("usuario_creacion_id")]
		public int UsuarioCreacionId { get; set; }

		[Column("fecha_creacion")]
		public DateTime FechaCreacion { get; set; }

		[Column("usuario_modifica_id")]
		public int? UsuarioModificacionId { get; set; }

		[Column("fecha_modifica")]
		public DateTime? FechaModificacion { get; set; }

		public FamiliaProducto FamiliaProducto { get; set; } 
		public UnidadMedida UnidadMedida { get; set; }
		public TipoIgv TipoIgv { get; set; }
		public EstadoProducto EstadoProducto { get; set; }
		public EstadoComercialProducto EstadoComercialProducto { get; set; }
		public Color Color { get; set; }
		public int Periodo { get; set; }
		public List<string> ProductoList { get; set; }
	}
}
