using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class EstadoAperturaCierreBusiness : IEstadoAperturaCierreBusiness
	{
		private readonly IEstadoAperturaCierreRepository estadoAperturaCierreRepository;

		public EstadoAperturaCierreBusiness(
			IEstadoAperturaCierreRepository estadoAperturaCierreRepository
			)
		{
			this.estadoAperturaCierreRepository = estadoAperturaCierreRepository;
		}
		public EstadoAperturaCierreDTO GetById(int id)
		{
			try
			{
				EstadoAperturaCierre estadoAperturaCierre = estadoAperturaCierreRepository.GetById(id);

				EstadoAperturaCierreDTO estadoAperturaCierreDTO = Mapper.Map<EstadoAperturaCierreDTO>(estadoAperturaCierre);

				return estadoAperturaCierreDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<EstadoAperturaCierreDTO> Search(EstadoAperturaCierreDTO dto)
		{
			try
			{
				List<EstadoAperturaCierre> estadoAperturaCierreList = estadoAperturaCierreRepository.Search(Mapper.Map<EstadoAperturaCierre>(dto));

				List<EstadoAperturaCierreDTO> estadoAperturaCierreDTOList = Mapper.Map<List<EstadoAperturaCierreDTO>>(estadoAperturaCierreList);

				return estadoAperturaCierreDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public EstadoAperturaCierreDTO SingleSearch(EstadoAperturaCierreDTO dto)
		{
			try
			{
				EstadoAperturaCierre estadoAperturaCierre = estadoAperturaCierreRepository.SingleSearch(Mapper.Map<EstadoAperturaCierre>(dto));

				EstadoAperturaCierreDTO estadoAperturaCierreDTO = Mapper.Map<EstadoAperturaCierreDTO>(estadoAperturaCierre);

				return estadoAperturaCierreDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(EstadoAperturaCierreDTO dto)
		{
			try
			{
				EstadoAperturaCierre estadoAperturaCierre = Mapper.Map<EstadoAperturaCierre>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = estadoAperturaCierreRepository.Insert(estadoAperturaCierre);

					dto.EstadoAperturaCierreId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(EstadoAperturaCierreDTO dto)
		{
			try
			{
				EstadoAperturaCierre estadoAperturaCierre = Mapper.Map<EstadoAperturaCierre>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = estadoAperturaCierreRepository.Update(estadoAperturaCierre);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(EstadoAperturaCierreDTO dto)
		{
			try
			{
				EstadoAperturaCierre estadoAperturaCierre = Mapper.Map<EstadoAperturaCierre>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					estadoAperturaCierreRepository.Delete(estadoAperturaCierre);

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
