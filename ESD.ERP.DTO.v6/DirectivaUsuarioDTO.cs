using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class DirectivaUsuarioDTO : DTOBase
	{
		public override int Id => DirectivaUsuarioId;
		public int DirectivaUsuarioId { get; set; }
		public int DirectivaReglaId { get; set; }
		public int UsuarioId { get; set; }
		public int Valor { get; set; }
		public string Codigo { get; set; }
		public string Descripcion { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public DirectivaReglaDTO DirectivaReglaDTO { get; set; }
		public UsuarioDTO UsuarioDTO { get; set; }
	}
}
