using Microsoft.Extensions.DependencyInjection;
using LoadBalancerProblem.Logic.Implementation;
using LoadBalancerProblem.Logic.Interface;
using LoadBalancerProblem.Models;

namespace LoadBalancerProblem
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<LoadBalancer>();
            services.AddScoped<ILoadBalancerManager, LoadBalancerManager>();
            services.AddScoped<IProviderManager, ProviderManager>();
            services.AddScoped<IRandomInvokationAlgorithm, RandomLoadBalancerAlgorithm>();
            services.AddScoped<IRoundRobinAlgorithm, RoundRobinLoadBalancerAlgorithm>();
        }
    }
}
