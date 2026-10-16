using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class RemuneracionMinimaVitalDTO : DTOBase
	{
		public override int Id => RemuneracionMinimaVitalId;
		public int RemuneracionMinimaVitalId { get; set; }
		public decimal Monto { get; set; }
		public DateTime FechaAlta { get; set; }
		public DateTime? FechaBaja { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificaId { get; set; }
		public DateTime? FechaModifica { get; set; }
	}
}
