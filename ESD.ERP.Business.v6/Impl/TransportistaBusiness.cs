using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TransportistaBusiness : ITransportistaBusiness
	{
		private readonly ITransportistaRepository transportistaRepository;

		public TransportistaBusiness(
			ITransportistaRepository transportistaRepository
			)
		{
			this.transportistaRepository = transportistaRepository;
		}
		public TransportistaDTO GetById(int id)
		{
			try
			{
				Transportista transportista = transportistaRepository.GetById(id);

				TransportistaDTO transportistaDTO = Mapper.Map<TransportistaDTO>(transportista);

				return transportistaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TransportistaDTO> Search(TransportistaDTO dto)
		{
			try
			{
				List<Transportista> transportistaList = transportistaRepository.Search(Mapper.Map<Transportista>(dto));

				List<TransportistaDTO> transportistaDTOList = Mapper.Map<List<TransportistaDTO>>(transportistaList);

				return transportistaDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TransportistaDTO SingleSearch(TransportistaDTO dto)
		{
			try
			{
				Transportista transportista = transportistaRepository.SingleSearch(Mapper.Map<Transportista>(dto));

				TransportistaDTO transportistaDTO = Mapper.Map<TransportistaDTO>(transportista);

				return transportistaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TransportistaDTO dto)
		{
			try
			{
				Transportista transportista = Mapper.Map<Transportista>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = transportistaRepository.Insert(transportista);

					dto.TransportistaId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TransportistaDTO dto)
		{
			try
			{
				Transportista transportista = Mapper.Map<Transportista>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = transportistaRepository.Update(transportista);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TransportistaDTO dto)
		{
			try
			{
				Transportista transportista = Mapper.Map<Transportista>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					transportistaRepository.Delete(transportista);

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
