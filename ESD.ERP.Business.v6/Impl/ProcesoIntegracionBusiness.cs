using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class ProcesoIntegracionBusiness : IProcesoIntegracionBusiness
	{
		private readonly IProcesoIntegracionRepository procesoIntegracionRepository;

		public ProcesoIntegracionBusiness(
			IProcesoIntegracionRepository procesoIntegracionRepository
			)
		{
			this.procesoIntegracionRepository = procesoIntegracionRepository;
		}
		public ProcesoIntegracionDTO GetById(int id)
		{
			try
			{
				ProcesoIntegracion procesoIntegracion = procesoIntegracionRepository.GetById(id);

				ProcesoIntegracionDTO procesoIntegracionDTO = Mapper.Map<ProcesoIntegracionDTO>(procesoIntegracion);

				return procesoIntegracionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<ProcesoIntegracionDTO> Search(ProcesoIntegracionDTO dto)
		{
			try
			{
				List<ProcesoIntegracion> procesoIntegracionList = procesoIntegracionRepository.Search(Mapper.Map<ProcesoIntegracion>(dto));

				List<ProcesoIntegracionDTO> procesoIntegracionDTOList = Mapper.Map<List<ProcesoIntegracionDTO>>(procesoIntegracionList);

				return procesoIntegracionDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public ProcesoIntegracionDTO SingleSearch(ProcesoIntegracionDTO dto)
		{
			try
			{
				ProcesoIntegracion procesoIntegracion = procesoIntegracionRepository.SingleSearch(Mapper.Map<ProcesoIntegracion>(dto));

				ProcesoIntegracionDTO procesoIntegracionDTO = Mapper.Map<ProcesoIntegracionDTO>(procesoIntegracion);

				return procesoIntegracionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(ProcesoIntegracionDTO dto)
		{
			try
			{
				ProcesoIntegracion procesoIntegracion = Mapper.Map<ProcesoIntegracion>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = procesoIntegracionRepository.Insert(procesoIntegracion);

					dto.ProcesoIntegracionId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(ProcesoIntegracionDTO dto)
		{
			try
			{
				ProcesoIntegracion procesoIntegracion = Mapper.Map<ProcesoIntegracion>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = procesoIntegracionRepository.Update(procesoIntegracion);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(ProcesoIntegracionDTO dto)
		{
			try
			{
				ProcesoIntegracion procesoIntegracion = Mapper.Map<ProcesoIntegracion>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					procesoIntegracionRepository.Delete(procesoIntegracion);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public void EjecutarJob(ProcesoIntegracionDTO dto)
      {
			try
			{
				ProcesoIntegracion procesoIntegracion = Mapper.Map<ProcesoIntegracion>(dto);

				procesoIntegracionRepository.EjecutarJob(procesoIntegracion);
			}
			catch (Exception)
			{
				throw;
			}
		}
   }
}
