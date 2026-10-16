using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
   public class TipoDatoBusiness : ITipoDatoBusiness
   {
      private readonly ITipoDatoRepository tipoDatoRepository;

      public TipoDatoBusiness(
         ITipoDatoRepository tipoDatoRepository
         )
      {
         this.tipoDatoRepository = tipoDatoRepository;
      }
      public TipoDatoDTO GetById(int id)
      {
         try
         {
            TipoDato tipoDato = tipoDatoRepository.GetById(id);

            TipoDatoDTO tipoDatoDTO = Mapper.Map<TipoDatoDTO>(tipoDato);

            return tipoDatoDTO;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public List<TipoDatoDTO> Search(TipoDatoDTO dto)
      {
         try
         {
            List<TipoDato> tipoDatoList = tipoDatoRepository.Search(Mapper.Map<TipoDato>(dto));

            List<TipoDatoDTO> tipoDatoDTOList = Mapper.Map<List<TipoDatoDTO>>(tipoDatoList);

            return tipoDatoDTOList;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public TipoDatoDTO SingleSearch(TipoDatoDTO dto)
      {
         try
         {
            TipoDato tipoDato = tipoDatoRepository.SingleSearch(Mapper.Map<TipoDato>(dto));

            TipoDatoDTO tipoDatoDTO = Mapper.Map<TipoDatoDTO>(tipoDato);

            return tipoDatoDTO;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public int Insert(TipoDatoDTO dto)
      {
         try
         {
            TipoDato tipoDato = Mapper.Map<TipoDato>(dto);

            int newId = 0;
            using (TransactionScope tx = new TransactionScope())
            {
               newId = tipoDatoRepository.Insert(tipoDato);

               dto.TipoDatoId = newId;
               tx.Complete();
            }

            return newId;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public int Update(TipoDatoDTO dto)
      {
         try
         {
            TipoDato tipoDato = Mapper.Map<TipoDato>(dto);

            int id = 0;
            using (TransactionScope tx = new TransactionScope())
            {
               id = tipoDatoRepository.Update(tipoDato);

               tx.Complete();
            }

            return id;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public void Delete(TipoDatoDTO dto)
      {
         try
         {
            TipoDato tipoDato = Mapper.Map<TipoDato>(dto);

            using (TransactionScope tx = new TransactionScope())
            {
               tipoDatoRepository.Delete(tipoDato);

               tx.Complete();
            }
         }
         catch (Exception)
         {
            throw;
         }
      }
      public TipoDatoDTO ObtenerPorCodigo(string codigo)
      {
         try
         {
            TipoDato tipoDato = tipoDatoRepository.ObtenerPorCodigo(codigo);

            TipoDatoDTO tipoDatoDTO = Mapper.Map<TipoDatoDTO>(tipoDato);

            return tipoDatoDTO;
         }
         catch (Exception)
         {
            throw;
         }
      }
   }
}
