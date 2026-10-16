using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Common;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class MovimientoAlmacenCierreBusiness : IMovimientoAlmacenCierreBusiness
	{
		private readonly IMovimientoAlmacenCierreRepository movimientoAlmacenCierreRepository;
		private readonly IEstadoAperturaCierreRepository estadoAperturaCierreRepository;
		private readonly IAlmacenRepository almacenRepository;
		private readonly IMovimientoAlmacenSaldoRepository movimientoAlmacenSaldoRepository;

		public MovimientoAlmacenCierreBusiness(
			IMovimientoAlmacenCierreRepository movimientoAlmacenCierreRepository,
			IEstadoAperturaCierreRepository estadoAperturaCierreRepository,
			IAlmacenRepository almacenRepository,
			IMovimientoAlmacenSaldoRepository movimientoAlmacenSaldoRepository
			)
		{
			this.movimientoAlmacenCierreRepository = movimientoAlmacenCierreRepository;
			this.estadoAperturaCierreRepository = estadoAperturaCierreRepository;
			this.almacenRepository = almacenRepository;
			this.movimientoAlmacenSaldoRepository = movimientoAlmacenSaldoRepository;
		}
		public MovimientoAlmacenCierreDTO GetById(int id)
		{
			try
			{
				MovimientoAlmacenCierre movimientoAlmacenCierre = movimientoAlmacenCierreRepository.GetById(id);

				MovimientoAlmacenCierreDTO movimientoAlmacenCierreDTO = Mapper.Map<MovimientoAlmacenCierreDTO>(movimientoAlmacenCierre);

				return movimientoAlmacenCierreDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<MovimientoAlmacenCierreDTO> Search(MovimientoAlmacenCierreDTO dto)
		{
			try
			{
				List<MovimientoAlmacenCierre> movimientoAlmacenCierreList = movimientoAlmacenCierreRepository.Search(Mapper.Map<MovimientoAlmacenCierre>(dto));

				List<MovimientoAlmacenCierreDTO> movimientoAlmacenCierreDTOList = Mapper.Map<List<MovimientoAlmacenCierreDTO>>(movimientoAlmacenCierreList);

				return movimientoAlmacenCierreDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public MovimientoAlmacenCierreDTO SingleSearch(MovimientoAlmacenCierreDTO dto)
		{
			try
			{
				MovimientoAlmacenCierre movimientoAlmacenCierre = movimientoAlmacenCierreRepository.SingleSearch(Mapper.Map<MovimientoAlmacenCierre>(dto));

				MovimientoAlmacenCierreDTO movimientoAlmacenCierreDTO = Mapper.Map<MovimientoAlmacenCierreDTO>(movimientoAlmacenCierre);

				return movimientoAlmacenCierreDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(MovimientoAlmacenCierreDTO dto)
		{
			try
			{
				MovimientoAlmacenCierre movimientoAlmacenCierre = Mapper.Map<MovimientoAlmacenCierre>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = movimientoAlmacenCierreRepository.Insert(movimientoAlmacenCierre);

					dto.MovimientoAlmacenCierreId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(MovimientoAlmacenCierreDTO dto)
		{
			try
			{
				MovimientoAlmacenCierre movimientoAlmacenCierre = Mapper.Map<MovimientoAlmacenCierre>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = movimientoAlmacenCierreRepository.Update(movimientoAlmacenCierre);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(MovimientoAlmacenCierreDTO dto)
		{
			try
			{
				MovimientoAlmacenCierre movimientoAlmacenCierre = Mapper.Map<MovimientoAlmacenCierre>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					movimientoAlmacenCierreRepository.Delete(movimientoAlmacenCierre);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Aperturar(MovimientoAlmacenCierreDTO dto)
		{
			try
			{
				MovimientoAlmacenCierre movimientoAlmacenCierre = Mapper.Map<MovimientoAlmacenCierre>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					EstadoAperturaCierre estadoAperturaCierre = estadoAperturaCierreRepository.ObtenerPorCodigo(EstadoAperturaCierreConstante.APERTURA);

					movimientoAlmacenCierre = movimientoAlmacenCierreRepository.SingleSearch(Mapper.Map<MovimientoAlmacenCierre>(dto));

					movimientoAlmacenCierre.EstadoAperturaCierreId = estadoAperturaCierre.EstadoAperturaCierreId;
					movimientoAlmacenCierre.FechaCierre = null;
					movimientoAlmacenCierre.TieneMovimientoPostCierre = SiNoConstante.N;

					movimientoAlmacenCierreRepository.Update(movimientoAlmacenCierre);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Cerrar(MovimientoAlmacenCierreDTO dto)
		{
			try
			{
				MovimientoAlmacenCierre movimientoAlmacenCierre = Mapper.Map<MovimientoAlmacenCierre>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					movimientoAlmacenCierreRepository.Reprocesar(movimientoAlmacenCierre);

					EstadoAperturaCierre estadoAperturaCierre = estadoAperturaCierreRepository.ObtenerPorCodigo(EstadoAperturaCierreConstante.CIERRE);

					movimientoAlmacenCierre = movimientoAlmacenCierreRepository.SingleSearch(Mapper.Map<MovimientoAlmacenCierre>(dto));

					movimientoAlmacenCierre.EstadoAperturaCierreId = estadoAperturaCierre.EstadoAperturaCierreId;
					movimientoAlmacenCierre.FechaCierre = DateTime.Today;
					movimientoAlmacenCierre.TieneMovimientoPostCierre = SiNoConstante.N;
					movimientoAlmacenCierre.UsuarioCreacionId = dto.UsuarioCreacionId;

					movimientoAlmacenCierreRepository.Update(movimientoAlmacenCierre);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Reprocesar(MovimientoAlmacenCierreDTO dto)
		{
			try
			{
				MovimientoAlmacenCierre movimientoAlmacenCierre = Mapper.Map<MovimientoAlmacenCierre>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					movimientoAlmacenCierreRepository.Reprocesar(movimientoAlmacenCierre);

					Almacen almacen = almacenRepository.GetById(movimientoAlmacenCierre.AlmacenId);
					if (almacen.EsVenta == SiNoConstante.S)
						movimientoAlmacenSaldoRepository.ReprocesarCantidadReserva(new MovimientoAlmacenSaldo()
						{
							PeriodoEmpresaSedeId = dto.PeriodoEmpresaSedeId,
							MesContableId = dto.MesContableId
						});

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

        public void ReprocesarSaldoAnual(MovimientoAlmacenCierreDTO dto)
        {
            using (TransactionScope tx = new())
            {

                int idInicio = 2;
                int idFin = DateTime.Now.Month + 1;

                for (int mesContableId = idInicio; mesContableId <= idFin; mesContableId++)
                {
                    movimientoAlmacenCierreRepository.Reprocesar(new MovimientoAlmacenCierre()
                    {
                        PeriodoEmpresaSedeId = dto.PeriodoEmpresaSedeId,
                        AlmacenId = dto.AlmacenId,
                        MesContableId = mesContableId
                    });
                }

                tx.Complete();
            }
        }
    }
}
