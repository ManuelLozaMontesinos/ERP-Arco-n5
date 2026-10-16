using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IParametroSistemaProxy
	{
		ParametroSistemaDTO GetById(ParametroSistemaDTO dto);
		List<ParametroSistemaDTO> Search(ParametroSistemaDTO dto);
		ParametroSistemaDTO SingleSearch(ParametroSistemaDTO dto);
		int Insert(ParametroSistemaDTO dto);
		int Update(ParametroSistemaDTO dto);
		void Delete(ParametroSistemaDTO dto);
	}
}
