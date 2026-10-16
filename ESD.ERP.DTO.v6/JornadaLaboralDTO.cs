using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class JornadaLaboralDTO : DTOBase
	{
		public override int Id => JornadaLaboralId;
		public int JornadaLaboralId { get; set; }
		public int EmpresaId { get; set; }
		public string Nombre { get; set; }
		public string HoraIngreso { get; set; }
		public decimal CantidadHoras { get; set; }
		public int DiaId { get; set; }
		public int DiaHastaId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public EmpresaDTO EmpresaDTO { get; set; }
		public DiaDTO DiaDTO { get; set; }
		public DiaDTO DiaHastaDTO { get; set; }
	}
}
