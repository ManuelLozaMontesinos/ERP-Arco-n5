using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class MesBusiness : IMesBusiness
	{
		private readonly IMesRepository mesRepository;

		public MesBusiness(
			IMesRepository mesRepository
			)
		{
			this.mesRepository = mesRepository;
		}
		public MesDTO GetById(int id)
		{
			try
			{
				Mes mes = mesRepository.GetById(id);

				MesDTO mesDTO = Mapper.Map<MesDTO>(mes);

				return mesDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<MesDTO> Search(MesDTO dto)
		{
			try
			{
				List<Mes> mesList = mesRepository.Search(Mapper.Map<Mes>(dto));

				List<MesDTO> mesDTOList = Mapper.Map<List<MesDTO>>(mesList);

				return mesDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public MesDTO SingleSearch(MesDTO dto)
		{
			try
			{
				Mes mes = mesRepository.SingleSearch(Mapper.Map<Mes>(dto));

				MesDTO mesDTO = Mapper.Map<MesDTO>(mes);

				return mesDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(MesDTO dto)
		{
			try
			{
				Mes mes = Mapper.Map<Mes>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = mesRepository.Insert(mes);

					dto.MesId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(MesDTO dto)
		{
			try
			{
				Mes mes = Mapper.Map<Mes>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = mesRepository.Update(mes);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(MesDTO dto)
		{
			try
			{
				Mes mes = Mapper.Map<Mes>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					mesRepository.Delete(mes);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public MesDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				Mes mes = mesRepository.ObtenerPorCodigo(codigo);

				MesDTO mesDTO = Mapper.Map<MesDTO>(mes);

				return mesDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
