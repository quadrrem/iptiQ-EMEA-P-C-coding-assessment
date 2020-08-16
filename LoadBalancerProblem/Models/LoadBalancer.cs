using System.Collections.Generic;

namespace LoadBalancerProblem.Models
{
    public class LoadBalancer
    {
        public LoadBalancer()
        {
            providers = new List<Provider>();
        }

        /// <summary>
        /// A loadbalancer can be connected to a list of 0 or more registered providers
        /// </summary>
        private IList<Provider> providers;

        public IList<Provider> Providers
        {
            get
            {
                return providers;
            }
            set
            {
                providers = value;
            }
        }
    }
}
