using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IParametroSistemaBusiness
	{
		ParametroSistemaDTO GetById(int id);
		List<ParametroSistemaDTO> Search(ParametroSistemaDTO dto);
		ParametroSistemaDTO SingleSearch(ParametroSistemaDTO dto);
		int Insert(ParametroSistemaDTO dto);
		int Update(ParametroSistemaDTO dto);
		void Delete(ParametroSistemaDTO dto);
	}
}
