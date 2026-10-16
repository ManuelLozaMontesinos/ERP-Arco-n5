using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class ComprobanteTrazaBusiness : IComprobanteTrazaBusiness
	{
		private readonly IComprobanteTrazaRepository comprobanteTrazaRepository;

		public ComprobanteTrazaBusiness(
			IComprobanteTrazaRepository comprobanteTrazaRepository
			)
		{
			this.comprobanteTrazaRepository = comprobanteTrazaRepository;
		}
		public ComprobanteTrazaDTO GetById(int id)
		{
			try
			{
				ComprobanteTraza comprobanteTraza = comprobanteTrazaRepository.GetById(id);

				ComprobanteTrazaDTO comprobanteTrazaDTO = Mapper.Map<ComprobanteTrazaDTO>(comprobanteTraza);

				return comprobanteTrazaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<ComprobanteTrazaDTO> Search(ComprobanteTrazaDTO dto)
		{
			try
			{
				List<ComprobanteTraza> comprobanteTrazaList = comprobanteTrazaRepository.Search(Mapper.Map<ComprobanteTraza>(dto));

				List<ComprobanteTrazaDTO> comprobanteTrazaDTOList = Mapper.Map<List<ComprobanteTrazaDTO>>(comprobanteTrazaList);

				return comprobanteTrazaDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public ComprobanteTrazaDTO SingleSearch(ComprobanteTrazaDTO dto)
		{
			try
			{
				ComprobanteTraza comprobanteTraza = comprobanteTrazaRepository.SingleSearch(Mapper.Map<ComprobanteTraza>(dto));

				ComprobanteTrazaDTO comprobanteTrazaDTO = Mapper.Map<ComprobanteTrazaDTO>(comprobanteTraza);

				return comprobanteTrazaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(ComprobanteTrazaDTO dto)
		{
			try
			{
				ComprobanteTraza comprobanteTraza = Mapper.Map<ComprobanteTraza>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = comprobanteTrazaRepository.Insert(comprobanteTraza);

					dto.ComprobanteTrazaId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(ComprobanteTrazaDTO dto)
		{
			try
			{
				ComprobanteTraza comprobanteTraza = Mapper.Map<ComprobanteTraza>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = comprobanteTrazaRepository.Update(comprobanteTraza);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(ComprobanteTrazaDTO dto)
		{
			try
			{
				ComprobanteTraza comprobanteTraza = Mapper.Map<ComprobanteTraza>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					comprobanteTrazaRepository.Delete(comprobanteTraza);

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
