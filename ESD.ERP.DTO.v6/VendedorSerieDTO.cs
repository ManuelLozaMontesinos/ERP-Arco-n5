using System;
using System.Collections.Generic;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class VendedorSerieDTO : DTOBase
	{
		public override int Id => VendedorSerieId;
		public int VendedorSerieId { get; set; }
		public int VendedorId { get; set; }
		public int DocumentoComercialSerieId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public VendedorDTO VendedorDTO { get; set; }
		public DocumentoComercialSerieDTO DocumentoComercialSerieDTO { get; set; }
		public List<int> DocumentoComercialSerieIdList { get; set; }
	}
}
