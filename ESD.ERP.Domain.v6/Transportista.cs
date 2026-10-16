using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("transpo")]
	public class Transportista : BaseEntity
	{
		public override int EntityId => TransportistaId;
		[Key]
		[Column("transpo_id", Order = 1)]
		public int TransportistaId { get; set; }

		[Column("empresa_id")]
		public int EmpresaId { get; set; }

		[Column("numero_ruc")]
		public string NumeroRuc { get; set; }

		[Column("razon_social")]
		public string RazonSocial { get; set; }

		[Column("num_ins_vehic")]
		public string NumeroInscripcionVehicular { get; set; }

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
	}
}
