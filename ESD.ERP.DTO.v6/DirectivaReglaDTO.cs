using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class DirectivaReglaDTO : DTOBase
	{
		public override int Id => DirectivaReglaId;
		public int DirectivaReglaId { get; set; }
		public string Codigo { get; set; }
		public string Descripcion { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
	}
}
