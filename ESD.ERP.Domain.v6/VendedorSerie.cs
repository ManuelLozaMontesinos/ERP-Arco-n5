using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("vend_serie")]
	public class VendedorSerie : BaseEntity
	{
		public override int EntityId => VendedorSerieId;
		[Key]
		[Column("vend_serie_id", Order = 1)]
		public int VendedorSerieId { get; set; }

		[Column("vendedor_id")]
		public int VendedorId { get; set; }

		[Column("doc_com_serie_id")]
		public int DocumentoComercialSerieId { get; set; }

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

		public Vendedor Vendedor { get; set; }
		public DocumentoComercialSerie DocumentoComercialSerie { get; set; }
		public List<int> DocumentoComercialSerieIdList { get; set; }
	}
}
