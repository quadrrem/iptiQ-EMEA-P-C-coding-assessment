using System;
using System.Collections.Generic;
using System.Text;

namespace LoadBalancerProblem.Logic.Interface
{
    public interface ILoadBalancerAlgorithm
    {
        int Invoke();
    }
}