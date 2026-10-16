using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("perso_m_hora")]
	public class PersonalMarcacionHora : BaseEntity
	{
		public override int EntityId => PersonalMarcacionHoraId;
		[Key]
		[Column("perso_m_hora_id", Order = 1)]
		public int PersonalMarcacionHoraId { get; set; }

		[Column("perso_marca_id")]
		public int PersonalMarcacionId { get; set; }

		[Column("hora_rem_id")]
		public int HoraRemunerativaId { get; set; }

		[Column("cantidad")]
		public decimal Cantidad { get; set; }

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

		public PersonalMarcacion PersonalMarcacion { get; set; }
		public HoraRemunerativa HoraRemunerativa { get; set; }
	}
}