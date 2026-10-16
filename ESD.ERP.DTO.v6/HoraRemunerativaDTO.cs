using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class HoraRemunerativaDTO : DTOBase
	{
		public override int Id => HoraRemunerativaId;
		public int HoraRemunerativaId { get; set; }
		public int EmpresaId { get; set; }
		public string Codigo { get; set; }
		public string Nombre { get; set; }
		public string Abreviatura { get; set; }
		public int VariableCalculoId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public EmpresaDTO EmpresaDTO { get; set; }
		public VariableCalculoDTO VariableCalculoDTO { get; set; }
	}
}
