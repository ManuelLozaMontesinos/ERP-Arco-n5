using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IPaisEmisorBusiness
	{
		PaisEmisorDTO GetById(int id);
		List<PaisEmisorDTO> Search(PaisEmisorDTO dto);
		PaisEmisorDTO SingleSearch(PaisEmisorDTO dto);
		int Insert(PaisEmisorDTO dto);
		int Update(PaisEmisorDTO dto);
		void Delete(PaisEmisorDTO dto);
	}
}
