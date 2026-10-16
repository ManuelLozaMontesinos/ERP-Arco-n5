using System;
using ESD.Core;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class ListaPrecioDTO : DTOBase
	{
		public override int Id => ListaPrecioId;
		public int ListaPrecioId { get; set; }
		public int EmpresaSedeId { get; set; }
		public string Nombre { get; set; }
		public string NombreMoneda {
         get {
				return string.Concat(Nombre,
					(MonedaDTO != null ? 
						string.Concat(SeparadorConstante.ESPACIO,"(",MonedaDTO.CodigoIso,")") 
						: string.Empty)
					);
			} 
		}
		public int ImpuestoTasaId { get; set; }
		public int MonedaId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public EmpresaSedeDTO EmpresaSedeDTO { get; set; }
		public ImpuestoTasaDTO ImpuestoTasaDTO { get; set; }
		public MonedaDTO MonedaDTO { get; set; }
		public ListaPrecioVendedorDTO listaPrecioVendedorDTO { get; set; }
	}
}
