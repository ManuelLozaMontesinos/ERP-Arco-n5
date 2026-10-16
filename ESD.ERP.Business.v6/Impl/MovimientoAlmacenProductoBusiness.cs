using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Business.Resources;
using ESD.ERP.Common;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class MovimientoAlmacenProductoBusiness : IMovimientoAlmacenProductoBusiness
	{
		private readonly IMovimientoAlmacenProductoRepository movimientoAlmacenProductoRepository;
		private readonly IMovimientoAlmacenRepository movimientoAlmacenRepository;
		private readonly IMovimientoAlmacenSaldoRepository movimientoAlmacenSaldoRepository;
		private readonly IMovimientoAlmacenSaldoBusiness movimientoAlmacenSaldoBusiness;
		private readonly IOrdenCompraRepository ordenCompraRepository;
		private readonly IOrdenCompraProductoRepository ordenCompraProductoRepository;
		private readonly ITipoCambioDiaRepository tipoCambioDiaRepository;
		private readonly IEstadoOrdenCompraRepository estadoOrdenCompraRepository;

		public MovimientoAlmacenProductoBusiness(
			IMovimientoAlmacenProductoRepository movimientoAlmacenProductoRepository,
			IMovimientoAlmacenRepository movimientoAlmacenRepository,
			IMovimientoAlmacenSaldoRepository movimientoAlmacenSaldoRepository,
			IMovimientoAlmacenSaldoBusiness movimientoAlmacenSaldoBusiness,
			IOrdenCompraRepository ordenCompraRepository,
			IOrdenCompraProductoRepository ordenCompraProductoRepository,
			ITipoCambioDiaRepository tipoCambioDiaRepository,
			IEstadoOrdenCompraRepository estadoOrdenCompraRepository
			)
		{
			this.movimientoAlmacenProductoRepository = movimientoAlmacenProductoRepository;
			this.movimientoAlmacenRepository = movimientoAlmacenRepository;
			this.movimientoAlmacenSaldoRepository = movimientoAlmacenSaldoRepository;
			this.movimientoAlmacenSaldoBusiness = movimientoAlmacenSaldoBusiness;
			this.ordenCompraRepository = ordenCompraRepository;
			this.ordenCompraProductoRepository = ordenCompraProductoRepository;
			this.tipoCambioDiaRepository = tipoCambioDiaRepository;
			this.estadoOrdenCompraRepository = estadoOrdenCompraRepository;
		}
		public MovimientoAlmacenProductoDTO GetById(int id)
		{
			try
			{
				MovimientoAlmacenProducto movimientoAlmacenProducto = movimientoAlmacenProductoRepository.GetById(id);

				MovimientoAlmacenProductoDTO movimientoAlmacenProductoDTO = Mapper.Map<MovimientoAlmacenProductoDTO>(movimientoAlmacenProducto);

				return movimientoAlmacenProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<MovimientoAlmacenProductoDTO> Search(MovimientoAlmacenProductoDTO dto)
		{
			try
			{
				List<MovimientoAlmacenProducto> movimientoAlmacenProductoList = movimientoAlmacenProductoRepository.Search(Mapper.Map<MovimientoAlmacenProducto>(dto));

				List<MovimientoAlmacenProductoDTO> movimientoAlmacenProductoDTOList = Mapper.Map<List<MovimientoAlmacenProductoDTO>>(movimientoAlmacenProductoList);

				return movimientoAlmacenProductoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public MovimientoAlmacenProductoDTO SingleSearch(MovimientoAlmacenProductoDTO dto)
		{
			try
			{
				MovimientoAlmacenProducto movimientoAlmacenProducto = movimientoAlmacenProductoRepository.SingleSearch(Mapper.Map<MovimientoAlmacenProducto>(dto));

				MovimientoAlmacenProductoDTO movimientoAlmacenProductoDTO = Mapper.Map<MovimientoAlmacenProductoDTO>(movimientoAlmacenProducto);

				return movimientoAlmacenProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(MovimientoAlmacenProductoDTO dto)
		{
			try
			{
				MovimientoAlmacenProducto movimientoAlmacenProducto = Mapper.Map<MovimientoAlmacenProducto>(dto);

				int newId = 0;
				using (TransactionScope tx = new())
				{

					MovimientoAlmacenSaldo movimientoAlmacenSaldo = obtenerMovimientoAlmacenSaldo(movimientoAlmacenProducto);

					movimientoAlmacenProducto.MovimientoAlmacen = movimientoAlmacenRepository.GetById(dto.MovimientoAlmacenId);

					if (movimientoAlmacenProducto.MovimientoAlmacen.TipoMovimientoAlmacen.Codigo == TipoMovimientoAlmacenConstante.INGRESO)
					{
						movimientoAlmacenSaldo.CantidadIngreso += movimientoAlmacenProducto.Cantidad;
					}
					else
					{
						decimal saldo = movimientoAlmacenSaldoBusiness.ObtenerSaldoDisponible(new MovimientoAlmacenProductoDTO()
						{
							MovimientoAlmacenDTO = new MovimientoAlmacenDTO()
							{
								PeriodoEmpresaSedeId = movimientoAlmacenProducto.MovimientoAlmacen.PeriodoEmpresaSedeId,
								AlmacenId = movimientoAlmacenProducto.MovimientoAlmacen.AlmacenId,
								MesContableId = movimientoAlmacenProducto.MovimientoAlmacen.MesContableId,
								TipoMovimientoAlmacenDTO = new TipoMovimientoAlmacenDTO() { Codigo = movimientoAlmacenProducto.MovimientoAlmacen.TipoMovimientoAlmacen.Codigo }
							},
							ProductoId = dto.ProductoId
						});
						if(movimientoAlmacenProducto.MovimientoAlmacen.TransaccionAlmacenId != 9 &&
                           movimientoAlmacenProducto.MovimientoAlmacen.TransaccionAlmacenId != 8 &&
                           movimientoAlmacenProducto.MovimientoAlmacen.TransaccionAlmacenId != 11)		
						{
							validarSaldoDisponible(movimientoAlmacenProducto, movimientoAlmacenProducto.Cantidad, saldo);
						} // VALIDAR SALDO PARA OPERACIONES DE MERMA - CONSUMO INTERNO - TRANSFERENCIA ENTRE ALMACENES
						movimientoAlmacenSaldo.CantidadSalida += movimientoAlmacenProducto.Cantidad;
					}

					newId = movimientoAlmacenProductoRepository.Insert(movimientoAlmacenProducto);

					if (movimientoAlmacenProducto.MovimientoAlmacen.AfectaSaldo == SiNoConstante.S)
					{
						if (movimientoAlmacenSaldo.EntityId == 0)
							movimientoAlmacenSaldoRepository.Insert(movimientoAlmacenSaldo);
						else
							movimientoAlmacenSaldoRepository.Update(movimientoAlmacenSaldo);
					}

					if(movimientoAlmacenProducto.MovimientoAlmacen.TransaccionAlmacen.Codigo == TransaccionAlmacenConstante.INGRESO_ORDEN_COMPRA)
					{
						if (movimientoAlmacenProducto.RegistroReferencialId.HasValue)
						{
							ordenCompraProductoRepository.ActualizarCantidadIngresada(new OrdenCompraProducto()
							{
								OrdenCompraId = movimientoAlmacenProducto.MovimientoAlmacen.OrdenCompraId.Value,
								OrdenCompraProductoId = movimientoAlmacenProducto.RegistroReferencialId.Value
							});

							actualizarEstadoOrdenCompra(movimientoAlmacenProducto.MovimientoAlmacen.OrdenCompraId.Value,
								movimientoAlmacenProducto.UsuarioCreacionId);
						}

					}

					dto.MovimientoAlmacenProductoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(MovimientoAlmacenProductoDTO dto)
		{
			try
			{
				MovimientoAlmacenProducto movimientoAlmacenProducto = Mapper.Map<MovimientoAlmacenProducto>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{

					MovimientoAlmacenProductoDTO movimientoAlmacenProductoDTO = GetById(dto.Id);

					decimal saldo = movimientoAlmacenSaldoBusiness.ObtenerSaldoDisponible(movimientoAlmacenProductoDTO);

					movimientoAlmacenProducto.MovimientoAlmacen = Mapper.Map<MovimientoAlmacen>(movimientoAlmacenProductoDTO.MovimientoAlmacenDTO);
					validarSaldoDisponible(movimientoAlmacenProducto, movimientoAlmacenProducto.Cantidad, saldo);

					MovimientoAlmacenSaldo movimientoAlmacenSaldo = obtenerMovimientoAlmacenSaldo(movimientoAlmacenProducto);

					if (movimientoAlmacenProducto.MovimientoAlmacen.TipoMovimientoAlmacen.Codigo == TipoMovimientoAlmacenConstante.INGRESO)
               {
						movimientoAlmacenSaldo.CantidadIngreso -= movimientoAlmacenProductoDTO.Cantidad;
						movimientoAlmacenSaldo.CantidadIngreso += movimientoAlmacenProducto.Cantidad;
               }
               else{
						movimientoAlmacenSaldo.CantidadSalida -= movimientoAlmacenProductoDTO.Cantidad;
						movimientoAlmacenSaldo.CantidadSalida += movimientoAlmacenProducto.Cantidad;
					}

					id = movimientoAlmacenProductoRepository.Update(movimientoAlmacenProducto);

					if (movimientoAlmacenProducto.MovimientoAlmacen.AfectaSaldo == SiNoConstante.S)
						movimientoAlmacenSaldoRepository.Update(movimientoAlmacenSaldo);

					if (movimientoAlmacenProducto.MovimientoAlmacen.TransaccionAlmacen.Codigo == TransaccionAlmacenConstante.INGRESO_ORDEN_COMPRA)
					{
						if (movimientoAlmacenProducto.RegistroReferencialId.HasValue)
                  {
							ordenCompraProductoRepository.ActualizarCantidadIngresada(new OrdenCompraProducto()
							{
								OrdenCompraId = movimientoAlmacenProducto.MovimientoAlmacen.OrdenCompraId.Value,
								OrdenCompraProductoId = movimientoAlmacenProducto.RegistroReferencialId.Value
							});

							actualizarEstadoOrdenCompra(movimientoAlmacenProducto.MovimientoAlmacen.OrdenCompraId.Value, 
								movimientoAlmacenProducto.UsuarioCreacionId);
						}
					}

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(MovimientoAlmacenProductoDTO dto)
		{
			try
			{
				Delete(new List<MovimientoAlmacenProductoDTO>() { dto });
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(List<MovimientoAlmacenProductoDTO> list)
		{
			try
			{

				using (TransactionScope tx = new TransactionScope())
				{
					foreach (MovimientoAlmacenProductoDTO dto in list) {
						MovimientoAlmacenProductoDTO movimientoAlmacenProductoDTO = GetById(dto.Id);

						MovimientoAlmacenProducto movimientoAlmacenProducto = Mapper.Map<MovimientoAlmacenProducto>(movimientoAlmacenProductoDTO);

						MovimientoAlmacenSaldo movimientoAlmacenSaldo = obtenerMovimientoAlmacenSaldo(movimientoAlmacenProducto);

						if (movimientoAlmacenProducto.MovimientoAlmacen.TipoMovimientoAlmacen.Codigo == TipoMovimientoAlmacenConstante.INGRESO)
						{
							decimal saldo = movimientoAlmacenSaldoBusiness.ObtenerSaldoDisponible(movimientoAlmacenProductoDTO);
							validarSaldoDisponible(movimientoAlmacenProducto, decimal.Zero, saldo);

							movimientoAlmacenSaldo.CantidadIngreso -= movimientoAlmacenProducto.Cantidad;
						}
						else
						{
							movimientoAlmacenSaldo.CantidadSalida -= movimientoAlmacenProducto.Cantidad;
						}

						movimientoAlmacenProductoRepository.Delete(movimientoAlmacenProducto);

						if(movimientoAlmacenProducto.MovimientoAlmacen.AfectaSaldo == SiNoConstante.S)
							movimientoAlmacenSaldoRepository.Update(movimientoAlmacenSaldo);

						if (movimientoAlmacenProducto.MovimientoAlmacen.TransaccionAlmacen.Codigo == TransaccionAlmacenConstante.INGRESO_ORDEN_COMPRA)
						{
							if (movimientoAlmacenProducto.RegistroReferencialId.HasValue)
                     {
								ordenCompraProductoRepository.ActualizarCantidadIngresada(new OrdenCompraProducto()
								{
									OrdenCompraId = movimientoAlmacenProducto.MovimientoAlmacen.OrdenCompraId.Value,
									OrdenCompraProductoId = movimientoAlmacenProducto.RegistroReferencialId.Value
								});

								actualizarEstadoOrdenCompra(movimientoAlmacenProducto.MovimientoAlmacen.OrdenCompraId.Value,
									movimientoAlmacenProducto.UsuarioCreacionId);

							}

						}
					}

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		private MovimientoAlmacenSaldo obtenerMovimientoAlmacenSaldo(MovimientoAlmacenProducto movimientoAlmacenProducto) {

			movimientoAlmacenProducto.MovimientoAlmacen = movimientoAlmacenRepository.GetById(movimientoAlmacenProducto.MovimientoAlmacenId);

			MovimientoAlmacenSaldo movimientoAlmacenSaldo = new MovimientoAlmacenSaldo()
			{
				PeriodoEmpresaSedeId = movimientoAlmacenProducto.MovimientoAlmacen.PeriodoEmpresaSedeId,
				AlmacenId = movimientoAlmacenProducto.MovimientoAlmacen.AlmacenId,
				ProductoId = movimientoAlmacenProducto.ProductoId,
				MesContableId = movimientoAlmacenProducto.MovimientoAlmacen.MesContableId
			};

			movimientoAlmacenSaldo = movimientoAlmacenSaldoRepository.SingleSearch(movimientoAlmacenSaldo);
			if (movimientoAlmacenSaldo == null)
			{
				movimientoAlmacenSaldo = new MovimientoAlmacenSaldo()
				{
					PeriodoEmpresaSedeId = movimientoAlmacenProducto.MovimientoAlmacen.PeriodoEmpresaSedeId,
					AlmacenId = movimientoAlmacenProducto.MovimientoAlmacen.AlmacenId,
					ProductoId = movimientoAlmacenProducto.ProductoId,
					MesContableId = movimientoAlmacenProducto.MovimientoAlmacen.MesContableId
				};
			}

			return movimientoAlmacenSaldo;
		}
		private void validarSaldoDisponible(MovimientoAlmacenProducto movimientoAlmacenProducto, decimal cantidad, decimal saldo)
		{
			try
			{
				decimal saldoDisponible = decimal.Zero;

				if (movimientoAlmacenProducto.MovimientoAlmacen.TipoMovimientoAlmacen.Codigo == TipoMovimientoAlmacenConstante.INGRESO)
				{

					saldoDisponible = saldo + cantidad;
					if (saldoDisponible < decimal.Zero)
					{
						saldo = decimal.Zero;
						throw new Exception(SR.GetString("EX_producto_sin_saldo_suficiente",
							string.Format(StringFormatConstant.DECIMAL, saldo)));
					}
				}
				else
				{
					saldoDisponible = saldo - cantidad;
					if (saldoDisponible < decimal.Zero)
						throw new Exception(SR.GetString("EX_cantidad_mayor_al_saldo",
							string.Format(StringFormatConstant.DECIMAL, saldo)));
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void AgregarDesdeReserva(MovimientoAlmacenProductoDTO dto)
		{
			try
			{
				MovimientoAlmacenProducto movimientoAlmacenProducto = Mapper.Map<MovimientoAlmacenProducto>(dto);

				using (TransactionScope tx = new TransactionScope())
				{

					MovimientoAlmacenSaldo movimientoAlmacenSaldo = movimientoAlmacenSaldoRepository.SingleSearch(new MovimientoAlmacenSaldo()
					{
						PeriodoEmpresaSedeId = movimientoAlmacenProducto.MovimientoAlmacen.PeriodoEmpresaSedeId,
						AlmacenId = movimientoAlmacenProducto.MovimientoAlmacen.AlmacenId,
						ProductoId = movimientoAlmacenProducto.ProductoId,
						MesContableId = movimientoAlmacenProducto.MovimientoAlmacen.MesContableId
					});

					if (movimientoAlmacenSaldo == null)
					{
						movimientoAlmacenSaldo = new MovimientoAlmacenSaldo()
						{
							PeriodoEmpresaSedeId = movimientoAlmacenProducto.MovimientoAlmacen.PeriodoEmpresaSedeId,
							AlmacenId = movimientoAlmacenProducto.MovimientoAlmacen.AlmacenId,
							ProductoId = movimientoAlmacenProducto.ProductoId,
							MesContableId = movimientoAlmacenProducto.MovimientoAlmacen.MesContableId,
							CantidadSalida = dto.Cantidad
						};

						movimientoAlmacenSaldoRepository.Insert(movimientoAlmacenSaldo);
					}
					else
					{
						movimientoAlmacenSaldo.CantidadSalida += dto.Cantidad;
						movimientoAlmacenSaldo.CantidadReserva -= dto.Cantidad;
						movimientoAlmacenSaldoRepository.Update(movimientoAlmacenSaldo);
					}

					movimientoAlmacenProductoRepository.Insert(movimientoAlmacenProducto);

					tx.Complete();
				}

			}
			catch (Exception)
			{
				throw;
			}
		}
		public void ActualizarSaldoTransferenciaSalida(MovimientoAlmacenProductoDTO dto)
		{
			try
			{
				MovimientoAlmacenProducto movimientoAlmacenProducto = Mapper.Map<MovimientoAlmacenProducto>(dto);

				using (TransactionScope tx = new TransactionScope())
				{

					MovimientoAlmacenSaldo movimientoAlmacenSaldo = obtenerMovimientoAlmacenSaldo(movimientoAlmacenProducto);

					decimal saldo = movimientoAlmacenSaldoBusiness.ObtenerSaldoDisponible(new MovimientoAlmacenProductoDTO()
					{
						MovimientoAlmacenDTO = new MovimientoAlmacenDTO()
						{
							PeriodoEmpresaSedeId = movimientoAlmacenProducto.MovimientoAlmacen.PeriodoEmpresaSedeId,
							AlmacenId = movimientoAlmacenProducto.MovimientoAlmacen.AlmacenId,
							MesContableId = movimientoAlmacenProducto.MovimientoAlmacen.MesContableId,
							TipoMovimientoAlmacenDTO = new TipoMovimientoAlmacenDTO() { Codigo = movimientoAlmacenProducto.MovimientoAlmacen.TipoMovimientoAlmacen.Codigo }
						},
						ProductoId = dto.ProductoId
					});
                    if (movimientoAlmacenProducto.MovimientoAlmacen.TransaccionAlmacenId != 11)
                    {
                        validarSaldoDisponible(movimientoAlmacenProducto, movimientoAlmacenProducto.Cantidad, saldo);
                    }

                    movimientoAlmacenSaldo.CantidadSalida += movimientoAlmacenProducto.Cantidad;

					if (movimientoAlmacenSaldo.EntityId == 0)
						movimientoAlmacenSaldoRepository.Insert(movimientoAlmacenSaldo);
					else
						movimientoAlmacenSaldoRepository.Update(movimientoAlmacenSaldo);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public void ActualizarProductoReclasificacion(MovimientoAlmacenProductoDTO dto)
      {
			try
			{
				MovimientoAlmacenProducto movimientoAlmacenProducto = Mapper.Map<MovimientoAlmacenProducto>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					movimientoAlmacenProductoRepository.ActualizarProductoReclasificacion(movimientoAlmacenProducto);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public void ActualizarSaldoAprobado(MovimientoAlmacenProductoDTO dto)
      {
			try
			{
				MovimientoAlmacenProducto movimientoAlmacenProducto = Mapper.Map<MovimientoAlmacenProducto>(dto);

				using (TransactionScope tx = new TransactionScope())
				{

					MovimientoAlmacenSaldo movimientoAlmacenSaldo = obtenerMovimientoAlmacenSaldo(movimientoAlmacenProducto);

					if (movimientoAlmacenProducto.MovimientoAlmacen.TipoMovimientoAlmacen.Codigo == TipoMovimientoAlmacenConstante.INGRESO)
					{
						movimientoAlmacenSaldo.CantidadIngreso += movimientoAlmacenProducto.Cantidad;
					}
					else
					{
						decimal saldo = movimientoAlmacenSaldoBusiness.ObtenerSaldoDisponible(new MovimientoAlmacenProductoDTO()
						{
							MovimientoAlmacenDTO = new MovimientoAlmacenDTO()
							{
								PeriodoEmpresaSedeId = movimientoAlmacenProducto.MovimientoAlmacen.PeriodoEmpresaSedeId,
								AlmacenId = movimientoAlmacenProducto.MovimientoAlmacen.AlmacenId,
								MesContableId = movimientoAlmacenProducto.MovimientoAlmacen.MesContableId,
								TipoMovimientoAlmacenDTO = new TipoMovimientoAlmacenDTO() { Codigo = movimientoAlmacenProducto.MovimientoAlmacen.TipoMovimientoAlmacen.Codigo }
							},
							ProductoId = dto.ProductoId
						});
                        if (movimientoAlmacenProducto.MovimientoAlmacen.TransaccionAlmacenId != 9 &&
                           movimientoAlmacenProducto.MovimientoAlmacen.TransaccionAlmacenId != 8 &&
                           movimientoAlmacenProducto.MovimientoAlmacen.TransaccionAlmacenId != 11)
                        {
                            validarSaldoDisponible(movimientoAlmacenProducto, movimientoAlmacenProducto.Cantidad, saldo);
                        }

                        movimientoAlmacenSaldo.CantidadSalida += movimientoAlmacenProducto.Cantidad;
					}

					if (movimientoAlmacenSaldo.EntityId == 0)
						movimientoAlmacenSaldoRepository.Insert(movimientoAlmacenSaldo);
					else
						movimientoAlmacenSaldoRepository.Update(movimientoAlmacenSaldo);

					tx.Complete();
				}

			}
			catch (Exception)
			{
				throw;
			}
		}
      public void AgregarOrdenCompraProducto(MovimientoAlmacenProductoDTO dto)
      {
			try
			{
				OrdenCompra ordenCompra = ordenCompraRepository.GetById(dto.MovimientoAlmacenDTO.OrdenCompraId.Value);
				MovimientoAlmacen movimientoAlmacen = movimientoAlmacenRepository.GetById(dto.MovimientoAlmacenId);

				TipoCambioDia tipoCambioDia = new();
				if (ordenCompra.Moneda.Codigo != MonedaConstante.SOLES)
            {
					tipoCambioDia = tipoCambioDiaRepository.SingleSearch(new TipoCambioDia()
					{
						TipoCambio = new TipoCambio() { Codigo = TipoCambioConstante.OFICIAL },
						Fecha = movimientoAlmacen.Fecha,
						MonedaId = ordenCompra.MonedaId
					});
				}

				using (TransactionScope tx = new())
				{
					List<OrdenCompraProducto> ordenCompraProductoList = ordenCompraProductoRepository.Search(new OrdenCompraProducto()
					{
						OrdenCompraId = dto.MovimientoAlmacenDTO.OrdenCompraId.Value,
						OrdenCompraProductoIdList = dto.OrdenCompraProductoIdList
					});

					foreach (OrdenCompraProducto ordenCompraProducto in ordenCompraProductoList)
					{
						MovimientoAlmacenProductoDTO movimientoAlmacenProductoDTO = new()
						{
							MovimientoAlmacenDTO = Mapper.Map<MovimientoAlmacenDTO>(movimientoAlmacen),
							MovimientoAlmacenId = dto.MovimientoAlmacenId,
							ProductoId = ordenCompraProducto.ProductoId,
							Descripcion = ordenCompraProducto.Descripcion,
							Cantidad = (ordenCompraProducto.Cantidad - ordenCompraProducto.CantidadIngresada),
							ValorUnitario = ordenCompra.Moneda.EsLocal == SiNoConstante.S ? ordenCompraProducto.ValorUnitario : 
								(ordenCompraProducto.ValorUnitario * tipoCambioDia.MontoVenta),
							RegistroReferencialId = ordenCompraProducto.EntityId,
							Estado = EstadoConstante.ACTIVO,
							UsuarioCreacionId = dto.UsuarioCreacionId
						};

						Insert(movimientoAlmacenProductoDTO);
               }

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		private void actualizarEstadoOrdenCompra(int ordenCompraId, int usuarioAccionId)
		{

			string codigo = string.Empty;

			List<OrdenCompraProducto> ordenCompraProductoList = ordenCompraProductoRepository.Search(new OrdenCompraProducto()
			{
				OrdenCompraId = ordenCompraId,
				Estado = EstadoConstante.ACTIVO
			});

			int total = ordenCompraProductoList.Count;
			int totalMovimiento = ordenCompraProductoList.Count(x => x.CantidadIngresada >= x.Cantidad);

			if (totalMovimiento == decimal.Zero)
				codigo = EstadoOrdenCompraConstante.ORDEN_COMPRA;
			else
			{
				if (totalMovimiento == total)
					codigo = EstadoOrdenCompraConstante.ATENDIDA;
				else
					codigo = EstadoOrdenCompraConstante.INGRESO_PARCIAL;
			}

			EstadoOrdenCompra estadoOrdenCompra = estadoOrdenCompraRepository.ObtenerPorCodigo(codigo);

			ordenCompraRepository.ActualizarEstado(new OrdenCompra()
			{
				OrdenCompraId = ordenCompraId,
				EstadoOrdenCompraId = estadoOrdenCompra.EntityId,
				UsuarioModificacionId = usuarioAccionId
			});
		}
	}
}
