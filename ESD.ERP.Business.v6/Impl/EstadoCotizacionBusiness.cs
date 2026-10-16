using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class EstadoCotizacionBusiness : IEstadoCotizacionBusiness
	{
		private readonly IEstadoCotizacionRepository estadoCotizacionRepository;

		public EstadoCotizacionBusiness(
			IEstadoCotizacionRepository estadoCotizacionRepository
			)
		{
			this.estadoCotizacionRepository = estadoCotizacionRepository;
		}
		public EstadoCotizacionDTO GetById(int id)
		{
			try
			{
				EstadoCotizacion estadoCotizacion = estadoCotizacionRepository.GetById(id);

				EstadoCotizacionDTO estadoCotizacionDTO = Mapper.Map<EstadoCotizacionDTO>(estadoCotizacion);

				return estadoCotizacionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<EstadoCotizacionDTO> Search(EstadoCotizacionDTO dto)
		{
			try
			{
				List<EstadoCotizacion> estadoCotizacionList = estadoCotizacionRepository.Search(Mapper.Map<EstadoCotizacion>(dto));

				List<EstadoCotizacionDTO> estadoCotizacionDTOList = Mapper.Map<List<EstadoCotizacionDTO>>(estadoCotizacionList);

				return estadoCotizacionDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public EstadoCotizacionDTO SingleSearch(EstadoCotizacionDTO dto)
		{
			try
			{
				EstadoCotizacion estadoCotizacion = estadoCotizacionRepository.SingleSearch(Mapper.Map<EstadoCotizacion>(dto));

				EstadoCotizacionDTO estadoCotizacionDTO = Mapper.Map<EstadoCotizacionDTO>(estadoCotizacion);

				return estadoCotizacionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(EstadoCotizacionDTO dto)
		{
			try
			{
				EstadoCotizacion estadoCotizacion = Mapper.Map<EstadoCotizacion>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = estadoCotizacionRepository.Insert(estadoCotizacion);

					dto.EstadoCotizacionId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(EstadoCotizacionDTO dto)
		{
			try
			{
				EstadoCotizacion estadoCotizacion = Mapper.Map<EstadoCotizacion>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = estadoCotizacionRepository.Update(estadoCotizacion);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(EstadoCotizacionDTO dto)
		{
			try
			{
				EstadoCotizacion estadoCotizacion = Mapper.Map<EstadoCotizacion>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					estadoCotizacionRepository.Delete(estadoCotizacion);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public EstadoCotizacionDTO ObtenerPorCodigo(string codigo)
      {
			try
			{
				EstadoCotizacion estadoCotizacion = estadoCotizacionRepository.ObtenerPorCodigo(codigo);

				EstadoCotizacionDTO estadoCotizacionDTO = Mapper.Map<EstadoCotizacionDTO>(estadoCotizacion);

				return estadoCotizacionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
   }
}
