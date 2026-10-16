using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class PersonalVacacionDTO : DTOBase
	{
		public override int Id => PersonalVacacionId;
		public int PersonalVacacionId { get; set; }
		public int PersonalId { get; set; }
		public DateTime? FechaInicio { get; set; }
		public DateTime? FechaFin { get; set; }
		public int DiasACuenta { get; set; }
		public int EstadoVacacionId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public PersonalDTO PersonalDTO { get; set; }
		public EstadoVacacionDTO EstadoVacacionDTO { get; set; }
	}
}
