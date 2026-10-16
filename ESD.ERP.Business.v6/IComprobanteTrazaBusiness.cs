using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IComprobanteTrazaBusiness
	{
		ComprobanteTrazaDTO GetById(int id);
		List<ComprobanteTrazaDTO> Search(ComprobanteTrazaDTO dto);
		ComprobanteTrazaDTO SingleSearch(ComprobanteTrazaDTO dto);
		int Insert(ComprobanteTrazaDTO dto);
		int Update(ComprobanteTrazaDTO dto);
		void Delete(ComprobanteTrazaDTO dto);
	}
}
