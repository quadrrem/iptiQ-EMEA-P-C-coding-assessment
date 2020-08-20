using LoadBalancerProblem.Logic.Implementation;
using LoadBalancerProblem.Logic.Interface;
using System;
using System.Threading.Tasks;
using Xunit;
using LoadBalancerProblem.Models;
using System.Collections.Generic;
using LoadBalancerProblem;

namespace LoadBalancerTestProject
{
    public class ProviderTest
    {
        [Fact]
        public void Step1_Test_Success()
        {
            var identifier = "testProvider";
            Assert.Equal(identifier, new Provider(identifier).Identifier);
        }

        [Fact]
        public void Step1_Test_Failure()
        {
            var identifier = "testProvider";
            Assert.NotEqual("testProvider1", new Provider(identifier).Identifier);
        }
    }
}
