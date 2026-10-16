using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class RutaProduccionBusiness : IRutaProduccionBusiness
	{
		private readonly IRutaProduccionRepository rutaProduccionRepository;

		public RutaProduccionBusiness(
			IRutaProduccionRepository rutaProduccionRepository
			)
		{
			this.rutaProduccionRepository = rutaProduccionRepository;
		}
		public RutaProduccionDTO GetById(int id)
		{
			try
			{
				RutaProduccion rutaProduccion = rutaProduccionRepository.GetById(id);

				RutaProduccionDTO rutaProduccionDTO = Mapper.Map<RutaProduccionDTO>(rutaProduccion);

				return rutaProduccionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<RutaProduccionDTO> Search(RutaProduccionDTO dto)
		{
			try
			{
				List<RutaProduccion> rutaProduccionList = rutaProduccionRepository.Search(Mapper.Map<RutaProduccion>(dto));

				List<RutaProduccionDTO> rutaProduccionDTOList = Mapper.Map<List<RutaProduccionDTO>>(rutaProduccionList);

				return rutaProduccionDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public RutaProduccionDTO SingleSearch(RutaProduccionDTO dto)
		{
			try
			{
				RutaProduccion rutaProduccion = rutaProduccionRepository.SingleSearch(Mapper.Map<RutaProduccion>(dto));

				RutaProduccionDTO rutaProduccionDTO = Mapper.Map<RutaProduccionDTO>(rutaProduccion);

				return rutaProduccionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(RutaProduccionDTO dto)
		{
			try
			{
				RutaProduccion rutaProduccion = Mapper.Map<RutaProduccion>(dto);

				int newId = 0;
				using (TransactionScope tx = new())
				{
					newId = rutaProduccionRepository.Insert(rutaProduccion);

					dto.RutaProduccionId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(RutaProduccionDTO dto)
		{
			try
			{
				RutaProduccion rutaProduccion = Mapper.Map<RutaProduccion>(dto);

				int id = 0;
				using (TransactionScope tx = new())
				{
					id = rutaProduccionRepository.Update(rutaProduccion);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(RutaProduccionDTO dto)
		{
			try
			{
				RutaProduccion rutaProduccion = Mapper.Map<RutaProduccion>(dto);

				using (TransactionScope tx = new())
				{
					rutaProduccionRepository.Delete(rutaProduccion);

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
