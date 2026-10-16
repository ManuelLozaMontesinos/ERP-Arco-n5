using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class OrdenProduccionProductoEntregaBusiness : IOrdenProduccionProductoEntregaBusiness
	{
		private readonly IOrdenProduccionProductoEntregaRepository ordenProduccionProductoEntregaRepository;
		private readonly IOrdenProduccionProductoRepository ordenProduccionProductoRepository;
		private readonly IComprobanteRepository comprobanteRepository;

		public OrdenProduccionProductoEntregaBusiness(
			IOrdenProduccionProductoEntregaRepository ordenProduccionProductoEntregaRepository,
			IOrdenProduccionProductoRepository ordenProduccionProductoRepository,
			IComprobanteRepository comprobanteRepository
			)
		{
			this.ordenProduccionProductoEntregaRepository = ordenProduccionProductoEntregaRepository;
			this.ordenProduccionProductoRepository = ordenProduccionProductoRepository;
			this.comprobanteRepository = comprobanteRepository;
		}
		public OrdenProduccionProductoEntregaDTO GetById(int id)
		{
			try
			{
				OrdenProduccionProductoEntrega ordenProduccionProductoEntrega = ordenProduccionProductoEntregaRepository.GetById(id);

				OrdenProduccionProductoEntregaDTO ordenProduccionProductoEntregaDTO = Mapper.Map<OrdenProduccionProductoEntregaDTO>(ordenProduccionProductoEntrega);

				return ordenProduccionProductoEntregaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<OrdenProduccionProductoEntregaDTO> Search(OrdenProduccionProductoEntregaDTO dto)
		{
			try
			{
				List<OrdenProduccionProductoEntrega> ordenProduccionProductoEntregaList = ordenProduccionProductoEntregaRepository.Search(Mapper.Map<OrdenProduccionProductoEntrega>(dto));

				List<OrdenProduccionProductoEntregaDTO> ordenProduccionProductoEntregaDTOList = Mapper.Map<List<OrdenProduccionProductoEntregaDTO>>(ordenProduccionProductoEntregaList);

				return ordenProduccionProductoEntregaDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public OrdenProduccionProductoEntregaDTO SingleSearch(OrdenProduccionProductoEntregaDTO dto)
		{
			try
			{
				OrdenProduccionProductoEntrega ordenProduccionProductoEntrega = ordenProduccionProductoEntregaRepository.SingleSearch(Mapper.Map<OrdenProduccionProductoEntrega>(dto));

				OrdenProduccionProductoEntregaDTO ordenProduccionProductoEntregaDTO = Mapper.Map<OrdenProduccionProductoEntregaDTO>(ordenProduccionProductoEntrega);

				return ordenProduccionProductoEntregaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(OrdenProduccionProductoEntregaDTO dto)
		{
			try
			{
				OrdenProduccionProductoEntrega ordenProduccionProductoEntrega = Mapper.Map<OrdenProduccionProductoEntrega>(dto);

				int newId = 0;
				using (TransactionScope tx = new())
				{
					newId = ordenProduccionProductoEntregaRepository.Insert(ordenProduccionProductoEntrega);

					dto.OrdenProduccionProductoEntregaId = newId;

					//actualizarCantidadEntregada(dto);

					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(OrdenProduccionProductoEntregaDTO dto)
		{
			try
			{
				OrdenProduccionProductoEntrega ordenProduccionProductoEntrega = Mapper.Map<OrdenProduccionProductoEntrega>(dto);

				int id = 0;
				using (TransactionScope tx = new())
				{
					id = ordenProduccionProductoEntregaRepository.Update(ordenProduccionProductoEntrega);

					//actualizarCantidadEntregada(dto);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(OrdenProduccionProductoEntregaDTO dto)
		{
			try
			{
				OrdenProduccionProductoEntrega ordenProduccionProductoEntrega = Mapper.Map<OrdenProduccionProductoEntrega>(dto);

				using (TransactionScope tx = new())
				{
					ordenProduccionProductoEntregaRepository.Delete(ordenProduccionProductoEntrega);

					//actualizarCantidadEntregada(dto);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		//private void actualizarCantidadEntregada(OrdenProduccionProductoEntregaDTO dto)
  //    {
		//	ordenProduccionProductoRepository.ActualizarCantidadEntregada(new OrdenProduccionProducto()
		//	{
		//		OrdenProduccionProductoId = dto.OrdenProduccionProductoId
		//	});

		//	comprobanteRepository.ActualizarEstadoDespachoPorCantidadEntregada(new Comprobante()
		//	{
		//		ComprobanteId = dto.OrdenProduccionProductoDTO.ComprobanteId
		//	});
		//}
	}
}
