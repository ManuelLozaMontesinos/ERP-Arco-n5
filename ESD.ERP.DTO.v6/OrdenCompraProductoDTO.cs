using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class OrdenCompraProductoDTO : DTOBase
	{
		public override int Id => OrdenCompraProductoId;
		public int OrdenCompraProductoId { get; set; }
		public int OrdenCompraId { get; set; }
		public int ProductoId { get; set; }
		public string Descripcion { get; set; }
		public decimal Cantidad { get; set; }
		public decimal CantidadPendiente
		{
			get { return Cantidad - CantidadIngresada; }
		}
		public decimal CantidadIngresada { get; set; }
		public decimal ValorUnitario { get; set; }
		public decimal PrecioUnitario { get; set; }
		public decimal MontoBruto { get; set; }
		public decimal MontoImpuesto { get; set; }
		public decimal MontoTotal { get; set; }
		public string EsAdicional { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public OrdenCompraDTO OrdenCompraDTO { get; set; }
		public ProductoDTO ProductoDTO { get; set; }
		public TipoAfectacionIgvDTO TipoAfectacionIgvDTO { get; set; }
		public string ConCantidadPendiente { get; set; }

	}
}
