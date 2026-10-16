using System;
using System.Collections.Generic;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class AlmacenSerieDTO : DTOBase
	{
		public override int Id => AlmacenSerieId;
		public int AlmacenSerieId { get; set; }
		public int AlmacenId { get; set; }
		public int DocumentoComercialSerieId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public AlmacenDTO AlmacenDTO { get; set; }
		public DocumentoComercialSerieDTO DocumentoComercialSerieDTO { get; set; }
		public List<int> DocumentoComercialSerieIdList { get; set; }
	}
}
