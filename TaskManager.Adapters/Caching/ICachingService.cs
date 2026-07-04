using Microsoft.EntityFrameworkCore.ValueGeneration;
using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Adapters.Caching
{
    public interface ICachingService
    {

        Task SetAsync(string Key, string value);
        Task<string> GetAsync(string Key);

    }
}
