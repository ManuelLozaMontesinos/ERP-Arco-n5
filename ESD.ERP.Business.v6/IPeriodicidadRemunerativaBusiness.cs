using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IPeriodicidadRemunerativaBusiness
	{
		PeriodicidadRemunerativaDTO GetById(int id);
		List<PeriodicidadRemunerativaDTO> Search(PeriodicidadRemunerativaDTO dto);
		PeriodicidadRemunerativaDTO SingleSearch(PeriodicidadRemunerativaDTO dto);
		int Insert(PeriodicidadRemunerativaDTO dto);
		int Update(PeriodicidadRemunerativaDTO dto);
		void Delete(PeriodicidadRemunerativaDTO dto);
		PeriodicidadRemunerativaDTO ObtenerPorCodigo(string codigo);
	}
}
