using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
   public class MenuBusiness : IMenuBusiness
   {
      private readonly IMenuRepository menuRepository;

      public MenuBusiness(
         IMenuRepository menuRepository
         )
      {
         this.menuRepository = menuRepository;
      }
      public MenuDTO GetById(int id)
      {
         try
         {
            Menu menu = menuRepository.GetById(id);

            MenuDTO menuDTO = Mapper.Map<MenuDTO>(menu);

            return menuDTO;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public List<MenuDTO> Search(MenuDTO dto)
      {
         try
         {
            List<Menu> menuList = menuRepository.Search(Mapper.Map<Menu>(dto));

            List<MenuDTO> menuDTOList = Mapper.Map<List<MenuDTO>>(menuList);

            return menuDTOList;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public MenuDTO SingleSearch(MenuDTO dto)
      {
         try
         {
            Menu menu = menuRepository.SingleSearch(Mapper.Map<Menu>(dto));

            MenuDTO menuDTO = Mapper.Map<MenuDTO>(menu);

            return menuDTO;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public int Insert(MenuDTO dto)
      {
         try
         {
            Menu menu = Mapper.Map<Menu>(dto);

            int newId = 0;
            using (TransactionScope tx = new TransactionScope())
            {
               newId = menuRepository.Insert(menu);

               dto.MenuId = newId;
               tx.Complete();
            }

            return newId;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public int Update(MenuDTO dto)
      {
         try
         {
            Menu menu = Mapper.Map<Menu>(dto);

            int id = 0;
            using (TransactionScope tx = new TransactionScope())
            {
               id = menuRepository.Update(menu);

               tx.Complete();
            }

            return id;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public void Delete(MenuDTO dto)
      {
         try
         {
            Menu menu = Mapper.Map<Menu>(dto);

            using (TransactionScope tx = new TransactionScope())
            {
               menuRepository.Delete(menu);

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
