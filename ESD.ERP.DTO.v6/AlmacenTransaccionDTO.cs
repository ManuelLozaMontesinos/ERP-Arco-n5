using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class AlmacenTransaccionDTO : DTOBase
	{
		public override int Id => AlmacenTransaccionId;
		public int AlmacenTransaccionId { get; set; }
		public int AlmacenId { get; set; }
		public int TransaccionAlmacenId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public AlmacenDTO AlmacenDTO { get; set; }
		public TransaccionAlmacenDTO TransaccionAlmacenDTO { get; set; }
	}
}
