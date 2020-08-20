namespace LoadBalancerProblem.Logic.Interface
{
    public interface ILoadBalancerAlgorithm
    {
        /// <summary>
        /// Generic provider invokation algorithm
        /// </summary>
        /// <returns>
        /// Returns invoked provider's index from the registered list
        /// </returns>
        int Invoke();
    }
}