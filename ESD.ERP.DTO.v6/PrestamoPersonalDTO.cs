using System;
using System.Collections.Generic;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class PrestamoPersonalDTO : DTOBase
	{
		public override int Id => PrestamoPersonalId;
		public int PrestamoPersonalId { get; set; }
		public string Codigo { get; set; }
		public int PersonalId { get; set; }
		public DateTime Fecha { get; set; }
		public int MonedaId { get; set; }
		public decimal MontoTipoCambio { get; set; }
		public decimal Monto { get; set; }
		public int CantidadCuotas { get; set; }
		public DateTime FechaInicio { get; set; }
		public int DiasCuota { get; set; }
		public int TipoPrestamoId { get; set; }
		public int EstadoPrestamoId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public PersonalDTO PersonalDTO { get; set; }
		public MonedaDTO MonedaDTO { get; set; }
		public TipoPrestamoDTO TipoPrestamoDTO { get; set; }

		public int PeriodoId { get; set; }
		public List<PrestamoCuotaDTO> PrestamoCuotaDTOList { get; set; }
		public EstadoPrestamoDTO EstadoPrestamoDTO { get; set; }
	}
}
