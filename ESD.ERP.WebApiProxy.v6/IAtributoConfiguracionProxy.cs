using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IAtributoConfiguracionProxy
	{
		AtributoConfiguracionDTO GetById(AtributoConfiguracionDTO dto);
		List<AtributoConfiguracionDTO> Search(AtributoConfiguracionDTO dto);
		AtributoConfiguracionDTO SingleSearch(AtributoConfiguracionDTO dto);
		int Insert(AtributoConfiguracionDTO dto);
		int Update(AtributoConfiguracionDTO dto);
		void Delete(AtributoConfiguracionDTO dto);
	}
}
