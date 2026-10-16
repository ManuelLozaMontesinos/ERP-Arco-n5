using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TipoAfectacionIgvBusiness : ITipoAfectacionIgvBusiness
	{
		private readonly ITipoAfectacionIgvRepository tipoAfectacionIgvRepository;

		public TipoAfectacionIgvBusiness(
			ITipoAfectacionIgvRepository tipoAfectacionIgvRepository
			)
		{
			this.tipoAfectacionIgvRepository = tipoAfectacionIgvRepository;
		}
		public TipoAfectacionIgvDTO GetById(int id)
		{
			try
			{
				TipoAfectacionIgv tipoAfectacionIgv = tipoAfectacionIgvRepository.GetById(id);

				TipoAfectacionIgvDTO tipoAfectacionIgvDTO = Mapper.Map<TipoAfectacionIgvDTO>(tipoAfectacionIgv);

				return tipoAfectacionIgvDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoAfectacionIgvDTO> Search(TipoAfectacionIgvDTO dto)
		{
			try
			{
				List<TipoAfectacionIgv> tipoAfectacionIgvList = tipoAfectacionIgvRepository.Search(Mapper.Map<TipoAfectacionIgv>(dto));

				List<TipoAfectacionIgvDTO> tipoAfectacionIgvDTOList = Mapper.Map<List<TipoAfectacionIgvDTO>>(tipoAfectacionIgvList);

				return tipoAfectacionIgvDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoAfectacionIgvDTO SingleSearch(TipoAfectacionIgvDTO dto)
		{
			try
			{
				TipoAfectacionIgv tipoAfectacionIgv = tipoAfectacionIgvRepository.SingleSearch(Mapper.Map<TipoAfectacionIgv>(dto));

				TipoAfectacionIgvDTO tipoAfectacionIgvDTO = Mapper.Map<TipoAfectacionIgvDTO>(tipoAfectacionIgv);

				return tipoAfectacionIgvDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoAfectacionIgvDTO dto)
		{
			try
			{
				TipoAfectacionIgv tipoAfectacionIgv = Mapper.Map<TipoAfectacionIgv>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = tipoAfectacionIgvRepository.Insert(tipoAfectacionIgv);

					dto.TipoAfectacionIgvId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoAfectacionIgvDTO dto)
		{
			try
			{
				TipoAfectacionIgv tipoAfectacionIgv = Mapper.Map<TipoAfectacionIgv>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = tipoAfectacionIgvRepository.Update(tipoAfectacionIgv);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoAfectacionIgvDTO dto)
		{
			try
			{
				TipoAfectacionIgv tipoAfectacionIgv = Mapper.Map<TipoAfectacionIgv>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					tipoAfectacionIgvRepository.Delete(tipoAfectacionIgv);

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
