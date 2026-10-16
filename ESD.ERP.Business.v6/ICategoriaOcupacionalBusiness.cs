using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ICategoriaOcupacionalBusiness
	{
		CategoriaOcupacionalDTO GetById(int id);
		List<CategoriaOcupacionalDTO> Search(CategoriaOcupacionalDTO dto);
		CategoriaOcupacionalDTO SingleSearch(CategoriaOcupacionalDTO dto);
		int Insert(CategoriaOcupacionalDTO dto);
		int Update(CategoriaOcupacionalDTO dto);
		void Delete(CategoriaOcupacionalDTO dto);
	}
}
