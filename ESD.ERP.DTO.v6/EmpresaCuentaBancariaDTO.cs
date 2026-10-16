using System;
using ESD.Core;
using ESD.Core.Service.DTO;

namespace ESD.ERP.DTO
{
	public class EmpresaCuentaBancariaDTO : DTOBase
	{
		public override int Id => EmpresaCuentaBancariaId;
		public int EmpresaCuentaBancariaId { get; set; }
		public int EmpresaId { get; set; }
		public int BancoId { get; set; }
		public string Nombre { get; set; }
		public string BancoNumero {
			get {
				if (BancoDTO == null)
					return string.Empty;

				return string.Concat(BancoDTO.Nombre, SeparadorConstante.GUION, Cuenta); 
			}
		}
		public int MonedaId { get; set; }
		public string Cuenta { get; set; }
		public string CuentaInterbancaria { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public EmpresaDTO EmpresaDTO { get; set; }
		public BancoDTO BancoDTO { get; set; }
		public MonedaDTO MonedaDTO { get; set; }
	}
}
