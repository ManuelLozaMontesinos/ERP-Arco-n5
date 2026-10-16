using ESD.Core.Service.DTO;
using System;
using System.Collections.Generic;

namespace ESD.ERP.DTO
{
	public class ListaPrecioVendedorDTO : DTOBase
	{
		public override int Id => ListaPrecioVendedorId;
		public int ListaPrecioVendedorId { get; set; }
		public int ListaPrecioId { get; set; }
		public int VendedorId { get; set; }
		public string Estado { get; set; }
		public int UsuarioCreacionId { get; set; }
		public DateTime FechaCreacion { get; set; }
		public int? UsuarioModificacionId { get; set; }
		public DateTime? FechaModificacion { get; set; }
		public ListaPrecioDTO ListaPrecioDTO { get; set; }
		public VendedorDTO VendedorDTO{ get; set; }
        public List<int> ListaPrecioIdList { get; set; }
    }
}
