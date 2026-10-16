using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ICentroCostoBusiness
	{
		CentroCostoDTO GetById(int id);
		List<CentroCostoDTO> Search(CentroCostoDTO dto);
		CentroCostoDTO SingleSearch(CentroCostoDTO dto);
		int Insert(CentroCostoDTO dto);
		int Update(CentroCostoDTO dto);
		void Delete(CentroCostoDTO dto);
	}
}
