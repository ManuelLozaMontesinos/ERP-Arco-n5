using System;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class CondicionPagoDTO : DTOBase
	{
		public override int Id => CondicionPagoId;
		public int CondicionPagoId { get; set; }
		public int EmpresaId { get; set; }
		public string Codigo { get; set; }
		public string Nombre { get; set; }
		public int TipoCondicionPagoId { get; set; }
		public int Dia { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificaId { get; set; }
		public DateTime? FechaModifica { get; set; }
		public EmpresaDTO EmpresaDTO { get; set; }
		public TipoCondicionPagoDTO TipoCondicionPagoDTO { get; set; }
	}
}
