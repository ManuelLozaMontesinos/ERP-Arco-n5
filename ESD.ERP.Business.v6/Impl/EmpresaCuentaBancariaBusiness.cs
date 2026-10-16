using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class EmpresaCuentaBancariaBusiness : IEmpresaCuentaBancariaBusiness
	{
		private readonly IEmpresaCuentaBancariaRepository empresaCuentaBancariaRepository;

		public EmpresaCuentaBancariaBusiness(
			IEmpresaCuentaBancariaRepository empresaCuentaBancariaRepository
			)
		{
			this.empresaCuentaBancariaRepository = empresaCuentaBancariaRepository;
		}
		public EmpresaCuentaBancariaDTO GetById(int id)
		{
			try
			{
				EmpresaCuentaBancaria empresaCuentaBancaria = empresaCuentaBancariaRepository.GetById(id);

				EmpresaCuentaBancariaDTO empresaCuentaBancariaDTO = Mapper.Map<EmpresaCuentaBancariaDTO>(empresaCuentaBancaria);

				return empresaCuentaBancariaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<EmpresaCuentaBancariaDTO> Search(EmpresaCuentaBancariaDTO dto)
		{
			try
			{
				List<EmpresaCuentaBancaria> empresaCuentaBancariaList = empresaCuentaBancariaRepository.Search(Mapper.Map<EmpresaCuentaBancaria>(dto));

				List<EmpresaCuentaBancariaDTO> empresaCuentaBancariaDTOList = Mapper.Map<List<EmpresaCuentaBancariaDTO>>(empresaCuentaBancariaList);

				return empresaCuentaBancariaDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public EmpresaCuentaBancariaDTO SingleSearch(EmpresaCuentaBancariaDTO dto)
		{
			try
			{
				EmpresaCuentaBancaria empresaCuentaBancaria = empresaCuentaBancariaRepository.SingleSearch(Mapper.Map<EmpresaCuentaBancaria>(dto));

				EmpresaCuentaBancariaDTO empresaCuentaBancariaDTO = Mapper.Map<EmpresaCuentaBancariaDTO>(empresaCuentaBancaria);

				return empresaCuentaBancariaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(EmpresaCuentaBancariaDTO dto)
		{
			try
			{
				EmpresaCuentaBancaria empresaCuentaBancaria = Mapper.Map<EmpresaCuentaBancaria>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = empresaCuentaBancariaRepository.Insert(empresaCuentaBancaria);

					dto.EmpresaCuentaBancariaId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(EmpresaCuentaBancariaDTO dto)
		{
			try
			{
				EmpresaCuentaBancaria empresaCuentaBancaria = Mapper.Map<EmpresaCuentaBancaria>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = empresaCuentaBancariaRepository.Update(empresaCuentaBancaria);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(EmpresaCuentaBancariaDTO dto)
		{
			try
			{
				EmpresaCuentaBancaria empresaCuentaBancaria = Mapper.Map<EmpresaCuentaBancaria>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					empresaCuentaBancariaRepository.Delete(empresaCuentaBancaria);

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
