using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class ComprobanteProductoEntregaBusiness : IComprobanteProductoEntregaBusiness
	{
		private readonly IComprobanteProductoEntregaRepository comprobanteProductoEntregaRepository;
		private readonly IComprobanteProductoRepository comprobanteProductoRepository;
		private readonly IComprobanteRepository comprobanteRepository;

		public ComprobanteProductoEntregaBusiness(
			IComprobanteProductoEntregaRepository comprobanteProductoEntregaRepository,
			IComprobanteProductoRepository comprobanteProductoRepository,
			IComprobanteRepository comprobanteRepository
			)
		{
			this.comprobanteProductoEntregaRepository = comprobanteProductoEntregaRepository;
			this.comprobanteProductoRepository = comprobanteProductoRepository;
			this.comprobanteRepository = comprobanteRepository;
		}
		public ComprobanteProductoEntregaDTO GetById(int id)
		{
			try
			{
				ComprobanteProductoEntrega comprobanteProductoEntrega = comprobanteProductoEntregaRepository.GetById(id);

				ComprobanteProductoEntregaDTO comprobanteProductoEntregaDTO = Mapper.Map<ComprobanteProductoEntregaDTO>(comprobanteProductoEntrega);

				return comprobanteProductoEntregaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<ComprobanteProductoEntregaDTO> Search(ComprobanteProductoEntregaDTO dto)
		{
			try
			{
				List<ComprobanteProductoEntrega> comprobanteProductoEntregaList = comprobanteProductoEntregaRepository.Search(Mapper.Map<ComprobanteProductoEntrega>(dto));

				List<ComprobanteProductoEntregaDTO> comprobanteProductoEntregaDTOList = Mapper.Map<List<ComprobanteProductoEntregaDTO>>(comprobanteProductoEntregaList);

				return comprobanteProductoEntregaDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public ComprobanteProductoEntregaDTO SingleSearch(ComprobanteProductoEntregaDTO dto)
		{
			try
			{
				ComprobanteProductoEntrega comprobanteProductoEntrega = comprobanteProductoEntregaRepository.SingleSearch(Mapper.Map<ComprobanteProductoEntrega>(dto));

				ComprobanteProductoEntregaDTO comprobanteProductoEntregaDTO = Mapper.Map<ComprobanteProductoEntregaDTO>(comprobanteProductoEntrega);

				return comprobanteProductoEntregaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(ComprobanteProductoEntregaDTO dto)
		{
			try
			{
				ComprobanteProductoEntrega comprobanteProductoEntrega = Mapper.Map<ComprobanteProductoEntrega>(dto);

				int newId = 0;
				using (TransactionScope tx = new())
				{
					newId = comprobanteProductoEntregaRepository.Insert(comprobanteProductoEntrega);

					dto.ComprobanteProductoEntregaId = newId;

					actualizarCantidadEntregada(dto);

					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(ComprobanteProductoEntregaDTO dto)
		{
			try
			{
				ComprobanteProductoEntrega comprobanteProductoEntrega = Mapper.Map<ComprobanteProductoEntrega>(dto);

				int id = 0;
				using (TransactionScope tx = new())
				{
					id = comprobanteProductoEntregaRepository.Update(comprobanteProductoEntrega);

					actualizarCantidadEntregada(dto);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(ComprobanteProductoEntregaDTO dto)
		{
			try
			{
				ComprobanteProductoEntrega comprobanteProductoEntrega = Mapper.Map<ComprobanteProductoEntrega>(dto);

				using (TransactionScope tx = new())
				{
					comprobanteProductoEntregaRepository.Delete(comprobanteProductoEntrega);

					actualizarCantidadEntregada(dto);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		private void actualizarCantidadEntregada(ComprobanteProductoEntregaDTO dto)
      {
			comprobanteProductoRepository.ActualizarCantidadEntregada(new ComprobanteProducto()
			{
				ComprobanteProductoId = dto.ComprobanteProductoId
			});

			comprobanteRepository.ActualizarEstadoDespachoPorCantidadEntregada(new Comprobante()
			{
				ComprobanteId = dto.ComprobanteProductoDTO.ComprobanteId
			});
		}
	}
}
