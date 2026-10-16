using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ESD.Core.Logging;
using ESD.Core.Service;
using ESD.ERP.Business;
using ESD.ERP.DTO;
using ESD.ERP.WebApi.Infrastructure;

namespace ESD.ERP.WebApi.Controllers
{
   [Produces("application/json")]
   [Route("api/RolMenu")]
   public class RolMenuController : AuthorizedController
   {
      private readonly IRolMenuBusiness rolMenuBusiness;

      public RolMenuController(
         IRolMenuBusiness rolMenuBusiness
         )
      {
         this.rolMenuBusiness = rolMenuBusiness;
      }
      [HttpPost("GetById")]
      public ActionResult<ServiceResponse> GetById([FromBody] RolMenuDTO dto)
      {
         Logger.InfoStart();
         ServiceResponse serviceResponse = new();

         try
         {
            RolMenuDTO rolMenuDTO = rolMenuBusiness.GetById(dto.Id);
            serviceResponse.Data = rolMenuDTO;
         }
         catch (Exception e)
         {
            Logger.Error(e);
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
         }

         Logger.InfoFinish();
         return serviceResponse;
      }
      [HttpPost("Search")]
      public ActionResult<ServiceResponse> Search([FromBody] RolMenuDTO dto)
      {
         Logger.InfoStart();
         ServiceResponse serviceResponse = new();

         try
         {
            List<RolMenuDTO> rolMenuDTOList = rolMenuBusiness.Search(dto);
            serviceResponse.Data = rolMenuDTOList;
         }
         catch (Exception e)
         {
            Logger.Error(e);
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
         }

         Logger.InfoFinish();
         return serviceResponse;
      }
      [HttpPost("SingleSearch")]
      public ActionResult<ServiceResponse> SingleSearch([FromBody] RolMenuDTO dto)
      {
         Logger.InfoStart();
         ServiceResponse serviceResponse = new();

         try
         {
            RolMenuDTO rolMenuDTO = rolMenuBusiness.SingleSearch(dto);
            serviceResponse.Data = rolMenuDTO;
         }
         catch (Exception e)
         {
            Logger.Error(e);
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
         }

         Logger.InfoFinish();
         return serviceResponse;
      }
      [HttpPost("Insert")]
      public ActionResult<ServiceResponse> Insert([FromBody] RolMenuDTO dto)
      {
         Logger.InfoStart();
         ServiceResponse serviceResponse = new();

         try
         {
            int newId = rolMenuBusiness.Insert(dto);
            serviceResponse.Data = newId;
         }
         catch (Exception e)
         {
            Logger.Error(e);
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
         }

         Logger.InfoFinish();
         return serviceResponse;
      }
      [HttpPost("Update")]
      public ActionResult<ServiceResponse> Update([FromBody] RolMenuDTO dto)
      {
         Logger.InfoStart();
         ServiceResponse serviceResponse = new();

         try
         {
            int id = rolMenuBusiness.Update(dto);
            serviceResponse.Data = id;
         }
         catch (Exception e)
         {
            Logger.Error(e);
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
         }

         Logger.InfoFinish();
         return serviceResponse;
      }
      [HttpPost("Delete")]
      public ActionResult<ServiceResponse> Delete([FromBody] RolMenuDTO dto)
      {
         Logger.InfoStart();
         ServiceResponse serviceResponse = new();

         try
         {
            rolMenuBusiness.Delete(dto);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
         }

         Logger.InfoFinish();
         return serviceResponse;
      }
      [HttpPost("ListarPorRolId")]
      public ActionResult<ServiceResponse> ListarPorRolId([FromBody] RolMenuDTO dto)
      {
         Logger.InfoStart();
         ServiceResponse serviceResponse = new();

         try
         {
            List<RolMenuDTO> rolMenuDTOList = rolMenuBusiness.ListarPorRolId(dto);
            serviceResponse.Data = rolMenuDTOList;
         }
         catch (Exception e)
         {
            Logger.Error(e);
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
         }

         Logger.InfoFinish();
         return serviceResponse;
      }
   }
}
