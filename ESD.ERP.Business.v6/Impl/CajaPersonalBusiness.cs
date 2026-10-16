using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class CajaPersonalBusiness : ICajaPersonalBusiness
	{
		private readonly ICajaPersonalRepository cajaPersonalRepository;

		public CajaPersonalBusiness(
			ICajaPersonalRepository cajaPersonalRepository
			)
		{
			this.cajaPersonalRepository = cajaPersonalRepository;
		}
		public CajaPersonalDTO GetById(int id)
		{
			try
			{
				CajaPersonal cajaPersonal = cajaPersonalRepository.GetById(id);

				CajaPersonalDTO cajaPersonalDTO = Mapper.Map<CajaPersonalDTO>(cajaPersonal);

				return cajaPersonalDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<CajaPersonalDTO> Search(CajaPersonalDTO dto)
		{
			try
			{
				List<CajaPersonal> cajaPersonalList = cajaPersonalRepository.Search(Mapper.Map<CajaPersonal>(dto));

				List<CajaPersonalDTO> cajaPersonalDTOList = Mapper.Map<List<CajaPersonalDTO>>(cajaPersonalList);

				return cajaPersonalDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public CajaPersonalDTO SingleSearch(CajaPersonalDTO dto)
		{
			try
			{
				CajaPersonal cajaPersonal = cajaPersonalRepository.SingleSearch(Mapper.Map<CajaPersonal>(dto));

				CajaPersonalDTO cajaPersonalDTO = Mapper.Map<CajaPersonalDTO>(cajaPersonal);

				return cajaPersonalDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(CajaPersonalDTO dto)
		{
			try
			{
				CajaPersonal cajaPersonal = Mapper.Map<CajaPersonal>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = cajaPersonalRepository.Insert(cajaPersonal);

					dto.CajaPersonalId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(CajaPersonalDTO dto)
		{
			try
			{
				CajaPersonal cajaPersonal = Mapper.Map<CajaPersonal>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = cajaPersonalRepository.Update(cajaPersonal);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(CajaPersonalDTO dto)
		{
			try
			{
				CajaPersonal cajaPersonal = Mapper.Map<CajaPersonal>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					cajaPersonalRepository.Delete(cajaPersonal);

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
