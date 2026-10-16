namespace ESD.ERP.IntegrationApi.Models
{
   public class RucDTO
   {
      public string RUC { get; set; }
      public string RazonSocial { get; set; }
      public string EstadoNombre { get; set; }
      public bool EsActivo { get; set; }
      public bool EsHabido { get; set; }
      public string CondicionDomicilio { get; set; }
      public string UbigeoDepartamento { get; set; }
      public string UbigeoProvincia { get; set; }
      public string UbigeoDistrito { get; set; }
      public string TipoVia { get; set; }
      public string NombreVia { get; set; }
      public string TipoZona { get; set; }
      public string NombreZona { get; set; }
      public string Numero { get; set; }
      public string Interior { get; set; }
      public string Lote { get; set; }
      public string Departamento { get; set; }
      public string Manzana { get; set; }
      public string Kilometro { get; set; }
   }
}