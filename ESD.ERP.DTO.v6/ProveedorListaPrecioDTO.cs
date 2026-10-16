using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class ProveedorListaPrecioDTO : DTOBase
	{
		public override int Id => ProveedorListaPrecioId;
		public int ProveedorListaPrecioId { get; set; }
		public int ProveedorId { get; set; }
		public int ImpuestoTasaId { get; set; }
		public int MonedaId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public ProveedorDTO ProveedorDTO { get; set; }
		public ImpuestoTasaDTO ImpuestoTasaDTO { get; set; }
		public MonedaDTO MonedaDTO { get; set; }
	}
}
