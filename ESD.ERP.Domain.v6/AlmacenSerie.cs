using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("almacen_serie")]
	public class AlmacenSerie : BaseEntity
	{
		public override int EntityId => AlmacenSerieId;
		[Key]
		[Column("almacen_serie_id", Order = 1)]
		public int AlmacenSerieId { get; set; }

		[Column("almacen_id")]
		public int AlmacenId { get; set; }

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

		public Almacen Almacen { get; set; }
		public DocumentoComercialSerie DocumentoComercialSerie { get; set; }
		public List<int> DocumentoComercialSerieIdList { get; set; }
	}
}
