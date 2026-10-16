using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
   public class TipoCambioBusiness : ITipoCambioBusiness
	{
		private readonly ITipoCambioRepository tipoCambioRepository;

		public TipoCambioBusiness(
			ITipoCambioRepository tipoCambioRepository
			)
		{
			this.tipoCambioRepository = tipoCambioRepository;
		}
		public TipoCambioDTO GetById(int id)
		{
			try
			{
				TipoCambio tipoCambio = tipoCambioRepository.GetById(id);

				TipoCambioDTO tipoCambioDTO = Mapper.Map<TipoCambioDTO>(tipoCambio);

				return tipoCambioDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoCambioDTO> Search(TipoCambioDTO dto)
		{
			try
			{
				List<TipoCambio> tipoCambioList = tipoCambioRepository.Search(Mapper.Map<TipoCambio>(dto));

				List<TipoCambioDTO> tipoCambioDTOList = Mapper.Map<List<TipoCambioDTO>>(tipoCambioList);

				return tipoCambioDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoCambioDTO SingleSearch(TipoCambioDTO dto)
		{
			try
			{
				TipoCambio tipoCambio = tipoCambioRepository.SingleSearch(Mapper.Map<TipoCambio>(dto));

				TipoCambioDTO tipoCambioDTO = Mapper.Map<TipoCambioDTO>(tipoCambio);

				return tipoCambioDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoCambioDTO dto)
		{
			try
			{
				TipoCambio tipoCambio = Mapper.Map<TipoCambio>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = tipoCambioRepository.Insert(tipoCambio);

					dto.TipoCambioId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoCambioDTO dto)
		{
			try
			{
				TipoCambio tipoCambio = Mapper.Map<TipoCambio>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = tipoCambioRepository.Update(tipoCambio);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoCambioDTO dto)
		{
			try
			{
				TipoCambio tipoCambio = Mapper.Map<TipoCambio>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					tipoCambioRepository.Delete(tipoCambio);

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
