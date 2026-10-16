using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ICategoriaOcupacionalProxy
	{
		CategoriaOcupacionalDTO GetById(CategoriaOcupacionalDTO dto);
		List<CategoriaOcupacionalDTO> Search(CategoriaOcupacionalDTO dto);
		CategoriaOcupacionalDTO SingleSearch(CategoriaOcupacionalDTO dto);
		int Insert(CategoriaOcupacionalDTO dto);
		int Update(CategoriaOcupacionalDTO dto);
		void Delete(CategoriaOcupacionalDTO dto);
	}
}
