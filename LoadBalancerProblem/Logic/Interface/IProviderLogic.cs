using System.Threading.Tasks;

namespace LoadBalancerProblem.Logic.Interface
{
    public interface IProviderLogic
    {
        /// <summary>
        /// Generates a provider with a unique identifier
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns>The asynchronous function returns a new provider with a unique identifier</returns>
        Provider Generate(string identifier);

        /// <summary>
        /// Retrives a provider that matches the passed parameter value
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns>If there is no provider that matches the passed parameter value, the asynchronous function will return null
        /// Otherwise, it returns a matched provider</returns>
        Task<string> Get(string identifier);
    }
}
