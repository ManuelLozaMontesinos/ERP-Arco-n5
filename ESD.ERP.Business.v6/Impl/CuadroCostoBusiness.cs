using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class CuadroCostoBusiness : ICuadroCostoBusiness
	{
		private readonly ICuadroCostoRepository cuadroCostoRepository;

		public CuadroCostoBusiness(
			ICuadroCostoRepository cuadroCostoRepository
			)
		{
			this.cuadroCostoRepository = cuadroCostoRepository;
		}
		public CuadroCostoDTO GetById(int id)
		{
			try
			{
				CuadroCosto cuadroCosto = cuadroCostoRepository.GetById(id);

				CuadroCostoDTO cuadroCostoDTO = Mapper.Map<CuadroCostoDTO>(cuadroCosto);

				return cuadroCostoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<CuadroCostoDTO> Search(CuadroCostoDTO dto)
		{
			try
			{
				List<CuadroCosto> cuadroCostoList = cuadroCostoRepository.Search(Mapper.Map<CuadroCosto>(dto));

				List<CuadroCostoDTO> cuadroCostoDTOList = Mapper.Map<List<CuadroCostoDTO>>(cuadroCostoList);

				return cuadroCostoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public CuadroCostoDTO SingleSearch(CuadroCostoDTO dto)
		{
			try
			{
				CuadroCosto cuadroCosto = cuadroCostoRepository.SingleSearch(Mapper.Map<CuadroCosto>(dto));

				CuadroCostoDTO cuadroCostoDTO = Mapper.Map<CuadroCostoDTO>(cuadroCosto);

				return cuadroCostoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(CuadroCostoDTO dto)
		{
			try
			{
				CuadroCosto cuadroCosto = Mapper.Map<CuadroCosto>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = cuadroCostoRepository.Insert(cuadroCosto);

					dto.CuadroCostoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(CuadroCostoDTO dto)
		{
			try
			{
				CuadroCosto cuadroCosto = Mapper.Map<CuadroCosto>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = cuadroCostoRepository.Update(cuadroCosto);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(CuadroCostoDTO dto)
		{
			try
			{
				CuadroCosto cuadroCosto = Mapper.Map<CuadroCosto>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					cuadroCostoRepository.Delete(cuadroCosto);

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
