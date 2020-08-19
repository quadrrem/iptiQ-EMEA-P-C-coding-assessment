using LoadBalancerProblem.Logic.Implementation;
using LoadBalancerProblem.Logic.Interface;
using LoadBalancerProblem.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoadBalancerProblem
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ILoadBalancerManager, LoadBalancerManager>();
            services.AddScoped<ILoadBalancerAlgorithm, RandomLoadBalancerAlgorithm>();
            services.AddScoped<ILoadBalancerAlgorithm, RoundRobinLoadBalancerAlgorithm>();
        }
    }
}
