using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TipoEsquemaPensionBusiness : ITipoEsquemaPensionBusiness
	{
		private readonly ITipoEsquemaPensionRepository tipoEsquemaPensionRepository;

		public TipoEsquemaPensionBusiness(
			ITipoEsquemaPensionRepository tipoEsquemaPensionRepository
			)
		{
			this.tipoEsquemaPensionRepository = tipoEsquemaPensionRepository;
		}
		public TipoEsquemaPensionDTO GetById(int id)
		{
			try
			{
				TipoEsquemaPension tipoEsquemaPension = tipoEsquemaPensionRepository.GetById(id);

				TipoEsquemaPensionDTO tipoEsquemaPensionDTO = Mapper.Map<TipoEsquemaPensionDTO>(tipoEsquemaPension);

				return tipoEsquemaPensionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoEsquemaPensionDTO> Search(TipoEsquemaPensionDTO dto)
		{
			try
			{
				List<TipoEsquemaPension> tipoEsquemaPensionList = tipoEsquemaPensionRepository.Search(Mapper.Map<TipoEsquemaPension>(dto));

				List<TipoEsquemaPensionDTO> tipoEsquemaPensionDTOList = Mapper.Map<List<TipoEsquemaPensionDTO>>(tipoEsquemaPensionList);

				return tipoEsquemaPensionDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoEsquemaPensionDTO SingleSearch(TipoEsquemaPensionDTO dto)
		{
			try
			{
				TipoEsquemaPension tipoEsquemaPension = tipoEsquemaPensionRepository.SingleSearch(Mapper.Map<TipoEsquemaPension>(dto));

				TipoEsquemaPensionDTO tipoEsquemaPensionDTO = Mapper.Map<TipoEsquemaPensionDTO>(tipoEsquemaPension);

				return tipoEsquemaPensionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoEsquemaPensionDTO dto)
		{
			try
			{
				TipoEsquemaPension tipoEsquemaPension = Mapper.Map<TipoEsquemaPension>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = tipoEsquemaPensionRepository.Insert(tipoEsquemaPension);

					dto.TipoEsquemaPensionId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoEsquemaPensionDTO dto)
		{
			try
			{
				TipoEsquemaPension tipoEsquemaPension = Mapper.Map<TipoEsquemaPension>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = tipoEsquemaPensionRepository.Update(tipoEsquemaPension);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoEsquemaPensionDTO dto)
		{
			try
			{
				TipoEsquemaPension tipoEsquemaPension = Mapper.Map<TipoEsquemaPension>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					tipoEsquemaPensionRepository.Delete(tipoEsquemaPension);

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
