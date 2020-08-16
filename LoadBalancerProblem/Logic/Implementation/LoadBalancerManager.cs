using LoadBalancerProblem.Logic.Interface;
using LoadBalancerProblem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LoadBalancerProblem.Logic.Interface.ILoadBalancerManager;

namespace LoadBalancerProblem.Logic.Implementation
{
    public class LoadBalancerManager : ILoadBalancerManager
    {
        private LoadBalancer loadBalancer = new LoadBalancer();
        private readonly ILoadBalancerAlgorithm _loadBalancerAlgorithm;

        public LoadBalancerManager(ILoadBalancerAlgorithm loadBalancerAlgorithm)
        {
            _loadBalancerAlgorithm = loadBalancerAlgorithm;
        }

        public LoadBalancer GetLoadBalancer()
        {
            return loadBalancer;
        }

        public RegistrationStatus Register(string providerIdentifier)
        {
            if(loadBalancer.Providers.Count == ILoadBalancerManager.MAX_NUMBER_OF_PROVIDERS)
            {
                return RegistrationStatus.MaxNumberExceed;
            }

            if (loadBalancer.Providers.Any(x => x.Identifier == providerIdentifier)) 
            {
                return RegistrationStatus.AlreadyRegistered;
            }

            loadBalancer.Providers.Add(new Provider(providerIdentifier));

            return RegistrationStatus.Success;
        }

        public DeregistrationStatus Deregister(string providerIdentifier)
        {
            var provider = loadBalancer.Providers.FirstOrDefault(x => x.Identifier == providerIdentifier);

            if (provider == null)
            {
                return DeregistrationStatus.DeregistrationFailure;
            }

            var result = loadBalancer.Providers.Remove(provider);

            if (result)
            {
                return DeregistrationStatus.DeregistrationSuccess;
            }

            return DeregistrationStatus.DeregistrationFailure;

        }

        public string Get()
        {
            return loadBalancer.Providers[_loadBalancerAlgorithm.Invoke()].Identifier;
        }
    }
}
