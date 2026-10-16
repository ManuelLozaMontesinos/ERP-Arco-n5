using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class RutaSubProcesoProduccionBusiness : IRutaSubProcesoProduccionBusiness
	{
		private readonly IRutaSubProcesoProduccionRepository rutaSubProcesoProduccionRepository;

		public RutaSubProcesoProduccionBusiness(
			IRutaSubProcesoProduccionRepository rutaSubProcesoProduccionRepository
			)
		{
			this.rutaSubProcesoProduccionRepository = rutaSubProcesoProduccionRepository;
		}
		public RutaSubProcesoProduccionDTO GetById(int id)
		{
			try
			{
				RutaSubProcesoProduccion rutaSubProcesoProduccion = rutaSubProcesoProduccionRepository.GetById(id);

				RutaSubProcesoProduccionDTO rutaSubProcesoProduccionDTO = Mapper.Map<RutaSubProcesoProduccionDTO>(rutaSubProcesoProduccion);

				return rutaSubProcesoProduccionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<RutaSubProcesoProduccionDTO> Search(RutaSubProcesoProduccionDTO dto)
		{
			try
			{
				List<RutaSubProcesoProduccion> rutaSubProcesoProduccionList = rutaSubProcesoProduccionRepository.Search(Mapper.Map<RutaSubProcesoProduccion>(dto));

				List<RutaSubProcesoProduccionDTO> rutaSubProcesoProduccionDTOList = Mapper.Map<List<RutaSubProcesoProduccionDTO>>(rutaSubProcesoProduccionList);

				return rutaSubProcesoProduccionDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public RutaSubProcesoProduccionDTO SingleSearch(RutaSubProcesoProduccionDTO dto)
		{
			try
			{
				RutaSubProcesoProduccion rutaSubProcesoProduccion = rutaSubProcesoProduccionRepository.SingleSearch(Mapper.Map<RutaSubProcesoProduccion>(dto));

				RutaSubProcesoProduccionDTO rutaSubProcesoProduccionDTO = Mapper.Map<RutaSubProcesoProduccionDTO>(rutaSubProcesoProduccion);

				return rutaSubProcesoProduccionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(RutaSubProcesoProduccionDTO dto)
		{
			try
			{
				RutaSubProcesoProduccion rutaSubProcesoProduccion = Mapper.Map<RutaSubProcesoProduccion>(dto);

				int newId = 0;
				using (TransactionScope tx = new())
				{
					newId = rutaSubProcesoProduccionRepository.Insert(rutaSubProcesoProduccion);

					dto.RutaSubProcesoProduccionId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(RutaSubProcesoProduccionDTO dto)
		{
			try
			{
				RutaSubProcesoProduccion rutaSubProcesoProduccion = Mapper.Map<RutaSubProcesoProduccion>(dto);

				int id = 0;
				using (TransactionScope tx = new())
				{
					id = rutaSubProcesoProduccionRepository.Update(rutaSubProcesoProduccion);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(RutaSubProcesoProduccionDTO dto)
		{
			try
			{
				RutaSubProcesoProduccion rutaSubProcesoProduccion = Mapper.Map<RutaSubProcesoProduccion>(dto);

				using (TransactionScope tx = new())
				{
					rutaSubProcesoProduccionRepository.Delete(rutaSubProcesoProduccion);

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
