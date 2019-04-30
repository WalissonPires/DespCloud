using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Base.Domain.Entities;
using Newtonsoft.Json;

namespace WebApi.Services.AddressSearch.Postmon
{
    public class PostmonService : IServiceZipCodeQuery
    {
        public async Task<Address> Search(string zipcode)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://api.postmon.com.br/v1/cep/" + zipcode);

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string requestContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<ZipcodeResult>(requestContent);

                var result = new Address
                {
                    ZipCode = zipcode,
                    Number = null,
                    Street = responseData.logradouro,
                    DistrictId = 0,
                    DistrictName = responseData.bairro,
                    CityId = responseData.cidade_info.codigo_ibge,
                    CityName = responseData.cidade,
                    CountyId = responseData.estado_info.codigo_ibge,
                    CountyName = responseData.estado_info.nome,
                    CountyInitials = responseData.estado
                };

                return result;
            }

            return null;
        }
    }
}
