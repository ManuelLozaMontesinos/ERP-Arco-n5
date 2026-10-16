using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service.DTO;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.MK.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.MK.Controllers
{
    [Area("MK")]
    [Route("[controller]")]
    public class ContactoController : AuthorizedController
    {
        private readonly IProveedorProxy proveedorProxy;
        private readonly ITipoNaturalezaJuridicaProxy tipoNaturalezaJuridicaProxy;
        private readonly ITipoDocumentoIdentidadProxy tipoDocumentoIdentidadProxy;
        private readonly IDepartamentoProxy departamentoProxy;
        private readonly IProvinciaProxy provinciaProxy;
        private readonly IDistritoProxy distritoProxy;
        private readonly IIntegrationApiProxy integrationApiProxy;
        private readonly ITipoProfesionOficioProxy tipoProfesionOficioProxy;
        private readonly ITipoVentaClienteProxy tipoVentaClienteProxy;
        private readonly IEmpresaSedeProxy empresaSedeProxy;
        private readonly IProveedorRedSocialProxy clienteRedSocialProxy;
        private readonly IRedSocialProxy redSocialProxy;
        private readonly ITipoViaProxy tipoViaProxy;
        private readonly ITipoZonaProxy tipoZonaProxy;
        private readonly IOrigenRegistroProxy origenRegistroProxy;
        private readonly ITipoProveedorProxy tipoProveedorProxy;

        public ContactoController(
           IProveedorProxy proveedorProxy,
           ITipoNaturalezaJuridicaProxy tipoNaturalezaJuridicaProxy,
           ITipoDocumentoIdentidadProxy tipoDocumentoIdentidadProxy,
           IDepartamentoProxy departamentoProxy,
           IProvinciaProxy provinciaProxy,
           IDistritoProxy distritoProxy,
           IIntegrationApiProxy integrationApiProxy,
           ITipoProfesionOficioProxy tipoProfesionOficioProxy,
           ITipoVentaClienteProxy tipoVentaClienteProxy,
           IEmpresaSedeProxy empresaSedeProxy,
           IProveedorRedSocialProxy clienteRedSocialProxy,
           IRedSocialProxy redSocialProxy,
           ITipoViaProxy tipoViaProxy,
           ITipoZonaProxy tipoZonaProxy,
           IOrigenRegistroProxy origenRegistroProxy,
           ITipoProveedorProxy tipoProveedorProxy
           )
        {
            this.proveedorProxy = proveedorProxy;
            this.tipoNaturalezaJuridicaProxy = tipoNaturalezaJuridicaProxy;
            this.tipoDocumentoIdentidadProxy = tipoDocumentoIdentidadProxy;
            this.departamentoProxy = departamentoProxy;
            this.provinciaProxy = provinciaProxy;
            this.distritoProxy = distritoProxy;
            this.integrationApiProxy = integrationApiProxy;
            this.tipoProfesionOficioProxy = tipoProfesionOficioProxy;
            this.tipoVentaClienteProxy = tipoVentaClienteProxy;
            this.empresaSedeProxy = empresaSedeProxy;
            this.clienteRedSocialProxy = clienteRedSocialProxy;
            this.redSocialProxy = redSocialProxy;
            this.tipoViaProxy = tipoViaProxy;
            this.tipoZonaProxy = tipoZonaProxy;
            this.origenRegistroProxy = origenRegistroProxy;
            this.tipoProveedorProxy = tipoProveedorProxy;
        }


    }
}