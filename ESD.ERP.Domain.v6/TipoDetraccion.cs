using ESD.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESD.ERP.Domain
{
	[Table("tipo_detrac")]
	public class TipoDetraccion : BaseEntity
	{
		public override int EntityId => TipoDetraccionId;
		[Key]
		[Column("tipo_detrac_id", Order = 1)]
		public int TipoDetraccionId { get; set; }

		[Column("tipo_detrac")]
		public string Codigo { get; set; }

		[Column("nombre")]
		public string Nombre { get; set; }

		[Column("tasa")]
		public decimal Tasa { get; set; }

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

      public List<string> TipoDetraccionList { get; set; }
   }
}
