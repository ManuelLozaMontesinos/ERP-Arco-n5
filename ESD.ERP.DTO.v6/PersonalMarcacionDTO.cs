using System;
using System.Collections.Generic;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class PersonalMarcacionDTO : DTOBase
	{
		public override int Id => PersonalMarcacionId;
		public int PersonalMarcacionId { get; set; }
		public int PersonalId { get; set; }
		public DateTime Fecha { get; set; }
		public string HoraMinuto { get; set; }
		public string IgnorarTolerancia { get; set; }
		public string EsFalta { get; set; }
		public string EsRemunerado { get; set; }
		public string Verificado { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public PersonalDTO PersonalDTO { get; set; }

		public List<PersonalMarcacionHoraDTO> PersonalMarcacionHoraDTOList { get; set; }

		public DateTime FechaDesde { get; set; }
		public DateTime FechaHasta { get; set; }
		public List<RelojMarcacionDTO> RelojMarcacionDTOList { get; set; }
	}
}
