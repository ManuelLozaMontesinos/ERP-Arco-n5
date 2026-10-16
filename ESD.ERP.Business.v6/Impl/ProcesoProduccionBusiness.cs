using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class ProcesoProduccionBusiness : IProcesoProduccionBusiness
	{
		private readonly IProcesoProduccionRepository procesoProduccionRepository;

		public ProcesoProduccionBusiness(
			IProcesoProduccionRepository procesoProduccionRepository
			)
		{
			this.procesoProduccionRepository = procesoProduccionRepository;
		}
		public ProcesoProduccionDTO GetById(int id)
		{
			try
			{
				ProcesoProduccion procesoProduccion = procesoProduccionRepository.GetById(id);

				ProcesoProduccionDTO procesoProduccionDTO = Mapper.Map<ProcesoProduccionDTO>(procesoProduccion);

				return procesoProduccionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<ProcesoProduccionDTO> Search(ProcesoProduccionDTO dto)
		{
			try
			{
				List<ProcesoProduccion> procesoProduccionList = procesoProduccionRepository.Search(Mapper.Map<ProcesoProduccion>(dto));

				List<ProcesoProduccionDTO> procesoProduccionDTOList = Mapper.Map<List<ProcesoProduccionDTO>>(procesoProduccionList);

				return procesoProduccionDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public ProcesoProduccionDTO SingleSearch(ProcesoProduccionDTO dto)
		{
			try
			{
				ProcesoProduccion procesoProduccion = procesoProduccionRepository.SingleSearch(Mapper.Map<ProcesoProduccion>(dto));

				ProcesoProduccionDTO procesoProduccionDTO = Mapper.Map<ProcesoProduccionDTO>(procesoProduccion);

				return procesoProduccionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(ProcesoProduccionDTO dto)
		{
			try
			{
				ProcesoProduccion procesoProduccion = Mapper.Map<ProcesoProduccion>(dto);

				int newId = 0;
				using (TransactionScope tx = new())
				{
					newId = procesoProduccionRepository.Insert(procesoProduccion);

					dto.ProcesoProduccionId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(ProcesoProduccionDTO dto)
		{
			try
			{
				ProcesoProduccion procesoProduccion = Mapper.Map<ProcesoProduccion>(dto);

				int id = 0;
				using (TransactionScope tx = new())
				{
					id = procesoProduccionRepository.Update(procesoProduccion);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(ProcesoProduccionDTO dto)
		{
			try
			{
				ProcesoProduccion procesoProduccion = Mapper.Map<ProcesoProduccion>(dto);

				using (TransactionScope tx = new())
				{
					procesoProduccionRepository.Delete(procesoProduccion);

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
