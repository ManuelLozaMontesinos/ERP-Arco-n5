using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IProvinciaProxy
	{
		ProvinciaDTO GetById(ProvinciaDTO dto);
		List<ProvinciaDTO> Search(ProvinciaDTO dto);
		ProvinciaDTO SingleSearch(ProvinciaDTO dto);
		int Insert(ProvinciaDTO dto);
		int Update(ProvinciaDTO dto);
		void Delete(ProvinciaDTO dto);
	}
}
