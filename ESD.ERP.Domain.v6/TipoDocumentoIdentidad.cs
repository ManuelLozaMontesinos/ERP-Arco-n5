using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("tipo_doc_iden")]
	public class TipoDocumentoIdentidad : BaseEntity
	{
		public override int EntityId => TipoDocumentoIdentidadId;
		[Key]
		[Column("tipo_doc_iden_id", Order = 1)]
		public int TipoDocumentoIdentidadId { get; set; }

		[Column("tipo_doc_iden")]
		public string Codigo { get; set; }

		[Column("nombre")]
		public string Nombre { get; set; }

		[Column("abrev")]
		public string Abreviatura { get; set; }

		[Column("formato_valida")]
		public string FormatoValidacion { get; set; }

		[Column("codigo_sunat")]
		public string CodigoSunat { get; set; }

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

		public List<string> NotTipoDocumentoIdentidadList { get; set; }
	}
}
