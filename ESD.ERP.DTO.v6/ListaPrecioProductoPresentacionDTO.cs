using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class ListaPrecioProductoPresentacionDTO : DTOBase
	{
		public override int Id => ListaPrecioProductoPresentacionId;
		public int ListaPrecioProductoPresentacionId { get; set; }
		public int ListaPrecioProductoId { get; set; }
		public int ProductoPresentacionVentaId { get; set; }
		public decimal PorcentajePrecioBase { get; set; }
		public int NumeroDecimalesRedondeo { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public ListaPrecioProductoDTO ListaPrecioProductoDTO { get; set; }
		public ProductoPresentacionVentaDTO ProductoPresentacionVentaDTO { get; set; }
	}
}
