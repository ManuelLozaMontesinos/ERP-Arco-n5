using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ISexoBusiness
	{
		SexoDTO GetById(int id);
		List<SexoDTO> Search(SexoDTO dto);
		SexoDTO SingleSearch(SexoDTO dto);
		int Insert(SexoDTO dto);
		int Update(SexoDTO dto);
		void Delete(SexoDTO dto);
	}
}
