using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IDataImportItemProxy
	{
		DataImportItemDTO GetById(DataImportItemDTO dto);
		List<DataImportItemDTO> Search(DataImportItemDTO dto);
		DataImportItemDTO SingleSearch(DataImportItemDTO dto);
		int Insert(DataImportItemDTO dto);
		int Update(DataImportItemDTO dto);
		void Delete(DataImportItemDTO dto);
	}
}
