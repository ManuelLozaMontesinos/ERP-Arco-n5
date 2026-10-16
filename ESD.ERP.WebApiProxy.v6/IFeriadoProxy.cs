using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IFeriadoProxy
	{
		FeriadoDTO GetById(FeriadoDTO dto);
		List<FeriadoDTO> Search(FeriadoDTO dto);
		FeriadoDTO SingleSearch(FeriadoDTO dto);
		int Insert(FeriadoDTO dto);
		int Update(FeriadoDTO dto);
		void Delete(FeriadoDTO dto);
	}
}
