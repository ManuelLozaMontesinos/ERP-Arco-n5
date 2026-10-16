using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("periodo_emp_corre")]
	public class PeriodoEmpresaCorrelativo : BaseEntity
	{
		public override int EntityId => PeriodoEmpresaCorrelativoId;
		[Key]
		[Column("periodo_emp_corre_id", Order = 1)]
		public int PeriodoEmpresaCorrelativoId { get; set; }

		[Column("periodo_emp_sede_id")]
		public int PeriodoEmpresaSedeId { get; set; }

		[Column("tabla_id")]
		public int TablaId { get; set; }

		[Column("correlativo")]
		public int Correlativo { get; set; }

		[Column("usuario_creacion_id")]
		public int UsuarioCreacionId { get; set; }

		[Column("fecha_creacion")]
		public DateTime FechaCreacion { get; set; }

		[Column("usuario_modifica_id")]
		public int? UsuarioModificaId { get; set; }

		[Column("fecha_modifica")]
		public DateTime? FechaModifica { get; set; }

		public PeriodoEmpresaSede PeriodoEmpresaSede { get; set; }
		public Tabla Tabla { get; set; }
	}
}
