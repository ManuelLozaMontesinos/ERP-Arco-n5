using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("trans_alma")]
	public class TransaccionAlmacen : BaseEntity
	{
		public override int EntityId => TransaccionAlmacenId;
		[Key]
		[Column("trans_alma_id", Order = 1)]
		public int TransaccionAlmacenId { get; set; }

		[Column("empresa_id")]
		public int EmpresaId { get; set; }

		[Column("trans_alma")]
		public string Codigo { get; set; }

		[Column("nombre")]
		public string Nombre { get; set; }

		[Column("tipo_mov_alma_id")]
		public int? TipoMovimientoAlmacenId { get; set; }

		[Column("origen_registro_id")]
		public int OrigenRegistroId { get; set; }

		[Column("es_configurable")]
		public string EsConfigurable { get; set; }

		[Column("es_compra")]
		public string EsCompra { get; set; }

		[Column("tiene_mant")]
		public string TieneMantenimiento { get; set; }

		[Column("tipo_operacion_id")]
		public int? TipoOperacionId { get; set; }

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

		public TipoMovimientoAlmacen TipoMovimientoAlmacen { get; set; }
		public OrigenRegistro OrigenRegistro { get; set; }
		public TipoOperacion TipoOperacion { get; set; }

		public int NotTipoMovimientoAlmacenId { get; set; }
		public List<string> NotTransaccionAlmacenList { get; set; }
	}
}
