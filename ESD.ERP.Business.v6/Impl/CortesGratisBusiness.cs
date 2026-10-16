using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Business.Resources;
using ESD.ERP.Common;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;
using ESD.ERP.Repository.Impl;

namespace ESD.ERP.Business.Impl
{
    public class CortesGratisBusiness : ICortesGratisBusiness
    {
        private readonly ICortesGratisRepository cortesGratisRepository;
        private readonly ITablaRepository tablaRepository;
        private readonly ICortesGratisRepository CortesGratisRepository;

        public CortesGratisBusiness(
            ICortesGratisRepository cortesGratisRepository,
            ICortesGratisRepository CortesGratisRepository,
            ITablaRepository tablaRepository
            )
        {
            this.cortesGratisRepository = cortesGratisRepository;
            this.CortesGratisRepository = CortesGratisRepository;
            this.tablaRepository = tablaRepository;
        }
        public CortesGratisDTO GetById(int id)
		{
			try
			{
				CortesGratis cortesGratis = cortesGratisRepository.GetById(id);

				CortesGratisDTO cortesGratisDTO = Mapper.Map<CortesGratisDTO>(cortesGratis);

				return cortesGratisDTO;
			}
			catch (Exception)
			{
				throw;
			}
		}
        public List<CortesGratisDTO> Search(CortesGratisDTO dto)
        {
            try
            {
                List<CortesGratis> cortesGratisList = cortesGratisRepository.Search(Mapper.Map<CortesGratis>(dto));

                List<CortesGratisDTO> cortesGratisDTOList = Mapper.Map<List<CortesGratisDTO>>(cortesGratisList);

                return cortesGratisDTOList;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public CortesGratisDTO SingleSearch(CortesGratisDTO dto)
        {
            try
            {
                CortesGratis cortesGratis = CortesGratisRepository.SingleSearch(Mapper.Map<CortesGratis>(dto));

                CortesGratisDTO cortesGratisDTO = Mapper.Map<CortesGratisDTO>(cortesGratis);

                return cortesGratisDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int Insert(CortesGratisDTO dto)
        {
            try
            {
                CortesGratis cortesGratis = Mapper.Map<CortesGratis>(dto);

                int newId = 0;
                using (TransactionScope tx = new TransactionScope())
                {
                    dto.CortesGratisId = newId;
                    tx.Complete();
                }

                return newId;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int Update(CortesGratisDTO dto)
        {
            try
            {
                CortesGratis cortesGratis = Mapper.Map<CortesGratis>(dto);

                int id = 0;
                using (TransactionScope tx = new TransactionScope())
                {
                    id = CortesGratisRepository.Update(cortesGratis);

                    tx.Complete();
                }

                return id;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}