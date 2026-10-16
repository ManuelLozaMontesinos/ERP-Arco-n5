using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class PersonalRemuneracionDTO : DTOBase
	{
		public override int Id => PersonalRemuneracionId;
		public int PersonalRemuneracionId { get; set; }
		public int PersonalId { get; set; }
		public int TipoRemuneracionId { get; set; }
		public int MonedaId { get; set; }
		public decimal Monto { get; set; }
		public DateTime FechaAlta { get; set; }
		public DateTime? FechaBaja { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public PersonalDTO PersonalDTO { get; set; }
		public MonedaDTO MonedaDTO { get; set; }
		public TipoRemuneracionDTO TipoRemuneracionDTO { get; set; }
	}
}
