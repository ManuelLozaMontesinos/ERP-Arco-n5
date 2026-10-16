using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class OcupacionDTO : DTOBase
	{
		public override int Id => OcupacionId;
		public int OcupacionId { get; set; }
		public string Codigo { get; set; }
		public string Nombre { get; set; }
		public string EsEjecutivo { get; set; }
		public string EsEmpleado { get; set; }
		public string EsObrero { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
	}
}
