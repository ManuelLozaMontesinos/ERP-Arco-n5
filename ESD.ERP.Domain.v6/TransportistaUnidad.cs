using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("transp_unid")]
	public class TransportistaUnidad : BaseEntity
	{
		public override int EntityId => TransportistaUnidadId;
		[Key]
		[Column("transp_unid_id", Order = 1)]
		public int TransportistaUnidadId { get; set; }

		[Column("transpo_id")]
		public int TransportistaId { get; set; }

		[Column("marca")]
		public string Marca { get; set; }

		[Column("modelo")]
		public string Modelo { get; set; }

		[Column("placa")]
		public string Placa { get; set; }
		[Column("placa_2")]
		public string Placa2 { get; set; }

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

		public Transportista Transportista { get; set; }
	}
}
