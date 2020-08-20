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
        private readonly LoadBalancer _loadBalancer;
        private ILoadBalancerAlgorithm _loadBalancerAlgorithm;
        private readonly IProviderManager _providerManager;

        /// <summary>
        /// Tracks the number of times heartbeat is checked for deregistered providers
        /// </summary>
        readonly IDictionary<string, int> DeregisteredProvidersHeartBeat = new Dictionary<string, int>();

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
            // checks whether the registered providers list already contains maximum number of providers
            if(_loadBalancer.RegisteredProviders.Count == MAX_NUMBER_OF_PROVIDERS)
            {
                return RegistrationStatus.MaxNumberOfProviderExceeded;
            }

            // checks if the to be registered provider is alredy registered
            if (_loadBalancer.RegisteredProviders.Any(x => x.Identifier == providerIdentifier)) 
            {
                return RegistrationStatus.ProviderAlreadyRegistered;
            }

            // ensures that the to be registered provider is not present in the deregistered list
            if (_loadBalancer.DeregisteredProviders != null && _loadBalancer.DeregisteredProviders.Any())
            {
                var provider = _loadBalancer.DeregisteredProviders.FirstOrDefault(x => x.Identifier == providerIdentifier);

                if (provider == null)
                {
                    return RegistrationStatus.RegistrationSuccess;
                }

                // if the to be registered provider is found in the deregistered list, remove it from the deregistered list 
                var result = _loadBalancer.DeregisteredProviders.Remove(provider);

                // if the removal of the provider from the deregistered list fails, return failure message
                if (!result)
                {
                    return RegistrationStatus.RegistrationFailure;
                }
            }

            // registers the provider and returns success
            _loadBalancer.RegisteredProviders.Add(new Provider(providerIdentifier));
            return RegistrationStatus.RegistrationSuccess;
        }

        public DeregistrationStatus Deregister(string providerIdentifier)
        {
            Provider toBeDeregisteredProvider = null;

            // checks if the to be deregistered provider exists in the registered list
            if (_loadBalancer.RegisteredProviders != null && _loadBalancer.RegisteredProviders.Any())
            {
                toBeDeregisteredProvider = _loadBalancer.RegisteredProviders.FirstOrDefault(x => x.Identifier == providerIdentifier);

                // if the to be deregistered provider is not found in the registered list, it returns failure
                if (toBeDeregisteredProvider == null)
                {
                    return DeregistrationStatus.DeregistrationFailure;
                }

                // removes the to be deregistered provider from the registered providers list
                var result = _loadBalancer.RegisteredProviders.Remove(toBeDeregisteredProvider);

                if (!result)
                {
                    return DeregistrationStatus.DeregistrationSuccess;
                }
            }

            // add the to be deregistered provider to the deregistered list with heartbeart count 0
            _loadBalancer.DeregisteredProviders.Add(toBeDeregisteredProvider);
            DeregisteredProvidersHeartBeat.Add(providerIdentifier, 0);
            return DeregistrationStatus.DeregistrationSuccess;
        }

        public string Get()
        {
            if (_loadBalancer.RegisteredProviders == null || !_loadBalancer.RegisteredProviders.Any())
            {
                return null;
            }

            // select a provider to process the request by using one of the algorithms
            var invokedProviderIndex = _loadBalancerAlgorithm.Invoke();
            var invokedProvider = _loadBalancer.RegisteredProviders[invokedProviderIndex];

            // sends the request to the provider manager
            // if the provider is able to process the request, result will be true
            // if the provider's queue has already reached maximum number of requests, result will be false
            var result = _providerManager.Get(invokedProvider);
           
            if (result)
            {
                return invokedProvider.Identifier;
            }

            // update the registered provider list with this provider's status
            invokedProvider.IsActive = false;
            _loadBalancer.RegisteredProviders[invokedProviderIndex] = invokedProvider;

            return null;           
        }

        public void Check()
        {
            // iterates through the registered providers list
            // if there is any inactive provider, it deregisters it
            foreach(var p in _loadBalancer.RegisteredProviders.ToList())
            {
                if(p.IsActive == false)
                {
                    Deregister(p.Identifier);
                }
            }

            // iterates through the deregistered providers list
            // if there is any provider whose heartbeat has been checked 2 times and found active
            // it registers the provider again
            foreach (var p in _loadBalancer.DeregisteredProviders.ToList())
            {
                if (p.IsActive == true)
                {
                    DeregisteredProvidersHeartBeat[p.Identifier]++;
                }

                if (DeregisteredProvidersHeartBeat[p.Identifier] >= 2)
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
