using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IGrupoParametroSistemaBusiness
	{
		GrupoParametroSistemaDTO GetById(int id);
		List<GrupoParametroSistemaDTO> Search(GrupoParametroSistemaDTO dto);
		GrupoParametroSistemaDTO SingleSearch(GrupoParametroSistemaDTO dto);
		int Insert(GrupoParametroSistemaDTO dto);
		int Update(GrupoParametroSistemaDTO dto);
		void Delete(GrupoParametroSistemaDTO dto);
	}
}
