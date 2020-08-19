using LoadBalancerProblem.Logic.Interface;
using System;

namespace LoadBalancerProblem.Logic.Implementation
{
    public class RoundRobinLoadBalancerAlgorithm : IRoundRobinAlgorithm
    {
        int counter = ILoadBalancerManager.MAX_NUMBER_OF_PROVIDERS;
        public int Invoke()
        {
            Console.WriteLine("Round robin algorithm invoked");
            counter = counter == 0 ? ILoadBalancerManager.MAX_NUMBER_OF_PROVIDERS - 1 : counter - 1;
            return counter;
        }
    }
}
