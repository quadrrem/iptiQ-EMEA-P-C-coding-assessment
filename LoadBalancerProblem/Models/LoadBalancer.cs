using System.Collections.Generic;

namespace LoadBalancerProblem.Models
{
    public class LoadBalancer
    {
        /// <summary>
        /// A loadBalancer is initialized with empty lists of registered and deregistered lists
        /// </summary>
        public LoadBalancer()
        {
            registeredProviders = new List<Provider>();
            deregisteredProviders = new List<Provider>();
        }

        /// <summary>
        /// A list of providers that can receive requests from the loadBalancer
        /// </summary>
        private IList<Provider> registeredProviders;

        public IList<Provider> RegisteredProviders
        {
            get
            {
                return registeredProviders;
            }
        }

        /// <summary>
        /// A list of providers that are busy to receive requests from the loadBalancer
        /// </summary>
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
