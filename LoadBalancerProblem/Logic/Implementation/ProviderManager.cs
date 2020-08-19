using LoadBalancerProblem.Logic.Interface;

namespace LoadBalancerProblem.Logic.Implementation
{
    public class ProviderManager : IProviderManager
    {
        public bool Get(Provider provider)
        {
            if(provider.Requests.Count < provider.Limit)
            {
                provider.Requests.Enqueue("request");
                return true;
            }
            else
            {
                provider.IsActive = false;
                return false;
            }
        }
    }
}
