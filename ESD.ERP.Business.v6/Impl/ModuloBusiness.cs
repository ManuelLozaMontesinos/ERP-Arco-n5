using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class ModuloBusiness : IModuloBusiness
	{
		private readonly IModuloRepository moduloRepository;

		public ModuloBusiness(
			IModuloRepository moduloRepository
			)
		{
			this.moduloRepository = moduloRepository;
		}
		public ModuloDTO GetById(int id)
		{
			try
			{
				Modulo modulo = moduloRepository.GetById(id);

				ModuloDTO moduloDTO = Mapper.Map<ModuloDTO>(modulo);

				return moduloDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<ModuloDTO> Search(ModuloDTO dto)
		{
			try
			{
				List<Modulo> moduloList = moduloRepository.Search(Mapper.Map<Modulo>(dto));

				List<ModuloDTO> moduloDTOList = Mapper.Map<List<ModuloDTO>>(moduloList);

				return moduloDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public ModuloDTO SingleSearch(ModuloDTO dto)
		{
			try
			{
				Modulo modulo = moduloRepository.SingleSearch(Mapper.Map<Modulo>(dto));

				ModuloDTO moduloDTO = Mapper.Map<ModuloDTO>(modulo);

				return moduloDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(ModuloDTO dto)
		{
			try
			{
				Modulo modulo = Mapper.Map<Modulo>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = moduloRepository.Insert(modulo);

					dto.ModuloId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(ModuloDTO dto)
		{
			try
			{
				Modulo modulo = Mapper.Map<Modulo>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = moduloRepository.Update(modulo);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(ModuloDTO dto)
		{
			try
			{
				Modulo modulo = Mapper.Map<Modulo>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					moduloRepository.Delete(modulo);

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
