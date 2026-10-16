using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IGrupoParametroSistemaProxy
	{
		GrupoParametroSistemaDTO GetById(GrupoParametroSistemaDTO dto);
		List<GrupoParametroSistemaDTO> Search(GrupoParametroSistemaDTO dto);
		GrupoParametroSistemaDTO SingleSearch(GrupoParametroSistemaDTO dto);
		int Insert(GrupoParametroSistemaDTO dto);
		int Update(GrupoParametroSistemaDTO dto);
		void Delete(GrupoParametroSistemaDTO dto);
	}
}
