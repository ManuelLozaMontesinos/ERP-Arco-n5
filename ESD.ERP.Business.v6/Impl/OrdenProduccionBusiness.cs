using System;
using System.Collections.Generic;
using System.Threading;
using System.Transactions;
using System.Linq;
using ESD.Core;
using ESD.ERP.Common;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;
using ESD.ERP.Repository.Impl;
using System.Collections;
using ESD.ERP.Business.Resources;

namespace ESD.ERP.Business.Impl
{
	public class OrdenProduccionBusiness : IOrdenProduccionBusiness
	{
		private readonly IOrdenProduccionRepository ordenProduccionRepository;
		private readonly ITablaBusiness tablaBusiness;
		private readonly IEstadoDespachoRepository estadoDespachoRepository;
		private readonly IOrdenProduccionSubProcesoRepository ordenProduccionSubProcesoRepository;
		private readonly IOrdenProduccionSubProcesoBusiness ordenProduccionSubProcesoBusiness;
		private readonly IOrdenProduccionProductoRepository ordenProduccionProductoRepository;
		private readonly ICotizacionProductoRepository cotizacionProductoRepository;
		private readonly ICotizacionRepository cotizacionRepository;


        public OrdenProduccionBusiness(
			IOrdenProduccionRepository ordenProduccionRepository,
            ITablaBusiness tablaBusiness,
            IEstadoDespachoRepository estadoDespachoRepository,
            IOrdenProduccionSubProcesoRepository ordenProduccionSubProcesoRepository,
            IOrdenProduccionSubProcesoBusiness ordenProduccionSubProcesoBusiness,
            IOrdenProduccionProductoRepository ordenProduccionProductoRepository,
			ICotizacionProductoRepository cotizacionProductoRepository,
			ICotizacionRepository cotizacionRepository
		)
        {
            this.ordenProduccionRepository = ordenProduccionRepository;
            this.tablaBusiness = tablaBusiness;
            this.estadoDespachoRepository = estadoDespachoRepository;
            this.ordenProduccionSubProcesoRepository = ordenProduccionSubProcesoRepository;
            this.ordenProduccionSubProcesoBusiness = ordenProduccionSubProcesoBusiness;
            this.ordenProduccionProductoRepository = ordenProduccionProductoRepository;
            this.cotizacionProductoRepository = cotizacionProductoRepository;
			this.cotizacionRepository = cotizacionRepository;
        }
        public OrdenProduccionDTO GetById(int id)
		{
			try
			{
				OrdenProduccion ordenProduccion = ordenProduccionRepository.GetById(id);

				OrdenProduccionDTO ordenProduccionDTO = Mapper.Map<OrdenProduccionDTO>(ordenProduccion);

				return ordenProduccionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<OrdenProduccionDTO> Search(OrdenProduccionDTO dto)
		{
			try
			{
				List<OrdenProduccion> ordenProduccionList = ordenProduccionRepository.Search(Mapper.Map<OrdenProduccion>(dto));

				List<OrdenProduccionDTO> ordenProduccionDTOList = Mapper.Map<List<OrdenProduccionDTO>>(ordenProduccionList);

				return ordenProduccionDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public OrdenProduccionDTO SingleSearch(OrdenProduccionDTO dto)
		{
			try
			{
				OrdenProduccion ordenProduccion = ordenProduccionRepository.SingleSearch(Mapper.Map<OrdenProduccion>(dto));

				OrdenProduccionDTO ordenProduccionDTO = Mapper.Map<OrdenProduccionDTO>(ordenProduccion);

				return ordenProduccionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(OrdenProduccionDTO dto)
		{
			try
			{
				OrdenProduccion ordenProduccion = Mapper.Map<OrdenProduccion>(dto);

				int newId = 0;
				using (TransactionScope tx = new())
				{

                    TablaCorrelativoDTO tablaCorrelativoDTO = new TablaCorrelativoDTO
                    {
                        Tabla = TablaConstante.ORDEN_PRODUCCION,
                        PeriodoEmpresaSedeDTO = new PeriodoEmpresaSedeDTO()
                        {
                            PeriodoEmpresaSedeId = ordenProduccion.PeriodoEmpresaSedeId
                        },
                        UsuarioAccionId = ordenProduccion.UsuarioCreacionId
                    };

                    string year = dto.PeriodoEmpresaSedeDTO.PeriodoDTO.Codigo.ToString();

                    ordenProduccion.Codigo = string.Concat(year.Substring(2, 2), tablaBusiness.ObtenerCorrelativo(tablaCorrelativoDTO));

                    newId = ordenProduccionRepository.Insert(ordenProduccion);

					dto.OrdenProduccionId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(OrdenProduccionDTO dto)
		{
			try
			{
				OrdenProduccion ordenProduccion = Mapper.Map<OrdenProduccion>(dto);

				int id = 0;
				using (TransactionScope tx = new())
				{
					id = ordenProduccionRepository.Update(ordenProduccion);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(OrdenProduccionDTO dto)
		{
			try
			{
				OrdenProduccion ordenProduccion = Mapper.Map<OrdenProduccion>(dto);

                Cotizacion cotiza = cotizacionRepository.GetById((int)dto.CotizacionId);

				RestablecerStockPedido(dto);
				
				using (TransactionScope tx = new())
				{
					ordenProduccionRepository.Delete(ordenProduccion);

					if(cotiza.EnviadoProduccion == SiNoConstante.S)
					{
						cotizacionRepository.ActualizarEnviadoProduccion(new Cotizacion()
						{
							CotizacionId = cotiza.CotizacionId,
							EnviadoProduccion = SiNoConstante.N,
							UsuarioCreacionId = dto.UsuarioCreacionId
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

		public void RestablecerStockPedido(OrdenProduccionDTO dto)
		{
			OrdenProduccion ordenProduccion = ordenProduccionRepository.GetById(dto.OrdenProduccionId);

			List<CotizacionProducto> cotizacionProductos = cotizacionProductoRepository.Search(new CotizacionProducto()
			{
				CotizacionId = (int)ordenProduccion.CotizacionId
            });

			List<OrdenProduccionProducto> ordenProduccionProducto = ordenProduccionProductoRepository.Search(new OrdenProduccionProducto()
			{
				OrdenProduccionId = ordenProduccion.OrdenProduccionId
			});

            var Stock = (from productoCoti in cotizacionProductos
                         from productoOP in ordenProduccionProducto
					     where productoCoti.CotizacionProductoId== productoOP.RegistroReferencialId && productoCoti.ProductoId != 3083
						 select productoCoti).ToList();

            var Parcial = (from productoCoti in cotizacionProductos
						   from productoOP in ordenProduccionProducto
                           where productoCoti.CotizacionProductoId == productoOP.RegistroReferencialId && productoOP.ProductoId != 3083
                           select productoOP).ToList();

            try
			{
				using (TransactionScope tx = new())
				{
                    foreach (CotizacionProducto cotizaProd in Stock)
                    {
						OrdenProduccionProducto ordenProduccionProds = Parcial.Where(x => x.RegistroReferencialId == cotizaProd.CotizacionProductoId).SingleOrDefault();

						var restante = cotizaProd.CantidadParcial - ordenProduccionProds.Cantidad;

						if (restante < 0)
						throw new Exception(SR.GetString("EX_cotizacion_producto_parcial"));

						CotizacionProducto cotizacionPrd = new()
						{
							CotizacionProductoId = cotizaProd.CotizacionProductoId,
							Cantidad = restante
						};
						cotizacionProductoRepository.ActualizarSaldoProduccion(cotizacionPrd);
                    }
                    tx.Complete();
				}
			}
			catch (Exception)
			{ 
				throw;
			}
		}

		public void CerrarDespacho(OrdenProduccionDTO dto)
		{
			try
			{
				OrdenProduccion ordenProduccion = Mapper.Map<OrdenProduccion>(dto);

				EstadoDespacho estadoDespacho = estadoDespachoRepository.ObtenerPorCodigo(EstadoDespachoConstante.ATENDIDO);
				DateTime fechaDespacho = DateTime.Now;

				using (TransactionScope tx = new())
				{
					ordenProduccionRepository.ActualizarEstadoDespacho(new OrdenProduccion()
					{
							OrdenProduccionId = dto.OrdenProduccionId,
						FechaDespacho = fechaDespacho,
						PersonalId = dto.PersonalId,
						EstadoDespachoId = estadoDespacho.EntityId,
						ObservacionDespacho = dto.ObservacionDespacho
					});

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void EliminarSubProcesos(OrdenProduccionDTO dto)
		{
			try
			{
				OrdenProduccion ordenProduccion = Mapper.Map<OrdenProduccion>(dto);

				using (TransactionScope tx = new())
				{
					ordenProduccionRepository.EliminarSubProcesos(new OrdenProduccion()
					{
						OrdenProduccionId = dto.OrdenProduccionId
					});

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void ActualizarFecha(OrdenProduccionDTO dto)
		{
			try
			{
				OrdenProduccion ordenProduccion = Mapper.Map<OrdenProduccion>(dto);

				using (TransactionScope tx = new())
				{
					ordenProduccionRepository.ActualizarFecha(new OrdenProduccion()
					{
						OrdenProduccionId = dto.OrdenProduccionId,
						Fecha = dto.Fecha,
						UsuarioCreacionId = dto.UsuarioCreacionId
					});

					tx.Complete();
                }
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void CerrarInsumo(OrdenProduccionDTO dto)
		{
			try
			{
				OrdenProduccion ordenProduccion = Mapper.Map<OrdenProduccion>(dto);

				using (TransactionScope tx = new())
				{
					ordenProduccionRepository.CerrarInsumo(new OrdenProduccion()
					{
						OrdenProduccionId = dto.OrdenProduccionId,
						EstadoDespachoId = 3,
						Observacion = dto.ObservacionDespacho,
						PersonalId = dto.PersonalId
					});

					tx.Complete();
                }
			}
			catch(Exception)
			{
				throw;
			}
		}

		public void CambiarEstado(OrdenProduccionDTO dto)
		{
			try
			{
				OrdenProduccion ordenProduccion = Mapper.Map<OrdenProduccion>(dto);

				using (TransactionScope tx = new())
				{
					ordenProduccionRepository.CambiarEstado(new OrdenProduccion()
					{
						OrdenProduccionId = dto.OrdenProduccionId,
						EstadoPlanoMelId = dto.EstadoPlanoMelId,
						EstadoPlanoMdfId = dto.EstadoPlanoMdfId,
						Comentarios = dto.Comentarios
					});

					tx.Complete();
                }
			}
			catch
			{
				throw;
			}
		}

        public void AsignarMaquina(OrdenProduccionDTO dto)
        {

            OrdenProduccion ordenProduccion = ordenProduccionRepository.GetById(dto.OrdenProduccionId);

            List<OrdenProduccionSubProceso> ordenProduccionSubProcesoList = ordenProduccionSubProcesoRepository.Search(new OrdenProduccionSubProceso()
            {
                OrdenProduccionId = ordenProduccion.OrdenProduccionId,
                SubProcesoProduccionId = 1,
                Estado = EstadoConstante.ACTIVO
            });

            try
            {
                using (TransactionScope tx = new())
                {
                    foreach (OrdenProduccionSubProceso ordenProduccionSubProceso in ordenProduccionSubProcesoList)
                    {
                        OrdenProduccionSubProcesoDTO ordenProduccionSubProcesoDTO = dto.OrdenProduccionSubProcesoDTOList.
                        Where(x => x.OrdenProduccionSubProcesoId == ordenProduccionSubProceso.EntityId).SingleOrDefault();

                        OrdenProduccionSubProcesoDTO ordenProduccionSubProcesoAsigMaquinaDTO = new()
                        {
                            OrdenProduccionSubProcesoId = ordenProduccionSubProceso.OrdenProduccionSubProcesoId,
                            MaquinaId = ordenProduccionSubProcesoDTO.MaquinaId,
                            UsuarioCreacionId = dto.UsuarioCreacionId
                        };
                        ordenProduccionSubProcesoBusiness.Maquina(ordenProduccionSubProcesoAsigMaquinaDTO);
                    }

                    tx.Complete();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
