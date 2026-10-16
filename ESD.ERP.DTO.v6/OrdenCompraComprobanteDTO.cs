using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class OrdenCompraComprobanteDTO : DTOBase
	{
		public override int Id => OrdenCompraComprobanteId;
		public int OrdenCompraComprobanteId { get; set; }
		public int OrdenCompraId { get; set; }
		public int TipoDocumentoComercialId { get; set; }
		public string Serie { get; set; }
		public string Numero { get; set; }
		public DateTime Fecha { get; set; }
		public DateTime? FechaVencimiento { get; set; }
		public decimal MontoBruto { get; set; }
		public decimal MontoImpuesto { get; set; }
		public decimal MontoTotal { get; set; }
		public int? CondicionPagoId { get; set; }
		public int? RegistroReferencialId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public OrdenCompraDTO OrdenCompraDTO { get; set; }
		public TipoDocumentoComercialDTO TipoDocumentoComercialDTO { get; set; }
		public CondicionPagoDTO CondicionPagoDTO { get; set; }
	}
}
