using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("jornada_lab")]
	public class JornadaLaboral : BaseEntity
	{
		public override int EntityId => JornadaLaboralId;
		[Key]
		[Column("jornada_lab_id", Order = 1)]
		public int JornadaLaboralId { get; set; }

		[Column("empresa_id")]
		public int EmpresaId { get; set; }

		[Column("nombre")]
		public string Nombre { get; set; }

		[Column("hora_ing")]
		public string HoraIngreso { get; set; }

		[Column("cant_hora")]
		public decimal CantidadHoras { get; set; }

		[Column("dia_id")]
		public int DiaId { get; set; }

		[Column("dia_hasta_id")]
		public int DiaHastaId { get; set; }

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
		public Dia Dia { get; set; }
		public Dia DiaHasta { get; set; }
	}
}
