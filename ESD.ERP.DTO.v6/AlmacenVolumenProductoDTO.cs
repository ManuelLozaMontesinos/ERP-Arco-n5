using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class AlmacenVolumenProductoDTO : DTOBase
	{
		public override int Id => AlmacenVolumenProductoId;
		public int AlmacenVolumenProductoId { get; set; }
		public int EmpresaSedeId { get; set; }
		public int ProductoId { get; set; }
		public decimal StockMinimo { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public EmpresaSedeDTO EmpresaSedeDTO { get; set; }
		public ProductoDTO ProductoDTO { get; set; }
		public ProveedorDTO ProveedorDTO { get; set; }

		public int PeriodoEmpresaSedeId { get; set; }
	}
}
