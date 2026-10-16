using ESD.ERP.DTO;
using ESD.ERP.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace ESD.ERP.Web.Controllers
{
   public class HomeController : AuthorizedController
   {
      public IActionResult Index()
      {
         string cid = Request.Query["cid"];

         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(cid);

         UsuarioDTO usuarioDTO = ObtenerUsuarioDTO();

         ViewBag.Empresa = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaDTO.NombreComercial;
         ViewBag.Usuario = usuarioDTO.UsuarioLogon;
         ViewBag.Periodo = periodoEmpresaSedeDTO.PeriodoDTO.Codigo;
         ViewBag.Sede = periodoEmpresaSedeDTO.EmpresaSedeDTO.Nombre;

         ViewBag.cid = cid;

         return View();
      }
   }
}