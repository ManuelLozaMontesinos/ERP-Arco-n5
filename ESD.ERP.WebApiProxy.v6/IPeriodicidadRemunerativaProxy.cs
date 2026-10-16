using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IPeriodicidadRemunerativaProxy
	{
		PeriodicidadRemunerativaDTO GetById(PeriodicidadRemunerativaDTO dto);
		List<PeriodicidadRemunerativaDTO> Search(PeriodicidadRemunerativaDTO dto);
		PeriodicidadRemunerativaDTO SingleSearch(PeriodicidadRemunerativaDTO dto);
		int Insert(PeriodicidadRemunerativaDTO dto);
		int Update(PeriodicidadRemunerativaDTO dto);
		void Delete(PeriodicidadRemunerativaDTO dto);
		PeriodicidadRemunerativaDTO ObtenerPorCodigo(PeriodicidadRemunerativaDTO dto);
	}
}
