using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
	public class ConceptoBaseImponibleBusiness : IConceptoBaseImponibleBusiness
	{
		private readonly IConceptoBaseImponibleRepository conceptoBaseImponibleRepository;

		public ConceptoBaseImponibleBusiness(
			IConceptoBaseImponibleRepository conceptoBaseImponibleRepository
			)
		{
			this.conceptoBaseImponibleRepository = conceptoBaseImponibleRepository;
		}
		public ConceptoBaseImponibleDTO GetById(int id)
		{
			try
			{
				ConceptoBaseImponible conceptoBaseImponible = conceptoBaseImponibleRepository.GetById(id);

				ConceptoBaseImponibleDTO conceptoBaseImponibleDTO = Mapper.Map<ConceptoBaseImponibleDTO>(conceptoBaseImponible);

				return conceptoBaseImponibleDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public List<ConceptoBaseImponibleDTO> Search(ConceptoBaseImponibleDTO dto)
		{
			try
			{
				List<ConceptoBaseImponible> conceptoBaseImponibleList = conceptoBaseImponibleRepository.Search(Mapper.Map<ConceptoBaseImponible>(dto));

				List<ConceptoBaseImponibleDTO> conceptoBaseImponibleDTOList = Mapper.Map<List<ConceptoBaseImponibleDTO>>(conceptoBaseImponibleList);

				return conceptoBaseImponibleDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public ConceptoBaseImponibleDTO SingleSearch(ConceptoBaseImponibleDTO dto)
		{
			try
			{
				ConceptoBaseImponible conceptoBaseImponible = conceptoBaseImponibleRepository.SingleSearch(Mapper.Map<ConceptoBaseImponible>(dto));

				ConceptoBaseImponibleDTO conceptoBaseImponibleDTO = Mapper.Map<ConceptoBaseImponibleDTO>(conceptoBaseImponible);

				return conceptoBaseImponibleDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Insert(ConceptoBaseImponibleDTO dto)
		{
			try
			{
				ConceptoBaseImponible conceptoBaseImponible = Mapper.Map<ConceptoBaseImponible>(dto);

				int newId = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					newId = conceptoBaseImponibleRepository.Insert(conceptoBaseImponible);

					dto.ConceptoBaseImponibleId = newId;
					tx.Complete();
				}

				return newId;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public int Update(ConceptoBaseImponibleDTO dto)
		{
			try
			{
				ConceptoBaseImponible conceptoBaseImponible = Mapper.Map<ConceptoBaseImponible>(dto);

				int id = 0;
				using (TransactionScope tx = new TransactionScope())
				{
					id = conceptoBaseImponibleRepository.Update(conceptoBaseImponible);

					tx.Complete();
				}

				return id;
			}
			catch (Exception)
			{
				throw;
			}
		}
		public void Delete(ConceptoBaseImponibleDTO dto)
		{
			try
			{
				ConceptoBaseImponible conceptoBaseImponible = Mapper.Map<ConceptoBaseImponible>(dto);

				using (TransactionScope tx = new TransactionScope())
				{
					conceptoBaseImponibleRepository.Delete(conceptoBaseImponible);

					tx.Complete();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
      public List<ConceptoBaseImponibleDTO> ListarPorConceptoRemunerativoId(ConceptoBaseImponibleDTO dto)
      {
			try
			{
				List<ConceptoBaseImponible> conceptoBaseImponibleList = conceptoBaseImponibleRepository.
					ListarPorConceptoRemunerativoId(Mapper.Map<ConceptoBaseImponible>(dto));

				List<ConceptoBaseImponibleDTO> conceptoBaseImponibleDTOList = Mapper.Map<List<ConceptoBaseImponibleDTO>>(conceptoBaseImponibleList);

				return conceptoBaseImponibleDTOList;
			}
			catch (Exception)
			{
				throw;
			}
		}
   }
}
