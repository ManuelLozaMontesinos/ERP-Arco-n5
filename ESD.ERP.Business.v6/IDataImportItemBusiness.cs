using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IDataImportItemBusiness
	{
		DataImportItemDTO GetById(int id);
		List<DataImportItemDTO> Search(DataImportItemDTO dto);
		DataImportItemDTO SingleSearch(DataImportItemDTO dto);
		int Insert(DataImportItemDTO dto);
		int Update(DataImportItemDTO dto);
		void Delete(DataImportItemDTO dto);
	}
}
