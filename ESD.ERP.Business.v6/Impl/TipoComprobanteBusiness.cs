using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TipoComprobanteBusiness : ITipoComprobanteBusiness
	{
		private readonly ITipoComprobanteRepository tipoComprobanteRepository;

		public TipoComprobanteBusiness(
			ITipoComprobanteRepository tipoComprobanteRepository
			)
		{
			this.tipoComprobanteRepository = tipoComprobanteRepository;
		}
		public TipoComprobanteDTO GetById(int id)
		{
			try
			{
				TipoComprobante tipoComprobante = tipoComprobanteRepository.GetById(id);

				TipoComprobanteDTO tipoComprobanteDTO = Mapper.Map<TipoComprobanteDTO>(tipoComprobante);

				return tipoComprobanteDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoComprobanteDTO> Search(TipoComprobanteDTO dto)
		{
			try
			{
				List<TipoComprobante> tipoComprobanteList = tipoComprobanteRepository.Search(Mapper.Map<TipoComprobante>(dto));

				List<TipoComprobanteDTO> tipoComprobanteDTOList = Mapper.Map<List<TipoComprobanteDTO>>(tipoComprobanteList);

				return tipoComprobanteDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoComprobanteDTO SingleSearch(TipoComprobanteDTO dto)
		{
			try
			{
				TipoComprobante tipoComprobante = tipoComprobanteRepository.SingleSearch(Mapper.Map<TipoComprobante>(dto));

				TipoComprobanteDTO tipoComprobanteDTO = Mapper.Map<TipoComprobanteDTO>(tipoComprobante);

				return tipoComprobanteDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoComprobanteDTO dto)
		{
			try
			{
				TipoComprobante tipoComprobante = Mapper.Map<TipoComprobante>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = tipoComprobanteRepository.Insert(tipoComprobante);

					dto.TipoComprobanteId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoComprobanteDTO dto)
		{
			try
			{
				TipoComprobante tipoComprobante = Mapper.Map<TipoComprobante>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = tipoComprobanteRepository.Update(tipoComprobante);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoComprobanteDTO dto)
		{
			try
			{
				TipoComprobante tipoComprobante = Mapper.Map<TipoComprobante>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					tipoComprobanteRepository.Delete(tipoComprobante);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoComprobanteDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				TipoComprobante tipoComprobante = tipoComprobanteRepository.ObtenerPorCodigo(codigo);

				TipoComprobanteDTO tipoComprobanteDTO = Mapper.Map<TipoComprobanteDTO>(tipoComprobante);

				return tipoComprobanteDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
