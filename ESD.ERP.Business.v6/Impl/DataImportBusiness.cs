using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class DataImportBusiness : IDataImportBusiness
	{
		private readonly IDataImportRepository dataImportRepository;

		public DataImportBusiness(
			IDataImportRepository dataImportRepository
			)
		{
			this.dataImportRepository = dataImportRepository;
		}
		public DataImportDTO GetById(int id)
		{
			try
			{
				DataImport dataImport = dataImportRepository.GetById(id);

				DataImportDTO dataImportDTO = Mapper.Map<DataImportDTO>(dataImport);

				return dataImportDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<DataImportDTO> Search(DataImportDTO dto)
		{
			try
			{
				List<DataImport> dataImportList = dataImportRepository.Search(Mapper.Map<DataImport>(dto));

				List<DataImportDTO> dataImportDTOList = Mapper.Map<List<DataImportDTO>>(dataImportList);

				return dataImportDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public DataImportDTO SingleSearch(DataImportDTO dto)
		{
			try
			{
				DataImport dataImport = dataImportRepository.SingleSearch(Mapper.Map<DataImport>(dto));

				DataImportDTO dataImportDTO = Mapper.Map<DataImportDTO>(dataImport);

				return dataImportDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(DataImportDTO dto)
		{
			try
			{
				DataImport dataImport = Mapper.Map<DataImport>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = dataImportRepository.Insert(dataImport);

					dto.DataImportId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(DataImportDTO dto)
		{
			try
			{
				DataImport dataImport = Mapper.Map<DataImport>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = dataImportRepository.Update(dataImport);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(DataImportDTO dto)
		{
			try
			{
				DataImport dataImport = Mapper.Map<DataImport>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					dataImportRepository.Delete(dataImport);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
