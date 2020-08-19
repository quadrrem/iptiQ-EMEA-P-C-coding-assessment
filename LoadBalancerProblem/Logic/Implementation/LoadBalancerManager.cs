using LoadBalancerProblem.Logic.Interface;
using LoadBalancerProblem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static LoadBalancerProblem.Logic.Interface.ILoadBalancerManager;

namespace LoadBalancerProblem.Logic.Implementation
{
    public class LoadBalancerManager : ILoadBalancerManager
    {
        private LoadBalancer loadBalancer = new LoadBalancer();
        private readonly ILoadBalancerAlgorithm _loadBalancerAlgorithm;
        IDictionary<string, int> ProviderHeartBeat = new Dictionary<string, int>();

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
            if(loadBalancer.RegisteredProviders.Count == MAX_NUMBER_OF_PROVIDERS)
            {
                return RegistrationStatus.MaxNumberOfProviderExceeded;
            }

            if (loadBalancer.RegisteredProviders.Any(x => x.Identifier == providerIdentifier)) 
            {
                return RegistrationStatus.ProviderAlreadyRegistered;
            }

            loadBalancer.RegisteredProviders.Add(new Provider(providerIdentifier));

            if(loadBalancer.DeregisteredProviders == null || !loadBalancer.DeregisteredProviders.Any())
            {
                return RegistrationStatus.RegistrationSuccess;
            }

            // check if the provider exists in the deregistered list 
            var provider = loadBalancer.DeregisteredProviders.FirstOrDefault(x => x.Identifier == providerIdentifier);

            if(provider == null)
            {
                return RegistrationStatus.RegistrationSuccess;
            }

            var result = loadBalancer.DeregisteredProviders.Remove(provider);

            if (!result)
            {
                return RegistrationStatus.RegistrationFailure;
            } 

            return RegistrationStatus.RegistrationSuccess;
        }

        public DeregistrationStatus Deregister(string providerIdentifier)
        {
            var provider = loadBalancer.RegisteredProviders.FirstOrDefault(x => x.Identifier == providerIdentifier);

            if (provider == null)
            {
                return DeregistrationStatus.DeregistrationFailure;
            }

            loadBalancer.DeregisteredProviders.Add(provider);
            ProviderHeartBeat.Add(providerIdentifier, 0);

            if (loadBalancer.RegisteredProviders == null || !loadBalancer.RegisteredProviders.Any())
            {
                return DeregistrationStatus.DeregistrationSuccess;
            }

            var result = loadBalancer.RegisteredProviders.Remove(provider);

            if (result)
            {
                return DeregistrationStatus.DeregistrationSuccess;
            }

            return DeregistrationStatus.DeregistrationFailure;

        }

        public string Get()
        {
            return loadBalancer.RegisteredProviders[_loadBalancerAlgorithm.Invoke()].Identifier;
        }

        public void Check()
        {
            foreach(var p in loadBalancer.RegisteredProviders.ToList())
            {
                if(p.IsActive == false)
                {
                    Deregister(p.Identifier);
                }
            }

            foreach (var p in loadBalancer.DeregisteredProviders.ToList())
            {
                if (p.IsActive == true)
                {
                    ProviderHeartBeat[p.Identifier]++;
                }

                if (ProviderHeartBeat[p.Identifier] >= 2)
                {
                    Register(p.Identifier);
                }

            }
        }

        public void PeriodicCheck()
        {
            var thread = new Thread(new ThreadStart(Check));

            // Start the thread.
            thread.Start();

            if (thread.IsAlive)
            {
                Console.WriteLine("The Main() thread calls this after "
                    + "starting the new InstanceCaller thread.");
            }
        }
    }
}
