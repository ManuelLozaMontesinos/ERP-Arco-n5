using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class JornadaLaboralBusiness : IJornadaLaboralBusiness
	{
		private readonly IJornadaLaboralRepository jornadaLaboralRepository;

		public JornadaLaboralBusiness(
			IJornadaLaboralRepository jornadaLaboralRepository
			)
		{
			this.jornadaLaboralRepository = jornadaLaboralRepository;
		}
		public JornadaLaboralDTO GetById(int id)
		{
			try
			{
				JornadaLaboral jornadaLab = jornadaLaboralRepository.GetById(id);

				JornadaLaboralDTO jornadaLabDTO = Mapper.Map<JornadaLaboralDTO>(jornadaLab);

				return jornadaLabDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<JornadaLaboralDTO> Search(JornadaLaboralDTO dto)
		{
			try
			{
				List<JornadaLaboral> jornadaLabList = jornadaLaboralRepository.Search(Mapper.Map<JornadaLaboral>(dto));

				List<JornadaLaboralDTO> jornadaLabDTOList = Mapper.Map<List<JornadaLaboralDTO>>(jornadaLabList);

				return jornadaLabDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public JornadaLaboralDTO SingleSearch(JornadaLaboralDTO dto)
		{
			try
			{
				JornadaLaboral jornadaLab = jornadaLaboralRepository.SingleSearch(Mapper.Map<JornadaLaboral>(dto));

				JornadaLaboralDTO jornadaLabDTO = Mapper.Map<JornadaLaboralDTO>(jornadaLab);

				return jornadaLabDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(JornadaLaboralDTO dto)
		{
			try
			{
				JornadaLaboral jornadaLab = Mapper.Map<JornadaLaboral>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = jornadaLaboralRepository.Insert(jornadaLab);

					dto.JornadaLaboralId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(JornadaLaboralDTO dto)
		{
			try
			{
				JornadaLaboral jornadaLab = Mapper.Map<JornadaLaboral>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = jornadaLaboralRepository.Update(jornadaLab);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(JornadaLaboralDTO dto)
		{
			try
			{
				JornadaLaboral jornadaLab = Mapper.Map<JornadaLaboral>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					jornadaLaboralRepository.Delete(jornadaLab);

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
