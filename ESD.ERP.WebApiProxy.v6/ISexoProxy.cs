using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ISexoProxy
	{
		SexoDTO GetById(SexoDTO dto);
		List<SexoDTO> Search(SexoDTO dto);
		SexoDTO SingleSearch(SexoDTO dto);
		int Insert(SexoDTO dto);
		int Update(SexoDTO dto);
		void Delete(SexoDTO dto);
	}
}
