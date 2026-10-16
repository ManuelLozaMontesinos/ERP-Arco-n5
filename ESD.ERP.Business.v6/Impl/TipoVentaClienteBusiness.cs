using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TipoVentaClienteBusiness : ITipoVentaClienteBusiness
	{
		private readonly ITipoVentaClienteRepository tipoVentaClienteRepository;

		public TipoVentaClienteBusiness(
			ITipoVentaClienteRepository tipoVentaClienteRepository
			)
		{
			this.tipoVentaClienteRepository = tipoVentaClienteRepository;
		}
		public TipoVentaClienteDTO GetById(int id)
		{
			try
			{
				TipoVentaCliente tipoVentaCliente = tipoVentaClienteRepository.GetById(id);

				TipoVentaClienteDTO tipoVentaClienteDTO = Mapper.Map<TipoVentaClienteDTO>(tipoVentaCliente);

				return tipoVentaClienteDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoVentaClienteDTO> Search(TipoVentaClienteDTO dto)
		{
			try
			{
				List<TipoVentaCliente> tipoVentaClienteList = tipoVentaClienteRepository.Search(Mapper.Map<TipoVentaCliente>(dto));

				List<TipoVentaClienteDTO> tipoVentaClienteDTOList = Mapper.Map<List<TipoVentaClienteDTO>>(tipoVentaClienteList);

				return tipoVentaClienteDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoVentaClienteDTO SingleSearch(TipoVentaClienteDTO dto)
		{
			try
			{
				TipoVentaCliente tipoVentaCliente = tipoVentaClienteRepository.SingleSearch(Mapper.Map<TipoVentaCliente>(dto));

				TipoVentaClienteDTO tipoVentaClienteDTO = Mapper.Map<TipoVentaClienteDTO>(tipoVentaCliente);

				return tipoVentaClienteDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoVentaClienteDTO dto)
		{
			try
			{
				TipoVentaCliente tipoVentaCliente = Mapper.Map<TipoVentaCliente>(dto);

				int newId = 0;
				using (TransactionScope tx = new())
				{
					newId = tipoVentaClienteRepository.Insert(tipoVentaCliente);

					dto.TipoVentaClienteId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoVentaClienteDTO dto)
		{
			try
			{
				TipoVentaCliente tipoVentaCliente = Mapper.Map<TipoVentaCliente>(dto);

				int id = 0;
				using (TransactionScope tx = new())
				{
					id = tipoVentaClienteRepository.Update(tipoVentaCliente);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoVentaClienteDTO dto)
		{
			try
			{
				TipoVentaCliente tipoVentaCliente = Mapper.Map<TipoVentaCliente>(dto);

				using (TransactionScope tx = new())
				{
					tipoVentaClienteRepository.Delete(tipoVentaCliente);

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
