using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ICentroCostoProxy
	{
		CentroCostoDTO GetById(CentroCostoDTO dto);
		List<CentroCostoDTO> Search(CentroCostoDTO dto);
		CentroCostoDTO SingleSearch(CentroCostoDTO dto);
		int Insert(CentroCostoDTO dto);
		int Update(CentroCostoDTO dto);
		void Delete(CentroCostoDTO dto);
	}
}
