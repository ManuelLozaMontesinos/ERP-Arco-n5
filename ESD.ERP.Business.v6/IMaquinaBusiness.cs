using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IMaquinaBusiness
	{
		MaquinaDTO GetById(int id);
		List<MaquinaDTO> Search(MaquinaDTO dto);
		MaquinaDTO SingleSearch(MaquinaDTO dto);
		int Insert(MaquinaDTO dto);
		int Update(MaquinaDTO dto);
		void Delete(MaquinaDTO dto);
	}
}
