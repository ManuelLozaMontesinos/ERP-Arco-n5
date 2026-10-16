using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IDirectivaReglaProxy
	{
		DirectivaReglaDTO GetById(DirectivaReglaDTO dto);
		List<DirectivaReglaDTO> Search(DirectivaReglaDTO dto);
		DirectivaReglaDTO SingleSearch(DirectivaReglaDTO dto);
		int Insert(DirectivaReglaDTO dto);
		int Update(DirectivaReglaDTO dto);
		void Delete(DirectivaReglaDTO dto);
	}
}
