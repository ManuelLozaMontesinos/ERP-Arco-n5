using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Repository;
using ESD.ERP.Business.Resources;

namespace ESD.ERP.Business.Impl
{
	public class EmpresaSedeBusiness : IEmpresaSedeBusiness
	{
		private readonly IEmpresaSedeRepository empresaSedeRepository;
		private readonly ITablaBusiness tablaBusiness;
		public EmpresaSedeBusiness(
			IEmpresaSedeRepository empresaSedeRepository,
			ITablaBusiness tablaBusiness
			)
		{
			this.empresaSedeRepository = empresaSedeRepository;
			this.tablaBusiness = tablaBusiness;
		}
		public EmpresaSedeDTO GetById(int id)
		{
			try
			{
				EmpresaSede empresaSede = empresaSedeRepository.GetById(id);

				EmpresaSedeDTO empresaSedeDTO = Mapper.Map<EmpresaSedeDTO>(empresaSede);

				return empresaSedeDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<EmpresaSedeDTO> Search(EmpresaSedeDTO dto)
		{
			try
			{
				List<EmpresaSede> empresaSedeList = empresaSedeRepository.Search(Mapper.Map<EmpresaSede>(dto));

				List<EmpresaSedeDTO> empresaSedeDTOList = Mapper.Map<List<EmpresaSedeDTO>>(empresaSedeList);

				return empresaSedeDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public EmpresaSedeDTO SingleSearch(EmpresaSedeDTO dto)
		{
			try
			{
				EmpresaSede empresaSede = empresaSedeRepository.SingleSearch(Mapper.Map<EmpresaSede>(dto));

				EmpresaSedeDTO empresaSedeDTO = Mapper.Map<EmpresaSedeDTO>(empresaSede);

				return empresaSedeDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(EmpresaSedeDTO dto)
		{
			try
			{
				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					EmpresaSede empresaSede = null;
					if (dto.EsCentral == SiNoConstante.S)
					{
						empresaSede = new EmpresaSede()
						{
							EmpresaId = dto.EmpresaId,
							EsCentral = dto.EsCentral,
							NotEmpresaSedeId = dto.EmpresaSedeId
						};

						empresaSede = empresaSedeRepository.SingleSearch(empresaSede);
						if (empresaSede != null)
						{
							throw new Exception(SR.GetString("EX_sede_central_ya_asignada"));
						}
					}

					empresaSede = Mapper.Map<EmpresaSede>(dto);

					TablaCorrelativoDTO tablaCorrelativoDTO = new TablaCorrelativoDTO
					{
						Tabla = TablaConstante.EMPRESA_SEDE,
						PeriodoEmpresaSedeDTO = new PeriodoEmpresaSedeDTO()
						{
							EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = empresaSede.EmpresaId }
						},
						UsuarioAccionId = empresaSede.UsuarioCreacionId
					};

					empresaSede.Codigo = tablaBusiness.ObtenerCorrelativo(tablaCorrelativoDTO);

					newId = empresaSedeRepository.Insert(empresaSede);

					dto.EmpresaSedeId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(EmpresaSedeDTO dto)
		{
			try
			{
				EmpresaSede empresaSede = Mapper.Map<EmpresaSede>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = empresaSedeRepository.Update(empresaSede);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(EmpresaSedeDTO dto)
		{
			try
			{
				EmpresaSede empresaSede = Mapper.Map<EmpresaSede>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					empresaSedeRepository.Delete(empresaSede);

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
