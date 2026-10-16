using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("atrib_config")]
	public class AtributoConfiguracion : BaseEntity
	{
		public override int EntityId => AtributoConfiguracionId;
		[Key]
		[Column("atrib_config_id", Order = 1)]
		public int AtributoConfiguracionId { get; set; }

		[Column("nombre")]
		public string Nombre { get; set; }

		[Column("atributo")]
		public string Atributo { get; set; }

		[Column("orden")]
		public int Orden { get; set; }

		[Column("editable")]
		public string Editable { get; set; }

		[Column("tiene_boton")]
		public string TieneBoton { get; set; }

		[Column("es_grupo")]
		public string EsGrupo { get; set; }

		[Column("atributo_lista")]
		public string AtributoLista { get; set; }

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
