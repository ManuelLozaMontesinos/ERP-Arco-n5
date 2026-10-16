using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
   public class GrupoParametroSistemaBusiness : IGrupoParametroSistemaBusiness
   {
      private readonly IGrupoParametroSistemaRepository grupoParametroSistemaRepository;

      public GrupoParametroSistemaBusiness(
         IGrupoParametroSistemaRepository grupoParametroSistemaRepository
         )
      {
         this.grupoParametroSistemaRepository = grupoParametroSistemaRepository;
      }
      public GrupoParametroSistemaDTO GetById(int id)
      {
         try
         {
            GrupoParametroSistema grupoParametroSistema = grupoParametroSistemaRepository.GetById(id);

            GrupoParametroSistemaDTO grupoParametroSistemaDTO = Mapper.Map<GrupoParametroSistemaDTO>(grupoParametroSistema);

            return grupoParametroSistemaDTO;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public List<GrupoParametroSistemaDTO> Search(GrupoParametroSistemaDTO dto)
      {
         try
         {
            List<GrupoParametroSistema> grupoParametroSistemaList = grupoParametroSistemaRepository.Search(Mapper.Map<GrupoParametroSistema>(dto));

            List<GrupoParametroSistemaDTO> grupoParametroSistemaDTOList = Mapper.Map<List<GrupoParametroSistemaDTO>>(grupoParametroSistemaList);

            return grupoParametroSistemaDTOList;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public GrupoParametroSistemaDTO SingleSearch(GrupoParametroSistemaDTO dto)
      {
         try
         {
            GrupoParametroSistema grupoParametroSistema = grupoParametroSistemaRepository.SingleSearch(Mapper.Map<GrupoParametroSistema>(dto));

            GrupoParametroSistemaDTO grupoParametroSistemaDTO = Mapper.Map<GrupoParametroSistemaDTO>(grupoParametroSistema);

            return grupoParametroSistemaDTO;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public int Insert(GrupoParametroSistemaDTO dto)
      {
         try
         {
            GrupoParametroSistema grupoParametroSistema = Mapper.Map<GrupoParametroSistema>(dto);

            int newId = 0;
            using (TransactionScope tx = new TransactionScope())
            {
               newId = grupoParametroSistemaRepository.Insert(grupoParametroSistema);

               dto.GrupoParametroSistemaId = newId;
               tx.Complete();
            }

            return newId;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public int Update(GrupoParametroSistemaDTO dto)
      {
         try
         {
            GrupoParametroSistema grupoParametroSistema = Mapper.Map<GrupoParametroSistema>(dto);

            int id = 0;
            using (TransactionScope tx = new TransactionScope())
            {
               id = grupoParametroSistemaRepository.Update(grupoParametroSistema);

               tx.Complete();
            }

            return id;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public void Delete(GrupoParametroSistemaDTO dto)
      {
         try
         {
            GrupoParametroSistema grupoParametroSistema = Mapper.Map<GrupoParametroSistema>(dto);

            using (TransactionScope tx = new TransactionScope())
            {
               grupoParametroSistemaRepository.Delete(grupoParametroSistema);

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
