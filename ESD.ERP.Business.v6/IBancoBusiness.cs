using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IBancoBusiness
	{
		BancoDTO GetById(int id);
		List<BancoDTO> Search(BancoDTO dto);
		BancoDTO SingleSearch(BancoDTO dto);
		int Insert(BancoDTO dto);
		int Update(BancoDTO dto);
		void Delete(BancoDTO dto);
	}
}
