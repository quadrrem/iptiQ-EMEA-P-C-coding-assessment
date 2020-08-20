namespace LoadBalancerProblem.Logic.Interface
{
    public interface IRoundRobinAlgorithm : ILoadBalancerAlgorithm
    {
        void SetRegisteredProvidersCounter(int counter);
    }
}
