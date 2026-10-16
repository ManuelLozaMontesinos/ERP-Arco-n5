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
	public class TablaBusiness : ITablaBusiness
	{
		private readonly ITablaRepository tablaRepository;
		private readonly IEmpresaCorrelativoRepository empresaCorrelativoRepository;
		private readonly IPeriodoEmpresaSedeRepository periodoEmpresaSedeRepository;
		private readonly IPeriodoEmpresaCorrelativoRepository periodoEmpresaCorrelativoRepository;

		public TablaBusiness(
			ITablaRepository tablaRepository,
			IEmpresaCorrelativoRepository empresaCorrelativoRepository,
			IPeriodoEmpresaSedeRepository periodoEmpresaSedeRepository,
			IPeriodoEmpresaCorrelativoRepository periodoEmpresaCorrelativoRepository
			)
		{
			this.tablaRepository = tablaRepository;
			this.empresaCorrelativoRepository = empresaCorrelativoRepository;
			this.periodoEmpresaSedeRepository = periodoEmpresaSedeRepository;
			this.periodoEmpresaCorrelativoRepository = periodoEmpresaCorrelativoRepository;
		}
		public TablaDTO GetById(int id)
		{
			try
			{
				Tabla tabla = tablaRepository.GetById(id);

				TablaDTO tablaDTO = Mapper.Map<TablaDTO>(tabla);

				return tablaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<TablaDTO> Search(TablaDTO dto)
		{
			try
			{
				List<Tabla> tablaList = tablaRepository.Search(Mapper.Map<Tabla>(dto));

				List<TablaDTO> tablaDTOList = Mapper.Map<List<TablaDTO>>(tablaList);

				return tablaDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public TablaDTO SingleSearch(TablaDTO dto)
		{
			try
			{
				Tabla tabla = tablaRepository.SingleSearch(Mapper.Map<Tabla>(dto));

				TablaDTO tablaDTO = Mapper.Map<TablaDTO>(tabla);

				return tablaDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(TablaDTO dto)
		{
			try
			{
				Tabla tabla = Mapper.Map<Tabla>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = tablaRepository.Insert(tabla);

					dto.TablaId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(TablaDTO dto)
		{
			try
			{
				Tabla tabla = Mapper.Map<Tabla>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = tablaRepository.Update(tabla);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(TablaDTO dto)
		{
			try
			{
				Tabla tabla = Mapper.Map<Tabla>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					tablaRepository.Delete(tabla);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		public string ObtenerCorrelativo(TablaCorrelativoDTO dto)
		{

			Tabla tabla = tablaRepository.ObtenerPorCodigo(dto.Tabla);

			int correlativo = 0;
			bool existeCorrelativo = true;

			if (tabla.CorrelativoPorEmpresa == SiNoConstante.N && tabla.CorrelativoPorEmpresaSede == SiNoConstante.N)
         {
				tabla.Correlativo += 1;
				correlativo = tabla.Correlativo;

				tablaRepository.Update(tabla);
			}

			if (tabla.CorrelativoPorEmpresa == SiNoConstante.S)
			{
				EmpresaCorrelativo empresaCorrelativo = empresaCorrelativoRepository.SingleSearch(new EmpresaCorrelativo()
				{ TablaId = tabla.EntityId, EmpresaId = dto.PeriodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId });

				if (empresaCorrelativo == null)
				{
               empresaCorrelativo = new EmpresaCorrelativo
               {
                  EmpresaId = dto.PeriodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId,
                  TablaId = tabla.EntityId,
                  UsuarioCreacionId = dto.UsuarioAccionId
               };
               existeCorrelativo = false;
				}

				empresaCorrelativo.Correlativo += 1;
				correlativo = empresaCorrelativo.Correlativo;

				if (!existeCorrelativo)
					empresaCorrelativoRepository.Insert(empresaCorrelativo);
				else
					empresaCorrelativoRepository.Update(empresaCorrelativo);
			}

			if (tabla.CorrelativoPorEmpresaSede == SiNoConstante.S)
			{
				int periodoEmpresaSedeId = dto.PeriodoEmpresaSedeDTO.PeriodoEmpresaSedeId;

				PeriodoEmpresaSede periodoEmpresaSede = null;
				if (periodoEmpresaSedeId == 0)
					periodoEmpresaSede = periodoEmpresaSedeRepository.SingleSearch(Mapper.Map<PeriodoEmpresaSede>(dto.PeriodoEmpresaSedeDTO));
				else
					periodoEmpresaSede = periodoEmpresaSedeRepository.GetById(periodoEmpresaSedeId);
				
				periodoEmpresaSedeId = periodoEmpresaSede.EntityId;

				if (dto.UsarSedeCentral)
				{
					if (periodoEmpresaSede.EmpresaSede.EsCentral != SiNoConstante.S)
               {
						periodoEmpresaSede = periodoEmpresaSedeRepository.SingleSearch(new PeriodoEmpresaSede()
						{
							PeriodoId = periodoEmpresaSede.PeriodoId,
							EmpresaSede = new EmpresaSede() { 
								EmpresaId = periodoEmpresaSede.EmpresaSede.EmpresaId,
								EsCentral = SiNoConstante.S
							},
						});

						periodoEmpresaSedeId = periodoEmpresaSede.EntityId;
					}
				}

				PeriodoEmpresaCorrelativo periodoEmpresaCorrelativo = periodoEmpresaCorrelativoRepository.SingleSearch(new PeriodoEmpresaCorrelativo()
				{ TablaId = tabla.EntityId, PeriodoEmpresaSedeId = periodoEmpresaSedeId });

				if (periodoEmpresaCorrelativo == null)
				{
					periodoEmpresaCorrelativo = new PeriodoEmpresaCorrelativo
					{
						PeriodoEmpresaSedeId = periodoEmpresaSedeId,
						TablaId = tabla.EntityId,
						UsuarioCreacionId = dto.UsuarioAccionId
					};
					existeCorrelativo = false;
				}

				periodoEmpresaCorrelativo.Correlativo += 1;
				correlativo = periodoEmpresaCorrelativo.Correlativo;

				if (!existeCorrelativo)
					periodoEmpresaCorrelativoRepository.Insert(periodoEmpresaCorrelativo);
				else
					periodoEmpresaCorrelativoRepository.Update(periodoEmpresaCorrelativo);
			}

			return string.Concat(
				tabla.Prefijo,
				correlativo.ToString(tabla.FormatoCorrelativo)
				);
		}
	}
}
