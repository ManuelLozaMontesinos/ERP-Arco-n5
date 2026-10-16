using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
   public class RolMenuBusiness : IRolMenuBusiness
   {
      private readonly IRolMenuRepository rolMenuRepository;

      public RolMenuBusiness(
         IRolMenuRepository rolMenuRepository
         )
      {
         this.rolMenuRepository = rolMenuRepository;
      }
      public RolMenuDTO GetById(int id)
      {
         try
         {
            RolMenu rolMenu = rolMenuRepository.GetById(id);

            RolMenuDTO rolMenuDTO = Mapper.Map<RolMenuDTO>(rolMenu);

            return rolMenuDTO;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public List<RolMenuDTO> Search(RolMenuDTO dto)
      {
         try
         {
            List<RolMenu> rolMenuList = rolMenuRepository.Search(Mapper.Map<RolMenu>(dto));

            List<RolMenuDTO> rolMenuDTOList = Mapper.Map<List<RolMenuDTO>>(rolMenuList);

            return rolMenuDTOList;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public RolMenuDTO SingleSearch(RolMenuDTO dto)
      {
         try
         {
            RolMenu rolMenu = rolMenuRepository.SingleSearch(Mapper.Map<RolMenu>(dto));

            RolMenuDTO rolMenuDTO = Mapper.Map<RolMenuDTO>(rolMenu);

            return rolMenuDTO;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public int Insert(RolMenuDTO dto)
      {
         try
         {
            RolMenu rolMenu = Mapper.Map<RolMenu>(dto);

            int newId = 0;
            using (TransactionScope tx = new TransactionScope())
            {
               newId = rolMenuRepository.Insert(rolMenu);

               dto.RolMenuId = newId;
               tx.Complete();
            }

            return newId;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public int Update(RolMenuDTO dto)
      {
         try
         {
            RolMenu rolMenu = Mapper.Map<RolMenu>(dto);

            int id = 0;
            using (TransactionScope tx = new TransactionScope())
            {
               id = rolMenuRepository.Update(rolMenu);

               tx.Complete();
            }

            return id;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public void Delete(RolMenuDTO dto)
      {
         try
         {
            RolMenu rolMenu = Mapper.Map<RolMenu>(dto);

            using (TransactionScope tx = new TransactionScope())
            {
               rolMenuRepository.Delete(rolMenu);

               tx.Complete();
            }
         }
         catch (Exception)
         {
            throw;
         }
      }
      public List<RolMenuDTO> ListarPorRolId(RolMenuDTO dto)
      {
         try
         {
            List<RolMenu> rolMenuList = rolMenuRepository.ListarPorRolId(Mapper.Map<RolMenu>(dto));

            List<RolMenuDTO> rolMenuDTOList = Mapper.Map<List<RolMenuDTO>>(rolMenuList);

            return rolMenuDTOList;
         }
         catch (Exception)
         {
            throw;
         }
      }
   }
}
