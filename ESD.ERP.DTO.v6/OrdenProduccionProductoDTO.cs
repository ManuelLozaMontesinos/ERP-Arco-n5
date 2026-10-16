using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class OrdenProduccionProductoDTO : DTOBase
	{
		public override int Id => OrdenProduccionProductoId;
		public int OrdenProduccionProductoId { get; set; }
		public int OrdenProduccionId { get; set; }
		public int ProductoId { get; set; }
		public string Descripcion { get; set; }
		public decimal Cantidad { get; set; }
		public decimal CantidadEntregada { get; set; }
		public DateTime? FechaEntrega { get; set; }
		public string EntregadoA { get; set; }
		public int? RegistroReferencialId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public int? EstadoDespachoId { get; set; }
        public int? PersonalDespachoId { get; set; }
        public string ObservacionDespacho { get; set; }
		public DateTime? FechaDespacho { get; set; }

        public OrdenProduccionDTO OrdenProduccionDTO { get; set; }
		public ProductoDTO ProductoDTO { get; set; }
        public EstadoDespachoDTO EstadoDespachoDTO { get; set; }
		public PersonalDTO PersonalDTO { get; set; }
    }
}
