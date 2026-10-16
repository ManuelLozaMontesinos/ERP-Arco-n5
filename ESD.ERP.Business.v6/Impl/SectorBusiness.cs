using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class SectorBusiness : ISectorBusiness
	{
		private readonly ISectorRepository sectorRepository;

		public SectorBusiness(
			ISectorRepository sectorRepository
			)
		{
			this.sectorRepository = sectorRepository;
		}
		public SectorDTO GetById(int id)
		{
			try
			{
				Sector sector = sectorRepository.GetById(id);

				SectorDTO sectorDTO = Mapper.Map<SectorDTO>(sector);

				return sectorDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<SectorDTO> Search(SectorDTO dto)
		{
			try
			{
				List<Sector> sectorList = sectorRepository.Search(Mapper.Map<Sector>(dto));

				List<SectorDTO> sectorDTOList = Mapper.Map<List<SectorDTO>>(sectorList);

				return sectorDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public SectorDTO SingleSearch(SectorDTO dto)
		{
			try
			{
				Sector sector = sectorRepository.SingleSearch(Mapper.Map<Sector>(dto));

				SectorDTO sectorDTO = Mapper.Map<SectorDTO>(sector);

				return sectorDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(SectorDTO dto)
		{
			try
			{
				Sector sector = Mapper.Map<Sector>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = sectorRepository.Insert(sector);

					dto.SectorId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(SectorDTO dto)
		{
			try
			{
				Sector sector = Mapper.Map<Sector>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = sectorRepository.Update(sector);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(SectorDTO dto)
		{
			try
			{
				Sector sector = Mapper.Map<Sector>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					sectorRepository.Delete(sector);

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
