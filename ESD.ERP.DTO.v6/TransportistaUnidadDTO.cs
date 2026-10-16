using System;
using ESD.Core;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class TransportistaUnidadDTO : DTOBase
	{
		public override int Id => TransportistaUnidadId;
		public int TransportistaUnidadId { get; set; }
		public int TransportistaId { get; set; }
		public string Marca { get; set; }
		public string Modelo { get; set; }
		public string Placa { get; set; }
		public string Placa2 { get; set; }
		public string MarcaModeloPlata
      {
         get { return string.Concat(Marca,
				(string.IsNullOrEmpty(Modelo) ? string.Empty : string.Concat(SeparadorConstante.ESPACIO, SeparadorConstante.GUION, SeparadorConstante.ESPACIO,Modelo)),
				(string.IsNullOrEmpty(Placa) ? string.Empty : string.Concat(SeparadorConstante.ESPACIO, SeparadorConstante.GUION, SeparadorConstante.ESPACIO, Placa)),
				(string.IsNullOrEmpty(Placa2) ? string.Empty : string.Concat(SeparadorConstante.ESPACIO, SeparadorConstante.GUION, SeparadorConstante.ESPACIO, Placa2))
				);
			}
      }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public TransportistaDTO TransportistaDTO { get; set; }
	}
}
