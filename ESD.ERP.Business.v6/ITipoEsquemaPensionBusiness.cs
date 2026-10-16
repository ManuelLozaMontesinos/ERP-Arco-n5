using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoEsquemaPensionBusiness
	{
		TipoEsquemaPensionDTO GetById(int id);
		List<TipoEsquemaPensionDTO> Search(TipoEsquemaPensionDTO dto);
		TipoEsquemaPensionDTO SingleSearch(TipoEsquemaPensionDTO dto);
		int Insert(TipoEsquemaPensionDTO dto);
		int Update(TipoEsquemaPensionDTO dto);
		void Delete(TipoEsquemaPensionDTO dto);
	}
}
