using System;
using System.Collections.Generic;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class CotizacionAutorizacionDTO : DTOBase
	{
		public override int Id => CotizacionAutorizacionId;
		public int CotizacionAutorizacionId { get; set; }
		public int TipoAutorizacionId { get; set; }
		public int CotizacionId { get; set; }
		public int? UsuarioId { get; set; }
		public DateTime? FechaAutorizacion { get; set; }
		public int EstadoAprobacionId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public TipoAutorizacionDTO TipoAutorizacionDTO { get; set; }
		public CotizacionDTO CotizacionDTO { get; set; }
		public UsuarioDTO UsuarioDTO { get; set; }
		public List<int> TipoAutorizacionIdList { get; set; }
		public List<CotizacionAutorizacionDTO> CotizacionAutorizacionDTOList { get; set; }
		public List<CotizacionProductoDTO> CotizacionProductoDTOList { get; set; }
		public EstadoAprobacionDTO EstadoAprobacionDTO { get; set; }
	}
}
