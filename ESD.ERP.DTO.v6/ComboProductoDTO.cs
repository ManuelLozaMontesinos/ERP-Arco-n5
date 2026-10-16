using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class ComboProductoDTO : DTOBase
	{
		public override int Id => ComboProductoId;
		public int ComboProductoId { get; set; }
		public int ProductoId { get; set; }
		public int ProductoIncluidoId { get; set; }
		public decimal Cantidad { get; set; }
		public decimal PorcentajePrecio { get; set; }
		public int NumeroDecimalesRedondeo { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public ProductoDTO Producto { get; set; }
		public ProductoDTO ProductoIncluidoDTO { get; set; }
	}
}
