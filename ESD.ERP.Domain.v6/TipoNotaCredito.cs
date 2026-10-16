using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("tipo_nota_c")]
	public class TipoNotaCredito : BaseEntity
	{
		public override int EntityId => TipoNotaCreditoId;
		[Key]
		[Column("tipo_nota_c_id", Order = 1)]
		public int TipoNotaCreditoId { get; set; }

		[Column("tipo_nota_c")]
		public string Codigo { get; set; }

		[Column("nombre")]
		public string Nombre { get; set; }

		[Column("afecta_alm")]
		public string AfectaAlmacen{ get; set; }

		[Column("es_ingreso")]
		public string EsIngreso { get; set; }

		[Column("permite_edit_item")]
		public string PermiteEditItem { get; set; }

		[Column("permite_edit_cant")]
		public string PermiteEditarCantidad { get; set; }
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

	}
}
