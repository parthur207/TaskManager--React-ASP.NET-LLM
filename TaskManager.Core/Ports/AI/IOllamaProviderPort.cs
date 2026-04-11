using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.ResposePattern;

namespace TaskManager.Core.Ports.AI
{
    public interface IOllamaProviderPort
    {
        Task<ResponseModel<object>> GenerateAsync(string prompt);
    }
}
