using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Business.Resources;
using ESD.ERP.Common;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class MovimientoAlmacenSaldoConsolidadoBusiness : IMovimientoAlmacenSaldoConsolidadoBusiness
	{
		private readonly IMovimientoAlmacenSaldoConsolidadoRepository movimientoAlmacenSaldoConsolidadoRepository;

		public MovimientoAlmacenSaldoConsolidadoBusiness(
			IMovimientoAlmacenSaldoConsolidadoRepository movimientoAlmacenSaldoConsolidadoRepository
			)
		{
			this.movimientoAlmacenSaldoConsolidadoRepository = movimientoAlmacenSaldoConsolidadoRepository;
		}
		public MovimientoAlmacenSaldoConsolidadoDTO GetById(int id)
		{
			try
			{
				MovimientoAlmacenSaldoConsolidado movimientoAlmacenSaldoConsolidado = movimientoAlmacenSaldoConsolidadoRepository.GetById(id);

				MovimientoAlmacenSaldoConsolidadoDTO movimientoAlmacenSaldoConsolidadoDTO = Mapper.Map<MovimientoAlmacenSaldoConsolidadoDTO>(movimientoAlmacenSaldoConsolidado);

				return movimientoAlmacenSaldoConsolidadoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<MovimientoAlmacenSaldoConsolidadoDTO> Search(MovimientoAlmacenSaldoConsolidadoDTO dto)
		{
			try
			{
				List<MovimientoAlmacenSaldoConsolidado> movimientoAlmacenSaldoConsolidadoList = movimientoAlmacenSaldoConsolidadoRepository.Search(Mapper.Map<MovimientoAlmacenSaldoConsolidado>(dto));

				List<MovimientoAlmacenSaldoConsolidadoDTO> movimientoAlmacenSaldoConsolidadoDTOList = Mapper.Map<List<MovimientoAlmacenSaldoConsolidadoDTO>>(movimientoAlmacenSaldoConsolidadoList);

				return movimientoAlmacenSaldoConsolidadoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public MovimientoAlmacenSaldoConsolidadoDTO SingleSearch(MovimientoAlmacenSaldoConsolidadoDTO dto)
		{
			try
			{
				MovimientoAlmacenSaldoConsolidado movimientoAlmacenSaldoConsolidado = movimientoAlmacenSaldoConsolidadoRepository.SingleSearch(Mapper.Map<MovimientoAlmacenSaldoConsolidado>(dto));

				MovimientoAlmacenSaldoConsolidadoDTO movimientoAlmacenSaldoConsolidadoDTO = Mapper.Map<MovimientoAlmacenSaldoConsolidadoDTO>(movimientoAlmacenSaldoConsolidado);

				return movimientoAlmacenSaldoConsolidadoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(MovimientoAlmacenSaldoConsolidadoDTO dto)
		{
			try
			{
				MovimientoAlmacenSaldoConsolidado movimientoAlmacenSaldoConsolidado = Mapper.Map<MovimientoAlmacenSaldoConsolidado>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = movimientoAlmacenSaldoConsolidadoRepository.Insert(movimientoAlmacenSaldoConsolidado);

					dto.MovimientoAlmacenSaldoConsolidadoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(MovimientoAlmacenSaldoConsolidadoDTO dto)
		{
			try
			{
				MovimientoAlmacenSaldoConsolidado movimientoAlmacenSaldoConsolidado = Mapper.Map<MovimientoAlmacenSaldoConsolidado>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = movimientoAlmacenSaldoConsolidadoRepository.Update(movimientoAlmacenSaldoConsolidado);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(MovimientoAlmacenSaldoConsolidadoDTO dto)
		{
			try
			{
				MovimientoAlmacenSaldoConsolidado movimientoAlmacenSaldoConsolidado = Mapper.Map<MovimientoAlmacenSaldoConsolidado>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					movimientoAlmacenSaldoConsolidadoRepository.Delete(movimientoAlmacenSaldoConsolidado);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Valorizar(MovimientoAlmacenSaldoConsolidadoDTO dto)
		{
			try
			{
				MovimientoAlmacenSaldoConsolidado movimientoAlmacenSaldoConsolidado = Mapper.Map<MovimientoAlmacenSaldoConsolidado>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					movimientoAlmacenSaldoConsolidadoRepository.Valorizar(movimientoAlmacenSaldoConsolidado);

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
