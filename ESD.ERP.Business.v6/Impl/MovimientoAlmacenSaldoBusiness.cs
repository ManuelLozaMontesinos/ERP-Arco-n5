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
	public class MovimientoAlmacenSaldoBusiness : IMovimientoAlmacenSaldoBusiness
	{
		private readonly IMovimientoAlmacenSaldoRepository movimientoAlmacenSaldoRepository;

		public MovimientoAlmacenSaldoBusiness(
			IMovimientoAlmacenSaldoRepository movimientoAlmacenSaldoRepository
			)
		{
			this.movimientoAlmacenSaldoRepository = movimientoAlmacenSaldoRepository;
		}
		public MovimientoAlmacenSaldoDTO GetById(int id)
		{
			try
			{
				MovimientoAlmacenSaldo movimientoAlmacenSaldo = movimientoAlmacenSaldoRepository.GetById(id);

				MovimientoAlmacenSaldoDTO movimientoAlmacenSaldoDTO = Mapper.Map<MovimientoAlmacenSaldoDTO>(movimientoAlmacenSaldo);

				return movimientoAlmacenSaldoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<MovimientoAlmacenSaldoDTO> Search(MovimientoAlmacenSaldoDTO dto)
		{
			try
			{
				List<MovimientoAlmacenSaldo> movimientoAlmacenSaldoList = movimientoAlmacenSaldoRepository.Search(Mapper.Map<MovimientoAlmacenSaldo>(dto));

				List<MovimientoAlmacenSaldoDTO> movimientoAlmacenSaldoDTOList = Mapper.Map<List<MovimientoAlmacenSaldoDTO>>(movimientoAlmacenSaldoList);

				return movimientoAlmacenSaldoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public MovimientoAlmacenSaldoDTO SingleSearch(MovimientoAlmacenSaldoDTO dto)
		{
			try
			{
				MovimientoAlmacenSaldo movimientoAlmacenSaldo = movimientoAlmacenSaldoRepository.SingleSearch(Mapper.Map<MovimientoAlmacenSaldo>(dto));

				MovimientoAlmacenSaldoDTO movimientoAlmacenSaldoDTO = Mapper.Map<MovimientoAlmacenSaldoDTO>(movimientoAlmacenSaldo);

				return movimientoAlmacenSaldoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(MovimientoAlmacenSaldoDTO dto)
		{
			try
			{
				MovimientoAlmacenSaldo movimientoAlmacenSaldo = Mapper.Map<MovimientoAlmacenSaldo>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = movimientoAlmacenSaldoRepository.Insert(movimientoAlmacenSaldo);

					dto.MovimientoAlmacenSaldoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(MovimientoAlmacenSaldoDTO dto)
		{
			try
			{
				MovimientoAlmacenSaldo movimientoAlmacenSaldo = Mapper.Map<MovimientoAlmacenSaldo>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = movimientoAlmacenSaldoRepository.Update(movimientoAlmacenSaldo);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(MovimientoAlmacenSaldoDTO dto)
		{
			try
			{
				MovimientoAlmacenSaldo movimientoAlmacenSaldo = Mapper.Map<MovimientoAlmacenSaldo>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					movimientoAlmacenSaldoRepository.Delete(movimientoAlmacenSaldo);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public decimal ObtenerSaldoDisponible(MovimientoAlmacenProductoDTO dto)
      {
			decimal saldoDisponible = decimal.Zero;

			try
			{
				MovimientoAlmacenSaldo movimientoAlmacenSaldo = new MovimientoAlmacenSaldo()
				{
					PeriodoEmpresaSedeId = dto.MovimientoAlmacenDTO.PeriodoEmpresaSedeId,
					AlmacenId = dto.MovimientoAlmacenDTO.AlmacenId,
					ProductoId = dto.ProductoId,
					MesContableId = dto.MovimientoAlmacenDTO.MesContableId
				};

				movimientoAlmacenSaldo = movimientoAlmacenSaldoRepository.SingleSearch(movimientoAlmacenSaldo);
				if (movimientoAlmacenSaldo == null)
					return saldoDisponible;

				if(dto.MovimientoAlmacenDTO.TipoMovimientoAlmacenDTO.Codigo == TipoMovimientoAlmacenConstante.INGRESO)
            {
					saldoDisponible = ((movimientoAlmacenSaldo.CantidadAnterior + movimientoAlmacenSaldo.CantidadIngreso)
						- dto.Cantidad)
						- (movimientoAlmacenSaldo.CantidadSalida + movimientoAlmacenSaldo.CantidadReserva);
            }
            else
            {
					saldoDisponible = (movimientoAlmacenSaldo.CantidadAnterior + movimientoAlmacenSaldo.CantidadIngreso)
						- ((movimientoAlmacenSaldo.CantidadSalida + movimientoAlmacenSaldo.CantidadReserva)
						- dto.Cantidad);
				}

				return saldoDisponible;
			}
			catch (Exception)
			{
				throw;
			}

		}
		public decimal ObtenerSaldoVenta(MovimientoAlmacenSaldoDTO dto)
		{
			decimal saldoDisponible = decimal.Zero;

			try
			{
				MovimientoAlmacenSaldo movimientoAlmacenSaldo = new MovimientoAlmacenSaldo()
				{
					PeriodoEmpresaSedeId = dto.PeriodoEmpresaSedeId,
					ProductoId = dto.ProductoId,
					MesContableId = dto.MesContableId
				};

				movimientoAlmacenSaldo = movimientoAlmacenSaldoRepository.ObtenerPorEmpresaSede(movimientoAlmacenSaldo);
				if (movimientoAlmacenSaldo == null)
					return saldoDisponible;

				saldoDisponible = (movimientoAlmacenSaldo.CantidadAnterior + movimientoAlmacenSaldo.CantidadIngreso)
					- ((movimientoAlmacenSaldo.CantidadSalida + movimientoAlmacenSaldo.CantidadReserva)
					- dto.CantidadIngreso);

				return saldoDisponible;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void ValidarSaldoVenta(MovimientoAlmacenSaldoDTO dto)
		{
			decimal saldoVenta = decimal.Zero;

			try
			{
				MovimientoAlmacenSaldo movimientoAlmacenSaldo = new MovimientoAlmacenSaldo()
				{
					PeriodoEmpresaSedeId = dto.PeriodoEmpresaSedeId,
					ProductoId = dto.ProductoId,
					MesContableId = dto.MesContableId
				};

				movimientoAlmacenSaldo = movimientoAlmacenSaldoRepository.ObtenerPorEmpresaSede(movimientoAlmacenSaldo);
				if (movimientoAlmacenSaldo == null)
					throw new Exception(SR.GetString("EX_producto_sin_saldo2"));

				saldoVenta = (movimientoAlmacenSaldo.CantidadAnterior + movimientoAlmacenSaldo.CantidadIngreso)
					- ((movimientoAlmacenSaldo.CantidadSalida + movimientoAlmacenSaldo.CantidadReserva)
					- dto.CantidadAnterior);

				if(saldoVenta < dto.CantidadIngreso)
					throw new Exception(SR.GetString("EX_cantidad_mayor_al_saldo_en_producto", string.Format(StringFormatConstant.DECIMAL, saldoVenta)
						, movimientoAlmacenSaldo.Producto.Descripcion));
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void ValidarSaldoVentaCombo(List<MovimientoAlmacenSaldoDTO> list)
		{
			decimal saldoVenta = decimal.Zero;

			try
			{
				foreach(MovimientoAlmacenSaldoDTO movimientoAlmacenSaldoDTO in list)
            {
					MovimientoAlmacenSaldo movimientoAlmacenSaldo = new MovimientoAlmacenSaldo()
					{
						PeriodoEmpresaSedeId = movimientoAlmacenSaldoDTO.PeriodoEmpresaSedeId,
						ProductoId = movimientoAlmacenSaldoDTO.ProductoId,
						MesContableId = movimientoAlmacenSaldoDTO.MesContableId
					};

					movimientoAlmacenSaldo = movimientoAlmacenSaldoRepository.ObtenerPorEmpresaSede(movimientoAlmacenSaldo);
					if (movimientoAlmacenSaldo == null)
						throw new Exception(SR.GetString("EX_combo_producto_sin_saldo"));

					saldoVenta = (movimientoAlmacenSaldo.CantidadAnterior + movimientoAlmacenSaldo.CantidadIngreso)
						- ((movimientoAlmacenSaldo.CantidadSalida + movimientoAlmacenSaldo.CantidadReserva)
						- movimientoAlmacenSaldoDTO.CantidadAnterior);

					if (saldoVenta < movimientoAlmacenSaldoDTO.CantidadIngreso)
						throw new Exception(SR.GetString("EX_combo_producto_sin_saldo"));
				}

			}
			catch (Exception)
			{
				throw;
			}
		}
		public void ActualizarCantidadReserva(MovimientoAlmacenSaldoDTO dto)
		{
			try
			{
				MovimientoAlmacenSaldo movimientoAlmacenSaldo = Mapper.Map<MovimientoAlmacenSaldo>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					movimientoAlmacenSaldoRepository.ActualizarCantidadReserva(movimientoAlmacenSaldo);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public void ReprocesarCantidadReserva(MovimientoAlmacenSaldoDTO dto)
      {
			try
			{
				MovimientoAlmacenSaldo movimientoAlmacenSaldo = Mapper.Map<MovimientoAlmacenSaldo>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					movimientoAlmacenSaldoRepository.ReprocesarCantidadReserva(movimientoAlmacenSaldo);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public List<dynamic> ListarSaldoMultiAlmacen(MovimientoAlmacenSaldoDTO dto)
      {
			try
			{
				List<dynamic> dynamicList = movimientoAlmacenSaldoRepository.ListarSaldoMultiAlmacen(Mapper.Map<MovimientoAlmacenSaldo>(dto));

				return dynamicList;
			}
			catch (Exception)
			{
				throw;
			}
		}
   }
}
