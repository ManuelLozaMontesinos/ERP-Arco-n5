using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IMotivoTrasladoBusiness
	{
		MotivoTrasladoDTO GetById(int id);
		List<MotivoTrasladoDTO> Search(MotivoTrasladoDTO dto);
		MotivoTrasladoDTO SingleSearch(MotivoTrasladoDTO dto);
		int Insert(MotivoTrasladoDTO dto);
		int Update(MotivoTrasladoDTO dto);
		void Delete(MotivoTrasladoDTO dto);
	}
}
