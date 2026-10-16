using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IComprobanteTrazaProxy
	{
		ComprobanteTrazaDTO GetById(ComprobanteTrazaDTO dto);
		List<ComprobanteTrazaDTO> Search(ComprobanteTrazaDTO dto);
		ComprobanteTrazaDTO SingleSearch(ComprobanteTrazaDTO dto);
		int Insert(ComprobanteTrazaDTO dto);
		int Update(ComprobanteTrazaDTO dto);
		void Delete(ComprobanteTrazaDTO dto);
	}
}