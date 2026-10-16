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
	public class ConceptoRemunerativoBusiness : IConceptoRemunerativoBusiness
	{
		private readonly IConceptoRemunerativoRepository conceptoRemunerativoRepository;
		private readonly ITablaBusiness tablaBusiness;
		private readonly IConceptoBaseImponibleRepository conceptoBaseImponibleRepository;

		public ConceptoRemunerativoBusiness(
			IConceptoRemunerativoRepository conceptoRemunerativoRepository,
			ITablaBusiness tablaBusiness,
			IConceptoBaseImponibleRepository conceptoBaseImponibleRepository
			)
		{
			this.conceptoRemunerativoRepository = conceptoRemunerativoRepository;
			this.tablaBusiness = tablaBusiness;
			this.conceptoBaseImponibleRepository = conceptoBaseImponibleRepository;
		}
		public ConceptoRemunerativoDTO GetById(int id)
		{
			try
			{
				ConceptoRemunerativo conceptoRemunerativo = conceptoRemunerativoRepository.GetById(id);

				ConceptoRemunerativoDTO conceptoRemunerativoDTO = Mapper.Map<ConceptoRemunerativoDTO>(conceptoRemunerativo);

				return conceptoRemunerativoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<ConceptoRemunerativoDTO> Search(ConceptoRemunerativoDTO dto)
		{
			try
			{
				List<ConceptoRemunerativo> conceptoRemunerativoList = conceptoRemunerativoRepository.Search(Mapper.Map<ConceptoRemunerativo>(dto));

				List<ConceptoRemunerativoDTO> conceptoRemunerativoDTOList = Mapper.Map<List<ConceptoRemunerativoDTO>>(conceptoRemunerativoList);

				return conceptoRemunerativoDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public ConceptoRemunerativoDTO SingleSearch(ConceptoRemunerativoDTO dto)
		{
			try
			{
				ConceptoRemunerativo conceptoRemunerativo = conceptoRemunerativoRepository.SingleSearch(Mapper.Map<ConceptoRemunerativo>(dto));

				ConceptoRemunerativoDTO conceptoRemunerativoDTO = Mapper.Map<ConceptoRemunerativoDTO>(conceptoRemunerativo);

				return conceptoRemunerativoDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(ConceptoRemunerativoDTO dto)
		{
			try
			{
				ConceptoRemunerativo conceptoRemunerativo = Mapper.Map<ConceptoRemunerativo>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{

					TablaCorrelativoDTO tablaCorrelativoDTO = new TablaCorrelativoDTO
					{
						Tabla = TablaConstante.CONCEPTO_REMUNERATIVO,
						PeriodoEmpresaSedeDTO = new PeriodoEmpresaSedeDTO()
						{
							EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = conceptoRemunerativo.EmpresaId }
						},
						UsuarioAccionId = conceptoRemunerativo.UsuarioCreacionId
					};

					conceptoRemunerativo.Codigo = tablaBusiness.ObtenerCorrelativo(tablaCorrelativoDTO);

					newId = conceptoRemunerativoRepository.Insert(conceptoRemunerativo);

					actualizarConceptoBaseImponibleList(dto.ConceptoBaseImponibleDTOList, newId);

					dto.ConceptoRemunerativoId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(ConceptoRemunerativoDTO dto)
		{
			try
			{
				ConceptoRemunerativo conceptoRemunerativo = Mapper.Map<ConceptoRemunerativo>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = conceptoRemunerativoRepository.Update(conceptoRemunerativo);

					actualizarConceptoBaseImponibleList(dto.ConceptoBaseImponibleDTOList, id);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		private void actualizarConceptoBaseImponibleList(List<ConceptoBaseImponibleDTO> list, int id) {
			if (list == null)
				return;

			foreach (ConceptoBaseImponibleDTO conceptoBaseImponibleDTO in list)
			{
				if (conceptoBaseImponibleDTO.Id == 0)
				{
					conceptoBaseImponibleDTO.ConceptoRemunerativoId = id;
					conceptoBaseImponibleRepository.Insert(Mapper.Map<ConceptoBaseImponible>(conceptoBaseImponibleDTO));
				}
				else
					conceptoBaseImponibleRepository.Update(Mapper.Map<ConceptoBaseImponible>(conceptoBaseImponibleDTO));
			}
		}
		public void Delete(ConceptoRemunerativoDTO dto)
		{
			try
			{
				ConceptoRemunerativo conceptoRemunerativo = Mapper.Map<ConceptoRemunerativo>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					conceptoRemunerativoRepository.Delete(conceptoRemunerativo);

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
