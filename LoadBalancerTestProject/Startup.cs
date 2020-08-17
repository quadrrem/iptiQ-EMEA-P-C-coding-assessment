using LoadBalancerProblem;
using LoadBalancerProblem.Logic.Implementation;
using LoadBalancerProblem.Logic.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoadBalancerTestProject
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<Provider>();
            services.AddScoped<ILoadBalancerManager, LoadBalancerManager>();
            services.AddScoped<ILoadBalancerAlgorithm, RandomLoadBalancerAlgorithm>();
            services.AddScoped<ILoadBalancerAlgorithm, RoundRobinLoadBalancerAlgorithm>();
        }
    }
}
