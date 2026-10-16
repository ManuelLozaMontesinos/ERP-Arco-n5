using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class UsuarioDTO : DTOBase
	{
		public override int Id => UsuarioId;
		public int UsuarioId { get; set; }
		public int? EmpresaId { get; set; }
		public string UsuarioLogon { get; set; }
		public string NombreCompleto { get; set; }
		public string Password { get; set; }
		public byte[] ClavePublica { get; set; }
		public string CambiarContrasenaProximo { get; set; }
		public string NoPuedeCambiarContrasena { get; set; }
		public string NoExpira { get; set; }
		public DateTime? FechaExpiracion { get; set; }
		public string Email { get; set; }
		public string EmailAlterno { get; set; }
		public string CodigoRecuperacionContrasena { get; set; }
		public string TieneHorario { get; set; }
		public int OrigenRegistroId { get; set; }
		public int? ProveedorId { get; set; }
		public string Estado { get; set; }

		public ProveedorDTO ProveedorDTO { get; set; }
		public OrigenRegistroDTO OrigenRegistroDTO { get; set; }

		public string ContrasenaAnterior { get; set; }

		public string CodigoHorarioExtendido { get; set; }
		public string MensajeHorarioExtendido { get; set; }
	}
}
