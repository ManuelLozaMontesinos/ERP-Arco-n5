using ESD.Core.Service.DTO;
using System;
using System.Collections.Generic;

namespace ESD.ERP.DTO
{
	public class RolDTO : DTOBase
	{
		public override int Id => RolId;
		public int RolId { get; set; }
		public int? EmpresaId { get; set; }
		public string Codigo { get; set; }
		public string Nombre { get; set; }
		public int OrigenRegistroId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public OrigenRegistroDTO OrigenRegistroDTO { get; set; }
		public List<RolMenuDTO> RolMenuDTOList { get; set; }
	}
}
