using Xunit;
using LoadBalancerProblem;

namespace LoadBalancerTestProject
{
    public class ProviderTest
    {
        [Fact]
        public void Step1_GenerateProvider_Success()
        {
            var identifier = "testProvider";
            Assert.Equal(identifier, new Provider(identifier).Identifier);
        }

        [Fact]
        public void Step1_GennerateProvider_Failure()
        {
            var identifier = "testProvider";
            Assert.NotEqual("testProvider1", new Provider(identifier).Identifier);
        }
    }
}
