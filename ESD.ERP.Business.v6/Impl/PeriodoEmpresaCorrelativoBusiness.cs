using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class PeriodoEmpresaCorrelativoBusiness : IPeriodoEmpresaCorrelativoBusiness
	{
		private readonly IPeriodoEmpresaCorrelativoRepository periodoEmpresaCorrelativoRepository;
		public PeriodoEmpresaCorrelativoBusiness(
			IPeriodoEmpresaCorrelativoRepository periodoEmpresaCorrelativoRepository
			)
		{
			this.periodoEmpresaCorrelativoRepository = periodoEmpresaCorrelativoRepository;
		}
		public PeriodoEmpresaCorrelativoDTO GetById(int id)
		{
			try
			{
				PeriodoEmpresaCorrelativo periodoEmpresaCorrelativo = periodoEmpresaCorrelativoRepository.GetById(id);

				PeriodoEmpresaCorrelativoDTO periodoEmpresaCorrelativoDTO = Mapper.Map<PeriodoEmpresaCorrelativoDTO>(periodoEmpresaCorrelativo);

				return periodoEmpresaCorrelativoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<PeriodoEmpresaCorrelativoDTO> Search(PeriodoEmpresaCorrelativoDTO dto)
		{
			try
			{
				List<PeriodoEmpresaCorrelativo> periodoEmpresaCorrelativoList = periodoEmpresaCorrelativoRepository.Search(Mapper.Map<PeriodoEmpresaCorrelativo>(dto));

				List<PeriodoEmpresaCorrelativoDTO> periodoEmpresaCorrelativoDTOList = Mapper.Map<List<PeriodoEmpresaCorrelativoDTO>>(periodoEmpresaCorrelativoList);

				return periodoEmpresaCorrelativoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public PeriodoEmpresaCorrelativoDTO SingleSearch(PeriodoEmpresaCorrelativoDTO dto)
		{
			try
			{
				PeriodoEmpresaCorrelativo periodoEmpresaCorrelativo = periodoEmpresaCorrelativoRepository.SingleSearch(Mapper.Map<PeriodoEmpresaCorrelativo>(dto));

				PeriodoEmpresaCorrelativoDTO periodoEmpresaCorrelativoDTO = Mapper.Map<PeriodoEmpresaCorrelativoDTO>(periodoEmpresaCorrelativo);

				return periodoEmpresaCorrelativoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(PeriodoEmpresaCorrelativoDTO dto)
		{
			try
			{
				PeriodoEmpresaCorrelativo periodoEmpresaCorrelativo = Mapper.Map<PeriodoEmpresaCorrelativo>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = periodoEmpresaCorrelativoRepository.Insert(periodoEmpresaCorrelativo);

					dto.PeriodoEmpresaCorrelativoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(PeriodoEmpresaCorrelativoDTO dto)
		{
			try
			{
				PeriodoEmpresaCorrelativo periodoEmpresaCorrelativo = Mapper.Map<PeriodoEmpresaCorrelativo>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = periodoEmpresaCorrelativoRepository.Update(periodoEmpresaCorrelativo);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(PeriodoEmpresaCorrelativoDTO dto)
		{
			try
			{
				PeriodoEmpresaCorrelativo periodoEmpresaCorrelativo = Mapper.Map<PeriodoEmpresaCorrelativo>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					periodoEmpresaCorrelativoRepository.Delete(periodoEmpresaCorrelativo);

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
