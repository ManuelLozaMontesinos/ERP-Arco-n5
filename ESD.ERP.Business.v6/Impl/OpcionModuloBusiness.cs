using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class OpcionModuloBusiness : IOpcionModuloBusiness
	{
		private readonly IOpcionModuloRepository opcionModuloRepository;

		public OpcionModuloBusiness(
			IOpcionModuloRepository opcionModuloRepository
			)
		{
			this.opcionModuloRepository = opcionModuloRepository;
		}
		public OpcionModuloDTO GetById(int id)
		{
			try
			{
				OpcionModulo opcionModulo = opcionModuloRepository.GetById(id);

				OpcionModuloDTO opcionModuloDTO = Mapper.Map<OpcionModuloDTO>(opcionModulo);

				return opcionModuloDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<OpcionModuloDTO> Search(OpcionModuloDTO dto)
		{
			try
			{
				List<OpcionModulo> opcionModuloList = opcionModuloRepository.Search(Mapper.Map<OpcionModulo>(dto));

				List<OpcionModuloDTO> opcionModuloDTOList = Mapper.Map<List<OpcionModuloDTO>>(opcionModuloList);

				return opcionModuloDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public OpcionModuloDTO SingleSearch(OpcionModuloDTO dto)
		{
			try
			{
				OpcionModulo opcionModulo = opcionModuloRepository.SingleSearch(Mapper.Map<OpcionModulo>(dto));

				OpcionModuloDTO opcionModuloDTO = Mapper.Map<OpcionModuloDTO>(opcionModulo);

				return opcionModuloDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(OpcionModuloDTO dto)
		{
			try
			{
				OpcionModulo opcionModulo = Mapper.Map<OpcionModulo>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = opcionModuloRepository.Insert(opcionModulo);

					dto.OpcionModuloId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(OpcionModuloDTO dto)
		{
			try
			{
				OpcionModulo opcionModulo = Mapper.Map<OpcionModulo>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = opcionModuloRepository.Update(opcionModulo);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(OpcionModuloDTO dto)
		{
			try
			{
				OpcionModulo opcionModulo = Mapper.Map<OpcionModulo>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					opcionModuloRepository.Delete(opcionModulo);

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
