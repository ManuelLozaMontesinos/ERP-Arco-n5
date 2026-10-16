using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IDataImportBusiness
	{
		DataImportDTO GetById(int id);
		List<DataImportDTO> Search(DataImportDTO dto);
		DataImportDTO SingleSearch(DataImportDTO dto);
		int Insert(DataImportDTO dto);
		int Update(DataImportDTO dto);
		void Delete(DataImportDTO dto);
	}
}
