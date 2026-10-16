using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class DepartamentoBusiness : IDepartamentoBusiness
	{
		private readonly IDepartamentoRepository departamentoRepository;

		public DepartamentoBusiness(
			IDepartamentoRepository departamentoRepository
			)
		{
			this.departamentoRepository = departamentoRepository;
		}
		public DepartamentoDTO GetById(int id)
		{
			try
			{
				Departamento departamento = departamentoRepository.GetById(id);

				DepartamentoDTO departamentoDTO = Mapper.Map<DepartamentoDTO>(departamento);

				return departamentoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<DepartamentoDTO> Search(DepartamentoDTO dto)
		{
			try
			{
				List<Departamento> departamentoList = departamentoRepository.Search(Mapper.Map<Departamento>(dto));

				List<DepartamentoDTO> departamentoDTOList = Mapper.Map<List<DepartamentoDTO>>(departamentoList);

				return departamentoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public DepartamentoDTO SingleSearch(DepartamentoDTO dto)
		{
			try
			{
				Departamento departamento = departamentoRepository.SingleSearch(Mapper.Map<Departamento>(dto));

				DepartamentoDTO departamentoDTO = Mapper.Map<DepartamentoDTO>(departamento);

				return departamentoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(DepartamentoDTO dto)
		{
			try
			{
				Departamento departamento = Mapper.Map<Departamento>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = departamentoRepository.Insert(departamento);

					dto.DepartamentoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(DepartamentoDTO dto)
		{
			try
			{
				Departamento departamento = Mapper.Map<Departamento>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = departamentoRepository.Update(departamento);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(DepartamentoDTO dto)
		{
			try
			{
				Departamento departamento = Mapper.Map<Departamento>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					departamentoRepository.Delete(departamento);

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
