using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class PeriodoEmpresaCorrelativoDTO : DTOBase
	{
		public override int Id => PeriodoEmpresaCorrelativoId;
		public int PeriodoEmpresaCorrelativoId { get; set; }
		public int PeriodoEmpresaSedeId { get; set; }
		public int TablaId { get; set; }
		public int Correlativo { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificaId { get; set; }
		public DateTime? FechaModifica { get; set; }
		public PeriodoEmpresaSedeDTO PeriodoEmpresaSedeDTO { get; set; }
		public TablaDTO TablaDTO { get; set; }
	}
}
