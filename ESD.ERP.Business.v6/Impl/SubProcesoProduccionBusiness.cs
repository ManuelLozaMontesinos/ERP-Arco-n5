using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class SubProcesoProduccionBusiness : ISubProcesoProduccionBusiness
	{
		private readonly ISubProcesoProduccionRepository subProcesoProduccionRepository;

		public SubProcesoProduccionBusiness(
			ISubProcesoProduccionRepository subProcesoProduccionRepository
			)
		{
			this.subProcesoProduccionRepository = subProcesoProduccionRepository;
		}
		public SubProcesoProduccionDTO GetById(int id)
		{
			try
			{
				SubProcesoProduccion subProcesoProduccion = subProcesoProduccionRepository.GetById(id);

				SubProcesoProduccionDTO subProcesoProduccionDTO = Mapper.Map<SubProcesoProduccionDTO>(subProcesoProduccion);

				return subProcesoProduccionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<SubProcesoProduccionDTO> Search(SubProcesoProduccionDTO dto)
		{
			try
			{
				List<SubProcesoProduccion> subProcesoProduccionList = subProcesoProduccionRepository.Search(Mapper.Map<SubProcesoProduccion>(dto));

				List<SubProcesoProduccionDTO> subProcesoProduccionDTOList = Mapper.Map<List<SubProcesoProduccionDTO>>(subProcesoProduccionList);

				return subProcesoProduccionDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public SubProcesoProduccionDTO SingleSearch(SubProcesoProduccionDTO dto)
		{
			try
			{
				SubProcesoProduccion subProcesoProduccion = subProcesoProduccionRepository.SingleSearch(Mapper.Map<SubProcesoProduccion>(dto));

				SubProcesoProduccionDTO subProcesoProduccionDTO = Mapper.Map<SubProcesoProduccionDTO>(subProcesoProduccion);

				return subProcesoProduccionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(SubProcesoProduccionDTO dto)
		{
			try
			{
				SubProcesoProduccion subProcesoProduccion = Mapper.Map<SubProcesoProduccion>(dto);

				int newId = 0;
				using (TransactionScope tx = new())
				{
					newId = subProcesoProduccionRepository.Insert(subProcesoProduccion);

					dto.SubProcesoProduccionId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(SubProcesoProduccionDTO dto)
		{
			try
			{
				SubProcesoProduccion subProcesoProduccion = Mapper.Map<SubProcesoProduccion>(dto);

				int id = 0;
				using (TransactionScope tx = new())
				{
					id = subProcesoProduccionRepository.Update(subProcesoProduccion);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(SubProcesoProduccionDTO dto)
		{
			try
			{
				SubProcesoProduccion subProcesoProduccion = Mapper.Map<SubProcesoProduccion>(dto);

				using (TransactionScope tx = new())
				{
					subProcesoProduccionRepository.Delete(subProcesoProduccion);

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
