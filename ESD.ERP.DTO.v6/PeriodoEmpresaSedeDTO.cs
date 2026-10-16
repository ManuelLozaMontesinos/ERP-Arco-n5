using System;
using System.Collections.Generic;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class PeriodoEmpresaSedeDTO : DTOBase
	{
		public override int Id => PeriodoEmpresaSedeId;
		public int PeriodoEmpresaSedeId { get; set; }
		public int PeriodoId { get; set; }
		public int EmpresaSedeId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public PeriodoDTO PeriodoDTO { get; set; }
		public EmpresaSedeDTO EmpresaSedeDTO { get; set; }
		public List<int> EmpresaSedeIdList { get; set; }
	}
}
