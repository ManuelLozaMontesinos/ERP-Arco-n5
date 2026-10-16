using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class ListaPrecioProductoDTO : DTOBase
	{
		public override int Id => ListaPrecioProductoId;
		public int ListaPrecioProductoId { get; set; }
		public int ListaPrecioId { get; set; }
		public int ProductoId { get; set; }
		public decimal ValorUnitario { get; set; }
		public decimal PrecioUnitario { get; set; }
		public string PermiteDescuento { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public ListaPrecioDTO ListaPrecioDTO { get; set; }
		public ProductoDTO ProductoDTO { get; set; }
		public TipoDescuentoDTO TipoDescuentoDTO { get; set; }
		public int MesContableId { get; set; }
		public int PeriodoId { get; set; }
	}
}
