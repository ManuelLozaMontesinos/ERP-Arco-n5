using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TipoCondicionPagoBusiness : ITipoCondicionPagoBusiness
	{
		private readonly ITipoCondicionPagoRepository tipoCondicionPagoRepository;

		public TipoCondicionPagoBusiness(
			ITipoCondicionPagoRepository tipoCondicionPagoRepository
			)
		{
			this.tipoCondicionPagoRepository = tipoCondicionPagoRepository;
		}
		public TipoCondicionPagoDTO GetById(int id)
		{
			try
			{
				TipoCondicionPago tipoCondicionPago = tipoCondicionPagoRepository.GetById(id);

				TipoCondicionPagoDTO tipoCondicionPagoDTO = Mapper.Map<TipoCondicionPagoDTO>(tipoCondicionPago);

				return tipoCondicionPagoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoCondicionPagoDTO> Search(TipoCondicionPagoDTO dto)
		{
			try
			{
				List<TipoCondicionPago> tipoCondicionPagoList = tipoCondicionPagoRepository.Search(Mapper.Map<TipoCondicionPago>(dto));

				List<TipoCondicionPagoDTO> tipoCondicionPagoDTOList = Mapper.Map<List<TipoCondicionPagoDTO>>(tipoCondicionPagoList);

				return tipoCondicionPagoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoCondicionPagoDTO SingleSearch(TipoCondicionPagoDTO dto)
		{
			try
			{
				TipoCondicionPago tipoCondicionPago = tipoCondicionPagoRepository.SingleSearch(Mapper.Map<TipoCondicionPago>(dto));

				TipoCondicionPagoDTO tipoCondicionPagoDTO = Mapper.Map<TipoCondicionPagoDTO>(tipoCondicionPago);

				return tipoCondicionPagoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoCondicionPagoDTO dto)
		{
			try
			{
				TipoCondicionPago tipoCondicionPago = Mapper.Map<TipoCondicionPago>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = tipoCondicionPagoRepository.Insert(tipoCondicionPago);

					dto.TipoCondicionPagoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoCondicionPagoDTO dto)
		{
			try
			{
				TipoCondicionPago tipoCondicionPago = Mapper.Map<TipoCondicionPago>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = tipoCondicionPagoRepository.Update(tipoCondicionPago);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoCondicionPagoDTO dto)
		{
			try
			{
				TipoCondicionPago tipoCondicionPago = Mapper.Map<TipoCondicionPago>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					tipoCondicionPagoRepository.Delete(tipoCondicionPago);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoCondicionPagoDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				TipoCondicionPago tipoCondicionPago = tipoCondicionPagoRepository.ObtenerPorCodigo(codigo);

				TipoCondicionPagoDTO tipoCondicionPagoDTO = Mapper.Map<TipoCondicionPagoDTO>(tipoCondicionPago);

				return tipoCondicionPagoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
