using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IDistritoProxy
	{
		DistritoDTO GetById(DistritoDTO dto);
		List<DistritoDTO> Search(DistritoDTO dto);
		DistritoDTO SingleSearch(DistritoDTO dto);
		int Insert(DistritoDTO dto);
		int Update(DistritoDTO dto);
		void Delete(DistritoDTO dto);
	}
}
