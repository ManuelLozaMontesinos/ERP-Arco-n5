using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class MovimientoAlmacenCorrelativoBusiness : IMovimientoAlmacenCorrelativoBusiness
	{
		private readonly IMovimientoAlmacenCorrelativoRepository movimientoAlmacenCorrelativoRepository;
		private readonly ITipoMovimientoAlmacenRepository tipoMovimientoAlmacenRepository;

		public MovimientoAlmacenCorrelativoBusiness(
			IMovimientoAlmacenCorrelativoRepository movimientoAlmacenCorrelativoRepository,
			ITipoMovimientoAlmacenRepository tipoMovimientoAlmacenRepository
			)
		{
			this.movimientoAlmacenCorrelativoRepository = movimientoAlmacenCorrelativoRepository;
			this.tipoMovimientoAlmacenRepository = tipoMovimientoAlmacenRepository;
		}
		public MovimientoAlmacenCorrelativoDTO GetById(int id)
		{
			try
			{
				MovimientoAlmacenCorrelativo movimientoAlmacenCorrelativo = movimientoAlmacenCorrelativoRepository.GetById(id);

				MovimientoAlmacenCorrelativoDTO movimientoAlmacenCorrelativoDTO = Mapper.Map<MovimientoAlmacenCorrelativoDTO>(movimientoAlmacenCorrelativo);

				return movimientoAlmacenCorrelativoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<MovimientoAlmacenCorrelativoDTO> Search(MovimientoAlmacenCorrelativoDTO dto)
		{
			try
			{
				List<MovimientoAlmacenCorrelativo> movimientoAlmacenCorrelativoList = movimientoAlmacenCorrelativoRepository.Search(Mapper.Map<MovimientoAlmacenCorrelativo>(dto));

				List<MovimientoAlmacenCorrelativoDTO> movimientoAlmacenCorrelativoDTOList = Mapper.Map<List<MovimientoAlmacenCorrelativoDTO>>(movimientoAlmacenCorrelativoList);

				return movimientoAlmacenCorrelativoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public MovimientoAlmacenCorrelativoDTO SingleSearch(MovimientoAlmacenCorrelativoDTO dto)
		{
			try
			{
				MovimientoAlmacenCorrelativo movimientoAlmacenCorrelativo = movimientoAlmacenCorrelativoRepository.SingleSearch(Mapper.Map<MovimientoAlmacenCorrelativo>(dto));

				MovimientoAlmacenCorrelativoDTO movimientoAlmacenCorrelativoDTO = Mapper.Map<MovimientoAlmacenCorrelativoDTO>(movimientoAlmacenCorrelativo);

				return movimientoAlmacenCorrelativoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(MovimientoAlmacenCorrelativoDTO dto)
		{
			try
			{
				MovimientoAlmacenCorrelativo movimientoAlmacenCorrelativo = Mapper.Map<MovimientoAlmacenCorrelativo>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = movimientoAlmacenCorrelativoRepository.Insert(movimientoAlmacenCorrelativo);

					dto.MovimientoAlmacenCorrelativoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(MovimientoAlmacenCorrelativoDTO dto)
		{
			try
			{
				MovimientoAlmacenCorrelativo movimientoAlmacenCorrelativo = Mapper.Map<MovimientoAlmacenCorrelativo>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = movimientoAlmacenCorrelativoRepository.Update(movimientoAlmacenCorrelativo);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(MovimientoAlmacenCorrelativoDTO dto)
		{
			try
			{
				MovimientoAlmacenCorrelativo movimientoAlmacenCorrelativo = Mapper.Map<MovimientoAlmacenCorrelativo>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					movimientoAlmacenCorrelativoRepository.Delete(movimientoAlmacenCorrelativo);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public string ObtenerCorrelativo(MovimientoAlmacenCorrelativoDTO dto)
		{

			bool existeCorrelativo = true;

			MovimientoAlmacenCorrelativo movimientoAlmacenCorrelativo = movimientoAlmacenCorrelativoRepository.SingleSearch(Mapper.Map<MovimientoAlmacenCorrelativo>(dto));

			if (movimientoAlmacenCorrelativo == null) {
				movimientoAlmacenCorrelativo = Mapper.Map<MovimientoAlmacenCorrelativo>(dto);

				TipoMovimientoAlmacen tipoMovimientoAlmacen = tipoMovimientoAlmacenRepository.GetById(dto.TipoMovimientoAlmacenId);
				movimientoAlmacenCorrelativo.TipoMovimientoAlmacen = tipoMovimientoAlmacen;

				existeCorrelativo = false;
			}

			movimientoAlmacenCorrelativo.Correlativo += 1;
			int correlativo = movimientoAlmacenCorrelativo.Correlativo;

			if (!existeCorrelativo)
				movimientoAlmacenCorrelativoRepository.Insert(movimientoAlmacenCorrelativo);
			else
				movimientoAlmacenCorrelativoRepository.Update(movimientoAlmacenCorrelativo);

			return string.Concat(movimientoAlmacenCorrelativo.TipoMovimientoAlmacen.Codigo,
				correlativo.ToString("D9"));
		}
	}
}
