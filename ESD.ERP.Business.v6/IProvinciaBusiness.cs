using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IProvinciaBusiness
	{
		ProvinciaDTO GetById(int id);
		List<ProvinciaDTO> Search(ProvinciaDTO dto);
		ProvinciaDTO SingleSearch(ProvinciaDTO dto);
		int Insert(ProvinciaDTO dto);
		int Update(ProvinciaDTO dto);
		void Delete(ProvinciaDTO dto);
	}
}
