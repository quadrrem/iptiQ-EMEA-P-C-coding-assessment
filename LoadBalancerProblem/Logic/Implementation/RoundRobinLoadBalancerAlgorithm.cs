using LoadBalancerProblem.Logic.Interface;
using System;

namespace LoadBalancerProblem.Logic.Implementation
{
    public class RoundRobinLoadBalancerAlgorithm : IRoundRobinAlgorithm
    {
        private int registeredProvidersCount;
        private int counter;

        public void SetRegisteredProvidersCounter(int count)
        {
            registeredProvidersCount = counter = count;
        }
        public int Invoke()
        {
            registeredProvidersCount = registeredProvidersCount == 0 ? counter - 1 : registeredProvidersCount - 1;
            return registeredProvidersCount;
        }
    }
}
