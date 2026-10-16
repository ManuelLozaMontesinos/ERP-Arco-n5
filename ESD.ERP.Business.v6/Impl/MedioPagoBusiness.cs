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
	public class MedioPagoBusiness : IMedioPagoBusiness
	{
		private readonly IMedioPagoRepository medioPagoRepository;
		private readonly ITablaBusiness tablaBusiness;

		public MedioPagoBusiness(
			IMedioPagoRepository medioPagoRepository,
			ITablaBusiness tablaBusiness
			)
		{
			this.medioPagoRepository = medioPagoRepository;
			this.tablaBusiness = tablaBusiness;
		}
		public MedioPagoDTO GetById(int id)
		{
			try
			{
				MedioPago medioPago = medioPagoRepository.GetById(id);

				MedioPagoDTO medioPagoDTO = Mapper.Map<MedioPagoDTO>(medioPago);

				return medioPagoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<MedioPagoDTO> Search(MedioPagoDTO dto)
		{
			try
			{
				List<MedioPago> medioPagoList = medioPagoRepository.Search(Mapper.Map<MedioPago>(dto));

				List<MedioPagoDTO> medioPagoDTOList = Mapper.Map<List<MedioPagoDTO>>(medioPagoList);

				return medioPagoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public MedioPagoDTO SingleSearch(MedioPagoDTO dto)
		{
			try
			{
				MedioPago medioPago = medioPagoRepository.SingleSearch(Mapper.Map<MedioPago>(dto));

				MedioPagoDTO medioPagoDTO = Mapper.Map<MedioPagoDTO>(medioPago);

				return medioPagoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(MedioPagoDTO dto)
		{
			try
			{
				MedioPago medioPago = Mapper.Map<MedioPago>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					TablaCorrelativoDTO tablaCorrelativoDTO = new TablaCorrelativoDTO
					{
						Tabla = TablaConstante.MEDIO_PAGO,
						PeriodoEmpresaSedeDTO = new PeriodoEmpresaSedeDTO()
						{
							EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = dto.EmpresaId }
						},
						UsuarioAccionId = dto.UsuarioCreacionId
					};

					medioPago.Codigo = tablaBusiness.ObtenerCorrelativo(tablaCorrelativoDTO);

					newId = medioPagoRepository.Insert(medioPago);

					dto.MedioPagoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(MedioPagoDTO dto)
		{
			try
			{
				MedioPago medioPago = Mapper.Map<MedioPago>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = medioPagoRepository.Update(medioPago);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(MedioPagoDTO dto)
		{
			try
			{
				MedioPago medioPago = Mapper.Map<MedioPago>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					medioPagoRepository.Delete(medioPago);

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
