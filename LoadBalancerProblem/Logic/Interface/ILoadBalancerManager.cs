using LoadBalancerProblem.Models;

namespace LoadBalancerProblem.Logic.Interface
{
    public interface ILoadBalancerManager
    {
        /// <summary>
        /// Maximum number of providers that a loadBalancer can connect to
        /// </summary>
        const int MAX_NUMBER_OF_PROVIDERS = 10;

        /// <summary>
        /// Registers the given provider with the loadBalancer using provider's identifier.
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns>
        /// If the loadBalancer is already connected to the maximum number of providers, the function returns MaxNumberOfProviderExceeded
        /// If the provider is already registered, the function returns ProviderAlreadyRegistered
        /// Besides, the above two reasons if for any other reason registration fails, the function returns RegistrationFailure
        /// If registration of the provider succeeds, the function returns RegistrationSuccess
        /// </returns>
        RegistrationStatus Register(string providerIdentifier);

        /// <summary>
        /// Deregisters a provider from the loadBalancer's registered providers list.
        /// </summary>
        /// <param name="providerIdentifier"></param>
        /// <returns>
        /// If the deregistration succeeds, the function returns DeregistrationSuccess.
        /// Otherwise, it returns DeregistrationFailure
        /// </returns>
        DeregistrationStatus Deregister(string providerIdentifier);

        /// <summary>
        /// Receives requests 
        /// </summary>
        /// <returns>
        /// Returns provider's identifier which has agreed to process the request
        /// Otherwise it returns null to confirm there is no provider that can handle the request
        /// </returns>
        string Get();

        /// <summary>
        /// Returns the loadBalancer instance with all registered and deregistered providers list
        /// </summary>
        /// <returns></returns>
        LoadBalancer GetLoadBalancer();

        /// <summary>
        /// Checks heartbeat of registered and deregistered providers.
        /// If any registered provider is found inactive, the function deregisters it
        /// and if any degistered provider is found active consecutively two times, it registers back that provider.
        /// </summary>
        void Check();

        /// <summary>
        /// Sets provider invokation algorithm(i.e., random invakation algorithm and round robin algorithm) in the run time
        /// </summary>
        /// <param name="loadBalancerAlgorithm"></param>
        void SetLoadBalancerAlgorithm(ILoadBalancerAlgorithm loadBalancerAlgorithm);
    }
}
