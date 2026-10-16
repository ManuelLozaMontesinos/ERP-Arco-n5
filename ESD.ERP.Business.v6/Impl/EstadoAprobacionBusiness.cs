using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class EstadoAprobacionBusiness : IEstadoAprobacionBusiness
	{
		private readonly IEstadoAprobacionRepository estadoAprobacionRepository;

		public EstadoAprobacionBusiness(
			IEstadoAprobacionRepository estadoAprobacionRepository
			)
		{
			this.estadoAprobacionRepository = estadoAprobacionRepository;
		}
		public EstadoAprobacionDTO GetById(int id)
		{
			try
			{
				EstadoAprobacion estadoAprobacion = estadoAprobacionRepository.GetById(id);

				EstadoAprobacionDTO estadoAprobacionDTO = Mapper.Map<EstadoAprobacionDTO>(estadoAprobacion);

				return estadoAprobacionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<EstadoAprobacionDTO> Search(EstadoAprobacionDTO dto)
		{
			try
			{
				List<EstadoAprobacion> estadoAprobacionList = estadoAprobacionRepository.Search(Mapper.Map<EstadoAprobacion>(dto));

				List<EstadoAprobacionDTO> estadoAprobacionDTOList = Mapper.Map<List<EstadoAprobacionDTO>>(estadoAprobacionList);

				return estadoAprobacionDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public EstadoAprobacionDTO SingleSearch(EstadoAprobacionDTO dto)
		{
			try
			{
				EstadoAprobacion estadoAprobacion = estadoAprobacionRepository.SingleSearch(Mapper.Map<EstadoAprobacion>(dto));

				EstadoAprobacionDTO estadoAprobacionDTO = Mapper.Map<EstadoAprobacionDTO>(estadoAprobacion);

				return estadoAprobacionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(EstadoAprobacionDTO dto)
		{
			try
			{
				EstadoAprobacion estadoAprobacion = Mapper.Map<EstadoAprobacion>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = estadoAprobacionRepository.Insert(estadoAprobacion);

					dto.EstadoAprobacionId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(EstadoAprobacionDTO dto)
		{
			try
			{
				EstadoAprobacion estadoAprobacion = Mapper.Map<EstadoAprobacion>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = estadoAprobacionRepository.Update(estadoAprobacion);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(EstadoAprobacionDTO dto)
		{
			try
			{
				EstadoAprobacion estadoAprobacion = Mapper.Map<EstadoAprobacion>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					estadoAprobacionRepository.Delete(estadoAprobacion);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public EstadoAprobacionDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				EstadoAprobacion estadoAprobacion = estadoAprobacionRepository.ObtenerPorCodigo(codigo);

				EstadoAprobacionDTO estadoAprobacionDTO = Mapper.Map<EstadoAprobacionDTO>(estadoAprobacion);

				return estadoAprobacionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
