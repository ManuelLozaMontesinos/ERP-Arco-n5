using System;
using System.Collections.Generic;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class ProveedorListaPrecioProductoDTO : DTOBase
	{
		public override int Id => ProveedorListaPrecioProductoId;
		public int ProveedorListaPrecioProductoId { get; set; }
		public int ProveedorListaPrecioId { get; set; }
		public int ProductoId { get; set; }
		public decimal ValorUnitarioAnterior { get; set; }
		public decimal PrecioUnitarioAnterior { get; set; }
		public DateTime? FechaAnterior { get; set; }
		public decimal ValorUnitario { get; set; }
		public decimal PrecioUnitario { get; set; }
		public DateTime FechaVigente { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public ProveedorListaPrecioDTO ProveedorListaPrecioDTO { get; set; }
		public ProductoDTO ProductoDTO { get; set; }
		public List<int> ProductoIdList { get; set; }
	}
}
