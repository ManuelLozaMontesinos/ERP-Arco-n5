using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class OrdenProduccionProductoEntregaDTO : DTOBase
	{
		public override int Id => OrdenProduccionProductoEntregaId;
		public int OrdenProduccionProductoEntregaId { get; set; }
		public int OrdenProduccionProductoId { get; set; }
		public decimal Cantidad { get; set; }
		public string EntregadoA { get; set; }
		public DateTime Fecha { get; set; }
		public string Observacion { get; set; }
		public int PersonalId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }

		public OrdenProduccionProductoDTO OrdenProduccionProductoDTO { get; set; }
		public PersonalDTO PersonalDTO { get; set; }
	}
}
