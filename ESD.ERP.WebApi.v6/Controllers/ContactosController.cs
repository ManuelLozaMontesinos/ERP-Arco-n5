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
    [Route("api/Contactos")]
    public class ContactosController : AuthorizedController
    {
        private readonly IContactosBusiness contactosBusiness;

        public ContactosController(
            IContactosBusiness contactosBusiness
            )
        {
            this.contactosBusiness = contactosBusiness;
        }
        [HttpPost("GetById")]
        public ActionResult<ServiceResponse> GetById([FromBody] ContactosDTO dto)
        {
            Logger.InfoStart();
            ServiceResponse serviceResponse = new();

            try
            {
                ContactosDTO contactosDTO = contactosBusiness.GetById(dto.Id);
                serviceResponse.Data = contactosDTO;
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
        public ActionResult<ServiceResponse> Search([FromBody] ContactosDTO dto)
        {
            Logger.InfoStart();
            ServiceResponse serviceResponse = new();

            try
            {
                List<ContactosDTO> contactosDTOList = contactosBusiness.Search(dto);
                serviceResponse.Data = contactosDTOList;
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
        public ActionResult<ServiceResponse> SingleSearch([FromBody] ContactosDTO dto)
        {
            Logger.InfoStart();
            ServiceResponse serviceResponse = new();

            try
            {
                ContactosDTO contactosDTO = contactosBusiness.SingleSearch(dto);
                serviceResponse.Data = contactosDTO;
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
        public ActionResult<ServiceResponse> Insert([FromBody] ContactosDTO dto)
        {
            Logger.InfoStart();
            ServiceResponse serviceResponse = new();

            try
            {
                int newId = contactosBusiness.Insert(dto);
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
        public ActionResult<ServiceResponse> Update([FromBody] ContactosDTO dto)
        {
            Logger.InfoStart();
            ServiceResponse serviceResponse = new();

            try
            {
                int id = contactosBusiness.Update(dto);
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
        public ActionResult<ServiceResponse> Delete([FromBody] ContactosDTO dto)
        {
            Logger.InfoStart();
            ServiceResponse serviceResponse = new();

            try
            {
                contactosBusiness.Delete(dto);
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
