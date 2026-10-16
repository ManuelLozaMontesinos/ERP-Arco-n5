using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class TipoAutorizacionModuloDTO : DTOBase
	{
		public override int Id => TipoAutorizacionModuloId;
		public int TipoAutorizacionModuloId { get; set; }
		public int OpcionModuloId { get; set; }
		public int TipoAutorizacionId { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public OpcionModuloDTO OpcionModuloDTO { get; set; }
		public TipoAutorizacionDTO TipoAutorizacionDTO { get; set; }
	}
}
