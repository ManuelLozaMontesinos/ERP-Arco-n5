using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class PersonalMaquinaDTO : DTOBase
	{
		public override int Id => PersonalMaquinaId;
		public int PersonalMaquinaId { get; set; }
        public int MaquinaId { get; set; }
        public int PersonalId { get; set; }
		public int AsistenteId { get; set; }
		public DateTime Fecha { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }

		public MaquinaDTO MaquinaDTO { get; set; }	
		public PersonalDTO PersonalDTO { get; set; }
		public PersonalDTO AsistenteDTO { get; set; }

	}
}
