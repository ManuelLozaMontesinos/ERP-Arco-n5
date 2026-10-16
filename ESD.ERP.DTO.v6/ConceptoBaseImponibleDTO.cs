using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class ConceptoBaseImponibleDTO : DTOBase
	{
		public override int Id => ConceptoBaseImponibleId;
		public int ConceptoBaseImponibleId { get; set; }
		public int TipoBaseImponibleId { get; set; }
		public int ConceptoRemunerativoId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public TipoBaseImponibleDTO TipoBaseImponibleDTO { get; set; }
		public ConceptoRemunerativoDTO ConceptoRemunerativoDTO { get; set; }
	}
}
