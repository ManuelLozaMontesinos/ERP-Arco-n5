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
    [Route("api/Puesto")]
    public class PuestoController : AuthorizedController
    {
        private readonly IPuestoBusiness puestoBusiness;

        public PuestoController(
            IPuestoBusiness puestoBusiness
            )
        {
            this.puestoBusiness = puestoBusiness;
        }
        [HttpPost("GetById")]
        public ActionResult<ServiceResponse> GetById([FromBody] PuestoDTO dto)
        {
            Logger.InfoStart();
            ServiceResponse serviceResponse = new();

            try
            {
                PuestoDTO puestoDTO = puestoBusiness.GetById(dto.Id);
                serviceResponse.Data = puestoDTO;
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
        public ActionResult<ServiceResponse> Search([FromBody] PuestoDTO dto)
        {
            Logger.InfoStart();
            ServiceResponse serviceResponse = new();

            try
            {
                List<PuestoDTO> puestoDTOList = puestoBusiness.Search(dto);
                serviceResponse.Data = puestoDTOList;
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
        public ActionResult<ServiceResponse> SingleSearch([FromBody] PuestoDTO dto)
        {
            Logger.InfoStart();
            ServiceResponse serviceResponse = new();

            try
            {
                PuestoDTO puestoDTO = puestoBusiness.SingleSearch(dto);
                serviceResponse.Data = puestoDTO;
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
        public ActionResult<ServiceResponse> Insert([FromBody] PuestoDTO dto)
        {
            Logger.InfoStart();
            ServiceResponse serviceResponse = new();

            try
            {
                int newId = puestoBusiness.Insert(dto);
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
        public ActionResult<ServiceResponse> Update([FromBody] PuestoDTO dto)
        {
            Logger.InfoStart();
            ServiceResponse serviceResponse = new();

            try
            {
                int id = puestoBusiness.Update(dto);
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
        public ActionResult<ServiceResponse> Delete([FromBody] PuestoDTO dto)
        {
            Logger.InfoStart();
            ServiceResponse serviceResponse = new();

            try
            {
                puestoBusiness.Delete(dto);
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
