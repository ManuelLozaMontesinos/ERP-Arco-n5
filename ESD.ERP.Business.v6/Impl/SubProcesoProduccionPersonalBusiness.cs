using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class SubProcesoProduccionPersonalBusiness : ISubProcesoProduccionPersonalBusiness
	{
		private readonly ISubProcesoProduccionPersonalRepository subProcesoProduccionPersonalRepository;

		public SubProcesoProduccionPersonalBusiness(
			ISubProcesoProduccionPersonalRepository subProcesoProduccionPersonalRepository
			)
		{
			this.subProcesoProduccionPersonalRepository = subProcesoProduccionPersonalRepository;
		}
		public SubProcesoProduccionPersonalDTO GetById(int id)
		{
			try
			{
				SubProcesoProduccionPersonal subProcesoProduccionPersonal = subProcesoProduccionPersonalRepository.GetById(id);

				SubProcesoProduccionPersonalDTO subProcesoProduccionPersonalDTO = Mapper.Map<SubProcesoProduccionPersonalDTO>(subProcesoProduccionPersonal);

				return subProcesoProduccionPersonalDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<SubProcesoProduccionPersonalDTO> Search(SubProcesoProduccionPersonalDTO dto)
		{
			try
			{
				List<SubProcesoProduccionPersonal> subProcesoProduccionPersonalList = subProcesoProduccionPersonalRepository.Search(Mapper.Map<SubProcesoProduccionPersonal>(dto));

				List<SubProcesoProduccionPersonalDTO> subProcesoProduccionPersonalDTOList = Mapper.Map<List<SubProcesoProduccionPersonalDTO>>(subProcesoProduccionPersonalList);

				return subProcesoProduccionPersonalDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public SubProcesoProduccionPersonalDTO SingleSearch(SubProcesoProduccionPersonalDTO dto)
		{
			try
			{
				SubProcesoProduccionPersonal subProcesoProduccionPersonal = subProcesoProduccionPersonalRepository.SingleSearch(Mapper.Map<SubProcesoProduccionPersonal>(dto));

				SubProcesoProduccionPersonalDTO subProcesoProduccionPersonalDTO = Mapper.Map<SubProcesoProduccionPersonalDTO>(subProcesoProduccionPersonal);

				return subProcesoProduccionPersonalDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(SubProcesoProduccionPersonalDTO dto)
		{
			try
			{
				SubProcesoProduccionPersonal subProcesoProduccionPersonal = Mapper.Map<SubProcesoProduccionPersonal>(dto);

				int newId = 0;
				using (TransactionScope tx = new())
				{
					newId = subProcesoProduccionPersonalRepository.Insert(subProcesoProduccionPersonal);

					dto.SubProcesoProduccionPersonalId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(SubProcesoProduccionPersonalDTO dto)
		{
			try
			{
				SubProcesoProduccionPersonal subProcesoProduccionPersonal = Mapper.Map<SubProcesoProduccionPersonal>(dto);

				int id = 0;
				using (TransactionScope tx = new())
				{
					id = subProcesoProduccionPersonalRepository.Update(subProcesoProduccionPersonal);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(SubProcesoProduccionPersonalDTO dto)
		{
			try
			{
				SubProcesoProduccionPersonal subProcesoProduccionPersonal = Mapper.Map<SubProcesoProduccionPersonal>(dto);

				using (TransactionScope tx = new())
				{
					subProcesoProduccionPersonalRepository.Delete(subProcesoProduccionPersonal);

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
