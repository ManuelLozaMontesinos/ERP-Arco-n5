using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class EstadoSunatBusiness : IEstadoSunatBusiness
	{
		private readonly IEstadoSunatRepository estadoSunatRepository;

		public EstadoSunatBusiness(
			IEstadoSunatRepository estadoSunatRepository
			)
		{
			this.estadoSunatRepository = estadoSunatRepository;
		}
		public EstadoSunatDTO GetById(int id)
		{
			try
			{
				EstadoSunat estadoSunat = estadoSunatRepository.GetById(id);

				EstadoSunatDTO estadoSunatDTO = Mapper.Map<EstadoSunatDTO>(estadoSunat);

				return estadoSunatDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<EstadoSunatDTO> Search(EstadoSunatDTO dto)
		{
			try
			{
				List<EstadoSunat> estadoSunatList = estadoSunatRepository.Search(Mapper.Map<EstadoSunat>(dto));

				List<EstadoSunatDTO> estadoSunatDTOList = Mapper.Map<List<EstadoSunatDTO>>(estadoSunatList);

				return estadoSunatDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public EstadoSunatDTO SingleSearch(EstadoSunatDTO dto)
		{
			try
			{
				EstadoSunat estadoSunat = estadoSunatRepository.SingleSearch(Mapper.Map<EstadoSunat>(dto));

				EstadoSunatDTO estadoSunatDTO = Mapper.Map<EstadoSunatDTO>(estadoSunat);

				return estadoSunatDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(EstadoSunatDTO dto)
		{
			try
			{
				EstadoSunat estadoSunat = Mapper.Map<EstadoSunat>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = estadoSunatRepository.Insert(estadoSunat);

					dto.EstadoSunatId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(EstadoSunatDTO dto)
		{
			try
			{
				EstadoSunat estadoSunat = Mapper.Map<EstadoSunat>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = estadoSunatRepository.Update(estadoSunat);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(EstadoSunatDTO dto)
		{
			try
			{
				EstadoSunat estadoSunat = Mapper.Map<EstadoSunat>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					estadoSunatRepository.Delete(estadoSunat);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public EstadoSunatDTO ObtenerPorCodigo(string codigo)
      {
			try
			{
				EstadoSunat estadoSunat = estadoSunatRepository.ObtenerPorCodigo(codigo);

				EstadoSunatDTO estadoSunatDTO = Mapper.Map<EstadoSunatDTO>(estadoSunat);

				return estadoSunatDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
   }
}
