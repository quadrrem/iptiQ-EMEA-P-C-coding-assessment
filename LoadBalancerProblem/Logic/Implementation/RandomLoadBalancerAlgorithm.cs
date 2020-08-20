using LoadBalancerProblem.Logic.Interface;
using System;

namespace LoadBalancerProblem.Logic.Implementation
{
    public class RandomLoadBalancerAlgorithm : IRandomInvokationAlgorithm
    {
        private int registeredProvidersCount;

        public void SetRegisteredProvidersCounter(int count)
        {
            registeredProvidersCount = count;
        }
        public int Invoke()
        {
            var random = new Random();
            return random.Next(0, registeredProvidersCount - 1);
        }
    }
}
