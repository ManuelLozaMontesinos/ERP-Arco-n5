using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class PrestamoCuotaDTO : DTOBase
	{
		public override int Id => PrestamoCuotaId;
		public int PrestamoCuotaId { get; set; }
		public int PrestamoPersonalId { get; set; }
		public int Cuota { get; set; }
		public decimal Monto { get; set; }
		public DateTime FechaVencimiento { get; set; }
		public DateTime? FechaCancelacion { get; set; }
		public int TipoCancelacionCuotaId { get; set; }
		public int EstadoCuotaId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public PrestamoPersonalDTO PrestamoPersonalDTO { get; set; }
		public TipoCancelacionCuotaDTO TipoCancelacionCuotaDTO { get; set; }
		public EstadoCuotaDTO EstadoCuotaDTO { get; set; }
	}
}
