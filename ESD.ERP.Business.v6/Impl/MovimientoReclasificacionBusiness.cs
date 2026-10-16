using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.Core.Data;
using ESD.Core.Logging;
using ESD.ERP.Business.Resources;
using ESD.ERP.Common;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;
using ESD.ERP.Repository.Impl;

namespace ESD.ERP.Business.Impl
{
	public class MovimientoReclasificacionBusiness : IMovimientoReclasificacionBusiness
	{
		private readonly IMovimientoReclasificacionRepository movimientoReclasificacionRepository;
		private readonly ITablaBusiness tablaBusiness;
		private readonly IMovimientoAlmacenRepository movimientoAlmacenRepository;
		private readonly IMovimientoAlmacenProductoRepository movimientoAlmacenProductoRepository;
		private readonly ITransaccionAlmacenRepository transaccionAlmacenRepository;
		private readonly ITipoMovimientoAlmacenRepository tipoMovimientoAlmacenRepository;
		private readonly IEstadoMovimientoAlmacenRepository estadoMovimientoAlmacenRepository;
        private readonly IMovimientoAlmacenCorrelativoBusiness movimientoAlmacenCorrelativoBusiness;
		private readonly IPeriodoEmpresaSedeRepository periodoEmpresaSedeRepository;
		private readonly IMovimientoAlmacenSaldoBusiness movimientoAlmacenSaldoBusiness;
		private readonly IMovimientoAlmacenBusiness movimientoAlmacenBusiness;
		private readonly IMovimientoAlmacenProductoBusiness movimientoAlmacenProductoBusiness;
		private readonly IEstadoMovimientoReclasificacionRepository estadoMovimientoReclasificacionRepository;


		public MovimientoReclasificacionBusiness(
			IMovimientoReclasificacionRepository movimientoReclasificacionRepository,
			ITablaBusiness tablaBusiness,
			IMovimientoAlmacenRepository movimientoAlmacenRepository,
			IMovimientoAlmacenProductoRepository movimientoAlmacenProductoRepository,
			ITransaccionAlmacenRepository transaccionAlmacenRepository,
			ITipoMovimientoAlmacenRepository tipoMovimientoAlmacenRepository,
			IEstadoMovimientoAlmacenRepository estadoMovimientoAlmacenRepository,
            IEstadoAprobacionRepository estadoAprobacionRepository,
            IMovimientoAlmacenCorrelativoBusiness movimientoAlmacenCorrelativoBusiness,
			IPeriodoEmpresaSedeRepository periodoEmpresaSedeRepository,
			IMovimientoAlmacenSaldoBusiness movimientoAlmacenSaldoBusiness,
			IMovimientoAlmacenBusiness movimientoAlmacenBusiness,
			IMovimientoAlmacenProductoBusiness movimientoAlmacenProductoBusiness,
			IEstadoMovimientoReclasificacionRepository estadoMovimientoReclasificacionRepository
			)
		{
			this.movimientoReclasificacionRepository = movimientoReclasificacionRepository;
			this.tablaBusiness = tablaBusiness;
			this.movimientoAlmacenRepository = movimientoAlmacenRepository;
			this.movimientoAlmacenProductoRepository = movimientoAlmacenProductoRepository;
			this.transaccionAlmacenRepository = transaccionAlmacenRepository;
			this.tipoMovimientoAlmacenRepository = tipoMovimientoAlmacenRepository;
			this.estadoMovimientoAlmacenRepository = estadoMovimientoAlmacenRepository;
			this.movimientoAlmacenCorrelativoBusiness = movimientoAlmacenCorrelativoBusiness;
			this.periodoEmpresaSedeRepository = periodoEmpresaSedeRepository;
			this.movimientoAlmacenSaldoBusiness = movimientoAlmacenSaldoBusiness;
			this.movimientoAlmacenBusiness = movimientoAlmacenBusiness;
			this.movimientoAlmacenProductoBusiness = movimientoAlmacenProductoBusiness;
			this.estadoMovimientoReclasificacionRepository = estadoMovimientoReclasificacionRepository;
		}
		public MovimientoReclasificacionDTO GetById(int id)
		{
			try
			{
				MovimientoReclasificacion movimientoReclasificacion = movimientoReclasificacionRepository.GetById(id);

				MovimientoReclasificacionDTO movimientoReclasificacionDTO = Mapper.Map<MovimientoReclasificacionDTO>(movimientoReclasificacion);

				return movimientoReclasificacionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<MovimientoReclasificacionDTO> Search(MovimientoReclasificacionDTO dto)
		{
			try
			{
				List<MovimientoReclasificacion> movimientoReclasificacionList = movimientoReclasificacionRepository.Search(Mapper.Map<MovimientoReclasificacion>(dto));

				List<MovimientoReclasificacionDTO> movimientoReclasificacionDTOList = Mapper.Map<List<MovimientoReclasificacionDTO>>(movimientoReclasificacionList);

				return movimientoReclasificacionDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public MovimientoReclasificacionDTO SingleSearch(MovimientoReclasificacionDTO dto)
		{
			try
			{
				MovimientoReclasificacion movimientoReclasificacion = movimientoReclasificacionRepository.SingleSearch(Mapper.Map<MovimientoReclasificacion>(dto));

				MovimientoReclasificacionDTO movimientoReclasificacionDTO = Mapper.Map<MovimientoReclasificacionDTO>(movimientoReclasificacion);

				return movimientoReclasificacionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(MovimientoReclasificacionDTO dto)
		{
			try
			{
				MovimientoReclasificacion movimientoReclasificacion = Mapper.Map<MovimientoReclasificacion>(dto);

				int newId = 0;
				using (TransactionScope tx = new())
				{
					TablaCorrelativoDTO tablaCorrelativoDTO = new()
               {
						Tabla = TablaConstante.RECLASIFICACION_ITEM,
						PeriodoEmpresaSedeDTO = new PeriodoEmpresaSedeDTO()
						{
							PeriodoId = movimientoReclasificacion.PeriodoEmpresaSede.PeriodoId,
                            EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = movimientoReclasificacion.PeriodoEmpresaSede.EmpresaSede.EmpresaId }
                            
						},
                        UsarSedeCentral = true,
                        UsuarioAccionId = movimientoReclasificacion.UsuarioCreacionId
					};

					movimientoReclasificacion.Codigo = tablaBusiness.ObtenerCorrelativo(tablaCorrelativoDTO);


					newId = movimientoReclasificacionRepository.Insert(movimientoReclasificacion);

					dto.MovimientoReclasificacionId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(MovimientoReclasificacionDTO dto)
		{
			try
			{
				MovimientoReclasificacion movimientoReclasificacion = Mapper.Map<MovimientoReclasificacion>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = movimientoReclasificacionRepository.Update(movimientoReclasificacion);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}

        public void Solicitar(MovimientoReclasificacionDTO dto)
        {
            try
            {
                MovimientoReclasificacion movimientoReclasificacion = Mapper.Map<MovimientoReclasificacion>(dto);
                             
                using (TransactionScope tx = new())
                {

                        movimientoReclasificacionRepository.ActualizarEstadoMovimiento(new MovimientoReclasificacion()
                        {
                            MovimientoReclasificacionId = dto.MovimientoReclasificacionId,
                            UsuarioCreacionId = dto.UsuarioCreacionId,
                            EstadoMovimientoReclasificacionId = 5
                        });

                    tx.Complete();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Aprobar(MovimientoReclasificacionDTO dto)
        {
            try
            {
                MovimientoReclasificacion movimientoReclasificacion = Mapper.Map<MovimientoReclasificacion>(dto);

                List<MovimientoAlmacenProducto> movimientoAlmacenProductoList = movimientoAlmacenProductoRepository.Search(new MovimientoAlmacenProducto()
                {
                    MovimientoAlmacenId = dto.MovimientoAlmacenId,
                    ConCantidadReclasificacion = SiNoConstante.S,
                    Estado = EstadoConstante.ACTIVO
                });

                if (movimientoAlmacenProductoList.Count == 0)
                    throw new Exception(SR.GetString("EX_reclasificacion_sin_items"));

                PeriodoEmpresaSede periodoEmpresaSede = periodoEmpresaSedeRepository.GetById(dto.PeriodoEmpresaSedeId);

                foreach (MovimientoAlmacenProducto movimientoAlmacenProducto in movimientoAlmacenProductoList)
                {
                    movimientoAlmacenSaldoBusiness.ValidarSaldoVenta(new MovimientoAlmacenSaldoDTO()
                    {
                        PeriodoEmpresaSedeId = periodoEmpresaSede.PeriodoEmpresaSedeId,
                        ProductoId = movimientoAlmacenProducto.ProductoReclasificacionId.Value,
                        MesContableId = dto.MesContableId,
                        CantidadIngreso = movimientoAlmacenProducto.CantidadReclasificacion
                    });
                }
                
                TransaccionAlmacen transaccionAlmacen = transaccionAlmacenRepository.SingleSearch(new TransaccionAlmacen()
                {
                    EmpresaId = periodoEmpresaSede.EmpresaSede.EmpresaId,
                    Codigo = TransaccionAlmacenConstante.RECLASIFICACION_DE_ITEMS_INGRESO
                });
                int transaccionIngresoId = transaccionAlmacen.EntityId;

                transaccionAlmacen = transaccionAlmacenRepository.SingleSearch(new TransaccionAlmacen()
                {
                    EmpresaId = periodoEmpresaSede.EmpresaSede.EmpresaId,
                    Codigo = TransaccionAlmacenConstante.RECLASIFICACION_DE_ITEMS_SALIDA
                });
                int transaccionSalidaId = transaccionAlmacen.EntityId;

                TipoMovimientoAlmacen tipoMovimientoAlmacen = tipoMovimientoAlmacenRepository.ObtenerPorCodigo(TipoMovimientoAlmacenConstante.INGRESO);
                int ingresoId = tipoMovimientoAlmacen.EntityId;

                tipoMovimientoAlmacen = tipoMovimientoAlmacenRepository.ObtenerPorCodigo(TipoMovimientoAlmacenConstante.SALIDA);
                int salidaId = tipoMovimientoAlmacen.EntityId;

                EstadoMovimientoAlmacen estadoMovimientoAlmacen = estadoMovimientoAlmacenRepository.ObtenerPorCodigo(EstadoMovimientoAlmacenConstante.MOVIMIENTO);

                MovimientoAlmacen movimientoAlmacenTmp = movimientoAlmacenRepository.GetById(dto.MovimientoAlmacenId);

                string tipoMovimientoAlmacenCodigo = movimientoAlmacenTmp.TipoMovimientoAlmacen.Codigo;

                EstadoMovimientoReclasificacion estadoMovimientoReclasificacion = estadoMovimientoReclasificacionRepository
                    .ObtenerPorCodigo(EstadoMovimientoReclasificacionConstante.APROBADO);

                MovimientoAlmacen movimientoAlmacen = new()
                {
                    PeriodoEmpresaSedeId = dto.PeriodoEmpresaSedeId,
                    AlmacenId = movimientoAlmacenTmp.AlmacenId,
                    Fecha = dto.Fecha,
                    MesContableId = dto.MesContableId,
                    ProveedorId = movimientoAlmacenTmp.ProveedorId,
                    SerieDocumento = string.Empty,
                    NumeroDocumento = dto.Codigo,
                    TieneGuiaRemision = SiNoConstante.N,
                    NumeroOrdenCompra = string.Empty,
                    NumeroGuiaRemision = string.Empty,
                    GuiaRemisionEnModificacion = SiNoConstante.N,
                    MovimientoReclasificacionId = dto.Id,
                    AfectaSaldo = SiNoConstante.S,
                    Observacion = dto.Observacion,
                    EstadoMovimientoAlmacenId = estadoMovimientoAlmacen.EntityId,
                    Estado = EstadoConstante.ACTIVO,
                    UsuarioCreacionId = dto.UsuarioCreacionId
                };

                using (TransactionScope tx = new TransactionScope())
                {
                    movimientoReclasificacionRepository.Update(movimientoReclasificacion);

                    if (tipoMovimientoAlmacenCodigo == TipoMovimientoAlmacenConstante.SALIDA)
                    {

                        movimientoAlmacen.TransaccionAlmacenId = transaccionIngresoId;
                        movimientoAlmacen.TipoMovimientoAlmacenId = ingresoId;
                        movimientoAlmacen.OrdenReclasificacion = 1;
                        MovimientoAlmacenCorrelativoDTO movimientoAlmacenCorrelativoDTO = new MovimientoAlmacenCorrelativoDTO()
                        {
                            PeriodoEmpresaSedeId = movimientoAlmacen.PeriodoEmpresaSedeId,
                            AlmacenId = movimientoAlmacen.AlmacenId,
                            TipoMovimientoAlmacenId = movimientoAlmacen.TipoMovimientoAlmacenId
                        };

                        movimientoAlmacen.Codigo = movimientoAlmacenCorrelativoBusiness.ObtenerCorrelativo(movimientoAlmacenCorrelativoDTO);
                        int newId = movimientoAlmacenRepository.Insert(movimientoAlmacen);

                        foreach (MovimientoAlmacenProducto movimientoAlmacenProducto in movimientoAlmacenProductoList)
                        {
                            MovimientoAlmacenProductoDTO movimientoAlmacenProductoDTO = new MovimientoAlmacenProductoDTO
                            {
                                MovimientoAlmacenId = newId,
                                ProductoId = movimientoAlmacenProducto.ProductoId,
                                Descripcion = string.Empty,
                                Cantidad = movimientoAlmacenProducto.CantidadReclasificacion,
                                Estado = EstadoConstante.ACTIVO,
                                UsuarioCreacionId = dto.UsuarioCreacionId
                            };

                            movimientoAlmacenProductoBusiness.Insert(movimientoAlmacenProductoDTO);
                        }

                        movimientoAlmacen.TransaccionAlmacenId = transaccionSalidaId;
                        movimientoAlmacen.TipoMovimientoAlmacenId = salidaId;
                        movimientoAlmacen.OrdenReclasificacion = 2;
                        movimientoAlmacenCorrelativoDTO = new MovimientoAlmacenCorrelativoDTO()
                        {
                            PeriodoEmpresaSedeId = movimientoAlmacen.PeriodoEmpresaSedeId,
                            AlmacenId = movimientoAlmacen.AlmacenId,
                            TipoMovimientoAlmacenId = movimientoAlmacen.TipoMovimientoAlmacenId
                        };

                        movimientoAlmacen.Codigo = movimientoAlmacenCorrelativoBusiness.ObtenerCorrelativo(movimientoAlmacenCorrelativoDTO);
                        newId = movimientoAlmacenRepository.Insert(movimientoAlmacen);

                        foreach (MovimientoAlmacenProducto movimientoAlmacenProducto in movimientoAlmacenProductoList)
                        {
                            MovimientoAlmacenProductoDTO movimientoAlmacenProductoDTO = new MovimientoAlmacenProductoDTO
                            {
                                MovimientoAlmacenId = newId,
                                ProductoId = movimientoAlmacenProducto.ProductoReclasificacionId.Value,
                                Descripcion = string.Empty,
                                Cantidad = movimientoAlmacenProducto.CantidadReclasificacion,
                                Estado = EstadoConstante.ACTIVO,
                                UsuarioCreacionId = dto.UsuarioCreacionId
                            };

                            movimientoAlmacenProductoBusiness.Insert(movimientoAlmacenProductoDTO);
                        }

                    }

                    movimientoReclasificacionRepository.AprobDesa(new MovimientoReclasificacion()
                    {
                        MovimientoReclasificacionId = movimientoReclasificacion.MovimientoReclasificacionId,
                        UsuarioCreacionId = movimientoReclasificacion.UsuarioCreacionId,
                        EstadoMovimientoReclasificacionId = 2
                    });
                    

                    tx.Complete();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Desaprobar(MovimientoReclasificacionDTO dto)
        {
            try
            {
                MovimientoReclasificacion movimientoReclasificacion = Mapper.Map<MovimientoReclasificacion>(dto);

                using (TransactionScope tx = new())
                {

                    movimientoReclasificacionRepository.AprobDesa(new MovimientoReclasificacion()
                    {
                        MovimientoReclasificacionId = dto.MovimientoReclasificacionId,
                        UsuarioCreacionId = dto.UsuarioCreacionId,
                        EstadoMovimientoReclasificacionId = 4
                    });
                   
                    tx.Complete();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Delete(MovimientoReclasificacionDTO dto)
		{
			try
			{
				MovimientoReclasificacion movimientoReclasificacion = movimientoReclasificacionRepository.GetById(dto.MovimientoReclasificacionId);

				EstadoMovimientoReclasificacion estadoMovimientoReclasificacion = estadoMovimientoReclasificacionRepository
					.ObtenerPorCodigo(EstadoMovimientoReclasificacionConstante.ANULADO);

				List<MovimientoAlmacenProducto> movimientoAlmacenProductoList = movimientoAlmacenProductoRepository.Search(new MovimientoAlmacenProducto()
				{
					MovimientoAlmacenId = movimientoReclasificacion.MovimientoAlmacenId,
					ConCantidadReclasificacion = SiNoConstante.S,
					Estado = EstadoConstante.ACTIVO
				});

				using (TransactionScope tx = new TransactionScope())
				{

					foreach(MovimientoAlmacenProducto movimientoAlmacenProducto in movimientoAlmacenProductoList)
               {
						movimientoAlmacenProducto.ProductoReclasificacionId = null;
						movimientoAlmacenProducto.CantidadReclasificacion = decimal.Zero;
						movimientoAlmacenProducto.UsuarioCreacionId = dto.UsuarioCreacionId;
						movimientoAlmacenProductoRepository.ActualizarProductoReclasificacion(movimientoAlmacenProducto);
					} 

					if(movimientoReclasificacion.EstadoMovimientoReclasificacion.Codigo == EstadoMovimientoReclasificacionConstante.APROBADO)
               {
						List<MovimientoAlmacen> movimientoAlmacenList = movimientoAlmacenRepository.Search(new MovimientoAlmacen()
						{
							PeriodoEmpresaSedeId = movimientoReclasificacion.PeriodoEmpresaSedeId,
							MovimientoReclasificacionId = movimientoReclasificacion.EntityId,
							Estado = EstadoConstante.ACTIVO
						});

						foreach (MovimientoAlmacen movimientoAlmacen in movimientoAlmacenList)
							movimientoAlmacenBusiness.Delete(new MovimientoAlmacenDTO() { 
								MovimientoAlmacenId = movimientoAlmacen.EntityId,
								UsuarioCreacionId = dto.UsuarioCreacionId
							});
               }

					movimientoReclasificacion.EstadoMovimientoReclasificacionId = estadoMovimientoReclasificacion.EntityId;
					movimientoReclasificacionRepository.Delete(movimientoReclasificacion);

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
