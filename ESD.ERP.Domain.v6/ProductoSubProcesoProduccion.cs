using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("produ_s_p_p")]
	public class ProductoSubProcesoProduccion : BaseEntity
	{
		public override int EntityId => ProductoSubProcesoProduccionId;
		[Key]
		[Column("produ_s_p_p_id", Order = 1)]
		public int ProductoSubProcesoProduccionId { get; set; }

		[Column("producto_id")]
		public int ProductoId { get; set; }

		[Column("sub_proce_p_id")]
		public int SubProcesoProduccionId { get; set; }
		[Column("orden")]
		public int Orden { get; set; }
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

		public Producto Producto { get; set; }
		public SubProcesoProduccion SubProcesoProduccion { get; set; }
		public List<int> ProductoIdList { get; set; }
	}
}
