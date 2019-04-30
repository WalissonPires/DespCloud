using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Services.AddressSearch.Postmon
{
    public class EstadoInfo
    {
        public string area_km2 { get; set; }
        public int codigo_ibge { get; set; }
        public string nome { get; set; }
    }

    public class CidadeInfo
    {
        public string area_km2 { get; set; }
        public int codigo_ibge { get; set; }
    }

    public class ZipcodeResult
    {
        public string bairro { get; set; }
        public string cidade { get; set; }
        public string logradouro { get; set; }
        public EstadoInfo estado_info { get; set; }
        public string cep { get; set; }
        public CidadeInfo cidade_info { get; set; }
        public string estado { get; set; }
    }
}
