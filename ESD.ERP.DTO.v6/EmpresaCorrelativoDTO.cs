using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class EmpresaCorrelativoDTO : DTOBase
	{
		public override int Id => EmpresaCorrelativoId;
		public int EmpresaCorrelativoId { get; set; }
		public int EmpresaId { get; set; }
		public int TablaId { get; set; }
		public int Correlativo { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public EmpresaDTO EmpresaDTO { get; set; }
		public TablaDTO TablaDTO { get; set; }
	}
}
