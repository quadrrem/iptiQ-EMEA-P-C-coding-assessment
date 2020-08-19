using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static LoadBalancerProblem.Logic.Interface.ILoadBalancerManager;
using LoadBalancerProblem.Logic.Interface;
using LoadBalancerProblem.Models;

namespace LoadBalancerProblem.Logic.Implementation
{
    public class LoadBalancerManager : ILoadBalancerManager
    {
        private LoadBalancer _loadBalancer;
        private ILoadBalancerAlgorithm _loadBalancerAlgorithm;
        private readonly IProviderManager _providerManager;

        IDictionary<string, int> ProviderHeartBeat = new Dictionary<string, int>();

        public LoadBalancerManager(
            IProviderManager providerManager,
            LoadBalancer loadBalancer)
        {
            _providerManager = providerManager;
            _loadBalancer = loadBalancer;
        }
        public void SetLoadBalancerAlgorithm(ILoadBalancerAlgorithm loadBalancerAlgorithm)
        {
            _loadBalancerAlgorithm = loadBalancerAlgorithm;
        }

        public LoadBalancer GetLoadBalancer()
        {
            return _loadBalancer;
        }

        public RegistrationStatus Register(string providerIdentifier)
        {
            if(_loadBalancer.RegisteredProviders.Count == MAX_NUMBER_OF_PROVIDERS)
            {
                return RegistrationStatus.MaxNumberOfProviderExceeded;
            }

            if (_loadBalancer.RegisteredProviders.Any(x => x.Identifier == providerIdentifier)) 
            {
                return RegistrationStatus.ProviderAlreadyRegistered;
            }

            _loadBalancer.RegisteredProviders.Add(new Provider(providerIdentifier));

            if(_loadBalancer.DeregisteredProviders == null || !_loadBalancer.DeregisteredProviders.Any())
            {
                return RegistrationStatus.RegistrationSuccess;
            }

            // check if the provider exists in the deregistered list 
            var provider = _loadBalancer.DeregisteredProviders.FirstOrDefault(x => x.Identifier == providerIdentifier);

            if(provider == null)
            {
                return RegistrationStatus.RegistrationSuccess;
            }

            var result = _loadBalancer.DeregisteredProviders.Remove(provider);

            if (!result)
            {
                return RegistrationStatus.RegistrationFailure;
            } 

            return RegistrationStatus.RegistrationSuccess;
        }

        public DeregistrationStatus Deregister(string providerIdentifier)
        {
            var provider = _loadBalancer.RegisteredProviders.FirstOrDefault(x => x.Identifier == providerIdentifier);

            if (provider == null)
            {
                return DeregistrationStatus.DeregistrationFailure;
            }

            _loadBalancer.DeregisteredProviders.Add(provider);
            ProviderHeartBeat.Add(providerIdentifier, 0);

            if (_loadBalancer.RegisteredProviders == null || !_loadBalancer.RegisteredProviders.Any())
            {
                return DeregistrationStatus.DeregistrationSuccess;
            }

            var result = _loadBalancer.RegisteredProviders.Remove(provider);

            if (result)
            {
                return DeregistrationStatus.DeregistrationSuccess;
            }

            return DeregistrationStatus.DeregistrationFailure;

        }

        public string Get()
        {
            if (_loadBalancer.RegisteredProviders == null || !_loadBalancer.RegisteredProviders.Any())
            {
                return null;
            }

            var invokedProviderIndex = _loadBalancerAlgorithm.Invoke();
            var invokedProvider = _loadBalancer.RegisteredProviders[invokedProviderIndex];

            var result = _providerManager.Get(invokedProvider);

            if (result)
            {
                return invokedProvider.Identifier;
            }

            invokedProvider.IsActive = false;
            _loadBalancer.RegisteredProviders[invokedProviderIndex] = invokedProvider;

            return null;           
        }

        public void Check()
        {
            foreach(var p in _loadBalancer.RegisteredProviders.ToList())
            {
                if(p.IsActive == false)
                {
                    Deregister(p.Identifier);
                }
            }

            foreach (var p in _loadBalancer.DeregisteredProviders.ToList())
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
