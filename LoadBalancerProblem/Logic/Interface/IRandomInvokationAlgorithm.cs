namespace LoadBalancerProblem.Logic.Interface
{
    public interface IRandomInvokationAlgorithm : ILoadBalancerAlgorithm
    {
        void SetRegisteredProvidersCounter(int counter);
    }
}
