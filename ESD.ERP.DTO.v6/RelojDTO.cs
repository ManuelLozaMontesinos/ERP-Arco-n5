using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class RelojDTO : DTOBase
	{
		public override int Id => RelojId;
		public int RelojId { get; set; }
		public int EmpresaId { get; set; }
		public string Nombre { get; set; }
		public string RutaArchivo { get; set; }
		public string EsCentral { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificackion { get; set; }
		public EmpresaDTO EmpresaDTO { get; set; }
	}
}
