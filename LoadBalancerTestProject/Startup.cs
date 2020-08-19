using LoadBalancerProblem;
using LoadBalancerProblem.Logic.Implementation;
using LoadBalancerProblem.Logic.Interface;
using LoadBalancerProblem.Models;
using Microsoft.Extensions.DependencyInjection;

namespace LoadBalancerTestProject
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<Provider>();
            services.AddScoped<LoadBalancer>();
            services.AddScoped<ILoadBalancerManager, LoadBalancerManager>();
            services.AddScoped<IProviderManager, ProviderManager>();
            services.AddScoped<IRandomInvokationAlgorithm, RandomLoadBalancerAlgorithm>();
            services.AddScoped<IRoundRobinAlgorithm, RoundRobinLoadBalancerAlgorithm>();
        }
    }
}
