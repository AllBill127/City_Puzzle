using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CityPuzzle.Rest_Services
{
    public interface IRestClient
    {
        Task<T> GetAsync<T>();
    }
}
