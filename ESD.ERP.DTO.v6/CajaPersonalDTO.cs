using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class CajaPersonalDTO : DTOBase
	{
		public override int Id => CajaPersonalId;
		public int CajaPersonalId { get; set; }
		public int CajaId { get; set; }
		public int PersonalId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public CajaDTO CajaDTO { get; set; }
		public PersonalDTO PersonalDTO { get; set; }
	}
}
