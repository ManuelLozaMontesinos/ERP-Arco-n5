using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Business.Resources;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class VariableCalculoBusiness : IVariableCalculoBusiness
	{
		private readonly IVariableCalculoRepository variableCalculoRepository;

		public VariableCalculoBusiness(
			IVariableCalculoRepository variableCalculoRepository
			)
		{
			this.variableCalculoRepository = variableCalculoRepository;
		}
		public VariableCalculoDTO GetById(int id)
		{
			try
			{
				VariableCalculo variableCalculo = variableCalculoRepository.GetById(id);

				VariableCalculoDTO variableCalculoDTO = Mapper.Map<VariableCalculoDTO>(variableCalculo);

				return variableCalculoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<VariableCalculoDTO> Search(VariableCalculoDTO dto)
		{
			try
			{
				List<VariableCalculo> variableCalculoList = variableCalculoRepository.Search(Mapper.Map<VariableCalculo>(dto));

				List<VariableCalculoDTO> variableCalculoDTOList = Mapper.Map<List<VariableCalculoDTO>>(variableCalculoList);

				return variableCalculoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public VariableCalculoDTO SingleSearch(VariableCalculoDTO dto)
		{
			try
			{
				VariableCalculo variableCalculo = variableCalculoRepository.SingleSearch(Mapper.Map<VariableCalculo>(dto));

				VariableCalculoDTO variableCalculoDTO = Mapper.Map<VariableCalculoDTO>(variableCalculo);

				return variableCalculoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(VariableCalculoDTO dto)
		{
			try
			{
				VariableCalculo variableCalculo = Mapper.Map<VariableCalculo>(dto);

				int newId = 0;

				variableCalculo.NotVariableCalculoId = variableCalculo.VariableCalculoId;
				variableCalculo = variableCalculoRepository.SingleSearch(variableCalculo);
				if (variableCalculo != null)
					throw new Exception(SR.GetString("EX_variable_calculo_ya_registrada", variableCalculo.Codigo));

				variableCalculo = Mapper.Map<VariableCalculo>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					newId = variableCalculoRepository.Insert(variableCalculo);

					dto.VariableCalculoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(VariableCalculoDTO dto)
		{
			try
			{
				VariableCalculo variableCalculo = Mapper.Map<VariableCalculo>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = variableCalculoRepository.Update(variableCalculo);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(VariableCalculoDTO dto)
		{
			try
			{
				VariableCalculo variableCalculo = Mapper.Map<VariableCalculo>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					variableCalculoRepository.Delete(variableCalculo);

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
