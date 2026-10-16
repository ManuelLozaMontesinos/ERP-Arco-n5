using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class ComprobanteCobranzaDTO : DTOBase
	{
		public override int Id => ComprobanteCobranzaId;
		public int ComprobanteCobranzaId { get; set; }
		public int ComprobanteId { get; set; }
		public DateTime Fecha { get; set; }
		public string NumeroOperacion { get; set; }
		public decimal Monto { get; set; }
		public int EmpresaCuentaBancariaId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificaId { get; set; }
		public DateTime? FechaModifica { get; set; }
		public ComprobanteDTO ComprobanteDTO { get; set; }
		public EmpresaCuentaBancariaDTO EmpresaCuentaBancariaDTO { get; set; }
	}
}
