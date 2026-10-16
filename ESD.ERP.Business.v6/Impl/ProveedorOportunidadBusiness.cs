using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Business.Resources;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class ProveedorOportunidadBusiness : IProveedorOportunidadBusiness
	{
		private readonly IProveedorOportunidadRepository proveedorOportunidadRepository;

		public ProveedorOportunidadBusiness(
			IProveedorOportunidadRepository proveedorOportunidadRepository
			)
		{
			this.proveedorOportunidadRepository = proveedorOportunidadRepository;
		}
		public ProveedorOportunidadDTO GetById(int id)
		{
			try
			{
				ProveedorOportunidad clienteOportunidad = proveedorOportunidadRepository.GetById(id);

				ProveedorOportunidadDTO clienteOportunidadDTO = Mapper.Map<ProveedorOportunidadDTO>(clienteOportunidad);

				return clienteOportunidadDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<ProveedorOportunidadDTO> Search(ProveedorOportunidadDTO dto)
		{
			try
			{
				List<ProveedorOportunidad> clienteOportunidadList = proveedorOportunidadRepository.Search(Mapper.Map<ProveedorOportunidad>(dto));

				List<ProveedorOportunidadDTO> clienteOportunidadDTOList = Mapper.Map<List<ProveedorOportunidadDTO>>(clienteOportunidadList);

				return clienteOportunidadDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public ProveedorOportunidadDTO SingleSearch(ProveedorOportunidadDTO dto)
		{
			try
			{
				ProveedorOportunidad clienteOportunidad = proveedorOportunidadRepository.SingleSearch(Mapper.Map<ProveedorOportunidad>(dto));

				ProveedorOportunidadDTO clienteOportunidadDTO = Mapper.Map<ProveedorOportunidadDTO>(clienteOportunidad);

				return clienteOportunidadDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(ProveedorOportunidadDTO dto)
		{
			try
			{
				ProveedorOportunidad clienteOportunidad = Mapper.Map<ProveedorOportunidad>(dto);

				clienteOportunidad = proveedorOportunidadRepository.SingleSearch(clienteOportunidad);
				if (clienteOportunidad != null)
					throw new Exception(SR.GetString("EX_oportunidad_venta_ya_registrada"));

				clienteOportunidad = Mapper.Map<ProveedorOportunidad>(dto);
				int newId = 0;
				using (TransactionScope tx = new())
				{
					newId = proveedorOportunidadRepository.Insert(clienteOportunidad);

					dto.ProveedorOportunidadId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(ProveedorOportunidadDTO dto)
		{
			try
			{
				ProveedorOportunidad clienteOportunidad = Mapper.Map<ProveedorOportunidad>(dto);

				int id = 0;
				using (TransactionScope tx = new())
				{
					id = proveedorOportunidadRepository.Update(clienteOportunidad);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(ProveedorOportunidadDTO dto)
		{
			try
			{
				ProveedorOportunidad clienteOportunidad = Mapper.Map<ProveedorOportunidad>(dto);

				using (TransactionScope tx = new())
				{
					proveedorOportunidadRepository.Delete(clienteOportunidad);

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
