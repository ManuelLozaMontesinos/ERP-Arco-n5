using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IMotivoTrasladoProxy
	{
		MotivoTrasladoDTO GetById(MotivoTrasladoDTO dto);
		List<MotivoTrasladoDTO> Search(MotivoTrasladoDTO dto);
		MotivoTrasladoDTO SingleSearch(MotivoTrasladoDTO dto);
		int Insert(MotivoTrasladoDTO dto);
		int Update(MotivoTrasladoDTO dto);
		void Delete(MotivoTrasladoDTO dto);
	}
}
