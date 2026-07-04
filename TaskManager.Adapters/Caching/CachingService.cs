using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Adapters.Caching
{
    public class CachingService : ICachingService
    {
        public Task<string> GetAsync(string Key)
        {
            throw new NotImplementedException();
        }

        public Task SetAsync(string Key, string value)
        {
            throw new NotImplementedException();
        }
    }
}
