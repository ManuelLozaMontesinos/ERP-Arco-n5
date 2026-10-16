using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class MesContableBusiness : IMesContableBusiness
	{
		private readonly IMesContableRepository mesContableRepository;

		public MesContableBusiness(
			IMesContableRepository mesContableRepository
			)
		{
			this.mesContableRepository = mesContableRepository;
		}
		public MesContableDTO GetById(int id)
		{
			try
			{
				MesContable mesContable = mesContableRepository.GetById(id);

				MesContableDTO mesContableDTO = Mapper.Map<MesContableDTO>(mesContable);

				return mesContableDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<MesContableDTO> Search(MesContableDTO dto)
		{
			try
			{
				List<MesContable> mesContableList = mesContableRepository.Search(Mapper.Map<MesContable>(dto));

				List<MesContableDTO> mesContableDTOList = Mapper.Map<List<MesContableDTO>>(mesContableList);

				return mesContableDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public MesContableDTO SingleSearch(MesContableDTO dto)
		{
			try
			{
				MesContable mesContable = mesContableRepository.SingleSearch(Mapper.Map<MesContable>(dto));

				MesContableDTO mesContableDTO = Mapper.Map<MesContableDTO>(mesContable);

				return mesContableDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(MesContableDTO dto)
		{
			try
			{
				MesContable mesContable = Mapper.Map<MesContable>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = mesContableRepository.Insert(mesContable);

					dto.MesContableId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(MesContableDTO dto)
		{
			try
			{
				MesContable mesContable = Mapper.Map<MesContable>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = mesContableRepository.Update(mesContable);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(MesContableDTO dto)
		{
			try
			{
				MesContable mesContable = Mapper.Map<MesContable>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					mesContableRepository.Delete(mesContable);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public MesContableDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				MesContable mesContable = mesContableRepository.ObtenerPorCodigo(codigo);

				MesContableDTO mesContableDTO = Mapper.Map<MesContableDTO>(mesContable);

				return mesContableDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
