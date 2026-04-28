using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.DI
{
    public static class DependencyInjection_Adapters
    {
        public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
        {
            /*services.AddHttpClient<IExchangeRateProvider, EcbExchangeRateProvider>();

            services.AddSingleton<, >();*/

            return services;
        }
    }
}
