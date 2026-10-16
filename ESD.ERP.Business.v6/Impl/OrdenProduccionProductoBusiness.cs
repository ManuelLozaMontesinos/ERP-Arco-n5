using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Common;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;
using ESD.ERP.Repository.Impl;
using Microsoft.IdentityModel.Tokens;

namespace ESD.ERP.Business.Impl
{
	public class OrdenProduccionProductoBusiness : IOrdenProduccionProductoBusiness
	{
		private readonly IOrdenProduccionRepository ordenProduccionRepository;
		private readonly IOrdenProduccionProductoRepository ordenProduccionProductoRepository;
		private readonly IFamiliaProductoRepository familiaProductoRepository;
		private readonly IEstadoOrdenProduccionRepository estadoOrdenProduccionRepository;
		private readonly IRutaProduccionRepository rutaProduccionRepository;
		private readonly IProductoSubProcesoProduccionRepository productoSubProcesoProduccionRepository;
		private readonly IRutaSubProcesoProduccionRepository rutaSubProcesoProduccionRepository;
		private readonly IEstadoOrdenProduccionSubProcesoRepository estadoOrdenProduccionSubProcesoRepository;
		private readonly IEstadoDespachoRepository estadoDespachoRepository;
		private readonly IOrdenProduccionSubProcesoRepository ordenProduccionSubProcesoRepository;

        public OrdenProduccionProductoBusiness(
			IOrdenProduccionProductoRepository ordenProduccionProductoRepository,
			IFamiliaProductoRepository familiaProductoRepository,
            IEstadoOrdenProduccionRepository estadoOrdenProduccionRepository,
            IRutaProduccionRepository rutaProduccionRepository,
            IProductoSubProcesoProduccionRepository productoSubProcesoProduccionRepository,
            IRutaSubProcesoProduccionRepository rutaSubProcesoProduccionRepository,
            IEstadoOrdenProduccionSubProcesoRepository estadoOrdenProduccionSubProcesoRepository,
            IEstadoDespachoRepository estadoDespachoRepository,
            IOrdenProduccionSubProcesoRepository ordenProduccionSubProcesoRepository,
			IOrdenProduccionRepository ordenProduccionRepository
            )
		{
			this.ordenProduccionProductoRepository = ordenProduccionProductoRepository;
			this.familiaProductoRepository = familiaProductoRepository;
			this.estadoOrdenProduccionRepository = estadoOrdenProduccionRepository;
			this.rutaProduccionRepository = rutaProduccionRepository;
			this.productoSubProcesoProduccionRepository = productoSubProcesoProduccionRepository;
			this.rutaSubProcesoProduccionRepository = rutaSubProcesoProduccionRepository;
			this.estadoOrdenProduccionSubProcesoRepository = estadoOrdenProduccionSubProcesoRepository;
			this.estadoDespachoRepository = estadoDespachoRepository;
			this.ordenProduccionSubProcesoRepository = ordenProduccionSubProcesoRepository;
			this.ordenProduccionRepository = ordenProduccionRepository;
        }
		public OrdenProduccionProductoDTO GetById(int id)
		{
			try
			{
				OrdenProduccionProducto ordenProduccionProducto = ordenProduccionProductoRepository.GetById(id);

				OrdenProduccionProductoDTO ordenProduccionProductoDTO = Mapper.Map<OrdenProduccionProductoDTO>(ordenProduccionProducto);

				return ordenProduccionProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<OrdenProduccionProductoDTO> Search(OrdenProduccionProductoDTO dto)
		{
			try
			{
				List<OrdenProduccionProducto> ordenProduccionProductoList = ordenProduccionProductoRepository.Search(Mapper.Map<OrdenProduccionProducto>(dto));

				List<OrdenProduccionProductoDTO> ordenProduccionProductoDTOList = Mapper.Map<List<OrdenProduccionProductoDTO>>(ordenProduccionProductoList);

				return ordenProduccionProductoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public OrdenProduccionProductoDTO SingleSearch(OrdenProduccionProductoDTO dto)
		{
			try
			{
				OrdenProduccionProducto ordenProduccionProducto = ordenProduccionProductoRepository.SingleSearch(Mapper.Map<OrdenProduccionProducto>(dto));

				OrdenProduccionProductoDTO ordenProduccionProductoDTO = Mapper.Map<OrdenProduccionProductoDTO>(ordenProduccionProducto);

				return ordenProduccionProductoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(OrdenProduccionProductoDTO dto)
		{
			try
			{
				OrdenProduccionProducto ordenProduccionProducto = Mapper.Map<OrdenProduccionProducto>(dto);

				int newId = 0;
				using (TransactionScope tx = new())
				{
					newId = ordenProduccionProductoRepository.Insert(ordenProduccionProducto);

					dto.OrdenProduccionProductoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(OrdenProduccionProductoDTO dto)
		{
			try
			{
				OrdenProduccionProducto ordenProduccionProducto = Mapper.Map<OrdenProduccionProducto>(dto);

				int id = 0;
				using (TransactionScope tx = new())
				{
					id = ordenProduccionProductoRepository.Update(ordenProduccionProducto);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
        public void Delete(OrdenProduccionProductoDTO dto)
		{
			try
			{
				OrdenProduccionProducto ordenProduccionProducto = Mapper.Map<OrdenProduccionProducto>(dto);

				using (TransactionScope tx = new())
				{
					ordenProduccionProductoRepository.Delete(ordenProduccionProducto);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void UpdateOrden(OrdenProduccionProductoDTO dto)
		{
			try
			{
				OrdenProduccionProducto ordenProduccionProducto = Mapper.Map<OrdenProduccionProducto>(dto);

                using (TransactionScope tx = new())
				{
					ordenProduccionProductoRepository.UpdateOrden(ordenProduccionProducto);
					
					tx.Complete();
                }
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void actualizarEnvioProd(OrdenProduccionProductoDTO dto)
		{
			try
			{
				OrdenProduccionProducto ordenProduccionProducto = Mapper.Map<OrdenProduccionProducto>(dto);

				using (TransactionScope tx = new())
				{
					ordenProduccionProductoRepository.actualizarEnvioProd(ordenProduccionProducto);

					tx.Complete();
                }
			}
			catch (Exception)
			{
				throw;
			}
		}

        public void GenerarSubProcesos(OrdenProduccionProductoDTO dto)
		{
			try
			{

                OrdenProduccion ordenProduccion= ordenProduccionRepository.GetById(dto.OrdenProduccionId);

                List<OrdenProduccionProducto> ordenProduccionProductoList = ordenProduccionProductoRepository.Search(Mapper.Map<OrdenProduccionProducto>(dto));
				List<OrdenProduccionProductoDTO> ordenProduccionProductoDTOList = Mapper.Map<List<OrdenProduccionProductoDTO>>(ordenProduccionProductoList);

                EstadoOrdenProduccion estadoOrdenProduccion = estadoOrdenProduccionRepository.ObtenerPorCodigo(EstadoOrdenProduccionConstante.EN_PROCESO);
                RutaProduccion rutaProduccion = rutaProduccionRepository.SingleSearch(new RutaProduccion());
                List<RutaSubProcesoProduccion> rutaSubProcesoProduccionList = rutaSubProcesoProduccionRepository.Search(new RutaSubProcesoProduccion()
                {
                    OrderBy = "rspp.orden",
                    RutaProduccionId = rutaProduccion.EntityId
                });

                string incluyeInsumo = SiNoConstante.N;	
                if (ordenProduccionProductoDTOList.Where(x => x.ProductoDTO.FamiliaProductoDTO.ClaseProductoDTO.GrupoProductoDTO.EsInsumo == SiNoConstante.S).Count() > 0)
                    incluyeInsumo = SiNoConstante.S;

                EstadoDespacho estadoDespacho = null;	
                if (incluyeInsumo == SiNoConstante.S)
                    estadoDespacho = estadoDespachoRepository.ObtenerPorCodigo(EstadoDespachoConstante.PENDIENTE);

                List<int> productoIdList = ordenProduccionProductoDTOList.GroupBy(x => x.ProductoId).Select(x => x.Key).ToList();	

                List<ProductoSubProcesoProduccion> productoSubProcesoProduccionList = productoSubProcesoProduccionRepository.Search(new ProductoSubProcesoProduccion()
                {
                    ProductoIdList = productoIdList
                });

                EstadoOrdenProduccionSubProceso estadoOrdenProduccionSubProceso = estadoOrdenProduccionSubProcesoRepository.
                    ObtenerPorCodigo(EstadoOrdenProduccionSubProcesoConstante.PENDIENTE);

                List<int> subProcesoProduccionIdList = productoSubProcesoProduccionList.GroupBy(x => x.SubProcesoProduccionId).Select(x => x.Key).ToList();
                if (subProcesoProduccionIdList.Count > 0)
                {
                    int orden = 1;
                    int? subProcesoProduccionId = null; 
                    foreach (RutaSubProcesoProduccion rutaSubProcesoProduccion in rutaSubProcesoProduccionList)
                    {
                        if (subProcesoProduccionIdList.Contains(rutaSubProcesoProduccion.SubProcesoProduccionId))
                        {
                            OrdenProduccionSubProceso ordenProduccionSubProceso = new()
                            {	
                                OrdenProduccionId = dto.OrdenProduccionId,
                                SubProcesoProduccionId = rutaSubProcesoProduccion.SubProcesoProduccionId,
                                Orden = orden,
                                SubProcesoProduccionAnteriorId = subProcesoProduccionId,
                                SubProcesoProduccionAnteriorConcluido = SiNoConstante.N,
                                Observacion = string.Empty,
                                EstadoOrdenProduccionSubProcesoId = estadoOrdenProduccionSubProceso.EntityId,
								AlmacenId = ordenProduccion.AlmacenId,
                                Estado = EstadoConstante.ACTIVO,
                                UsuarioCreacionId = dto.UsuarioCreacionId
                            };

                            subProcesoProduccionId = rutaSubProcesoProduccion.SubProcesoProduccionId;
                            ordenProduccionSubProcesoRepository.Insert(ordenProduccionSubProceso);
                        }
                        orden++;
                    }
                }
            }

            catch (Exception)
			{
				throw;
			}
		}

        public void actualizarInsumos(OrdenProduccionProductoDTO dto)
        {
            try
            {
                OrdenProduccionProducto ordenProduccionProducto = Mapper.Map<OrdenProduccionProducto>(dto);

                using (TransactionScope tx = new())
                {
                    ordenProduccionProductoRepository.actualizarInsumos(ordenProduccionProducto);

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
