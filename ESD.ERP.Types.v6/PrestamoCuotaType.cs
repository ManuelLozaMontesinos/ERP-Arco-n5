using System;

namespace ESD.ERP.Types
{
	public class PrestamoCuotaType
	{
		public int PrestamoCuotaId { get; set; }
		public int PrestamoPersonalId { get; set; }
		public int Cuota { get; set; }
		public decimal Monto { get; set; }
		public DateTime FechaVencimiento { get; set; }
		public int TipoCancelacionCuotaId { get; set; }
		public int EstadoCuotaId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
	}
}
