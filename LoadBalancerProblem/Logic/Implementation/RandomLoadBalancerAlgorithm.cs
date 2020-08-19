using LoadBalancerProblem.Logic.Interface;
using System;

namespace LoadBalancerProblem.Logic.Implementation
{
    public class RandomLoadBalancerAlgorithm : ILoadBalancerAlgorithm
    {
        public int Invoke()
        {
            var random = new Random();
            return random.Next(0, ILoadBalancerManager.MAX_NUMBER_OF_PROVIDERS - 1);
        }
    }
}
