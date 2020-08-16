using LoadBalancerProblem.Logic.Interface;
using LoadBalancerProblem.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LoadBalancerProblem.Logic.Implementation
{
    public class ProviderLogic : IProviderLogic
    {
        public Provider Generate(string identifier)
        {
            return new Provider(identifier);
        }

        public async Task<string> Get(string identifier)
        {
            return Generate(identifier).Identifier;
        }
    }
}
