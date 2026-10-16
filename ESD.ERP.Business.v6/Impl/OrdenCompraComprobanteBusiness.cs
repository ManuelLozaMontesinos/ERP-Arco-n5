using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class OrdenCompraComprobanteBusiness : IOrdenCompraComprobanteBusiness
	{
		private readonly IOrdenCompraComprobanteRepository ordenCompraComprobanteRepository;

		public OrdenCompraComprobanteBusiness(
			IOrdenCompraComprobanteRepository ordenCompraComprobanteRepository
			)
		{
			this.ordenCompraComprobanteRepository = ordenCompraComprobanteRepository;
		}
		public OrdenCompraComprobanteDTO GetById(int id)
		{
			try
			{
				OrdenCompraComprobante ordenCompraComprobante = ordenCompraComprobanteRepository.GetById(id);

				OrdenCompraComprobanteDTO ordenCompraComprobanteDTO = Mapper.Map<OrdenCompraComprobanteDTO>(ordenCompraComprobante);

				return ordenCompraComprobanteDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<OrdenCompraComprobanteDTO> Search(OrdenCompraComprobanteDTO dto)
		{
			try
			{
				List<OrdenCompraComprobante> ordenCompraComprobanteList = ordenCompraComprobanteRepository.Search(Mapper.Map<OrdenCompraComprobante>(dto));

				List<OrdenCompraComprobanteDTO> ordenCompraComprobanteDTOList = Mapper.Map<List<OrdenCompraComprobanteDTO>>(ordenCompraComprobanteList);

				return ordenCompraComprobanteDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public OrdenCompraComprobanteDTO SingleSearch(OrdenCompraComprobanteDTO dto)
		{
			try
			{
				OrdenCompraComprobante ordenCompraComprobante = ordenCompraComprobanteRepository.SingleSearch(Mapper.Map<OrdenCompraComprobante>(dto));

				OrdenCompraComprobanteDTO ordenCompraComprobanteDTO = Mapper.Map<OrdenCompraComprobanteDTO>(ordenCompraComprobante);

				return ordenCompraComprobanteDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(OrdenCompraComprobanteDTO dto)
		{
			try
			{
				OrdenCompraComprobante ordenCompraComprobante = Mapper.Map<OrdenCompraComprobante>(dto);

				int newId = 0;
				using (TransactionScope tx = new())
				{
					newId = ordenCompraComprobanteRepository.Insert(ordenCompraComprobante);

					dto.OrdenCompraComprobanteId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(OrdenCompraComprobanteDTO dto)
		{
			try
			{
				OrdenCompraComprobante ordenCompraComprobante = Mapper.Map<OrdenCompraComprobante>(dto);

				int id = 0;
				using (TransactionScope tx = new())
				{
					id = ordenCompraComprobanteRepository.Update(ordenCompraComprobante);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(OrdenCompraComprobanteDTO dto)
		{
			try
			{
				OrdenCompraComprobante ordenCompraComprobante = Mapper.Map<OrdenCompraComprobante>(dto);

				using (TransactionScope tx = new())
				{
					ordenCompraComprobanteRepository.Delete(ordenCompraComprobante);

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
