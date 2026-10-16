using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("perso_marca")]
	public class PersonalMarcacion : BaseEntity
	{
		public override int EntityId => PersonalMarcacionId;
		[Key]
		[Column("perso_marca_id", Order = 1)]
		public int PersonalMarcacionId { get; set; }

		[Column("personal_id")]
		public int PersonalId { get; set; }

		[Column("fecha")]
		public DateTime Fecha { get; set; }

		[Column("hora_minuto")]
		public string HoraMinuto { get; set; }

		[Column("ignora_tolera")]
		public string IgnorarTolerancia { get; set; }

		[Column("es_falta")]
		public string EsFalta { get; set; }

		[Column("es_remunera")]
		public string EsRemunerado { get; set; }

		[Column("verificado")]
		public string Verificado { get; set; }

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

		public Personal Personal { get; set; }

		public DateTime FechaDesde { get; set; }
		public DateTime FechaHasta { get; set; }
	}
}
