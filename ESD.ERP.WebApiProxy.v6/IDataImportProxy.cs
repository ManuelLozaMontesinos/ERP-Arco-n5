using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IDataImportProxy
	{
		DataImportDTO GetById(DataImportDTO dto);
		List<DataImportDTO> Search(DataImportDTO dto);
		DataImportDTO SingleSearch(DataImportDTO dto);
		int Insert(DataImportDTO dto);
		int Update(DataImportDTO dto);
		void Delete(DataImportDTO dto);
	}
}
