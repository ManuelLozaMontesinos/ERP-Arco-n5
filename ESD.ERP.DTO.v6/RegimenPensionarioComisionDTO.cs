using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class RegimenPensionarioComisionDTO : DTOBase
	{
		public override int Id => RegimenPensionarioComisionId;
		public int RegimenPensionarioEsquemaId { get; set; }
		public int RegimenPensionarioComisionId { get; set; }
		public int TipoAporteComisionId { get; set; }
		public decimal Porcentaje { get; set; }
		public DateTime FechaAlta { get; set; }
		public DateTime? FechaBaja { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public RegimenPensionarioEsquemaDTO RegimenPensionarioEsquemaDTO { get; set; }
		public TipoAporteComisionDTO TipoAporteComisionDTO { get; set; }
	}
}
