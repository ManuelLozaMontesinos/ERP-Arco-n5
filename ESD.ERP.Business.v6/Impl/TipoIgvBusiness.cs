using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TipoIgvBusiness : ITipoIgvBusiness
	{
		private readonly ITipoIgvRepository tipoIgvRepository;

		public TipoIgvBusiness(
			ITipoIgvRepository tipoIgvRepository
			)
		{
			this.tipoIgvRepository = tipoIgvRepository;
		}
		public TipoIgvDTO GetById(int id)
		{
			try
			{
				TipoIgv tipoIgv = tipoIgvRepository.GetById(id);

				TipoIgvDTO tipoIgvDTO = Mapper.Map<TipoIgvDTO>(tipoIgv);

				return tipoIgvDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoIgvDTO> Search(TipoIgvDTO dto)
		{
			try
			{
				List<TipoIgv> tipoIgvList = tipoIgvRepository.Search(Mapper.Map<TipoIgv>(dto));

				List<TipoIgvDTO> tipoIgvDTOList = Mapper.Map<List<TipoIgvDTO>>(tipoIgvList);

				return tipoIgvDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoIgvDTO SingleSearch(TipoIgvDTO dto)
		{
			try
			{
				TipoIgv tipoIgv = tipoIgvRepository.SingleSearch(Mapper.Map<TipoIgv>(dto));

				TipoIgvDTO tipoIgvDTO = Mapper.Map<TipoIgvDTO>(tipoIgv);

				return tipoIgvDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoIgvDTO dto)
		{
			try
			{
				TipoIgv tipoIgv = Mapper.Map<TipoIgv>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = tipoIgvRepository.Insert(tipoIgv);

					dto.TipoIgvId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoIgvDTO dto)
		{
			try
			{
				TipoIgv tipoIgv = Mapper.Map<TipoIgv>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = tipoIgvRepository.Update(tipoIgv);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoIgvDTO dto)
		{
			try
			{
				TipoIgv tipoIgv = Mapper.Map<TipoIgv>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					tipoIgvRepository.Delete(tipoIgv);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public TipoIgvDTO ObtenerPorCodigo(string codigo)
      {
			try
			{
				TipoIgv tipoIgv = tipoIgvRepository.ObtenerPorCodigo(codigo);

				TipoIgvDTO tipoIgvDTO = Mapper.Map<TipoIgvDTO>(tipoIgv);

				return tipoIgvDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
   }
}
