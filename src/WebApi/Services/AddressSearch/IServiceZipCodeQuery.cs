using Base.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Services.AddressSearch
{
    public interface IServiceZipCodeQuery
    {
        Task<Address> Search(string zipcode);
    }
}
