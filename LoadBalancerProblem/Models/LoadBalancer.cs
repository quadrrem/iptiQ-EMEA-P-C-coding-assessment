using System.Collections.Generic;

namespace LoadBalancerProblem.Models
{
    public class LoadBalancer
    {
        public LoadBalancer()
        {
            registeredProviders = new List<Provider>();
            deregisteredProviders = new List<Provider>();
        }

        /// <summary>
        /// A loadbalancer can be connected to a list of 0 or more registered providers
        /// </summary>
        private IList<Provider> registeredProviders;

        public IList<Provider> RegisteredProviders
        {
            get
            {
                return registeredProviders;
            }
        }

        private IList<Provider> deregisteredProviders;

        public IList<Provider> DeregisteredProviders
        {
            get
            {
                return deregisteredProviders;
            }
        }
    }
}
