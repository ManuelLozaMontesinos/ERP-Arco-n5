using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TipoNotaCreditoBusiness : ITipoNotaCreditoBusiness
	{
		private readonly ITipoNotaCreditoRepository tipoNotaCreditoRepository;

		public TipoNotaCreditoBusiness(
			ITipoNotaCreditoRepository tipoNotaCreditoRepository
			)
		{
			this.tipoNotaCreditoRepository = tipoNotaCreditoRepository;
		}
		public TipoNotaCreditoDTO GetById(int id)
		{
			try
			{
				TipoNotaCredito tipoNotaCredito = tipoNotaCreditoRepository.GetById(id);

				TipoNotaCreditoDTO tipoNotaCreditoDTO = Mapper.Map<TipoNotaCreditoDTO>(tipoNotaCredito);

				return tipoNotaCreditoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoNotaCreditoDTO> Search(TipoNotaCreditoDTO dto)
		{
			try
			{
				List<TipoNotaCredito> tipoNotaCreditoList = tipoNotaCreditoRepository.Search(Mapper.Map<TipoNotaCredito>(dto));

				List<TipoNotaCreditoDTO> tipoNotaCreditoDTOList = Mapper.Map<List<TipoNotaCreditoDTO>>(tipoNotaCreditoList);

				return tipoNotaCreditoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoNotaCreditoDTO SingleSearch(TipoNotaCreditoDTO dto)
		{
			try
			{
				TipoNotaCredito tipoNotaCredito = tipoNotaCreditoRepository.SingleSearch(Mapper.Map<TipoNotaCredito>(dto));

				TipoNotaCreditoDTO tipoNotaCreditoDTO = Mapper.Map<TipoNotaCreditoDTO>(tipoNotaCredito);

				return tipoNotaCreditoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoNotaCreditoDTO dto)
		{
			try
			{
				TipoNotaCredito tipoNotaCredito = Mapper.Map<TipoNotaCredito>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = tipoNotaCreditoRepository.Insert(tipoNotaCredito);

					dto.TipoNotaCreditoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoNotaCreditoDTO dto)
		{
			try
			{
				TipoNotaCredito tipoNotaCredito = Mapper.Map<TipoNotaCredito>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = tipoNotaCreditoRepository.Update(tipoNotaCredito);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoNotaCreditoDTO dto)
		{
			try
			{
				TipoNotaCredito tipoNotaCredito = Mapper.Map<TipoNotaCredito>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					tipoNotaCreditoRepository.Delete(tipoNotaCredito);

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
