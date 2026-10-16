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
    [Route("api/ListaPrecioVendedor")]
    public class ListaPrecioVendedorController : AuthorizedController
    {
        private readonly IListaPrecioVendedorBusiness listaPrecioVendedorBusiness;

        public ListaPrecioVendedorController(
            IListaPrecioVendedorBusiness listaPrecioVendedorBusiness
            )
        {
            this.listaPrecioVendedorBusiness = listaPrecioVendedorBusiness;
        }
        [HttpPost("GetById")]
        public ActionResult<ServiceResponse> GetById([FromBody] ListaPrecioVendedorDTO dto)
        {
            Logger.InfoStart();
            ServiceResponse serviceResponse = new();

            try
            {
                ListaPrecioVendedorDTO ListaPrecioVendedorDTO = listaPrecioVendedorBusiness.GetById(dto.Id);
                serviceResponse.Data = ListaPrecioVendedorDTO;
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
        public ActionResult<ServiceResponse> Search([FromBody] ListaPrecioVendedorDTO dto)
        {
            Logger.InfoStart();
            ServiceResponse serviceResponse = new();

            try
            {
                List<ListaPrecioVendedorDTO> listaPrecioProveedorDTOList = listaPrecioVendedorBusiness.Search(dto);
                serviceResponse.Data = listaPrecioProveedorDTOList;
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
        public ActionResult<ServiceResponse> SingleSearch([FromBody] ListaPrecioVendedorDTO dto)
        {
            Logger.InfoStart();
            ServiceResponse serviceResponse = new();

            try
            {
                ListaPrecioVendedorDTO ListaPrecioVendedorDTO = listaPrecioVendedorBusiness.SingleSearch(dto);
                serviceResponse.Data = ListaPrecioVendedorDTO;
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
        public ActionResult<ServiceResponse> Insert([FromBody] ListaPrecioVendedorDTO dto)
        {
            Logger.InfoStart();
            ServiceResponse serviceResponse = new();

            try
            {
                int newId = listaPrecioVendedorBusiness.Insert(dto);
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
        public ActionResult<ServiceResponse> Update([FromBody] ListaPrecioVendedorDTO dto)
        {
            Logger.InfoStart();
            ServiceResponse serviceResponse = new();

            try
            {
                int id = listaPrecioVendedorBusiness.Update(dto);
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
        public ActionResult<ServiceResponse> Delete([FromBody] ListaPrecioVendedorDTO dto)
        {
            Logger.InfoStart();
            ServiceResponse serviceResponse = new();

            try
            {
                listaPrecioVendedorBusiness.Delete(dto);
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
        [HttpPost("ActualizarListaPrecioVendedorId")]
        public ActionResult<ServiceResponse> ActualizarListaPrecioVendedorId([FromBody] ListaPrecioVendedorDTO dto)
        {
            Logger.InfoStart();
            ServiceResponse serviceResponse = new();

            try
            {
                listaPrecioVendedorBusiness.ActualizarListaPrecioVendedorId(dto);
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
