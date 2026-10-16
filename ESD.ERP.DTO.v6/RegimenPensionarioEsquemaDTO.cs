using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class RegimenPensionarioEsquemaDTO : DTOBase
	{
		public override int Id => RegimenPensionarioEsquemaId;
		public int RegimenPensionarioEsquemaId { get; set; }
		public int RegimenPensionarioId { get; set; }
		public int TipoEsquemaPensionId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public RegimenPensionarioDTO RegimenPensionarioDTO { get; set; }
		public TipoEsquemaPensionDTO TipoEsquemaPensionDTO { get; set; }
	}
}
