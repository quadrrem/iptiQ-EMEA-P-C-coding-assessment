using LoadBalancerProblem.Logic.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoadBalancerProblem.Logic.Implementation
{
    public class RoundRobinLoadBalancerAlgorithm : ILoadBalancerAlgorithm
    {
        int counter = ILoadBalancerManager.MAX_NUMBER_OF_PROVIDERS;
        public int Invoke()
        {
            counter = counter == 0 ? ILoadBalancerManager.MAX_NUMBER_OF_PROVIDERS - 1 : counter - 1;
            return counter;
        }
    }
}
