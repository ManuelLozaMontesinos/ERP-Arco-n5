using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class EstadoMovimientoAlmacenBusiness : IEstadoMovimientoAlmacenBusiness
	{
		private readonly IEstadoMovimientoAlmacenRepository estadoMovimientoAlmacenRepository;

		public EstadoMovimientoAlmacenBusiness(
			IEstadoMovimientoAlmacenRepository estadoMovimientoAlmacenRepository
			)
		{
			this.estadoMovimientoAlmacenRepository = estadoMovimientoAlmacenRepository;
		}
		public EstadoMovimientoAlmacenDTO GetById(int id)
		{
			try
			{
				EstadoMovimientoAlmacen estadoMovimientoAlmacen = estadoMovimientoAlmacenRepository.GetById(id);

				EstadoMovimientoAlmacenDTO estadoMovimientoAlmacenDTO = Mapper.Map<EstadoMovimientoAlmacenDTO>(estadoMovimientoAlmacen);

				return estadoMovimientoAlmacenDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<EstadoMovimientoAlmacenDTO> Search(EstadoMovimientoAlmacenDTO dto)
		{
			try
			{
				List<EstadoMovimientoAlmacen> estadoMovimientoAlmacenList = estadoMovimientoAlmacenRepository.Search(Mapper.Map<EstadoMovimientoAlmacen>(dto));

				List<EstadoMovimientoAlmacenDTO> estadoMovimientoAlmacenDTOList = Mapper.Map<List<EstadoMovimientoAlmacenDTO>>(estadoMovimientoAlmacenList);

				return estadoMovimientoAlmacenDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public EstadoMovimientoAlmacenDTO SingleSearch(EstadoMovimientoAlmacenDTO dto)
		{
			try
			{
				EstadoMovimientoAlmacen estadoMovimientoAlmacen = estadoMovimientoAlmacenRepository.SingleSearch(Mapper.Map<EstadoMovimientoAlmacen>(dto));

				EstadoMovimientoAlmacenDTO estadoMovimientoAlmacenDTO = Mapper.Map<EstadoMovimientoAlmacenDTO>(estadoMovimientoAlmacen);

				return estadoMovimientoAlmacenDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(EstadoMovimientoAlmacenDTO dto)
		{
			try
			{
				EstadoMovimientoAlmacen estadoMovimientoAlmacen = Mapper.Map<EstadoMovimientoAlmacen>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = estadoMovimientoAlmacenRepository.Insert(estadoMovimientoAlmacen);

					dto.EstadoMovimientoAlmacenId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(EstadoMovimientoAlmacenDTO dto)
		{
			try
			{
				EstadoMovimientoAlmacen estadoMovimientoAlmacen = Mapper.Map<EstadoMovimientoAlmacen>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = estadoMovimientoAlmacenRepository.Update(estadoMovimientoAlmacen);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(EstadoMovimientoAlmacenDTO dto)
		{
			try
			{
				EstadoMovimientoAlmacen estadoMovimientoAlmacen = Mapper.Map<EstadoMovimientoAlmacen>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					estadoMovimientoAlmacenRepository.Delete(estadoMovimientoAlmacen);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public EstadoMovimientoAlmacenDTO ObtenerPorCodigo(string codigo)
		{
			try
			{
				EstadoMovimientoAlmacen estadoMovimientoAlmacen = estadoMovimientoAlmacenRepository.ObtenerPorCodigo(codigo);

				EstadoMovimientoAlmacenDTO estadoMovimientoAlmacenDTO = Mapper.Map<EstadoMovimientoAlmacenDTO>(estadoMovimientoAlmacen);

				return estadoMovimientoAlmacenDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
