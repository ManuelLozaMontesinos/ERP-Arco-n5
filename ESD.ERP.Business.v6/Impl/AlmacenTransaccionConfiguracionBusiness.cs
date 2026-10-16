using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class AlmacenTransaccionConfiguracionBusiness : IAlmacenTransaccionConfiguracionBusiness
	{
		private readonly IAlmacenTransaccionConfiguracionRepository almacenTransaccionConfiguracionRepository;

		public AlmacenTransaccionConfiguracionBusiness(
			IAlmacenTransaccionConfiguracionRepository almacenTransaccionConfiguracionRepository
			)
		{
			this.almacenTransaccionConfiguracionRepository = almacenTransaccionConfiguracionRepository;
		}
		public AlmacenTransaccionConfiguracionDTO GetById(int id)
		{
			try
			{
				AlmacenTransaccionConfiguracion almacenTransaccionConfiguracion = almacenTransaccionConfiguracionRepository.GetById(id);

				AlmacenTransaccionConfiguracionDTO almacenTransaccionConfiguracionDTO = Mapper.Map<AlmacenTransaccionConfiguracionDTO>(almacenTransaccionConfiguracion);

				return almacenTransaccionConfiguracionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<AlmacenTransaccionConfiguracionDTO> Search(AlmacenTransaccionConfiguracionDTO dto)
		{
			try
			{
				List<AlmacenTransaccionConfiguracion> almacenTransaccionConfiguracionList = almacenTransaccionConfiguracionRepository
					.Search(Mapper.Map<AlmacenTransaccionConfiguracion>(dto));

				List<AlmacenTransaccionConfiguracionDTO> almacenTransaccionConfiguracionDTOList = Mapper.Map<List<AlmacenTransaccionConfiguracionDTO>>
					(almacenTransaccionConfiguracionList);

				return almacenTransaccionConfiguracionDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public AlmacenTransaccionConfiguracionDTO SingleSearch(AlmacenTransaccionConfiguracionDTO dto)
		{
			try
			{
				AlmacenTransaccionConfiguracion almacenTransaccionConfiguracion = almacenTransaccionConfiguracionRepository.SingleSearch(Mapper.Map<AlmacenTransaccionConfiguracion>(dto));

				AlmacenTransaccionConfiguracionDTO almacenTransaccionConfiguracionDTO = Mapper.Map<AlmacenTransaccionConfiguracionDTO>(almacenTransaccionConfiguracion);

				return almacenTransaccionConfiguracionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(AlmacenTransaccionConfiguracionDTO dto)
		{
			try
			{
				AlmacenTransaccionConfiguracion almacenTransaccionConfiguracion = Mapper.Map<AlmacenTransaccionConfiguracion>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = almacenTransaccionConfiguracionRepository.Insert(almacenTransaccionConfiguracion);

					dto.AlmacenTransaccionConfiguracionId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(AlmacenTransaccionConfiguracionDTO dto)
		{
			try
			{
				AlmacenTransaccionConfiguracion almacenTransaccionConfiguracion = Mapper.Map<AlmacenTransaccionConfiguracion>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = almacenTransaccionConfiguracionRepository.Update(almacenTransaccionConfiguracion);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(AlmacenTransaccionConfiguracionDTO dto)
		{
			try
			{
				AlmacenTransaccionConfiguracion almacenTransaccionConfiguracion = Mapper.Map<AlmacenTransaccionConfiguracion>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					almacenTransaccionConfiguracionRepository.Delete(almacenTransaccionConfiguracion);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public List<AlmacenTransaccionConfiguracionDTO> ListarPorPorAlmacenTransaccionId(AlmacenTransaccionConfiguracionDTO dto)
      {
			try
			{
				List<AlmacenTransaccionConfiguracion> almacenTransaccionConfiguracionList = almacenTransaccionConfiguracionRepository.
					ListarPorPorAlmacenTransaccionId(Mapper.Map<AlmacenTransaccionConfiguracion>(dto));

				List<AlmacenTransaccionConfiguracionDTO> almacenTransaccionConfiguracionDTOList = Mapper.Map<List<AlmacenTransaccionConfiguracionDTO>>
					(almacenTransaccionConfiguracionList);

				return almacenTransaccionConfiguracionDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
   }
}
