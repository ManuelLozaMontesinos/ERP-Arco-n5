using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TipoAutorizacionBusiness : ITipoAutorizacionBusiness
	{
		private readonly ITipoAutorizacionRepository tipoAutorizacionRepository;

		public TipoAutorizacionBusiness(
			ITipoAutorizacionRepository tipoAutorizacionRepository
			)
		{
			this.tipoAutorizacionRepository = tipoAutorizacionRepository;
		}
		public TipoAutorizacionDTO GetById(int id)
		{
			try
			{
				TipoAutorizacion tipoAutorizacion = tipoAutorizacionRepository.GetById(id);

				TipoAutorizacionDTO tipoAutorizacionDTO = Mapper.Map<TipoAutorizacionDTO>(tipoAutorizacion);

				return tipoAutorizacionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoAutorizacionDTO> Search(TipoAutorizacionDTO dto)
		{
			try
			{
				List<TipoAutorizacion> tipoAutorizacionList = tipoAutorizacionRepository.Search(Mapper.Map<TipoAutorizacion>(dto));

				List<TipoAutorizacionDTO> tipoAutorizacionDTOList = Mapper.Map<List<TipoAutorizacionDTO>>(tipoAutorizacionList);

				return tipoAutorizacionDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoAutorizacionDTO SingleSearch(TipoAutorizacionDTO dto)
		{
			try
			{
				TipoAutorizacion tipoAutorizacion = tipoAutorizacionRepository.SingleSearch(Mapper.Map<TipoAutorizacion>(dto));

				TipoAutorizacionDTO tipoAutorizacionDTO = Mapper.Map<TipoAutorizacionDTO>(tipoAutorizacion);

				return tipoAutorizacionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoAutorizacionDTO dto)
		{
			try
			{
				TipoAutorizacion tipoAutorizacion = Mapper.Map<TipoAutorizacion>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = tipoAutorizacionRepository.Insert(tipoAutorizacion);

					dto.TipoAutorizacionId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoAutorizacionDTO dto)
		{
			try
			{
				TipoAutorizacion tipoAutorizacion = Mapper.Map<TipoAutorizacion>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = tipoAutorizacionRepository.Update(tipoAutorizacion);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoAutorizacionDTO dto)
		{
			try
			{
				TipoAutorizacion tipoAutorizacion = Mapper.Map<TipoAutorizacion>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					tipoAutorizacionRepository.Delete(tipoAutorizacion);

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
