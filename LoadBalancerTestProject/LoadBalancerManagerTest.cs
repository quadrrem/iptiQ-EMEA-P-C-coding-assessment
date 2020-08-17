using LoadBalancerProblem.Logic.Implementation;
using LoadBalancerProblem.Logic.Interface;
using LoadBalancerProblem.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using static LoadBalancerProblem.Logic.Interface.ILoadBalancerManager;

namespace LoadBalancerTestProject
{
    public class LoadBalancerManagerTest
    {
        private readonly ILoadBalancerManager _loadBalancerManager;

        public LoadBalancerManagerTest(ILoadBalancerManager loadBalancerManager)
        {
            _loadBalancerManager = loadBalancerManager;
        }
        [Fact]
        public void Step2_RegisterProvider_Success_With10Providers()
        {
            Assert.Equal(RegistrationStatus.Success, _loadBalancerManager.Register("providerIdentifier1"));
            Assert.Equal(RegistrationStatus.Success, _loadBalancerManager.Register("providerIdentifier2"));
            Assert.Equal(RegistrationStatus.Success, _loadBalancerManager.Register("providerIdentifier3"));
            Assert.Equal(RegistrationStatus.Success, _loadBalancerManager.Register("providerIdentifier4"));
            Assert.Equal(RegistrationStatus.Success, _loadBalancerManager.Register("providerIdentifier5"));
            Assert.Equal(RegistrationStatus.Success, _loadBalancerManager.Register("providerIdentifier6"));
            Assert.Equal(RegistrationStatus.Success, _loadBalancerManager.Register("providerIdentifier7"));
            Assert.Equal(RegistrationStatus.Success, _loadBalancerManager.Register("providerIdentifier8"));
            Assert.Equal(RegistrationStatus.Success, _loadBalancerManager.Register("providerIdentifier9"));
            Assert.Equal(RegistrationStatus.Success, _loadBalancerManager.Register("providerIdentifier10"));
        }

        [Fact]
        public void Step2_RegisterProvider_Failure_With11Providers()
        {
            Assert.Equal(RegistrationStatus.Success, _loadBalancerManager.Register("providerIdentifier1"));
            Assert.Equal(RegistrationStatus.Success, _loadBalancerManager.Register("providerIdentifier2"));
            Assert.Equal(RegistrationStatus.Success, _loadBalancerManager.Register("providerIdentifier3"));
            Assert.Equal(RegistrationStatus.Success, _loadBalancerManager.Register("providerIdentifier4"));
            Assert.Equal(RegistrationStatus.Success, _loadBalancerManager.Register("providerIdentifier5"));
            Assert.Equal(RegistrationStatus.Success, _loadBalancerManager.Register("providerIdentifier6"));
            Assert.Equal(RegistrationStatus.Success, _loadBalancerManager.Register("providerIdentifier7"));
            Assert.Equal(RegistrationStatus.Success, _loadBalancerManager.Register("providerIdentifier8"));
            Assert.Equal(RegistrationStatus.Success, _loadBalancerManager.Register("providerIdentifier9"));
            Assert.Equal(RegistrationStatus.Success, _loadBalancerManager.Register("providerIdentifier10"));

            Assert.Equal(RegistrationStatus.MaxNumberExceed, _loadBalancerManager.Register("providerIdentifier11"));
        }

        [Fact]
        public void Step2_RegisterProvider_Failure_ProviderAlreadyRegistered()
        {
            Assert.Equal(RegistrationStatus.Success, _loadBalancerManager.Register("providerIdentifier1"));
            Assert.Equal(RegistrationStatus.AlreadyRegistered, _loadBalancerManager.Register("providerIdentifier1"));
        }

        [Fact]
        public void Step3_RandomLoadBalancerInvocationTest_Success()
        {
            var providers = new List<string>();

            for(var i = 1; i <= 10; i++)
            {
                var identifier = "providerIdentifier" + i;
                providers.Add(identifier);
                Assert.Equal(RegistrationStatus.Success, _loadBalancerManager.Register(identifier));
            }

            var invokedProvider = _loadBalancerManager.Get();
            Assert.NotNull(invokedProvider);
            Assert.Contains(invokedProvider, providers);
        }

        [Fact]
        public void Step4_RoundRobinLoadBalancerInvocationTest_Success()
        {
            var providers = new List<string>();

            for (var i = 1; i <= 10; i++)
            {
                var identifier = "providerIdentifier" + i;
                providers.Add(identifier);
                Assert.Equal(RegistrationStatus.Success, _loadBalancerManager.Register(identifier));
            }


            Assert.Equal(9, providers.IndexOf(_loadBalancerManager.Get()));
            Assert.Equal(8, providers.IndexOf(_loadBalancerManager.Get()));
            Assert.Equal(7, providers.IndexOf(_loadBalancerManager.Get()));
            Assert.Equal(6, providers.IndexOf(_loadBalancerManager.Get()));
            Assert.Equal(5, providers.IndexOf(_loadBalancerManager.Get()));
            Assert.Equal(4, providers.IndexOf(_loadBalancerManager.Get()));
            Assert.Equal(3, providers.IndexOf(_loadBalancerManager.Get()));
            Assert.Equal(2, providers.IndexOf(_loadBalancerManager.Get()));
            Assert.Equal(1, providers.IndexOf(_loadBalancerManager.Get()));
            Assert.Equal(0, providers.IndexOf(_loadBalancerManager.Get()));

            Assert.Equal(9, providers.IndexOf(_loadBalancerManager.Get()));
            Assert.Equal(8, providers.IndexOf(_loadBalancerManager.Get()));
            Assert.Equal(7, providers.IndexOf(_loadBalancerManager.Get()));
            Assert.Equal(6, providers.IndexOf(_loadBalancerManager.Get()));
            Assert.Equal(5, providers.IndexOf(_loadBalancerManager.Get()));
            Assert.Equal(4, providers.IndexOf(_loadBalancerManager.Get()));
            Assert.Equal(3, providers.IndexOf(_loadBalancerManager.Get()));
            Assert.Equal(2, providers.IndexOf(_loadBalancerManager.Get()));
            Assert.Equal(1, providers.IndexOf(_loadBalancerManager.Get()));
            Assert.Equal(0, providers.IndexOf(_loadBalancerManager.Get()));
        }

        [Fact]
        public void Step5_DeregisterProvider_Success()
        {
            Assert.Equal(RegistrationStatus.Success, _loadBalancerManager.Register("providerIdentifier1"));
            Assert.Equal(DeregistrationStatus.DeregistrationSuccess, _loadBalancerManager.Deregister("providerIdentifier1"));

            Assert.Empty(_loadBalancerManager.GetLoadBalancer().Providers);
        }

        [Fact]
        public void Step5_DeregisterProvider_Failure()
        {
            Assert.Equal(RegistrationStatus.Success, _loadBalancerManager.Register("providerIdentifier1"));
            Assert.Equal(DeregistrationStatus.DeregistrationFailure, _loadBalancerManager.Deregister("providerIdentifier11"));

            Assert.Equal(1, _loadBalancerManager.GetLoadBalancer().Providers.Count);
        }
    }
}
