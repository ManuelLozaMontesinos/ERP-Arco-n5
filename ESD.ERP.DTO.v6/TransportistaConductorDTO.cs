using System;
using ESD.Core;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class TransportistaConductorDTO : DTOBase
	{
		public override int Id => TransportistaConductorId;
		public int TransportistaConductorId { get; set; }
		public int TransportistaId { get; set; }
		public string ApellidoPaterno { get; set; }
		public string ApellidoMaterno { get; set; }
		public string Nombres { get; set; }
		public string ApellidosNombres
      {
         get {
				return string.Concat(
					(string.IsNullOrEmpty(ApellidoPaterno) ? string.Empty : ApellidoPaterno),
					(string.IsNullOrEmpty(ApellidoMaterno) ? string.Empty : string.Concat(SeparadorConstante.ESPACIO, ApellidoMaterno)),
					(string.IsNullOrEmpty(Nombres) ? string.Empty : string.Concat(SeparadorConstante.ESPACIO, Nombres))
					);
			}
      }
		public int TipoDocumentoIdentidadId { get; set; }
		public string NumeroDocumentoIdentidad { get; set; }
		public string NumeroLicencia { get; set; }
		public string Telefono { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public TransportistaDTO TransportistaDTO { get; set; }
		public TipoDocumentoIdentidadDTO TipoDocumentoIdentidadDTO { get; set; }
	}
}
