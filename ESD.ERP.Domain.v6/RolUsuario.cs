using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("rol_usuario")]
	public class RolUsuario : BaseEntity
	{
		public override int EntityId => RolUsuarioId;
		[Key]
		[Column("rol_usuario_id", Order = 1)]
		public int RolUsuarioId { get; set; }

		[Column("usuario_id")]
		public int UsuarioId { get; set; }

		[Column("rol_id")]
		public int RolId { get; set; }

		public Usuario Usuario { get; set; }
		public Rol Rol { get; set; }
		public List<string> RolList { get; set; }
	}
}
