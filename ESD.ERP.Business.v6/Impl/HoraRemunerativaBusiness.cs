using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Business.Resources;
using ESD.ERP.Common;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class HoraRemunerativaBusiness : IHoraRemunerativaBusiness
	{
		private readonly IHoraRemunerativaRepository horaRemunerativaRepository;
		private readonly ITablaBusiness tablaBusiness;
		private readonly IVariableCalculoRepository variableCalculoRepository;

		public HoraRemunerativaBusiness(
			IHoraRemunerativaRepository horaRemunerativaRepository,
			ITablaBusiness tablaBusiness,
			IVariableCalculoRepository variableCalculoRepository
			)
		{
			this.horaRemunerativaRepository = horaRemunerativaRepository;
			this.tablaBusiness = tablaBusiness;
			this.variableCalculoRepository = variableCalculoRepository;
		}
		public HoraRemunerativaDTO GetById(int id)
		{
			try
			{
				HoraRemunerativa horaRemunerativa = horaRemunerativaRepository.GetById(id);

				HoraRemunerativaDTO horaRemunerativaDTO = Mapper.Map<HoraRemunerativaDTO>(horaRemunerativa);

				return horaRemunerativaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<HoraRemunerativaDTO> Search(HoraRemunerativaDTO dto)
		{
			try
			{
				List<HoraRemunerativa> horaRemunerativaList = horaRemunerativaRepository.Search(Mapper.Map<HoraRemunerativa>(dto));

				List<HoraRemunerativaDTO> horaRemunerativaDTOList = Mapper.Map<List<HoraRemunerativaDTO>>(horaRemunerativaList);

				return horaRemunerativaDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public HoraRemunerativaDTO SingleSearch(HoraRemunerativaDTO dto)
		{
			try
			{
				HoraRemunerativa horaRemunerativa = horaRemunerativaRepository.SingleSearch(Mapper.Map<HoraRemunerativa>(dto));

				HoraRemunerativaDTO horaRemunerativaDTO = Mapper.Map<HoraRemunerativaDTO>(horaRemunerativa);

				return horaRemunerativaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(HoraRemunerativaDTO dto)
		{
			try
			{
				HoraRemunerativa horaRemunerativa = Mapper.Map<HoraRemunerativa>(dto);

				int newId = 0;

				horaRemunerativa.VariableCalculo.NotVariableCalculoId = horaRemunerativa.VariableCalculoId;
				VariableCalculo variableCalculo = variableCalculoRepository.SingleSearch(horaRemunerativa.VariableCalculo);
				if (variableCalculo != null)
					throw new Exception(SR.GetString("EX_variable_calculo_ya_registrada", variableCalculo.Codigo));

				using (TransactionScope tx = new TransactionScope())
				{
					if (horaRemunerativa.VariableCalculo.EntityId == 0)
						horaRemunerativa.VariableCalculoId = variableCalculoRepository.Insert(horaRemunerativa.VariableCalculo);

					TablaCorrelativoDTO tablaCorrelativoDTO = new TablaCorrelativoDTO
					{
						Tabla = TablaConstante.HORA_REMUNERATIVA,
						PeriodoEmpresaSedeDTO = new PeriodoEmpresaSedeDTO()
						{
							EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = horaRemunerativa.EmpresaId }
						},
						UsuarioAccionId = horaRemunerativa.UsuarioCreacionId
					};

					horaRemunerativa.Codigo = tablaBusiness.ObtenerCorrelativo(tablaCorrelativoDTO);

					newId = horaRemunerativaRepository.Insert(horaRemunerativa);

					dto.HoraRemunerativaId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(HoraRemunerativaDTO dto)
		{
			try
			{
				HoraRemunerativa horaRemunerativa = Mapper.Map<HoraRemunerativa>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					variableCalculoRepository.Update(horaRemunerativa.VariableCalculo);
					id = horaRemunerativaRepository.Update(horaRemunerativa);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(HoraRemunerativaDTO dto)
		{
			try
			{
				HoraRemunerativa horaRemunerativa = Mapper.Map<HoraRemunerativa>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					horaRemunerativaRepository.Delete(horaRemunerativa);

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
