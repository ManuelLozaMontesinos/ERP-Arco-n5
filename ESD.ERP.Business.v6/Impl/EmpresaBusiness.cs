using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class EmpresaBusiness : IEmpresaBusiness
	{
		private readonly IEmpresaRepository empresaRepository;

		public EmpresaBusiness(
			IEmpresaRepository empresaRepository
			)
		{
			this.empresaRepository = empresaRepository;
		}
		public EmpresaDTO GetById(int id)
		{
			try
			{
				Empresa empresa = empresaRepository.GetById(id);

				EmpresaDTO empresaDTO = Mapper.Map<EmpresaDTO>(empresa);

				return empresaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<EmpresaDTO> Search(EmpresaDTO dto)
		{
			try
			{
				List<Empresa> empresaList = empresaRepository.Search(Mapper.Map<Empresa>(dto));

				List<EmpresaDTO> empresaDTOList = Mapper.Map<List<EmpresaDTO>>(empresaList);

				return empresaDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public EmpresaDTO SingleSearch(EmpresaDTO dto)
		{
			try
			{
				Empresa empresa = empresaRepository.SingleSearch(Mapper.Map<Empresa>(dto));

				EmpresaDTO empresaDTO = Mapper.Map<EmpresaDTO>(empresa);

				return empresaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(EmpresaDTO dto)
		{
			try
			{
				Empresa empresa = Mapper.Map<Empresa>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = empresaRepository.Insert(empresa);

					dto.EmpresaId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(EmpresaDTO dto)
		{
			try
			{
				Empresa empresa = Mapper.Map<Empresa>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = empresaRepository.Update(empresa);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(EmpresaDTO dto)
		{
			try
			{
				Empresa empresa = Mapper.Map<Empresa>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					empresaRepository.Delete(empresa);

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
