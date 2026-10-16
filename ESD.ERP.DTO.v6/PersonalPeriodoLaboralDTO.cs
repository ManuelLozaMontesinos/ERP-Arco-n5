using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class PersonalPeriodoLaboralDTO : DTOBase
	{
		public override int Id => PersonalPeriodoLaboralId;
		public int PersonalPeriodoLaboralId { get; set; }
		public int PersonalId { get; set; }
		public DateTime FechaIngreso { get; set; }
		public DateTime? FechaAlta { get; set; }
		public DateTime? FechaCese { get; set; }
		public int? MotivoFinPeriodoId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public PersonalDTO PersonalDTO { get; set; }
		public MotivoFinPeriodoDTO MotivoFinPeriodoDTO { get; set; }
	}
}
