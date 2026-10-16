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
	public class PrestamoPersonalBusiness : IPrestamoPersonalBusiness
	{
		private readonly IPrestamoPersonalRepository prestamoPersonalRepository;
		private readonly ITablaBusiness tablaBusiness;
		private readonly IPrestamoCuotaRepository prestamoCuotaRepository;
		public PrestamoPersonalBusiness(
			IPrestamoPersonalRepository prestamoPersonalRepository,
			ITablaBusiness tablaBusiness,
			IPrestamoCuotaRepository prestamoCuotaRepository
			)
		{
			this.prestamoPersonalRepository = prestamoPersonalRepository;
			this.tablaBusiness = tablaBusiness;
			this.prestamoCuotaRepository = prestamoCuotaRepository;
		}
		public PrestamoPersonalDTO GetById(int id)
		{
			try
			{
				PrestamoPersonal prestamoPersonal = prestamoPersonalRepository.GetById(id);

				PrestamoPersonalDTO prestamoPersonalDTO = Mapper.Map<PrestamoPersonalDTO>(prestamoPersonal);

				return prestamoPersonalDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<PrestamoPersonalDTO> Search(PrestamoPersonalDTO dto)
		{
			try
			{
				List<PrestamoPersonal> prestamoPersonalList = prestamoPersonalRepository.Search(Mapper.Map<PrestamoPersonal>(dto));

				List<PrestamoPersonalDTO> prestamoPersonalDTOList = Mapper.Map<List<PrestamoPersonalDTO>>(prestamoPersonalList);

				return prestamoPersonalDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public PrestamoPersonalDTO SingleSearch(PrestamoPersonalDTO dto)
		{
			try
			{
				PrestamoPersonal prestamoPersonal = prestamoPersonalRepository.SingleSearch(Mapper.Map<PrestamoPersonal>(dto));

				PrestamoPersonalDTO prestamoPersonalDTO = Mapper.Map<PrestamoPersonalDTO>(prestamoPersonal);

				return prestamoPersonalDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(PrestamoPersonalDTO dto)
		{
			try
			{
				PrestamoPersonal prestamoPersonal = Mapper.Map<PrestamoPersonal>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					TablaCorrelativoDTO tablaCorrelativoDTO = new TablaCorrelativoDTO
					{
						Tabla = TablaConstante.PRESTAMO,
						PeriodoEmpresaSedeDTO = new PeriodoEmpresaSedeDTO()
						{
							PeriodoId = dto.PeriodoId,
							EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = prestamoPersonal.Personal.EmpresaSede.EmpresaId }
						},
						UsarSedeCentral = true,
						UsuarioAccionId = prestamoPersonal.UsuarioCreacionId
					};

					prestamoPersonal.Codigo = tablaBusiness.ObtenerCorrelativo(tablaCorrelativoDTO);

					newId = prestamoPersonalRepository.Insert(prestamoPersonal);
					if(dto.PrestamoCuotaDTOList != null)
               {
						if(dto.PrestamoCuotaDTOList.Count > 0){
							foreach (PrestamoCuotaDTO prestamoCuotaDTO in dto.PrestamoCuotaDTOList)
								prestamoCuotaDTO.PrestamoPersonalId = newId;

							List<PrestamoCuota> prestamoCuotaList = Mapper.Map<List<PrestamoCuota>>(dto.PrestamoCuotaDTOList);
							prestamoCuotaRepository.BulkMerge(prestamoCuotaList);
						}
					}

					dto.PrestamoPersonalId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(PrestamoPersonalDTO dto)
		{
			try
			{
				PrestamoPersonal prestamoPersonal = Mapper.Map<PrestamoPersonal>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = prestamoPersonalRepository.Update(prestamoPersonal);

					if (dto.PrestamoCuotaDTOList != null)
					{
						if (dto.PrestamoCuotaDTOList.Count > 0)
						{
							List<PrestamoCuota> prestamoCuotaList = Mapper.Map<List<PrestamoCuota>>(dto.PrestamoCuotaDTOList);
							prestamoCuotaRepository.BulkMerge(prestamoCuotaList);
						}
					}
					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(PrestamoPersonalDTO dto)
		{
			try
			{
				PrestamoPersonal prestamoPersonal = Mapper.Map<PrestamoPersonal>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					prestamoPersonalRepository.Delete(prestamoPersonal);

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
