using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("grupo_param_sistema")]
	public class GrupoParametroSistema : BaseEntity
	{
		public override int EntityId => GrupoParametroSistemaId;
		[Key]
		[Column("grupo_param_sistema_id", Order = 1)]
		public int GrupoParametroSistemaId { get; set; }

		[Column("grupo_param_sistema")]
		public string Codigo { get; set; }

		[Column("nombre")]
		public string Nombre { get; set; }

		[Column("habilita_item")]
		public string HabilitaItem { get; set; }

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

	}
}
