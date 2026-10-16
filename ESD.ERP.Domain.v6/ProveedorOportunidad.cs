using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("prove_opo")]
	public class ProveedorOportunidad : BaseEntity
	{
		public override int EntityId =>ProveedorOportunidadId;
		[Key]
		[Column("prove_opo_id", Order = 1)]
		public int ProveedorOportunidadId { get; set; }

		[Column("prove_id")]
		public int ProveedorId { get; set; }

		[Column("oportu_venta_id")]
		public int OportunidadVentaId { get; set; }

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

		public Proveedor Proveedor { get; set; }
		public OportunidadVenta OportunidadVenta { get; set; }
	}
}
