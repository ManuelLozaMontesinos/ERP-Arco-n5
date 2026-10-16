using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("estado_cotiza")]
	public class EstadoCotizacion : BaseEntity
	{
		public override int EntityId => EstadoCotizacionId;
		[Key]
		[Column("estado_cotiza_id", Order = 1)]
		public int EstadoCotizacionId { get; set; }

		[Column("estado_cotiza")]
		public string Codigo { get; set; }

		[Column("nombre")]
		public string Nombre { get; set; }

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

		public List<string> EstadoCotizacionList { get; set; }
	}
}
