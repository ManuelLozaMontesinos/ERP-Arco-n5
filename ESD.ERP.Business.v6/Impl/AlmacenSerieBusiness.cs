using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class AlmacenSerieBusiness : IAlmacenSerieBusiness
	{
		private readonly IAlmacenSerieRepository almacenSerieRepository;

		public AlmacenSerieBusiness(
			IAlmacenSerieRepository almacenSerieRepository
			)
		{
			this.almacenSerieRepository = almacenSerieRepository;
		}
		public AlmacenSerieDTO GetById(int id)
		{
			try
			{
				AlmacenSerie almacenSerie = almacenSerieRepository.GetById(id);

				AlmacenSerieDTO almacenSerieDTO = Mapper.Map<AlmacenSerieDTO>(almacenSerie);

				return almacenSerieDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<AlmacenSerieDTO> Search(AlmacenSerieDTO dto)
		{
			try
			{
				List<AlmacenSerie> almacenSerieList = almacenSerieRepository.Search(Mapper.Map<AlmacenSerie>(dto));

				List<AlmacenSerieDTO> almacenSerieDTOList = Mapper.Map<List<AlmacenSerieDTO>>(almacenSerieList);

				return almacenSerieDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public AlmacenSerieDTO SingleSearch(AlmacenSerieDTO dto)
		{
			try
			{
				AlmacenSerie almacenSerie = almacenSerieRepository.SingleSearch(Mapper.Map<AlmacenSerie>(dto));

				AlmacenSerieDTO almacenSerieDTO = Mapper.Map<AlmacenSerieDTO>(almacenSerie);

				return almacenSerieDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(AlmacenSerieDTO dto)
		{
			try
			{
				AlmacenSerie almacenSerie = Mapper.Map<AlmacenSerie>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = almacenSerieRepository.Insert(almacenSerie);

					dto.AlmacenSerieId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(AlmacenSerieDTO dto)
		{
			try
			{
				AlmacenSerie almacenSerie = Mapper.Map<AlmacenSerie>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = almacenSerieRepository.Update(almacenSerie);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(AlmacenSerieDTO dto)
		{
			try
			{
				AlmacenSerie almacenSerie = Mapper.Map<AlmacenSerie>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					almacenSerieRepository.Delete(almacenSerie);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void ActualizarListaDocumentoComercialSerieId(AlmacenSerieDTO dto)
		{
			try
			{
				AlmacenSerie almacenSerie = Mapper.Map<AlmacenSerie>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					almacenSerieRepository.ActualizarListaDocumentoComercialSerieId(almacenSerie);

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
