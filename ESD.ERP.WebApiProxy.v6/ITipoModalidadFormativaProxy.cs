using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoModalidadFormativaProxy
	{
		TipoModalidadFormativaDTO GetById(TipoModalidadFormativaDTO dto);
		List<TipoModalidadFormativaDTO> Search(TipoModalidadFormativaDTO dto);
		TipoModalidadFormativaDTO SingleSearch(TipoModalidadFormativaDTO dto);
		int Insert(TipoModalidadFormativaDTO dto);
		int Update(TipoModalidadFormativaDTO dto);
		void Delete(TipoModalidadFormativaDTO dto);
	}
}
