using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("centro_costo")]
	public class CentroCosto : BaseEntity
	{
		public override int EntityId => CentroCostoId;
		[Key]
		[Column("centro_costo_id", Order = 1)]
		public int CentroCostoId { get; set; }

		[Column("empresa_id")]
		public int EmpresaId { get; set; }

		[Column("cen_costo_padre_id")]
		public int? CentroCostoPadreId { get; set; }

		[Column("nombre")]
		public string Nombre { get; set; }

		[Column("sigla")]
		public string Sigla { get; set; }

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

		public Empresa Empresa { get; set; }
		public CentroCosto CentroCostoPadre { get; set; }
	}
}
