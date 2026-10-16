using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class EstadoComprobanteBusiness : IEstadoComprobanteBusiness
	{
		private readonly IEstadoComprobanteRepository estadoComprobanteRepository;

		public EstadoComprobanteBusiness(
			IEstadoComprobanteRepository estadoComprobanteRepository
			)
		{
			this.estadoComprobanteRepository = estadoComprobanteRepository;
		}
		public EstadoComprobanteDTO GetById(int id)
		{
			try
			{
				EstadoComprobante estadoComprobante = estadoComprobanteRepository.GetById(id);

				EstadoComprobanteDTO estadoComprobanteDTO = Mapper.Map<EstadoComprobanteDTO>(estadoComprobante);

				return estadoComprobanteDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<EstadoComprobanteDTO> Search(EstadoComprobanteDTO dto)
		{
			try
			{
				List<EstadoComprobante> estadoComprobanteList = estadoComprobanteRepository.Search(Mapper.Map<EstadoComprobante>(dto));

				List<EstadoComprobanteDTO> estadoComprobanteDTOList = Mapper.Map<List<EstadoComprobanteDTO>>(estadoComprobanteList);

				return estadoComprobanteDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public EstadoComprobanteDTO SingleSearch(EstadoComprobanteDTO dto)
		{
			try
			{
				EstadoComprobante estadoComprobante = estadoComprobanteRepository.SingleSearch(Mapper.Map<EstadoComprobante>(dto));

				EstadoComprobanteDTO estadoComprobanteDTO = Mapper.Map<EstadoComprobanteDTO>(estadoComprobante);

				return estadoComprobanteDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(EstadoComprobanteDTO dto)
		{
			try
			{
				EstadoComprobante estadoComprobante = Mapper.Map<EstadoComprobante>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = estadoComprobanteRepository.Insert(estadoComprobante);

					dto.EstadoComprobanteId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(EstadoComprobanteDTO dto)
		{
			try
			{
				EstadoComprobante estadoComprobante = Mapper.Map<EstadoComprobante>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = estadoComprobanteRepository.Update(estadoComprobante);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(EstadoComprobanteDTO dto)
		{
			try
			{
				EstadoComprobante estadoComprobante = Mapper.Map<EstadoComprobante>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					estadoComprobanteRepository.Delete(estadoComprobante);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public EstadoComprobanteDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				EstadoComprobante estadoComprobante = estadoComprobanteRepository.ObtenerPorCodigo(codigo);

				EstadoComprobanteDTO estadoComprobanteDTO = Mapper.Map<EstadoComprobanteDTO>(estadoComprobante);

				return estadoComprobanteDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
