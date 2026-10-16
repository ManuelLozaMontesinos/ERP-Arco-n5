using System;
using System.Collections.Generic;
using System.Transactions;
using ESD.Core;
using ESD.ERP.Domain;
using ESD.ERP.DTO;
using ESD.ERP.Repository;

namespace ESD.ERP.Business.Impl
{
   public class EstadoMovimientoReclasificacionBusiness : IEstadoMovimientoReclasificacionBusiness
   {
      private readonly IEstadoMovimientoReclasificacionRepository estadoMovimientoReclasificacionRepository;

      public EstadoMovimientoReclasificacionBusiness(
         IEstadoMovimientoReclasificacionRepository estadoMovimientoReclasificacionRepository
         )
      {
         this.estadoMovimientoReclasificacionRepository = estadoMovimientoReclasificacionRepository;
      }
      public EstadoMovimientoReclasificacionDTO GetById(int id)
      {
         try
         {
            EstadoMovimientoReclasificacion estadoMovimientoReclasificacion = estadoMovimientoReclasificacionRepository.GetById(id);

            EstadoMovimientoReclasificacionDTO estadoMovimientoReclasificacionDTO = Mapper.Map<EstadoMovimientoReclasificacionDTO>(estadoMovimientoReclasificacion);

            return estadoMovimientoReclasificacionDTO;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public List<EstadoMovimientoReclasificacionDTO> Search(EstadoMovimientoReclasificacionDTO dto)
      {
         try
         {
            List<EstadoMovimientoReclasificacion> estadoMovimientoReclasificacionList = estadoMovimientoReclasificacionRepository.Search(Mapper.Map<EstadoMovimientoReclasificacion>(dto));

            List<EstadoMovimientoReclasificacionDTO> estadoMovimientoReclasificacionDTOList = Mapper.Map<List<EstadoMovimientoReclasificacionDTO>>(estadoMovimientoReclasificacionList);

            return estadoMovimientoReclasificacionDTOList;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public EstadoMovimientoReclasificacionDTO SingleSearch(EstadoMovimientoReclasificacionDTO dto)
      {
         try
         {
            EstadoMovimientoReclasificacion estadoMovimientoReclasificacion = estadoMovimientoReclasificacionRepository.SingleSearch(Mapper.Map<EstadoMovimientoReclasificacion>(dto));

            EstadoMovimientoReclasificacionDTO estadoMovimientoReclasificacionDTO = Mapper.Map<EstadoMovimientoReclasificacionDTO>(estadoMovimientoReclasificacion);

            return estadoMovimientoReclasificacionDTO;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public int Insert(EstadoMovimientoReclasificacionDTO dto)
      {
         try
         {
            EstadoMovimientoReclasificacion estadoMovimientoReclasificacion = Mapper.Map<EstadoMovimientoReclasificacion>(dto);

            int newId = 0;
            using (TransactionScope tx = new TransactionScope())
            {
               newId = estadoMovimientoReclasificacionRepository.Insert(estadoMovimientoReclasificacion);

               dto.EstadoMovimientoReclasificacionId = newId;
               tx.Complete();
            }

            return newId;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public int Update(EstadoMovimientoReclasificacionDTO dto)
      {
         try
         {
            EstadoMovimientoReclasificacion estadoMovimientoReclasificacion = Mapper.Map<EstadoMovimientoReclasificacion>(dto);

            int id = 0;
            using (TransactionScope tx = new TransactionScope())
            {
               id = estadoMovimientoReclasificacionRepository.Update(estadoMovimientoReclasificacion);

               tx.Complete();
            }

            return id;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public void Delete(EstadoMovimientoReclasificacionDTO dto)
      {
         try
         {
            EstadoMovimientoReclasificacion estadoMovimientoReclasificacion = Mapper.Map<EstadoMovimientoReclasificacion>(dto);

            using (TransactionScope tx = new TransactionScope())
            {
               estadoMovimientoReclasificacionRepository.Delete(estadoMovimientoReclasificacion);

               tx.Complete();
            }
         }
         catch (Exception)
         {
            throw;
         }
      }
      public EstadoMovimientoReclasificacionDTO ObtenerPorCodigo(string codigo)
      {
         try
         {
            EstadoMovimientoReclasificacion estadoMovimientoReclasificacion = estadoMovimientoReclasificacionRepository.ObtenerPorCodigo(codigo);

            EstadoMovimientoReclasificacionDTO estadoMovimientoReclasificacionDTO = Mapper.Map<EstadoMovimientoReclasificacionDTO>(estadoMovimientoReclasificacion);

            return estadoMovimientoReclasificacionDTO;
         }
         catch (Exception)
         {
            throw;
         }
      }
   }
}
