using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
   public class DirectivaUsuarioBusiness : IDirectivaUsuarioBusiness
   {
      private readonly IDirectivaUsuarioRepository directivaUsuarioRepository;

      public DirectivaUsuarioBusiness(
         IDirectivaUsuarioRepository directivaUsuarioRepository
         )
      {
         this.directivaUsuarioRepository = directivaUsuarioRepository;
      }
      public DirectivaUsuarioDTO GetById(int id)
      {
         try
         {
            DirectivaUsuario directivaUsuario = directivaUsuarioRepository.GetById(id);

            DirectivaUsuarioDTO directivaUsuarioDTO = Mapper.Map<DirectivaUsuarioDTO>(directivaUsuario);

            return directivaUsuarioDTO;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public List<DirectivaUsuarioDTO> Search(DirectivaUsuarioDTO dto)
      {
         try
         {
            List<DirectivaUsuario> directivaUsuarioList = directivaUsuarioRepository.Search(Mapper.Map<DirectivaUsuario>(dto));

            List<DirectivaUsuarioDTO> directivaUsuarioDTOList = Mapper.Map<List<DirectivaUsuarioDTO>>(directivaUsuarioList);

            return directivaUsuarioDTOList;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public DirectivaUsuarioDTO SingleSearch(DirectivaUsuarioDTO dto)
      {
         try
         {
            DirectivaUsuario directivaUsuario = directivaUsuarioRepository.SingleSearch(Mapper.Map<DirectivaUsuario>(dto));

            DirectivaUsuarioDTO directivaUsuarioDTO = Mapper.Map<DirectivaUsuarioDTO>(directivaUsuario);

            return directivaUsuarioDTO;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public int Insert(DirectivaUsuarioDTO dto)
      {
         try
         {
            DirectivaUsuario directivaUsuario = Mapper.Map<DirectivaUsuario>(dto);

            int newId = 0;
            using (TransactionScope tx = new TransactionScope())
            {
               newId = directivaUsuarioRepository.Insert(directivaUsuario);

               dto.DirectivaUsuarioId = newId;
               tx.Complete();
            }

            return newId;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public int Update(DirectivaUsuarioDTO dto)
      {
         try
         {
            DirectivaUsuario directivaUsuario = Mapper.Map<DirectivaUsuario>(dto);

            int id = 0;
            using (TransactionScope tx = new TransactionScope())
            {
               id = directivaUsuarioRepository.Update(directivaUsuario);

               tx.Complete();
            }

            return id;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public void Delete(DirectivaUsuarioDTO dto)
      {
         try
         {
            DirectivaUsuario directivaUsuario = Mapper.Map<DirectivaUsuario>(dto);

            using (TransactionScope tx = new TransactionScope())
            {
               directivaUsuarioRepository.Delete(directivaUsuario);

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
