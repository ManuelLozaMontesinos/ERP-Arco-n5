using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class MonedaBusiness : IMonedaBusiness
	{
		private readonly IMonedaRepository monedaRepository;

		public MonedaBusiness(
			IMonedaRepository monedaRepository
			)
		{
			this.monedaRepository = monedaRepository;
		}
		public MonedaDTO GetById(int id)
		{
			try
			{
				Moneda moneda = monedaRepository.GetById(id);

				MonedaDTO monedaDTO = Mapper.Map<MonedaDTO>(moneda);

				return monedaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<MonedaDTO> Search(MonedaDTO dto)
		{
			try
			{
				List<Moneda> monedaList = monedaRepository.Search(Mapper.Map<Moneda>(dto));

				List<MonedaDTO> monedaDTOList = Mapper.Map<List<MonedaDTO>>(monedaList);

				return monedaDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public MonedaDTO SingleSearch(MonedaDTO dto)
		{
			try
			{
				Moneda moneda = monedaRepository.SingleSearch(Mapper.Map<Moneda>(dto));

				MonedaDTO monedaDTO = Mapper.Map<MonedaDTO>(moneda);

				return monedaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(MonedaDTO dto)
		{
			try
			{
				Moneda moneda = Mapper.Map<Moneda>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = monedaRepository.Insert(moneda);

					dto.MonedaId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(MonedaDTO dto)
		{
			try
			{
				Moneda moneda = Mapper.Map<Moneda>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = monedaRepository.Update(moneda);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(MonedaDTO dto)
		{
			try
			{
				Moneda moneda = Mapper.Map<Moneda>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					monedaRepository.Delete(moneda);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public MonedaDTO ObtenerPorCodigo(string codigo)
      {
			try
			{
				Moneda moneda = monedaRepository.ObtenerPorCodigo(codigo);

				MonedaDTO monedaDTO = Mapper.Map<MonedaDTO>(moneda);

				return monedaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
   }
}
