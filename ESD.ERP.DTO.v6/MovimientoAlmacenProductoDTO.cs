using System;
using System.Collections.Generic;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class MovimientoAlmacenProductoDTO : DTOBase
	{
		public override int Id => MovimientoAlmacenProductoId;
		public int MovimientoAlmacenProductoId { get; set; }
		public int MovimientoAlmacenId { get; set; }
		public int ProductoId { get; set; }
		public string Descripcion { get; set; }
		public decimal CantidadDocumento { get; set; }
		public decimal PesoNetoDocumento { get; set; }
		public decimal PesoBrutoDocumento { get; set; }
		public decimal Cantidad { get; set; }
		public decimal PesoNeto { get; set; }
		public decimal PesoBruto { get; set; }
		public decimal ValorUnitario { get; set; }
		public int? ProductoReclasificacionId { get; set; }
		public decimal CantidadReclasificacion { get; set; }
		public int? RegistroReferencialId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public MovimientoAlmacenDTO MovimientoAlmacenDTO { get; set; }
		public ProductoDTO ProductoDTO { get; set; }
		public UsuarioDTO UsuarioDTO { get; set; }
		public ProductoDTO ProductoReclasificacionDTO { get; set; }

		public string ConCantidadReclasificacion { get; set; }
		public List<int> OrdenCompraProductoIdList { get; set; }
	}
}
