using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class VinculoFamiliarBusiness : IVinculoFamiliarBusiness
	{
		private readonly IVinculoFamiliarRepository vinculoFamiliarRepository;

		public VinculoFamiliarBusiness(
			IVinculoFamiliarRepository vinculoFamiliarRepository
			)
		{
			this.vinculoFamiliarRepository = vinculoFamiliarRepository;
		}
		public VinculoFamiliarDTO GetById(int id)
		{
			try
			{
				VinculoFamiliar vinculoFamiliar = vinculoFamiliarRepository.GetById(id);

				VinculoFamiliarDTO vinculoFamiliarDTO = Mapper.Map<VinculoFamiliarDTO>(vinculoFamiliar);

				return vinculoFamiliarDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<VinculoFamiliarDTO> Search(VinculoFamiliarDTO dto)
		{
			try
			{
				List<VinculoFamiliar> vinculoFamiliarList = vinculoFamiliarRepository.Search(Mapper.Map<VinculoFamiliar>(dto));

				List<VinculoFamiliarDTO> vinculoFamiliarDTOList = Mapper.Map<List<VinculoFamiliarDTO>>(vinculoFamiliarList);

				return vinculoFamiliarDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public VinculoFamiliarDTO SingleSearch(VinculoFamiliarDTO dto)
		{
			try
			{
				VinculoFamiliar vinculoFamiliar = vinculoFamiliarRepository.SingleSearch(Mapper.Map<VinculoFamiliar>(dto));

				VinculoFamiliarDTO vinculoFamiliarDTO = Mapper.Map<VinculoFamiliarDTO>(vinculoFamiliar);

				return vinculoFamiliarDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(VinculoFamiliarDTO dto)
		{
			try
			{
				VinculoFamiliar vinculoFamiliar = Mapper.Map<VinculoFamiliar>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = vinculoFamiliarRepository.Insert(vinculoFamiliar);

					dto.VinculoFamiliarId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(VinculoFamiliarDTO dto)
		{
			try
			{
				VinculoFamiliar vinculoFamiliar = Mapper.Map<VinculoFamiliar>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = vinculoFamiliarRepository.Update(vinculoFamiliar);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(VinculoFamiliarDTO dto)
		{
			try
			{
				VinculoFamiliar vinculoFamiliar = Mapper.Map<VinculoFamiliar>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					vinculoFamiliarRepository.Delete(vinculoFamiliar);

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
