using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class PrestamoCuotaBusiness : IPrestamoCuotaBusiness
	{
		private readonly IPrestamoCuotaRepository prestamoCuotaRepository;

		public PrestamoCuotaBusiness(
			IPrestamoCuotaRepository prestamoCuotaRepository
			)
		{
			this.prestamoCuotaRepository = prestamoCuotaRepository;
		}
		public PrestamoCuotaDTO GetById(int id)
		{
			try
			{
				PrestamoCuota prestamoCuota = prestamoCuotaRepository.GetById(id);

				PrestamoCuotaDTO prestamoCuotaDTO = Mapper.Map<PrestamoCuotaDTO>(prestamoCuota);

				return prestamoCuotaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<PrestamoCuotaDTO> Search(PrestamoCuotaDTO dto)
		{
			try
			{
				List<PrestamoCuota> prestamoCuotaList = prestamoCuotaRepository.Search(Mapper.Map<PrestamoCuota>(dto));

				List<PrestamoCuotaDTO> prestamoCuotaDTOList = Mapper.Map<List<PrestamoCuotaDTO>>(prestamoCuotaList);

				return prestamoCuotaDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public PrestamoCuotaDTO SingleSearch(PrestamoCuotaDTO dto)
		{
			try
			{
				PrestamoCuota prestamoCuota = prestamoCuotaRepository.SingleSearch(Mapper.Map<PrestamoCuota>(dto));

				PrestamoCuotaDTO prestamoCuotaDTO = Mapper.Map<PrestamoCuotaDTO>(prestamoCuota);

				return prestamoCuotaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(PrestamoCuotaDTO dto)
		{
			try
			{
				PrestamoCuota prestamoCuota = Mapper.Map<PrestamoCuota>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = prestamoCuotaRepository.Insert(prestamoCuota);

					dto.PrestamoCuotaId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(PrestamoCuotaDTO dto)
		{
			try
			{
				PrestamoCuota prestamoCuota = Mapper.Map<PrestamoCuota>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = prestamoCuotaRepository.Update(prestamoCuota);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(PrestamoCuotaDTO dto)
		{
			try
			{
				PrestamoCuota prestamoCuota = Mapper.Map<PrestamoCuota>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					prestamoCuotaRepository.Delete(prestamoCuota);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public void BulkMerge(List<PrestamoCuotaDTO> list)
      {
			try
			{
				List<PrestamoCuota> prestamoCuotaList = Mapper.Map<List<PrestamoCuota>>(list);

				using (TransactionScope tx = new TransactionScope())
				{
					prestamoCuotaRepository.BulkMerge(prestamoCuotaList);

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
