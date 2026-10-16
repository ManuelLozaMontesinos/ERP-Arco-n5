using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IAtributoConfiguracionBusiness
	{
		AtributoConfiguracionDTO GetById(int id);
		List<AtributoConfiguracionDTO> Search(AtributoConfiguracionDTO dto);
		AtributoConfiguracionDTO SingleSearch(AtributoConfiguracionDTO dto);
		int Insert(AtributoConfiguracionDTO dto);
		int Update(AtributoConfiguracionDTO dto);
		void Delete(AtributoConfiguracionDTO dto);
	}
}
