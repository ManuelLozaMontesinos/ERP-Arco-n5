using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class CentroCostoBusiness : ICentroCostoBusiness
	{
		private readonly ICentroCostoRepository centroCostoRepository;

		public CentroCostoBusiness(
			ICentroCostoRepository centroCostoRepository
			)
		{
			this.centroCostoRepository = centroCostoRepository;
		}
		public CentroCostoDTO GetById(int id)
		{
			try
			{
				CentroCosto centroCosto = centroCostoRepository.GetById(id);

				CentroCostoDTO centroCostoDTO = Mapper.Map<CentroCostoDTO>(centroCosto);

				return centroCostoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<CentroCostoDTO> Search(CentroCostoDTO dto)
		{
			try
			{
				List<CentroCosto> centroCostoList = centroCostoRepository.Search(Mapper.Map<CentroCosto>(dto));

				List<CentroCostoDTO> centroCostoDTOList = Mapper.Map<List<CentroCostoDTO>>(centroCostoList);

				return centroCostoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public CentroCostoDTO SingleSearch(CentroCostoDTO dto)
		{
			try
			{
				CentroCosto centroCosto = centroCostoRepository.SingleSearch(Mapper.Map<CentroCosto>(dto));

				CentroCostoDTO centroCostoDTO = Mapper.Map<CentroCostoDTO>(centroCosto);

				return centroCostoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(CentroCostoDTO dto)
		{
			try
			{
				CentroCosto centroCosto = Mapper.Map<CentroCosto>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = centroCostoRepository.Insert(centroCosto);

					dto.CentroCostoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(CentroCostoDTO dto)
		{
			try
			{
				CentroCosto centroCosto = Mapper.Map<CentroCosto>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = centroCostoRepository.Update(centroCosto);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(CentroCostoDTO dto)
		{
			try
			{
				CentroCosto centroCosto = Mapper.Map<CentroCosto>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					centroCostoRepository.Delete(centroCosto);

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
