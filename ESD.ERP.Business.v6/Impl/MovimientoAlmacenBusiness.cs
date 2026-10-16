using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Business.Resources;
using ESD.ERP.Common;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;
using ESD.ERP.Repository.Impl;

namespace ESD.ERP.Business.Impl
{
	public class MovimientoAlmacenBusiness : IMovimientoAlmacenBusiness
	{
		private readonly IMovimientoAlmacenRepository movimientoAlmacenRepository;
		private readonly IMovimientoAlmacenCorrelativoBusiness movimientoAlmacenCorrelativoBusiness;
		private readonly IMovimientoAlmacenProductoBusiness movimientoAlmacenProductoBusiness;
		private readonly IEstadoMovimientoAlmacenRepository estadoMovimientoAlmacenRepository;
		private readonly IEstadoAprobacionRepository estadoAprobacionRepository;
		private readonly IOrdenCompraRepository ordenCompraRepository;
		private readonly IOrdenCompraComprobanteRepository ordenCompraComprobanteRepository;
		private readonly IMovimientoAlmacenCierreRepository movimientoAlmacenCierreRepository;

        public MovimientoAlmacenBusiness(
			IMovimientoAlmacenRepository movimientoAlmacenRepository,
			IMovimientoAlmacenCorrelativoBusiness movimientoAlmacenCorrelativoBusiness,
			IMovimientoAlmacenProductoBusiness movimientoAlmacenProductoBusiness,
			IEstadoMovimientoAlmacenRepository estadoMovimientoAlmacenRepository,
			IEstadoAprobacionRepository estadoAprobacionRepository,
			IOrdenCompraRepository ordenCompraRepository,
			IOrdenCompraComprobanteRepository ordenCompraComprobanteRepository,
			IMovimientoAlmacenCierreRepository movimientoAlmacenCierreRepository
            )
		{
			this.movimientoAlmacenRepository = movimientoAlmacenRepository;
			this.movimientoAlmacenCorrelativoBusiness = movimientoAlmacenCorrelativoBusiness;
			this.movimientoAlmacenProductoBusiness = movimientoAlmacenProductoBusiness;
			this.estadoMovimientoAlmacenRepository = estadoMovimientoAlmacenRepository;
			this.estadoAprobacionRepository = estadoAprobacionRepository;
			this.ordenCompraRepository = ordenCompraRepository;
			this.ordenCompraComprobanteRepository = ordenCompraComprobanteRepository;
			this.movimientoAlmacenCierreRepository = movimientoAlmacenCierreRepository;
        }
		public MovimientoAlmacenDTO GetById(int id)
		{
			try
			{
				MovimientoAlmacen movimientoAlmacen = movimientoAlmacenRepository.GetById(id);

				MovimientoAlmacenDTO movimientoAlmacenDTO = Mapper.Map<MovimientoAlmacenDTO>(movimientoAlmacen);

				return movimientoAlmacenDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<MovimientoAlmacenDTO> Search(MovimientoAlmacenDTO dto)
		{
			try
			{
				List<MovimientoAlmacen> movimientoAlmacenList = movimientoAlmacenRepository.Search(Mapper.Map<MovimientoAlmacen>(dto));

				List<MovimientoAlmacenDTO> movimientoAlmacenDTOList = Mapper.Map<List<MovimientoAlmacenDTO>>(movimientoAlmacenList);

				return movimientoAlmacenDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public MovimientoAlmacenDTO SingleSearch(MovimientoAlmacenDTO dto)
		{
			try
			{
				MovimientoAlmacen movimientoAlmacen = movimientoAlmacenRepository.SingleSearch(Mapper.Map<MovimientoAlmacen>(dto));

				MovimientoAlmacenDTO movimientoAlmacenDTO = Mapper.Map<MovimientoAlmacenDTO>(movimientoAlmacen);

				return movimientoAlmacenDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(MovimientoAlmacenDTO dto)
		{
			try
			{
				MovimientoAlmacen movimientoAlmacen = Mapper.Map<MovimientoAlmacen>(dto);

				int newId = 0;
				using (TransactionScope tx = new())
				{
					MovimientoAlmacenCorrelativoDTO movimientoAlmacenCorrelativoDTO = new()
					{
						PeriodoEmpresaSedeId = dto.PeriodoEmpresaSedeId,
						AlmacenId = dto.AlmacenId,
						TipoMovimientoAlmacenId = dto.TipoMovimientoAlmacenId
					};

					movimientoAlmacen.Codigo = movimientoAlmacenCorrelativoBusiness.ObtenerCorrelativo(movimientoAlmacenCorrelativoDTO);

					newId = movimientoAlmacenRepository.Insert(movimientoAlmacen);
					dto.MovimientoAlmacenId = newId;

					if(dto.TransaccionAlmacenDTO != null)
						if (dto.TransaccionAlmacenDTO.Codigo == TransaccionAlmacenConstante.INGRESO_ORDEN_COMPRA)
						{
							movimientoAlmacen.MovimientoAlmacenId = newId;
							guardarOrdenCompraComprobante(movimientoAlmacen);
						}

					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(MovimientoAlmacenDTO dto)
		{
			try
			{
				MovimientoAlmacen movimientoAlmacen = Mapper.Map<MovimientoAlmacen>(dto);

				int id = 0;
				using (TransactionScope tx = new())
				{
					id = movimientoAlmacenRepository.Update(movimientoAlmacen);

					if (dto.TransaccionAlmacenDTO != null)
						if (dto.TransaccionAlmacenDTO.Codigo == TransaccionAlmacenConstante.INGRESO_ORDEN_COMPRA)
							guardarOrdenCompraComprobante(movimientoAlmacen);

					ActualizarMesContable(new MovimientoAlmacenDTO()
					{
						MovimientoAlmacenId = dto.MovimientoAlmacenId
					});

                    tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		private void guardarOrdenCompraComprobante(MovimientoAlmacen movimientoAlmacen)
      {
			if (!movimientoAlmacen.TipoDocumentoComercialId.HasValue)
				return;

			OrdenCompraComprobante ordenCompraComprobante = ordenCompraComprobanteRepository.SingleSearch(new OrdenCompraComprobante()
			{
				OrdenCompraId = movimientoAlmacen.OrdenCompraId.Value,
				RegistroReferencialId = movimientoAlmacen.EntityId
			});	

			if (ordenCompraComprobante == null)
			{
				OrdenCompra ordenCompra = ordenCompraRepository.GetById(movimientoAlmacen.OrdenCompraId.Value);
				ordenCompraComprobante = new OrdenCompraComprobante()
				{
					OrdenCompraId = movimientoAlmacen.OrdenCompraId.Value,
					TipoDocumentoComercialId = movimientoAlmacen.TipoDocumentoComercialId.Value,
					Serie = movimientoAlmacen.SerieDocumento,
					Numero = movimientoAlmacen.NumeroDocumento,
					Fecha = movimientoAlmacen.Fecha,
					CondicionPagoId = ordenCompra.CondicionPagoId,
					RegistroReferencialId = movimientoAlmacen.EntityId,
					Estado = EstadoConstante.ACTIVO,
					UsuarioCreacionId = movimientoAlmacen.UsuarioCreacionId
				};

				ordenCompraComprobanteRepository.Insert(ordenCompraComprobante);
         }
         else
         {
				ordenCompraComprobante.TipoDocumentoComercialId = movimientoAlmacen.TipoDocumentoComercialId.Value;
				ordenCompraComprobante.Serie = movimientoAlmacen.SerieDocumento;
				ordenCompraComprobante.Numero = movimientoAlmacen.NumeroDocumento;
				ordenCompraComprobante.Fecha = movimientoAlmacen.Fecha;
				ordenCompraComprobante.UsuarioCreacionId = movimientoAlmacen.UsuarioCreacionId;

				ordenCompraComprobanteRepository.Update(ordenCompraComprobante);
			}
		}
		public void Delete(MovimientoAlmacenDTO dto)
		{
			try
			{
				MovimientoAlmacen movimientoAlmacen = Mapper.Map<MovimientoAlmacen>(dto);

				List<MovimientoAlmacenProductoDTO> movimientoAlmacenProductoDTOList = movimientoAlmacenProductoBusiness.Search(new MovimientoAlmacenProductoDTO()
				{
					MovimientoAlmacenId = dto.Id,
					Estado = EstadoConstante.ACTIVO
				});

				EstadoMovimientoAlmacen estadoMovimientoAlmacen = estadoMovimientoAlmacenRepository.ObtenerPorCodigo(EstadoMovimientoAlmacenConstante.ANULADO);

				using (TransactionScope tx = new())
				{
					movimientoAlmacen.EstadoMovimientoAlmacenId = estadoMovimientoAlmacen.EntityId;
					movimientoAlmacenRepository.Delete(movimientoAlmacen);

					movimientoAlmacenProductoBusiness.Delete(movimientoAlmacenProductoDTOList);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public int RecepcionarTransferencia(MovimientoAlmacenDTO dto)
      {
			try
			{
				MovimientoAlmacen movimientoAlmacen = movimientoAlmacenRepository.GetById(dto.Id);
				string afectaSaldo = movimientoAlmacen.AfectaSaldo;

				List<MovimientoAlmacenProductoDTO> movimientoAlmacenProductoDTOList = movimientoAlmacenProductoBusiness.Search(new MovimientoAlmacenProductoDTO()
				{
					MovimientoAlmacenId = dto.Id,
					Estado = EstadoConstante.ACTIVO
				});

				int newId = 0;
				using (TransactionScope tx = new())
				{
					newId = Insert(dto);
					dto.MovimientoAlmacenId = newId;

					foreach (MovimientoAlmacenProductoDTO movimientoAlmacenProductoDTO in movimientoAlmacenProductoDTOList)
					{

						movimientoAlmacenProductoDTO.UsuarioCreacionId = dto.UsuarioCreacionId;

						if (afectaSaldo == SiNoConstante.N) {
							movimientoAlmacenProductoDTO.MovimientoAlmacenDTO = Mapper.Map<MovimientoAlmacenDTO>(movimientoAlmacen);
							movimientoAlmacenProductoBusiness.ActualizarSaldoTransferenciaSalida(movimientoAlmacenProductoDTO);
						}

						movimientoAlmacenProductoDTO.MovimientoAlmacenDTO = dto;
						movimientoAlmacenProductoDTO.MovimientoAlmacenId = dto.MovimientoAlmacenId;
						movimientoAlmacenProductoBusiness.Insert(movimientoAlmacenProductoDTO);
					}

					movimientoAlmacen.AfectaSaldo = SiNoConstante.S;
					movimientoAlmacen.EstadoMovimientoAlmacenId = dto.EstadoMovimientoAlmacenId;
					movimientoAlmacenRepository.ActualizarEstadoMovimiento(movimientoAlmacen);

					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
      public List<MovimientoAlmacenDTO> BuscarPorEmpresa(MovimientoAlmacenDTO dto)
      {
			try
			{
				List<MovimientoAlmacen> movimientoAlmacenList = movimientoAlmacenRepository.BuscarPorEmpresa(Mapper.Map<MovimientoAlmacen>(dto));

				List<MovimientoAlmacenDTO> movimientoAlmacenDTOList = Mapper.Map<List<MovimientoAlmacenDTO>>(movimientoAlmacenList);

				return movimientoAlmacenDTOList;
			}
			catch (Exception)
			{
				throw;
			}
	  }
      public void Solicitar(MovimientoAlmacenDTO dto)
      {
			try
			{
				MovimientoAlmacen movimientoAlmacen = Mapper.Map<MovimientoAlmacen>(dto);

				List<MovimientoAlmacenProductoDTO> movimientoAlmacenProductoDTOList = movimientoAlmacenProductoBusiness.Search(new MovimientoAlmacenProductoDTO()
				{
					MovimientoAlmacenId = dto.MovimientoAlmacenId,
					Estado = EstadoConstante.ACTIVO
				});

				if (movimientoAlmacenProductoDTOList.Count == 0)
					throw new Exception(SR.GetString("EX_movimiento_almacen_sin_items"));

				EstadoAprobacion estadoAprobacion = estadoAprobacionRepository.ObtenerPorCodigo(EstadoAprobacionConstante.PENDIENTE_APROBACION);

				using (TransactionScope tx = new())
				{
					movimientoAlmacenRepository.Update(movimientoAlmacen);

					movimientoAlmacenRepository.ActualizarEstadoAprobacion(new MovimientoAlmacen() {
						MovimientoAlmacenId = dto.MovimientoAlmacenId,
						EstadoAprobacionId = estadoAprobacion.EntityId,
						PersonalAutorizaId = null,
						FechaAutorizacion = null
					});

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
	  }

	  public void SolicitarModificacionEdicion(MovimientoAlmacenDTO dto)
	  {
			try
			{
				MovimientoAlmacen movimientoAlmacen = Mapper.Map<MovimientoAlmacen>(dto);

                EstadoAprobacion estadoAprobacion = estadoAprobacionRepository.ObtenerPorCodigo(EstadoAprobacionConstante.PENDIENTE_APROBACION);

                using (TransactionScope tx = new())
                {
                    movimientoAlmacenRepository.Update(movimientoAlmacen);

					movimientoAlmacenRepository.ActualizarEstadoAprobacion(new MovimientoAlmacen()
					{
                        MovimientoAlmacenId = dto.MovimientoAlmacenId,
                        EstadoAprobacionId = estadoAprobacion.EntityId,
						EsEditable = SiNoConstante.N
                    });

                    tx.Complete();
                }
            }
            catch (Exception)
            {
                throw;
            }
      }

      public void Aprobar(MovimientoAlmacenDTO dto)
      {
			try
			{
				MovimientoAlmacen movimientoAlmacen = movimientoAlmacenRepository.GetById(dto.Id);

				List<MovimientoAlmacenProductoDTO> movimientoAlmacenProductoDTOList = movimientoAlmacenProductoBusiness.Search(new MovimientoAlmacenProductoDTO()
				{
					MovimientoAlmacenId = dto.Id,
					Estado = EstadoConstante.ACTIVO
				});

				EstadoAprobacion estadoAprobacion = estadoAprobacionRepository.ObtenerPorCodigo(EstadoAprobacionConstante.APROBADO);
                EstadoAprobacion AprobarEdicion = estadoAprobacionRepository.ObtenerPorCodigo(EstadoAprobacionConstante.EN_EDICION);

				var mesId = DateTime.Now.Month + 1;

                using (TransactionScope tx = new())
				{

					if(movimientoAlmacen.EsEditable == SiNoConstante.N)
					{                      
                        movimientoAlmacenRepository.ActualizarEstadoAprobacion(new MovimientoAlmacen()
                        {
                            MovimientoAlmacenId = dto.MovimientoAlmacenId,
                            EstadoAprobacionId = AprobarEdicion.EntityId,
                            PersonalAutorizaId = dto.PersonalAutorizaId,
                            FechaAutorizacion = DateTime.Now,
							EsEditable = SiNoConstante.S,
							UsuarioCreacionId = dto.UsuarioCreacionId
                        });

                        movimientoAlmacenRepository.ActualizarEstadoMovimiento(new MovimientoAlmacen()
						{
							MovimientoAlmacenId = dto.MovimientoAlmacenId,
							AfectaSaldo = SiNoConstante.N,
							EstadoMovimientoAlmacenId = movimientoAlmacen.EstadoMovimientoAlmacenId,
							UsuarioCreacionId = dto.UsuarioCreacionId
						});
                    }

                    if (movimientoAlmacen.EsEditable == null)
					{
						foreach (MovimientoAlmacenProductoDTO movimientoAlmacenProductoDTO in movimientoAlmacenProductoDTOList)
						{

							movimientoAlmacenProductoDTO.UsuarioCreacionId = dto.UsuarioCreacionId;

							movimientoAlmacenProductoDTO.MovimientoAlmacenDTO = Mapper.Map<MovimientoAlmacenDTO>(movimientoAlmacen);
							movimientoAlmacenProductoBusiness.ActualizarSaldoAprobado(movimientoAlmacenProductoDTO);
						}

						movimientoAlmacen.AfectaSaldo = SiNoConstante.S;
						movimientoAlmacen.EstadoMovimientoAlmacenId = movimientoAlmacen.EstadoMovimientoAlmacenId;
						movimientoAlmacenRepository.ActualizarEstadoMovimiento(movimientoAlmacen);

						movimientoAlmacenRepository.ActualizarEstadoAprobacion(new MovimientoAlmacen()
						{
							MovimientoAlmacenId = dto.MovimientoAlmacenId,
							EstadoAprobacionId = estadoAprobacion.EntityId,
							PersonalAutorizaId = dto.PersonalAutorizaId,
							FechaAutorizacion = DateTime.Now
						});

                        movimientoAlmacenCierreRepository.Reprocesar(new MovimientoAlmacenCierre()
                        {
                            PeriodoEmpresaSedeId = movimientoAlmacen.PeriodoEmpresaSedeId,
                            AlmacenId = movimientoAlmacen.AlmacenId,
                            MesContableId = mesId
                        });
                    }

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
	  }

      public void Desaprobar(MovimientoAlmacenDTO dto)
      {
			try
			{
				MovimientoAlmacen movimientoAlmacen = movimientoAlmacenRepository.GetById(dto.Id);

				EstadoAprobacion estadoAprobacion = estadoAprobacionRepository.ObtenerPorCodigo(EstadoAprobacionConstante.DESAPROBADO);

				using (TransactionScope tx = new())
				{

					movimientoAlmacenRepository.ActualizarEstadoAprobacion(new MovimientoAlmacen()
					{
						MovimientoAlmacenId = dto.MovimientoAlmacenId,
						EstadoAprobacionId = estadoAprobacion.EntityId,
						PersonalAutorizaId = null,
						FechaAutorizacion = DateTime.Now
					});

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
	  }

	  public void ActualizarMesContable(MovimientoAlmacenDTO dto)
	  {
			try
			{
                using (TransactionScope tx = new())
                {
                    movimientoAlmacenRepository.ActualizarMesContable(new MovimientoAlmacen()
                    {
                        MovimientoAlmacenId = dto.MovimientoAlmacenId,
                    });

                    tx.Complete();
                }
			}
			catch(Exception)
			{
				throw;
			}
	  }
   }
}
