using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TipoModalidadFormativaBusiness : ITipoModalidadFormativaBusiness
	{
		private readonly ITipoModalidadFormativaRepository tipoModalidadFormativaRepository;

		public TipoModalidadFormativaBusiness(
			ITipoModalidadFormativaRepository tipoModalidadFormativaRepository
			)
		{
			this.tipoModalidadFormativaRepository = tipoModalidadFormativaRepository;
		}
		public TipoModalidadFormativaDTO GetById(int id)
		{
			try
			{
				TipoModalidadFormativa tipoModalidadFormativa = tipoModalidadFormativaRepository.GetById(id);

				TipoModalidadFormativaDTO tipoModalidadFormativaDTO = Mapper.Map<TipoModalidadFormativaDTO>(tipoModalidadFormativa);

				return tipoModalidadFormativaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoModalidadFormativaDTO> Search(TipoModalidadFormativaDTO dto)
		{
			try
			{
				List<TipoModalidadFormativa> tipoModalidadFormativaList = tipoModalidadFormativaRepository.Search(Mapper.Map<TipoModalidadFormativa>(dto));

				List<TipoModalidadFormativaDTO> tipoModalidadFormativaDTOList = Mapper.Map<List<TipoModalidadFormativaDTO>>(tipoModalidadFormativaList);

				return tipoModalidadFormativaDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoModalidadFormativaDTO SingleSearch(TipoModalidadFormativaDTO dto)
		{
			try
			{
				TipoModalidadFormativa tipoModalidadFormativa = tipoModalidadFormativaRepository.SingleSearch(Mapper.Map<TipoModalidadFormativa>(dto));

				TipoModalidadFormativaDTO tipoModalidadFormativaDTO = Mapper.Map<TipoModalidadFormativaDTO>(tipoModalidadFormativa);

				return tipoModalidadFormativaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoModalidadFormativaDTO dto)
		{
			try
			{
				TipoModalidadFormativa tipoModalidadFormativa = Mapper.Map<TipoModalidadFormativa>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = tipoModalidadFormativaRepository.Insert(tipoModalidadFormativa);

					dto.TipoModalidadFormativaId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoModalidadFormativaDTO dto)
		{
			try
			{
				TipoModalidadFormativa tipoModalidadFormativa = Mapper.Map<TipoModalidadFormativa>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = tipoModalidadFormativaRepository.Update(tipoModalidadFormativa);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoModalidadFormativaDTO dto)
		{
			try
			{
				TipoModalidadFormativa tipoModalidadFormativa = Mapper.Map<TipoModalidadFormativa>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					tipoModalidadFormativaRepository.Delete(tipoModalidadFormativa);

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
