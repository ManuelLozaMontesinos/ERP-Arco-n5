using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class EstadoOrdenProduccionSubProcesoDTO : DTOBase
	{
		public override int Id => EstadoOrdenProduccionSubProcesoId;
		public int EstadoOrdenProduccionSubProcesoId { get; set; }
		public string Codigo { get; set; }
		public string Nombre { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
	}
}
