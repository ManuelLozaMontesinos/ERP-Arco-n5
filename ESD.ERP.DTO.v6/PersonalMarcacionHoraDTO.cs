using System;
using System.Collections.Generic;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class PersonalMarcacionHoraDTO : DTOBase
	{
		public override int Id => PersonalMarcacionHoraId;
		public int PersonalMarcacionHoraId { get; set; }
		public int PersonalMarcacionId { get; set; }
		public int HoraRemunerativaId { get; set; }
		public decimal Cantidad { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public PersonalMarcacionDTO PersonalMarcacionDTO { get; set; }
		public HoraRemunerativaDTO HoraRemunerativaDTO { get; set; }
	}
}
