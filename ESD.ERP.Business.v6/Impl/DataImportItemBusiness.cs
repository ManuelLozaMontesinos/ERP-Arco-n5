using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class DataImportItemBusiness : IDataImportItemBusiness
	{
		private readonly IDataImportItemRepository dataImportItemRepository;

		public DataImportItemBusiness(
			IDataImportItemRepository dataImportItemRepository
			)
		{
			this.dataImportItemRepository = dataImportItemRepository;
		}
		public DataImportItemDTO GetById(int id)
		{
			try
			{
				DataImportItem dataImportItem = dataImportItemRepository.GetById(id);

				DataImportItemDTO dataImportItemDTO = Mapper.Map<DataImportItemDTO>(dataImportItem);

				return dataImportItemDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<DataImportItemDTO> Search(DataImportItemDTO dto)
		{
			try
			{
				List<DataImportItem> dataImportItemList = dataImportItemRepository.Search(Mapper.Map<DataImportItem>(dto));

				List<DataImportItemDTO> dataImportItemDTOList = Mapper.Map<List<DataImportItemDTO>>(dataImportItemList);

				return dataImportItemDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public DataImportItemDTO SingleSearch(DataImportItemDTO dto)
		{
			try
			{
				DataImportItem dataImportItem = dataImportItemRepository.SingleSearch(Mapper.Map<DataImportItem>(dto));

				DataImportItemDTO dataImportItemDTO = Mapper.Map<DataImportItemDTO>(dataImportItem);

				return dataImportItemDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(DataImportItemDTO dto)
		{
			try
			{
				DataImportItem dataImportItem = Mapper.Map<DataImportItem>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = dataImportItemRepository.Insert(dataImportItem);

					dto.DataImportItemId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(DataImportItemDTO dto)
		{
			try
			{
				DataImportItem dataImportItem = Mapper.Map<DataImportItem>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = dataImportItemRepository.Update(dataImportItem);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(DataImportItemDTO dto)
		{
			try
			{
				DataImportItem dataImportItem = Mapper.Map<DataImportItem>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					dataImportItemRepository.Delete(dataImportItem);

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
