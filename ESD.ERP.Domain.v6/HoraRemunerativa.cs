using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("hora_rem")]
	public class HoraRemunerativa : BaseEntity
	{
		public override int EntityId => HoraRemunerativaId;
		[Key]
		[Column("hora_rem_id", Order = 1)]
		public int HoraRemunerativaId { get; set; }

		[Column("empresa_id")]
		public int EmpresaId { get; set; }

		[Column("hora_rem")]
		public string Codigo { get; set; }

		[Column("nombre")]
		public string Nombre { get; set; }

		[Column("abrev")]
		public string Abreviatura { get; set; }

		[Column("var_calc_id")]
		public int VariableCalculoId { get; set; }

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
		public VariableCalculo VariableCalculo { get; set; }
	}
}
