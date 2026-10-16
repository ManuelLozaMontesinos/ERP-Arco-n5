using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class DistritoBusiness : IDistritoBusiness
	{
		private readonly IDistritoRepository distritoRepository;

		public DistritoBusiness(
			IDistritoRepository distritoRepository
			)
		{
			this.distritoRepository = distritoRepository;
		}
		public DistritoDTO GetById(int id)
		{
			try
			{
				Distrito distrito = distritoRepository.GetById(id);

				DistritoDTO distritoDTO = Mapper.Map<DistritoDTO>(distrito);

				return distritoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<DistritoDTO> Search(DistritoDTO dto)
		{
			try
			{
				List<Distrito> distritoList = distritoRepository.Search(Mapper.Map<Distrito>(dto));

				List<DistritoDTO> distritoDTOList = Mapper.Map<List<DistritoDTO>>(distritoList);

				return distritoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public DistritoDTO SingleSearch(DistritoDTO dto)
		{
			try
			{
				Distrito distrito = distritoRepository.SingleSearch(Mapper.Map<Distrito>(dto));

				DistritoDTO distritoDTO = Mapper.Map<DistritoDTO>(distrito);

				return distritoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(DistritoDTO dto)
		{
			try
			{
				Distrito distrito = Mapper.Map<Distrito>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = distritoRepository.Insert(distrito);

					dto.DistritoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(DistritoDTO dto)
		{
			try
			{
				Distrito distrito = Mapper.Map<Distrito>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = distritoRepository.Update(distrito);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(DistritoDTO dto)
		{
			try
			{
				Distrito distrito = Mapper.Map<Distrito>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					distritoRepository.Delete(distrito);

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
