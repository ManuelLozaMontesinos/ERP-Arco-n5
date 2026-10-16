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
    [Route("api/CortesGratis")]
    public class CortesGratisController : AuthorizedController
    {
        private readonly ICortesGratisBusiness cortesGratisBusiness;

        public CortesGratisController(
            ICortesGratisBusiness cortesGratisBusiness
            )
        {
            this.cortesGratisBusiness = cortesGratisBusiness;
        }
        [HttpPost("GetById")]
        public ActionResult<ServiceResponse> GetById([FromBody] CortesGratisDTO dto)
        {
            Logger.InfoStart();
            ServiceResponse serviceResponse = new();

            try
            {
                CortesGratisDTO cortesGratisDTO = cortesGratisBusiness.GetById(dto.Id);
                serviceResponse.Data = cortesGratisDTO;
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
        public ActionResult<ServiceResponse> Search([FromBody] CortesGratisDTO dto)
        {
            Logger.InfoStart();
            ServiceResponse serviceResponse = new();

            try
            {
                List<CortesGratisDTO> cortesGratisDTOList = cortesGratisBusiness.Search(dto);
                serviceResponse.Data = cortesGratisDTOList;
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
        public ActionResult<ServiceResponse> SingleSearch([FromBody] CortesGratisDTO dto)
        {
            Logger.InfoStart();
            ServiceResponse serviceResponse = new();

            try
            {
                CortesGratisDTO cortesGratisDTO = cortesGratisBusiness.SingleSearch(dto);
                serviceResponse.Data = cortesGratisDTO;
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
        public ActionResult<ServiceResponse> Insert([FromBody] CortesGratisDTO dto)
        {
            Logger.InfoStart();
            ServiceResponse serviceResponse = new();

            try
            {
                int newId = cortesGratisBusiness.Insert(dto);
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
        public ActionResult<ServiceResponse> Update([FromBody] CortesGratisDTO dto)
        {
            Logger.InfoStart();
            ServiceResponse serviceResponse = new();

            try
            {
                int id = cortesGratisBusiness.Update(dto);
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
    }
}
