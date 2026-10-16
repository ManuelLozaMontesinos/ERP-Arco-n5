using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TransportistaConductorBusiness : ITransportistaConductorBusiness
	{
		private readonly ITransportistaConductorRepository transportistaConductorRepository;

		public TransportistaConductorBusiness(
			ITransportistaConductorRepository transportistaConductorRepository
			)
		{
			this.transportistaConductorRepository = transportistaConductorRepository;
		}
		public TransportistaConductorDTO GetById(int id)
		{
			try
			{
				TransportistaConductor transportistaConductor = transportistaConductorRepository.GetById(id);

				TransportistaConductorDTO transportistaConductorDTO = Mapper.Map<TransportistaConductorDTO>(transportistaConductor);

				return transportistaConductorDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TransportistaConductorDTO> Search(TransportistaConductorDTO dto)
		{
			try
			{
				List<TransportistaConductor> transportistaConductorList = transportistaConductorRepository.Search(Mapper.Map<TransportistaConductor>(dto));

				List<TransportistaConductorDTO> transportistaConductorDTOList = Mapper.Map<List<TransportistaConductorDTO>>(transportistaConductorList);

				return transportistaConductorDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TransportistaConductorDTO SingleSearch(TransportistaConductorDTO dto)
		{
			try
			{
				TransportistaConductor transportistaConductor = transportistaConductorRepository.SingleSearch(Mapper.Map<TransportistaConductor>(dto));

				TransportistaConductorDTO transportistaConductorDTO = Mapper.Map<TransportistaConductorDTO>(transportistaConductor);

				return transportistaConductorDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TransportistaConductorDTO dto)
		{
			try
			{
				TransportistaConductor transportistaConductor = Mapper.Map<TransportistaConductor>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = transportistaConductorRepository.Insert(transportistaConductor);

					dto.TransportistaConductorId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TransportistaConductorDTO dto)
		{
			try
			{
				TransportistaConductor transportistaConductor = Mapper.Map<TransportistaConductor>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = transportistaConductorRepository.Update(transportistaConductor);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TransportistaConductorDTO dto)
		{
			try
			{
				TransportistaConductor transportistaConductor = Mapper.Map<TransportistaConductor>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					transportistaConductorRepository.Delete(transportistaConductor);

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
