using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class OrdenCompraEntregaBusiness : IOrdenCompraEntregaBusiness
	{
		private readonly IOrdenCompraEntregaRepository ordenCompraEntregaRepository;

		public OrdenCompraEntregaBusiness(
			IOrdenCompraEntregaRepository ordenCompraEntregaRepository
			)
		{
			this.ordenCompraEntregaRepository = ordenCompraEntregaRepository;
		}
		public OrdenCompraEntregaDTO GetById(int id)
		{
			try
			{
				OrdenCompraEntrega ordenCompraEntrega = ordenCompraEntregaRepository.GetById(id);

				OrdenCompraEntregaDTO ordenCompraEntregaDTO = Mapper.Map<OrdenCompraEntregaDTO>(ordenCompraEntrega);

				return ordenCompraEntregaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<OrdenCompraEntregaDTO> Search(OrdenCompraEntregaDTO dto)
		{
			try
			{
				List<OrdenCompraEntrega> ordenCompraEntregaList = ordenCompraEntregaRepository.Search(Mapper.Map<OrdenCompraEntrega>(dto));

				List<OrdenCompraEntregaDTO> ordenCompraEntregaDTOList = Mapper.Map<List<OrdenCompraEntregaDTO>>(ordenCompraEntregaList);

				return ordenCompraEntregaDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public OrdenCompraEntregaDTO SingleSearch(OrdenCompraEntregaDTO dto)
		{
			try
			{
				OrdenCompraEntrega ordenCompraEntrega = ordenCompraEntregaRepository.SingleSearch(Mapper.Map<OrdenCompraEntrega>(dto));

				OrdenCompraEntregaDTO ordenCompraEntregaDTO = Mapper.Map<OrdenCompraEntregaDTO>(ordenCompraEntrega);

				return ordenCompraEntregaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(OrdenCompraEntregaDTO dto)
		{
			try
			{
				OrdenCompraEntrega ordenCompraEntrega = Mapper.Map<OrdenCompraEntrega>(dto);

				int newId = 0;
				using (TransactionScope tx = new())
				{
					newId = ordenCompraEntregaRepository.Insert(ordenCompraEntrega);

					dto.OrdenCompraEntregaId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(OrdenCompraEntregaDTO dto)
		{
			try
			{
				OrdenCompraEntrega ordenCompraEntrega = Mapper.Map<OrdenCompraEntrega>(dto);

				int id = 0;
				using (TransactionScope tx = new())
				{
					id = ordenCompraEntregaRepository.Update(ordenCompraEntrega);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(OrdenCompraEntregaDTO dto)
		{
			try
			{
				OrdenCompraEntrega ordenCompraEntrega = Mapper.Map<OrdenCompraEntrega>(dto);

				using (TransactionScope tx = new())
				{
					ordenCompraEntregaRepository.Delete(ordenCompraEntrega);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public void AgregarDesdeOrdenCompraProducto(OrdenCompraEntregaDTO dto)
      {
			try
			{
				OrdenCompraEntrega ordenCompraEntrega = Mapper.Map<OrdenCompraEntrega>(dto);

				using (TransactionScope tx = new())
				{
					ordenCompraEntregaRepository.AgregarDesdeOrdenCompraProducto(ordenCompraEntrega);

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
