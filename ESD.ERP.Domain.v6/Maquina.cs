using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("maquina")]
	public class Maquina : BaseEntity
	{
		public override int EntityId => MaquinaId;
		[Key]
		[Column("maquina_id", Order = 1)]
		public int MaquinaId { get; set; }

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
		[Column("almacen_id")]
		public int? AlmacenId { get; set; }

		public Almacen Almacen { get; set; }

	}
}
