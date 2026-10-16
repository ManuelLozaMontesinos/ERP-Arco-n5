using System;
using System.Collections.Generic;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class ComprobanteAutorizacionDTO : DTOBase
	{
		public override int Id => ComprobanteAutorizacionId;
		public int ComprobanteAutorizacionId { get; set; }
		public int TipoAutorizacionId { get; set; }
		public int ComprobanteId { get; set; }
		public int? UsuarioId { get; set; }
		public DateTime? FechaAutorizacion { get; set; }
		public int EstadoAprobacionId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public TipoAutorizacionDTO TipoAutorizacionDTO { get; set; }
		public ComprobanteDTO ComprobanteDTO { get; set; }
		public UsuarioDTO UsuarioDTO { get; set; }
		public List<int> TipoAutorizacionIdList { get; set; }
		public List<ComprobanteAutorizacionDTO> ComprobanteAutorizacionDTOList { get; set; }
		public List<ComprobanteProductoDTO> ComprobanteProductoDTOList { get; set; }
		public EstadoAprobacionDTO EstadoAprobacionDTO { get; set; }
	}
}
