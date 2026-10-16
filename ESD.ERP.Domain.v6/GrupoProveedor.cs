using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("grupo_prove")]
	public class GrupoProveedor : BaseEntity
	{
		public override int EntityId => GrupoProveedorId;
		[Key]
		[Column("grupo_prove_id", Order = 1)]
		public int GrupoProveedorId { get; set; }

		[Column("empresa_id")]
		public int EmpresaId { get; set; }

		[Column("nombre")]
		public string Nombre { get; set; }
		[Column("ignora_inc_fracc")]
		public string IgnorarIncrementoFraccion { get; set; }

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
