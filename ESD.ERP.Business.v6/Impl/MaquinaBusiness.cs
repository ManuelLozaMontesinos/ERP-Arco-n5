using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class MaquinaBusiness : IMaquinaBusiness
	{
		private readonly IMaquinaRepository maquinaRepository;

		public MaquinaBusiness(
			IMaquinaRepository maquinaRepository
			)
		{
			this.maquinaRepository = maquinaRepository;
		}
		public MaquinaDTO GetById(int id)
		{
			try
			{
				Maquina maquina = maquinaRepository.GetById(id);

				MaquinaDTO maquinaDTO = Mapper.Map<MaquinaDTO>(maquina);

				return maquinaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<MaquinaDTO> Search(MaquinaDTO dto)
		{
			try
			{
				List<Maquina> maquinaList = maquinaRepository.Search(Mapper.Map<Maquina>(dto));

				List<MaquinaDTO> maquinaDTOList = Mapper.Map<List<MaquinaDTO>>(maquinaList);

				return maquinaDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public MaquinaDTO SingleSearch(MaquinaDTO dto)
		{
			try
			{
				Maquina maquina = maquinaRepository.SingleSearch(Mapper.Map<Maquina>(dto));

				MaquinaDTO maquinaDTO = Mapper.Map<MaquinaDTO>(maquina);

				return maquinaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(MaquinaDTO dto)
		{
			try
			{
				Maquina maquina = Mapper.Map<Maquina>(dto);

				int newId = 0;
				using (TransactionScope tx = new())
				{
					newId = maquinaRepository.Insert(maquina);

					dto.MaquinaId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(MaquinaDTO dto)
		{
			try
			{
				Maquina maquina = Mapper.Map<Maquina>(dto);

				int id = 0;
				using (TransactionScope tx = new())
				{
					id = maquinaRepository.Update(maquina);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(MaquinaDTO dto)
		{
			try
			{
				Maquina maquina = Mapper.Map<Maquina>(dto);

				using (TransactionScope tx = new())
				{
					maquinaRepository.Delete(maquina);

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
