using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
   public class DirectivaReglaBusiness : IDirectivaReglaBusiness
   {
      private readonly IDirectivaReglaRepository directivaReglaRepository;

      public DirectivaReglaBusiness(
         IDirectivaReglaRepository directivaReglaRepository
         )
      {
         this.directivaReglaRepository = directivaReglaRepository;
      }
      public DirectivaReglaDTO GetById(int id)
      {
         try
         {
            DirectivaRegla directivaRegla = directivaReglaRepository.GetById(id);

            DirectivaReglaDTO directivaReglaDTO = Mapper.Map<DirectivaReglaDTO>(directivaRegla);

            return directivaReglaDTO;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public List<DirectivaReglaDTO> Search(DirectivaReglaDTO dto)
      {
         try
         {
            List<DirectivaRegla> directivaReglaList = directivaReglaRepository.Search(Mapper.Map<DirectivaRegla>(dto));

            List<DirectivaReglaDTO> directivaReglaDTOList = Mapper.Map<List<DirectivaReglaDTO>>(directivaReglaList);

            return directivaReglaDTOList;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public DirectivaReglaDTO SingleSearch(DirectivaReglaDTO dto)
      {
         try
         {
            DirectivaRegla directivaRegla = directivaReglaRepository.SingleSearch(Mapper.Map<DirectivaRegla>(dto));

            DirectivaReglaDTO directivaReglaDTO = Mapper.Map<DirectivaReglaDTO>(directivaRegla);

            return directivaReglaDTO;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public int Insert(DirectivaReglaDTO dto)
      {
         try
         {
            DirectivaRegla directivaRegla = Mapper.Map<DirectivaRegla>(dto);

            int newId = 0;
            using (TransactionScope tx = new TransactionScope())
            {
               newId = directivaReglaRepository.Insert(directivaRegla);

               dto.DirectivaReglaId = newId;
               tx.Complete();
            }

            return newId;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public int Update(DirectivaReglaDTO dto)
      {
         try
         {
            DirectivaRegla directivaRegla = Mapper.Map<DirectivaRegla>(dto);

            int id = 0;
            using (TransactionScope tx = new TransactionScope())
            {
               id = directivaReglaRepository.Update(directivaRegla);

               tx.Complete();
            }

            return id;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public void Delete(DirectivaReglaDTO dto)
      {
         try
         {
            DirectivaRegla directivaRegla = Mapper.Map<DirectivaRegla>(dto);

            using (TransactionScope tx = new TransactionScope())
            {
               directivaReglaRepository.Delete(directivaRegla);

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
