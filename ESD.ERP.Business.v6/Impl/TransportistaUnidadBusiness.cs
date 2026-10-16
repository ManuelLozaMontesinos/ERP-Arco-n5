using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class TransportistaUnidadBusiness : ITransportistaUnidadBusiness
	{
		private readonly ITransportistaUnidadRepository transportistaUnidadRepository;

		public TransportistaUnidadBusiness(
			ITransportistaUnidadRepository transportistaUnidadRepository
			)
		{
			this.transportistaUnidadRepository = transportistaUnidadRepository;
		}
		public TransportistaUnidadDTO GetById(int id)
		{
			try
			{
				TransportistaUnidad transportistaUnidad = transportistaUnidadRepository.GetById(id);

				TransportistaUnidadDTO transportistaUnidadDTO = Mapper.Map<TransportistaUnidadDTO>(transportistaUnidad);

				return transportistaUnidadDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TransportistaUnidadDTO> Search(TransportistaUnidadDTO dto)
		{
			try
			{
				List<TransportistaUnidad> transportistaUnidadList = transportistaUnidadRepository.Search(Mapper.Map<TransportistaUnidad>(dto));

				List<TransportistaUnidadDTO> transportistaUnidadDTOList = Mapper.Map<List<TransportistaUnidadDTO>>(transportistaUnidadList);

				return transportistaUnidadDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TransportistaUnidadDTO SingleSearch(TransportistaUnidadDTO dto)
		{
			try
			{
				TransportistaUnidad transportistaUnidad = transportistaUnidadRepository.SingleSearch(Mapper.Map<TransportistaUnidad>(dto));

				TransportistaUnidadDTO transportistaUnidadDTO = Mapper.Map<TransportistaUnidadDTO>(transportistaUnidad);

				return transportistaUnidadDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TransportistaUnidadDTO dto)
		{
			try
			{
				TransportistaUnidad transportistaUnidad = Mapper.Map<TransportistaUnidad>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = transportistaUnidadRepository.Insert(transportistaUnidad);

					dto.TransportistaUnidadId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TransportistaUnidadDTO dto)
		{
			try
			{
				TransportistaUnidad transportistaUnidad = Mapper.Map<TransportistaUnidad>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = transportistaUnidadRepository.Update(transportistaUnidad);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TransportistaUnidadDTO dto)
		{
			try
			{
				TransportistaUnidad transportistaUnidad = Mapper.Map<TransportistaUnidad>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					transportistaUnidadRepository.Delete(transportistaUnidad);

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
