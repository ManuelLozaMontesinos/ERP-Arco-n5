using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IVendedorBusiness
	{
		VendedorDTO GetById(int id);
		List<VendedorDTO> Search(VendedorDTO dto);
		VendedorDTO SingleSearch(VendedorDTO dto);
		int Insert(VendedorDTO dto);
		int Update(VendedorDTO dto);
		void Delete(VendedorDTO dto);
	}
}
