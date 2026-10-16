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
   public class RolBusiness : IRolBusiness
   {
      private readonly IRolRepository rolRepository;
      private readonly ITablaBusiness tablaBusiness;
      private readonly IRolMenuRepository rolMenuRepository;

      public RolBusiness(
         IRolRepository rolRepository,
         ITablaBusiness tablaBusiness,
         IRolMenuRepository rolMenuRepository
         )
      {
         this.rolRepository = rolRepository;
         this.tablaBusiness = tablaBusiness;
         this.rolMenuRepository = rolMenuRepository;
      }
      public RolDTO GetById(int id)
      {
         try
         {
            Rol rol = rolRepository.GetById(id);

            RolDTO rolDTO = Mapper.Map<RolDTO>(rol);

            return rolDTO;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public List<RolDTO> Search(RolDTO dto)
      {
         try
         {
            List<Rol> rolList = rolRepository.Search(Mapper.Map<Rol>(dto));

            List<RolDTO> rolDTOList = Mapper.Map<List<RolDTO>>(rolList);

            return rolDTOList;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public RolDTO SingleSearch(RolDTO dto)
      {
         try
         {
            Rol rol = rolRepository.SingleSearch(Mapper.Map<Rol>(dto));

            RolDTO rolDTO = Mapper.Map<RolDTO>(rol);

            return rolDTO;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public int Insert(RolDTO dto)
      {
         try
         {
            Rol rol = Mapper.Map<Rol>(dto);

            int newId = 0;
            using (TransactionScope tx = new TransactionScope())
            {
               TablaCorrelativoDTO tablaCorrelativoDTO = new TablaCorrelativoDTO
               {
                  Tabla = TablaConstante.ROL,
                  PeriodoEmpresaSedeDTO = new PeriodoEmpresaSedeDTO()
                  {
                     EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = rol.EmpresaId.Value }
                  },
                  UsuarioAccionId = rol.UsuarioCreacionId
               };

               rol.Codigo = tablaBusiness.ObtenerCorrelativo(tablaCorrelativoDTO);

               newId = rolRepository.Insert(rol);
               if(dto.RolMenuDTOList != null){
                  foreach (RolMenuDTO rolMenuDTO in dto.RolMenuDTOList)
                     rolMenuDTO.RolId = newId;

                  RolMenu rolMenu = new RolMenu()
                  {
                     RolId = newId,
                     RolMenuList = Mapper.Map<List<RolMenu>>(dto.RolMenuDTOList)
                  };

                  rolMenuRepository.ActualizarLote(rolMenu);
               }

               dto.RolId = newId;
               tx.Complete();
            }

            return newId;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public int Update(RolDTO dto)
      {
         try
         {
            Rol rol = Mapper.Map<Rol>(dto);

            int id = 0;
            using (TransactionScope tx = new TransactionScope())
            {
               id = rolRepository.Update(rol);

               if (dto.RolMenuDTOList != null)
               {
                  RolMenu rolMenu = new RolMenu()
                  {
                     RolId = id,
                     RolMenuList = Mapper.Map<List<RolMenu>>(dto.RolMenuDTOList)
                  };

                  rolMenuRepository.ActualizarLote(rolMenu);
               }
               tx.Complete();
            }

            return id;
         }
         catch (Exception)
         {
            throw;
         }
      }
      public void Delete(RolDTO dto)
      {
         try
         {
            Rol rol = Mapper.Map<Rol>(dto);

            using (TransactionScope tx = new TransactionScope())
            {
               rolRepository.Delete(rol);

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
