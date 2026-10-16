using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class EstadoVacacionBusiness : IEstadoVacacionBusiness
	{
		private readonly IEstadoVacacionRepository estadoVacacionRepository;

		public EstadoVacacionBusiness(
			IEstadoVacacionRepository estadoVacacionRepository
			)
		{
			this.estadoVacacionRepository = estadoVacacionRepository;
		}
		public EstadoVacacionDTO GetById(int id)
		{
			try
			{
				EstadoVacacion estadoVacacion = estadoVacacionRepository.GetById(id);

				EstadoVacacionDTO estadoVacacionDTO = Mapper.Map<EstadoVacacionDTO>(estadoVacacion);

				return estadoVacacionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<EstadoVacacionDTO> Search(EstadoVacacionDTO dto)
		{
			try
			{
				List<EstadoVacacion> estadoVacacionList = estadoVacacionRepository.Search(Mapper.Map<EstadoVacacion>(dto));

				List<EstadoVacacionDTO> estadoVacacionDTOList = Mapper.Map<List<EstadoVacacionDTO>>(estadoVacacionList);

				return estadoVacacionDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public EstadoVacacionDTO SingleSearch(EstadoVacacionDTO dto)
		{
			try
			{
				EstadoVacacion estadoVacacion = estadoVacacionRepository.SingleSearch(Mapper.Map<EstadoVacacion>(dto));

				EstadoVacacionDTO estadoVacacionDTO = Mapper.Map<EstadoVacacionDTO>(estadoVacacion);

				return estadoVacacionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(EstadoVacacionDTO dto)
		{
			try
			{
				EstadoVacacion estadoVacacion = Mapper.Map<EstadoVacacion>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = estadoVacacionRepository.Insert(estadoVacacion);

					dto.EstadoVacacionId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(EstadoVacacionDTO dto)
		{
			try
			{
				EstadoVacacion estadoVacacion = Mapper.Map<EstadoVacacion>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = estadoVacacionRepository.Update(estadoVacacion);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(EstadoVacacionDTO dto)
		{
			try
			{
				EstadoVacacion estadoVacacion = Mapper.Map<EstadoVacacion>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					estadoVacacionRepository.Delete(estadoVacacion);

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
