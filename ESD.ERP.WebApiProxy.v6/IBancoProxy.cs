using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IBancoProxy
	{
		BancoDTO GetById(BancoDTO dto);
		List<BancoDTO> Search(BancoDTO dto);
		BancoDTO SingleSearch(BancoDTO dto);
		int Insert(BancoDTO dto);
		int Update(BancoDTO dto);
		void Delete(BancoDTO dto);
	}
}
