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
	public class AlmacenTransaccionBusiness : IAlmacenTransaccionBusiness
	{
		private readonly IAlmacenTransaccionRepository almacenTransaccionRepository;

		public AlmacenTransaccionBusiness(
			IAlmacenTransaccionRepository almacenTransaccionRepository
			)
		{
			this.almacenTransaccionRepository = almacenTransaccionRepository;
		}
		public AlmacenTransaccionDTO GetById(int id)
		{
			try
			{
				AlmacenTransaccion almacenTransaccion = almacenTransaccionRepository.GetById(id);

				AlmacenTransaccionDTO almacenTransaccionDTO = Mapper.Map<AlmacenTransaccionDTO>(almacenTransaccion);

				return almacenTransaccionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<AlmacenTransaccionDTO> Search(AlmacenTransaccionDTO dto)
		{
			try
			{
				List<AlmacenTransaccion> almacenTransaccionList = almacenTransaccionRepository.Search(Mapper.Map<AlmacenTransaccion>(dto));

				List<AlmacenTransaccionDTO> almacenTransaccionDTOList = Mapper.Map<List<AlmacenTransaccionDTO>>(almacenTransaccionList);

				return almacenTransaccionDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public AlmacenTransaccionDTO SingleSearch(AlmacenTransaccionDTO dto)
		{
			try
			{
				AlmacenTransaccion almacenTransaccion = almacenTransaccionRepository.SingleSearch(Mapper.Map<AlmacenTransaccion>(dto));

				AlmacenTransaccionDTO almacenTransaccionDTO = Mapper.Map<AlmacenTransaccionDTO>(almacenTransaccion);

				return almacenTransaccionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(AlmacenTransaccionDTO dto)
		{
			try
			{
				AlmacenTransaccion almacenTransaccion = Mapper.Map<AlmacenTransaccion>(dto);

				almacenTransaccion = almacenTransaccionRepository.SingleSearch(almacenTransaccion);
				if (almacenTransaccion != null)
					throw new Exception(SR.GetString("EX_almacen_transaccion_ya_registrada"));

				almacenTransaccion = Mapper.Map<AlmacenTransaccion>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = almacenTransaccionRepository.Insert(almacenTransaccion);

					dto.AlmacenTransaccionId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(AlmacenTransaccionDTO dto)
		{
			try
			{
				AlmacenTransaccion almacenTransaccion = Mapper.Map<AlmacenTransaccion>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = almacenTransaccionRepository.Update(almacenTransaccion);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(AlmacenTransaccionDTO dto)
		{
			try
			{
				AlmacenTransaccion almacenTransaccion = Mapper.Map<AlmacenTransaccion>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					almacenTransaccionRepository.Delete(almacenTransaccion);

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
