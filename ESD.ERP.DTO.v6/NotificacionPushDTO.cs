using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class NotificacionPushDTO : DTOBase
	{
		public override int Id => NotificacionPushId;
		public int NotificacionPushId { get; set; }
		public DateTime? Fecha { get; set; }
		public DateTime FechaRegistro { get; set; }
		public string Titulo { get; set; }
		public string Descripcion { get; set; }
		public string Lectura { get; set; }
		public int UsuarioId { get; set; }
	}
}
