using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class AlmacenBusiness : IAlmacenBusiness
	{
		private readonly IAlmacenRepository almacenRepository;

		public AlmacenBusiness(
			IAlmacenRepository almacenRepository
			)
		{
			this.almacenRepository = almacenRepository;
		}
		public AlmacenDTO GetById(int id)
		{
			try
			{
				Almacen almacen = almacenRepository.GetById(id);

				AlmacenDTO almacenDTO = Mapper.Map<AlmacenDTO>(almacen);

				return almacenDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<AlmacenDTO> Search(AlmacenDTO dto)
		{
			try
			{
				List<Almacen> almacenList = almacenRepository.Search(Mapper.Map<Almacen>(dto));

				List<AlmacenDTO> almacenDTOList = Mapper.Map<List<AlmacenDTO>>(almacenList);

				return almacenDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public AlmacenDTO SingleSearch(AlmacenDTO dto)
		{
			try
			{
				Almacen almacen = almacenRepository.SingleSearch(Mapper.Map<Almacen>(dto));

				AlmacenDTO almacenDTO = Mapper.Map<AlmacenDTO>(almacen);

				return almacenDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(AlmacenDTO dto)
		{
			try
			{
				Almacen almacen = Mapper.Map<Almacen>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = almacenRepository.Insert(almacen);

					dto.AlmacenId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(AlmacenDTO dto)
		{
			try
			{
				Almacen almacen = Mapper.Map<Almacen>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = almacenRepository.Update(almacen);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(AlmacenDTO dto)
		{
			try
			{
				Almacen almacen = Mapper.Map<Almacen>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					almacenRepository.Delete(almacen);

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
