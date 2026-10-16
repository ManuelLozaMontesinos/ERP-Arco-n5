using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Common;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class CondicionPagoBusiness : ICondicionPagoBusiness
	{
		private readonly ICondicionPagoRepository condicionPagoRepository;
		private readonly ITablaBusiness tablaBusiness;

		public CondicionPagoBusiness(
			ICondicionPagoRepository condicionPagoRepository,
			ITablaBusiness tablaBusiness
			)
		{
			this.condicionPagoRepository = condicionPagoRepository;
			this.tablaBusiness = tablaBusiness;
		}
		public CondicionPagoDTO GetById(int id)
		{
			try
			{
				CondicionPago condicionPago = condicionPagoRepository.GetById(id);

				CondicionPagoDTO condicionPagoDTO = Mapper.Map<CondicionPagoDTO>(condicionPago);

				return condicionPagoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<CondicionPagoDTO> Search(CondicionPagoDTO dto)
		{
			try
			{
				List<CondicionPago> condicionPagoList = condicionPagoRepository.Search(Mapper.Map<CondicionPago>(dto));

				List<CondicionPagoDTO> condicionPagoDTOList = Mapper.Map<List<CondicionPagoDTO>>(condicionPagoList);

				return condicionPagoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public CondicionPagoDTO SingleSearch(CondicionPagoDTO dto)
		{
			try
			{
				CondicionPago condicionPago = condicionPagoRepository.SingleSearch(Mapper.Map<CondicionPago>(dto));

				CondicionPagoDTO condicionPagoDTO = Mapper.Map<CondicionPagoDTO>(condicionPago);

				return condicionPagoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(CondicionPagoDTO dto)
		{
			try
			{
				CondicionPago condicionPago = Mapper.Map<CondicionPago>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					TablaCorrelativoDTO tablaCorrelativoDTO = new TablaCorrelativoDTO
					{
						Tabla = TablaConstante.CONDICION_PAGO,
						PeriodoEmpresaSedeDTO = new PeriodoEmpresaSedeDTO()
						{
							EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = dto.EmpresaId }
						},
						UsuarioAccionId = dto.UsuarioCreacionId
					};

					condicionPago.Codigo = tablaBusiness.ObtenerCorrelativo(tablaCorrelativoDTO);
					newId = condicionPagoRepository.Insert(condicionPago);

					dto.CondicionPagoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(CondicionPagoDTO dto)
		{
			try
			{
				CondicionPago condicionPago = Mapper.Map<CondicionPago>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = condicionPagoRepository.Update(condicionPago);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(CondicionPagoDTO dto)
		{
			try
			{
				CondicionPago condicionPago = Mapper.Map<CondicionPago>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					condicionPagoRepository.Delete(condicionPago);

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
