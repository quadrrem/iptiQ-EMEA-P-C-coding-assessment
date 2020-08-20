namespace LoadBalancerProblem.Logic.Interface
{
    public interface IProviderManager
    {
        /// <summary>
        /// Receives requests from the load balancer
        /// </summary>
        /// <param name="provider">
        /// Passed provider is assigned to process the request
        /// </param>
        /// <param name="request">incoming request from the loadBalancer</param>
        /// <returns>
        /// If the requet is assigned to the provider successfully, it return true
        /// Otherwise false
        /// </returns>
        bool Get(Provider provider, string request);
    }
}
