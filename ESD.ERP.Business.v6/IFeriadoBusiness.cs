using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IFeriadoBusiness
	{
		FeriadoDTO GetById(int id);
		List<FeriadoDTO> Search(FeriadoDTO dto);
		FeriadoDTO SingleSearch(FeriadoDTO dto);
		int Insert(FeriadoDTO dto);
		int Update(FeriadoDTO dto);
		void Delete(FeriadoDTO dto);
	}
}
