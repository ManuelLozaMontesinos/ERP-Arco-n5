using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class EmpresaCorrelativoBusiness : IEmpresaCorrelativoBusiness
	{
		private readonly IEmpresaCorrelativoRepository empresaCorrelativoRepository;
		private readonly ITablaRepository tablaRepository;

		public EmpresaCorrelativoBusiness(
			IEmpresaCorrelativoRepository empresaCorrelativoRepository,
			ITablaRepository tablaRepository
			)
		{
			this.empresaCorrelativoRepository = empresaCorrelativoRepository;
			this.tablaRepository = tablaRepository;
		}
		public EmpresaCorrelativoDTO GetById(int id)
		{
			try
			{
				EmpresaCorrelativo empresaCorrelativo = empresaCorrelativoRepository.GetById(id);

				EmpresaCorrelativoDTO empresaCorrelativoDTO = Mapper.Map<EmpresaCorrelativoDTO>(empresaCorrelativo);

				return empresaCorrelativoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<EmpresaCorrelativoDTO> Search(EmpresaCorrelativoDTO dto)
		{
			try
			{
				List<EmpresaCorrelativo> empresaCorrelativoList = empresaCorrelativoRepository.Search(Mapper.Map<EmpresaCorrelativo>(dto));

				List<EmpresaCorrelativoDTO> empresaCorrelativoDTOList = Mapper.Map<List<EmpresaCorrelativoDTO>>(empresaCorrelativoList);

				return empresaCorrelativoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public EmpresaCorrelativoDTO SingleSearch(EmpresaCorrelativoDTO dto)
		{
			try
			{
				EmpresaCorrelativo empresaCorrelativo = empresaCorrelativoRepository.SingleSearch(Mapper.Map<EmpresaCorrelativo>(dto));

				EmpresaCorrelativoDTO empresaCorrelativoDTO = Mapper.Map<EmpresaCorrelativoDTO>(empresaCorrelativo);

				return empresaCorrelativoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(EmpresaCorrelativoDTO dto)
		{
			try
			{
				EmpresaCorrelativo empresaCorrelativo = Mapper.Map<EmpresaCorrelativo>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = empresaCorrelativoRepository.Insert(empresaCorrelativo);

					dto.EmpresaCorrelativoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(EmpresaCorrelativoDTO dto)
		{
			try
			{
				EmpresaCorrelativo empresaCorrelativo = Mapper.Map<EmpresaCorrelativo>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = empresaCorrelativoRepository.Update(empresaCorrelativo);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(EmpresaCorrelativoDTO dto)
		{
			try
			{
				EmpresaCorrelativo empresaCorrelativo = Mapper.Map<EmpresaCorrelativo>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					empresaCorrelativoRepository.Delete(empresaCorrelativo);

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
