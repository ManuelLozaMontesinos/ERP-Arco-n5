using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class VendedorDTO : DTOBase
	{
		public override int Id => VendedorId;
		public int VendedorId { get; set; }
		public string Codigo { get; set; }
		public int ProveedorId { get; set; }
		public string Abreviatura { get; set; }
		public string ServidorImpresion { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public ProveedorDTO ProveedorDTO { get; set; }
	}
}
