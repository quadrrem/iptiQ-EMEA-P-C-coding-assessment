using LoadBalancerProblem.Logic.Implementation;
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
        [Fact]
        public void Step2_RegisterProvider_Success_With10Providers()
        {
            var loadBalancerAlgorithm = new RandomLoadBalancerAlgorithm();
            var loadBalancerManager = new LoadBalancerManager(loadBalancerAlgorithm);

            Assert.Equal(RegistrationStatus.Success, loadBalancerManager.Register("providerIdentifier1"));
            Assert.Equal(RegistrationStatus.Success, loadBalancerManager.Register("providerIdentifier2"));
            Assert.Equal(RegistrationStatus.Success, loadBalancerManager.Register("providerIdentifier3"));
            Assert.Equal(RegistrationStatus.Success, loadBalancerManager.Register("providerIdentifier4"));
            Assert.Equal(RegistrationStatus.Success, loadBalancerManager.Register("providerIdentifier5"));
            Assert.Equal(RegistrationStatus.Success, loadBalancerManager.Register("providerIdentifier6"));
            Assert.Equal(RegistrationStatus.Success, loadBalancerManager.Register("providerIdentifier7"));
            Assert.Equal(RegistrationStatus.Success, loadBalancerManager.Register("providerIdentifier8"));
            Assert.Equal(RegistrationStatus.Success, loadBalancerManager.Register("providerIdentifier9"));
            Assert.Equal(RegistrationStatus.Success, loadBalancerManager.Register("providerIdentifier10"));
        }

        [Fact]
        public void Step2_RegisterProvider_Failure_With11Providers()
        {
            var loadBalancerAlgorithm = new RandomLoadBalancerAlgorithm();
            var loadBalancerManager = new LoadBalancerManager(loadBalancerAlgorithm);

            Assert.Equal(RegistrationStatus.Success, loadBalancerManager.Register("providerIdentifier1"));
            Assert.Equal(RegistrationStatus.Success, loadBalancerManager.Register("providerIdentifier2"));
            Assert.Equal(RegistrationStatus.Success, loadBalancerManager.Register("providerIdentifier3"));
            Assert.Equal(RegistrationStatus.Success, loadBalancerManager.Register("providerIdentifier4"));
            Assert.Equal(RegistrationStatus.Success, loadBalancerManager.Register("providerIdentifier5"));
            Assert.Equal(RegistrationStatus.Success, loadBalancerManager.Register("providerIdentifier6"));
            Assert.Equal(RegistrationStatus.Success, loadBalancerManager.Register("providerIdentifier7"));
            Assert.Equal(RegistrationStatus.Success, loadBalancerManager.Register("providerIdentifier8"));
            Assert.Equal(RegistrationStatus.Success, loadBalancerManager.Register("providerIdentifier9"));
            Assert.Equal(RegistrationStatus.Success, loadBalancerManager.Register("providerIdentifier10"));

            Assert.Equal(RegistrationStatus.MaxNumberExceed, loadBalancerManager.Register("providerIdentifier11"));
        }

        [Fact]
        public void Step2_RegisterProvider_Failure_ProviderAlreadyRegistered()
        {
            var loadBalancerAlgorithm = new RandomLoadBalancerAlgorithm();
            var loadBalancerManager = new LoadBalancerManager(loadBalancerAlgorithm);

            Assert.Equal(RegistrationStatus.Success, loadBalancerManager.Register("providerIdentifier1"));
            Assert.Equal(RegistrationStatus.AlreadyRegistered, loadBalancerManager.Register("providerIdentifier1"));
        }

        [Fact]
        public void Step3_RandomLoadBalancerInvocationTest_Success()
        {
            var loadBalancerAlgorithm = new RandomLoadBalancerAlgorithm();
            var loadBalancerManager = new LoadBalancerManager(loadBalancerAlgorithm);

            var providers = new List<string>();

            for(var i = 1; i <= 10; i++)
            {
                var identifier = "providerIdentifier" + i;
                providers.Add(identifier);
                Assert.Equal(RegistrationStatus.Success, loadBalancerManager.Register(identifier));
            }

            var invokedProvider = loadBalancerManager.Get();
            Assert.NotNull(invokedProvider);
            Assert.Contains(invokedProvider, providers);
        }

        [Fact]
        public void Step4_RoundRobinLoadBalancerInvocationTest_Success()
        {
            var loadBalancerAlgorithm = new RoundRobinLoadBalancerAlgorithm();
            var loadBalancerManager = new LoadBalancerManager(loadBalancerAlgorithm);

            var providers = new List<string>();

            for (var i = 1; i <= 10; i++)
            {
                var identifier = "providerIdentifier" + i;
                providers.Add(identifier);
                Assert.Equal(RegistrationStatus.Success, loadBalancerManager.Register(identifier));
            }


            Assert.Equal(9, providers.IndexOf(loadBalancerManager.Get()));
            Assert.Equal(8, providers.IndexOf(loadBalancerManager.Get()));
            Assert.Equal(7, providers.IndexOf(loadBalancerManager.Get()));
            Assert.Equal(6, providers.IndexOf(loadBalancerManager.Get()));
            Assert.Equal(5, providers.IndexOf(loadBalancerManager.Get()));
            Assert.Equal(4, providers.IndexOf(loadBalancerManager.Get()));
            Assert.Equal(3, providers.IndexOf(loadBalancerManager.Get()));
            Assert.Equal(2, providers.IndexOf(loadBalancerManager.Get()));
            Assert.Equal(1, providers.IndexOf(loadBalancerManager.Get()));
            Assert.Equal(0, providers.IndexOf(loadBalancerManager.Get()));

            Assert.Equal(9, providers.IndexOf(loadBalancerManager.Get()));
            Assert.Equal(8, providers.IndexOf(loadBalancerManager.Get()));
            Assert.Equal(7, providers.IndexOf(loadBalancerManager.Get()));
            Assert.Equal(6, providers.IndexOf(loadBalancerManager.Get()));
            Assert.Equal(5, providers.IndexOf(loadBalancerManager.Get()));
            Assert.Equal(4, providers.IndexOf(loadBalancerManager.Get()));
            Assert.Equal(3, providers.IndexOf(loadBalancerManager.Get()));
            Assert.Equal(2, providers.IndexOf(loadBalancerManager.Get()));
            Assert.Equal(1, providers.IndexOf(loadBalancerManager.Get()));
            Assert.Equal(0, providers.IndexOf(loadBalancerManager.Get()));
        }

        [Fact]
        public void Step5_DeregisterProvider_Success()
        {
            var loadBalancerAlgorithm = new RandomLoadBalancerAlgorithm();
            var loadBalancerManager = new LoadBalancerManager(loadBalancerAlgorithm);

            Assert.Equal(RegistrationStatus.Success, loadBalancerManager.Register("providerIdentifier1"));
            Assert.Equal(DeregistrationStatus.DeregistrationSuccess, loadBalancerManager.Deregister("providerIdentifier1"));

            Assert.Empty(loadBalancerManager.GetLoadBalancer().Providers);
        }

        [Fact]
        public void Step5_DeregisterProvider_Failure()
        {
            var loadBalancerAlgorithm = new RandomLoadBalancerAlgorithm();
            var loadBalancerManager = new LoadBalancerManager(loadBalancerAlgorithm);

            Assert.Equal(RegistrationStatus.Success, loadBalancerManager.Register("providerIdentifier1"));
            Assert.Equal(DeregistrationStatus.DeregistrationFailure, loadBalancerManager.Deregister("providerIdentifier11"));

            Assert.Equal(1, loadBalancerManager.GetLoadBalancer().Providers.Count);
        }
    }
}
