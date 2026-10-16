using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class OrdenCompraEntregaDTO : DTOBase
	{
		public override int Id => OrdenCompraEntregaId;
		public int OrdenCompraEntregaId { get; set; }
		public int OrdenCompraProductoId { get; set; }
		public decimal Cantidad { get; set; }
		public DateTime? Fecha { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public OrdenCompraProductoDTO OrdenCompraProductoDTO { get; set; }
	}
}
