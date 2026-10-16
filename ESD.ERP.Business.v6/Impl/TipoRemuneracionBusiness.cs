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
	public class TipoRemuneracionBusiness : ITipoRemuneracionBusiness
	{
		private readonly ITipoRemuneracionRepository tipoRemuneracionRepository;
		private readonly IVariableCalculoRepository variableCalculoRepository;

		public TipoRemuneracionBusiness(
			ITipoRemuneracionRepository tipoRemuneracionRepository,
			IVariableCalculoRepository variableCalculoRepository
			)
		{
			this.tipoRemuneracionRepository = tipoRemuneracionRepository;
			this.variableCalculoRepository = variableCalculoRepository;
		}
		public TipoRemuneracionDTO GetById(int id)
		{
			try
			{
				TipoRemuneracion tipoRemuneracion = tipoRemuneracionRepository.GetById(id);

				TipoRemuneracionDTO tipoRemuneracionDTO = Mapper.Map<TipoRemuneracionDTO>(tipoRemuneracion);

				return tipoRemuneracionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TipoRemuneracionDTO> Search(TipoRemuneracionDTO dto)
		{
			try
			{
				List<TipoRemuneracion> tipoRemuneracionList = tipoRemuneracionRepository.Search(Mapper.Map<TipoRemuneracion>(dto));

				List<TipoRemuneracionDTO> tipoRemuneracionDTOList = Mapper.Map<List<TipoRemuneracionDTO>>(tipoRemuneracionList);

				return tipoRemuneracionDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TipoRemuneracionDTO SingleSearch(TipoRemuneracionDTO dto)
		{
			try
			{
				TipoRemuneracion tipoRemuneracion = tipoRemuneracionRepository.SingleSearch(Mapper.Map<TipoRemuneracion>(dto));

				TipoRemuneracionDTO tipoRemuneracionDTO = Mapper.Map<TipoRemuneracionDTO>(tipoRemuneracion);

				return tipoRemuneracionDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TipoRemuneracionDTO dto)
		{
			try
			{
				TipoRemuneracion tipoRemuneracion = Mapper.Map<TipoRemuneracion>(dto);

				tipoRemuneracion.VariableCalculo.NotVariableCalculoId = tipoRemuneracion.VariableCalculoId;
				VariableCalculo variableCalculo = variableCalculoRepository.SingleSearch(tipoRemuneracion.VariableCalculo);
				if (variableCalculo != null)
					throw new Exception(SR.GetString("EX_variable_calculo_ya_registrada", variableCalculo.Codigo));

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					if (tipoRemuneracion.VariableCalculo.EntityId == 0)
						tipoRemuneracion.VariableCalculoId = variableCalculoRepository.Insert(tipoRemuneracion.VariableCalculo);

					newId = tipoRemuneracionRepository.Insert(tipoRemuneracion);

					dto.TipoRemuneracionId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TipoRemuneracionDTO dto)
		{
			try
			{
				TipoRemuneracion tipoRemuneracion = Mapper.Map<TipoRemuneracion>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = tipoRemuneracionRepository.Update(tipoRemuneracion);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TipoRemuneracionDTO dto)
		{
			try
			{
				TipoRemuneracion tipoRemuneracion = Mapper.Map<TipoRemuneracion>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					tipoRemuneracionRepository.Delete(tipoRemuneracion);

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
