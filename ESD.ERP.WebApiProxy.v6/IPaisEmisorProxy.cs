using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IPaisEmisorProxy
	{
		PaisEmisorDTO GetById(PaisEmisorDTO dto);
		List<PaisEmisorDTO> Search(PaisEmisorDTO dto);
		PaisEmisorDTO SingleSearch(PaisEmisorDTO dto);
		int Insert(PaisEmisorDTO dto);
		int Update(PaisEmisorDTO dto);
		void Delete(PaisEmisorDTO dto);
	}
}
