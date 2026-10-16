using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESD.Core.Data;

namespace ESD.ERP.Domain
{
	[Table("periodo_emp_sede")]
	public class PeriodoEmpresaSede : BaseEntity
	{
		public override int EntityId => PeriodoEmpresaSedeId;
		[Key]
		[Column("periodo_emp_sede_id", Order = 1)]
		public int PeriodoEmpresaSedeId { get; set; }

		[Column("periodo_id")]
		public int PeriodoId { get; set; }

		[Column("empresa_sede_id")]
		public int EmpresaSedeId { get; set; }

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

		public Periodo Periodo { get; set; }
		public EmpresaSede EmpresaSede { get; set; }
		public List<int> EmpresaSedeIdList { get; set; }
	}
}