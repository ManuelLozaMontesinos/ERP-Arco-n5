using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("personal_maquina")]
	public class PersonalMaquina : BaseEntity
	{
		public override int EntityId => MaquinaId;
		[Key]
		[Column("personal_maquina_id", Order = 1)]
		public int PersonalMaquinaId { get; set; }

		[Column("maquina_id")]
		public int MaquinaId { get; set; }

        [Column("personal_id")]
        public int PersonalId { get; set; }

        [Column("asistente_id")]
        public int AsistenteId { get; set; }

        [Column("fecha")]
        public DateTime Fecha { get; set; }

		[Column("usuario_creacion_id")]
		public int UsuarioCreacionId { get; set; }

		[Column("fecha_creacion")]
		public DateTime FechaCreacion { get; set; }

		[Column("usuario_modifica_id")]
		public int? UsuarioModificacionId { get; set; }

		[Column("fecha_modifica")]
		public DateTime? FechaModificacion { get; set; }


		public Maquina Maquina {  get; set; }
        public Personal Personal { get; set; }
		public Personal Asistente { get; set; }

    }
}
