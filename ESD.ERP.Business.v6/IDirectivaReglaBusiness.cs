using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IDirectivaReglaBusiness
	{
		DirectivaReglaDTO GetById(int id);
		List<DirectivaReglaDTO> Search(DirectivaReglaDTO dto);
		DirectivaReglaDTO SingleSearch(DirectivaReglaDTO dto);
		int Insert(DirectivaReglaDTO dto);
		int Update(DirectivaReglaDTO dto);
		void Delete(DirectivaReglaDTO dto);
	}
}
