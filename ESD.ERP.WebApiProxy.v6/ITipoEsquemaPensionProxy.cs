using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoEsquemaPensionProxy
	{
		TipoEsquemaPensionDTO GetById(TipoEsquemaPensionDTO dto);
		List<TipoEsquemaPensionDTO> Search(TipoEsquemaPensionDTO dto);
		TipoEsquemaPensionDTO SingleSearch(TipoEsquemaPensionDTO dto);
		int Insert(TipoEsquemaPensionDTO dto);
		int Update(TipoEsquemaPensionDTO dto);
		void Delete(TipoEsquemaPensionDTO dto);
	}
}
