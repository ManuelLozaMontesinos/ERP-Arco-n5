using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class CajaBusiness : ICajaBusiness
	{
		private readonly ICajaRepository cajaRepository;

		public CajaBusiness(
			ICajaRepository cajaRepository
			)
		{
			this.cajaRepository = cajaRepository;
		}
		public CajaDTO GetById(int id)
		{
			try
			{
				Caja caja = cajaRepository.GetById(id);

				CajaDTO cajaDTO = Mapper.Map<CajaDTO>(caja);

				return cajaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<CajaDTO> Search(CajaDTO dto)
		{
			try
			{
				List<Caja> cajaList = cajaRepository.Search(Mapper.Map<Caja>(dto));

				List<CajaDTO> cajaDTOList = Mapper.Map<List<CajaDTO>>(cajaList);

				return cajaDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public CajaDTO SingleSearch(CajaDTO dto)
		{
			try
			{
				Caja caja = cajaRepository.SingleSearch(Mapper.Map<Caja>(dto));

				CajaDTO cajaDTO = Mapper.Map<CajaDTO>(caja);

				return cajaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(CajaDTO dto)
		{
			try
			{
				Caja caja = Mapper.Map<Caja>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = cajaRepository.Insert(caja);

					dto.CajaId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(CajaDTO dto)
		{
			try
			{
				Caja caja = Mapper.Map<Caja>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = cajaRepository.Update(caja);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(CajaDTO dto)
		{
			try
			{
				Caja caja = Mapper.Map<Caja>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					cajaRepository.Delete(caja);

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
