using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class TransportistaDTO : DTOBase
	{
		public override int Id => TransportistaId;
		public int TransportistaId { get; set; }
		public int EmpresaId { get; set; }
		public string NumeroRuc { get; set; }
		public string RazonSocial { get; set; }
		public string NumeroInscripcionVehicular { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public EmpresaDTO EmpresaDTO { get; set; }
	}
}
