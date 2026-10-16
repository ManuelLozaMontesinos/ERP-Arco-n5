using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IVendedorProxy
	{
		VendedorDTO GetById(VendedorDTO dto);
		List<VendedorDTO> Search(VendedorDTO dto);
		VendedorDTO SingleSearch(VendedorDTO dto);
		int Insert(VendedorDTO dto);
		int Update(VendedorDTO dto);
		void Delete(VendedorDTO dto);
	}
}
