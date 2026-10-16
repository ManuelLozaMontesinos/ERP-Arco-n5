using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class PersonalContratoDTO : DTOBase
	{
		public override int Id => PersonalContratoId;
		public int PersonalContratoId { get; set; }
		public int PersonalId { get; set; }
		public DateTime FechaSuscripcion { get; set; }
		public DateTime FechaInicio { get; set; }
		public DateTime? FechaFin { get; set; }
		public int? MotivoFinPeriodoId { get; set; }
		public int CargoPersonalId { get; set; }
		public int RegimenLaboralId { get; set; }
		public int TipoContratoId { get; set; }
		public string EsDiscapa { get; set; }
		public int MonedaId { get; set; }
		public decimal Monto { get; set; }
		public string Observa { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public PersonalDTO PersonalDTO { get; set; }
		public MotivoFinPeriodoDTO MotivoFinPeriodoDTO { get; set; }
		public CargoPersonalDTO CargoPersonalDTO { get; set; }
		public RegimenLaboralDTO RegimenLaboralDTO { get; set; }
		public TipoContratoDTO TipoContratoDTO { get; set; }
		public MonedaDTO MonedaDTO { get; set; }
	}
}
