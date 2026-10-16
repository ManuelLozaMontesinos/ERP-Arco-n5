using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoModalidadFormativaBusiness
	{
		TipoModalidadFormativaDTO GetById(int id);
		List<TipoModalidadFormativaDTO> Search(TipoModalidadFormativaDTO dto);
		TipoModalidadFormativaDTO SingleSearch(TipoModalidadFormativaDTO dto);
		int Insert(TipoModalidadFormativaDTO dto);
		int Update(TipoModalidadFormativaDTO dto);
		void Delete(TipoModalidadFormativaDTO dto);
	}
}
