using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class EstadoPrestamoBusiness : IEstadoPrestamoBusiness
	{
		private readonly IEstadoPrestamoRepository estadoPrestamoRepository;

		public EstadoPrestamoBusiness(
			IEstadoPrestamoRepository estadoPrestamoRepository
			)
		{
			this.estadoPrestamoRepository = estadoPrestamoRepository;
		}
		public EstadoPrestamoDTO GetById(int id)
		{
			try
			{
				EstadoPrestamo estadoPrestamo = estadoPrestamoRepository.GetById(id);

				EstadoPrestamoDTO estadoPrestamoDTO = Mapper.Map<EstadoPrestamoDTO>(estadoPrestamo);

				return estadoPrestamoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<EstadoPrestamoDTO> Search(EstadoPrestamoDTO dto)
		{
			try
			{
				List<EstadoPrestamo> estadoPrestamoList = estadoPrestamoRepository.Search(Mapper.Map<EstadoPrestamo>(dto));

				List<EstadoPrestamoDTO> estadoPrestamoDTOList = Mapper.Map<List<EstadoPrestamoDTO>>(estadoPrestamoList);

				return estadoPrestamoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public EstadoPrestamoDTO SingleSearch(EstadoPrestamoDTO dto)
		{
			try
			{
				EstadoPrestamo estadoPrestamo = estadoPrestamoRepository.SingleSearch(Mapper.Map<EstadoPrestamo>(dto));

				EstadoPrestamoDTO estadoPrestamoDTO = Mapper.Map<EstadoPrestamoDTO>(estadoPrestamo);

				return estadoPrestamoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(EstadoPrestamoDTO dto)
		{
			try
			{
				EstadoPrestamo estadoPrestamo = Mapper.Map<EstadoPrestamo>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = estadoPrestamoRepository.Insert(estadoPrestamo);

					dto.EstadoPrestamoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(EstadoPrestamoDTO dto)
		{
			try
			{
				EstadoPrestamo estadoPrestamo = Mapper.Map<EstadoPrestamo>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = estadoPrestamoRepository.Update(estadoPrestamo);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(EstadoPrestamoDTO dto)
		{
			try
			{
				EstadoPrestamo estadoPrestamo = Mapper.Map<EstadoPrestamo>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					estadoPrestamoRepository.Delete(estadoPrestamo);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public EstadoPrestamoDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				EstadoPrestamo estadoPrestamo = estadoPrestamoRepository.ObtenerPorCodigo(codigo);

				EstadoPrestamoDTO estadoPrestamoDTO = Mapper.Map<EstadoPrestamoDTO>(estadoPrestamo);

				return estadoPrestamoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
