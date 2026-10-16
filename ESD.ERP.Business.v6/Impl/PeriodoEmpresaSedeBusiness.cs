using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Business.Resources;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class PeriodoEmpresaSedeBusiness : IPeriodoEmpresaSedeBusiness
	{
		private readonly IPeriodoEmpresaSedeRepository periodoEmpresaSedeRepository;

		public PeriodoEmpresaSedeBusiness(
			IPeriodoEmpresaSedeRepository periodoEmpresaSedeRepository
			)
		{
			this.periodoEmpresaSedeRepository = periodoEmpresaSedeRepository;
		}
		public PeriodoEmpresaSedeDTO GetById(int id)
		{
			try
			{
				PeriodoEmpresaSede periodoEmpresaSede = periodoEmpresaSedeRepository.GetById(id);

				PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = Mapper.Map<PeriodoEmpresaSedeDTO>(periodoEmpresaSede);

				return periodoEmpresaSedeDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<PeriodoEmpresaSedeDTO> Search(PeriodoEmpresaSedeDTO dto)
		{
			try
			{
				List<PeriodoEmpresaSede> periodoEmpresaSedeList = periodoEmpresaSedeRepository.Search(Mapper.Map<PeriodoEmpresaSede>(dto));

				List<PeriodoEmpresaSedeDTO> periodoEmpresaSedeDTOList = Mapper.Map<List<PeriodoEmpresaSedeDTO>>(periodoEmpresaSedeList);

				return periodoEmpresaSedeDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public PeriodoEmpresaSedeDTO SingleSearch(PeriodoEmpresaSedeDTO dto)
		{
			try
			{
				PeriodoEmpresaSede periodoEmpresaSede = periodoEmpresaSedeRepository.SingleSearch(Mapper.Map<PeriodoEmpresaSede>(dto));

				PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = Mapper.Map<PeriodoEmpresaSedeDTO>(periodoEmpresaSede);

				return periodoEmpresaSedeDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(PeriodoEmpresaSedeDTO dto)
		{
			try
			{
				PeriodoEmpresaSede periodoEmpresaSede = periodoEmpresaSedeRepository.SingleSearch(new PeriodoEmpresaSede()
				{
					EmpresaSedeId = dto.EmpresaSedeId,
					PeriodoId = dto.PeriodoId
				});

				if (periodoEmpresaSede != null)
					throw new Exception(SR.GetString("EX_periodo_empresa_sede_ya_registrado"));

				periodoEmpresaSede = Mapper.Map<PeriodoEmpresaSede>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = periodoEmpresaSedeRepository.Insert(periodoEmpresaSede);

					dto.PeriodoEmpresaSedeId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(PeriodoEmpresaSedeDTO dto)
		{
			try
			{
				PeriodoEmpresaSede periodoEmpresaSede = Mapper.Map<PeriodoEmpresaSede>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = periodoEmpresaSedeRepository.Update(periodoEmpresaSede);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(PeriodoEmpresaSedeDTO dto)
		{
			try
			{
				PeriodoEmpresaSede periodoEmpresaSede = Mapper.Map<PeriodoEmpresaSede>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					periodoEmpresaSedeRepository.Delete(periodoEmpresaSede);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<EmpresaDTO> ObtenerEmpresaList(PeriodoEmpresaSedeDTO dto)
		{
			try
			{
				PeriodoEmpresaSede periodoEmpresaSede = Mapper.Map<PeriodoEmpresaSede>(dto);

				List<PeriodoEmpresaSede> periodoEmpresaSedeList = periodoEmpresaSedeRepository.Search(periodoEmpresaSede);

				var groupedList =
							 from c in periodoEmpresaSedeList
							 group c by new
							 {
								 c.EmpresaSede.Empresa.EmpresaId,
								 c.EmpresaSede.Empresa.Nombre
							 };

				List<EmpresaDTO> empresaDTOList = new List<EmpresaDTO>();
				foreach (var itemList in groupedList)
				{
					EmpresaDTO empresaDTO = new EmpresaDTO();
					empresaDTO.EmpresaId = itemList.Key.EmpresaId;
					empresaDTO.Nombre = itemList.Key.Nombre;
					empresaDTOList.Add(empresaDTO);
				}

				return empresaDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
