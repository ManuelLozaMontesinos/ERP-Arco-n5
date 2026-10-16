using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IMaquinaProxy
	{
		MaquinaDTO GetById(MaquinaDTO dto);
		List<MaquinaDTO> Search(MaquinaDTO dto);
		MaquinaDTO SingleSearch(MaquinaDTO dto);
		int Insert(MaquinaDTO dto);
		int Update(MaquinaDTO dto);
		void Delete(MaquinaDTO dto);
	}
}
