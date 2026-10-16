using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TipoOperacionBusiness : ITipoOperacionBusiness
	{
		private readonly ITipoOperacionRepository tipoOperacionRepository;

		public TipoOperacionBusiness(
			ITipoOperacionRepository tipoOperacionRepository
			)
		{
			this.tipoOperacionRepository = tipoOperacionRepository;
		}
		public TipoOperacionDTO GetById(int id)
		{
			try
			{
				TipoOperacion tipoOperacion = tipoOperacionRepository.GetById(id);

				TipoOperacionDTO tipoOperacionDTO = Mapper.Map<TipoOperacionDTO>(tipoOperacion);

				return tipoOperacionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoOperacionDTO> Search(TipoOperacionDTO dto)
		{
			try
			{
				List<TipoOperacion> tipoOperacionList = tipoOperacionRepository.Search(Mapper.Map<TipoOperacion>(dto));

				List<TipoOperacionDTO> tipoOperacionDTOList = Mapper.Map<List<TipoOperacionDTO>>(tipoOperacionList);

				return tipoOperacionDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoOperacionDTO SingleSearch(TipoOperacionDTO dto)
		{
			try
			{
				TipoOperacion tipoOperacion = tipoOperacionRepository.SingleSearch(Mapper.Map<TipoOperacion>(dto));

				TipoOperacionDTO tipoOperacionDTO = Mapper.Map<TipoOperacionDTO>(tipoOperacion);

				return tipoOperacionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoOperacionDTO dto)
		{
			try
			{
				TipoOperacion tipoOperacion = Mapper.Map<TipoOperacion>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = tipoOperacionRepository.Insert(tipoOperacion);

					dto.TipoOperacionId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoOperacionDTO dto)
		{
			try
			{
				TipoOperacion tipoOperacion = Mapper.Map<TipoOperacion>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = tipoOperacionRepository.Update(tipoOperacion);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoOperacionDTO dto)
		{
			try
			{
				TipoOperacion tipoOperacion = Mapper.Map<TipoOperacion>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					tipoOperacionRepository.Delete(tipoOperacion);

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
