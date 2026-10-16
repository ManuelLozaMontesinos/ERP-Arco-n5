using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class SctrPensionBusiness : ISctrPensionBusiness
	{
		private readonly ISctrPensionRepository sctrPensionRepository;

		public SctrPensionBusiness(
			ISctrPensionRepository sctrPensionRepository
			)
		{
			this.sctrPensionRepository = sctrPensionRepository;
		}
		public SctrPensionDTO GetById(int id)
		{
			try
			{
				SctrPension sctrPension = sctrPensionRepository.GetById(id);

				SctrPensionDTO sctrPensionDTO = Mapper.Map<SctrPensionDTO>(sctrPension);

				return sctrPensionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<SctrPensionDTO> Search(SctrPensionDTO dto)
		{
			try
			{
				List<SctrPension> sctrPensionList = sctrPensionRepository.Search(Mapper.Map<SctrPension>(dto));

				List<SctrPensionDTO> sctrPensionDTOList = Mapper.Map<List<SctrPensionDTO>>(sctrPensionList);

				return sctrPensionDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public SctrPensionDTO SingleSearch(SctrPensionDTO dto)
		{
			try
			{
				SctrPension sctrPension = sctrPensionRepository.SingleSearch(Mapper.Map<SctrPension>(dto));

				SctrPensionDTO sctrPensionDTO = Mapper.Map<SctrPensionDTO>(sctrPension);

				return sctrPensionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(SctrPensionDTO dto)
		{
			try
			{
				SctrPension sctrPension = Mapper.Map<SctrPension>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = sctrPensionRepository.Insert(sctrPension);

					dto.SctrPensionId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(SctrPensionDTO dto)
		{
			try
			{
				SctrPension sctrPension = Mapper.Map<SctrPension>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = sctrPensionRepository.Update(sctrPension);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(SctrPensionDTO dto)
		{
			try
			{
				SctrPension sctrPension = Mapper.Map<SctrPension>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					sctrPensionRepository.Delete(sctrPension);

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
