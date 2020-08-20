using System.Collections.Generic;
using System.Linq;
using Xunit;
using LoadBalancerProblem.Logic;
using LoadBalancerProblem.Logic.Interface;

namespace LoadBalancerTestProject
{
    public class LoadBalancerManagerTest
    {
        private ILoadBalancerManager _loadBalancerManager;
        private IRoundRobinAlgorithm _roundRobinAlgorithm;
        private IRandomInvokationAlgorithm _randomInvokationAlgorithm;

        public LoadBalancerManagerTest(
            ILoadBalancerManager loadBalancerManager,
            IRoundRobinAlgorithm roundRobinAlgorithm,
            IRandomInvokationAlgorithm randomInvokationAlgorithm)
        { 
            _loadBalancerManager = loadBalancerManager;
            _roundRobinAlgorithm = roundRobinAlgorithm;
            _randomInvokationAlgorithm = randomInvokationAlgorithm;
        }

        [Fact]
        public void Step2_RegisterProvider_Success_With10Providers()
        {
            Assert.Equal(RegistrationStatus.RegistrationSuccess, _loadBalancerManager.Register("providerIdentifier1"));
            Assert.Equal(RegistrationStatus.RegistrationSuccess, _loadBalancerManager.Register("providerIdentifier2"));
            Assert.Equal(RegistrationStatus.RegistrationSuccess, _loadBalancerManager.Register("providerIdentifier3"));
            Assert.Equal(RegistrationStatus.RegistrationSuccess, _loadBalancerManager.Register("providerIdentifier4"));
            Assert.Equal(RegistrationStatus.RegistrationSuccess, _loadBalancerManager.Register("providerIdentifier5"));
            Assert.Equal(RegistrationStatus.RegistrationSuccess, _loadBalancerManager.Register("providerIdentifier6"));
            Assert.Equal(RegistrationStatus.RegistrationSuccess, _loadBalancerManager.Register("providerIdentifier7"));
            Assert.Equal(RegistrationStatus.RegistrationSuccess, _loadBalancerManager.Register("providerIdentifier8"));
            Assert.Equal(RegistrationStatus.RegistrationSuccess, _loadBalancerManager.Register("providerIdentifier9"));
            Assert.Equal(RegistrationStatus.RegistrationSuccess, _loadBalancerManager.Register("providerIdentifier10"));
        }

        [Fact]
        public void Step2_RegisterProvider_Failure_With11Providers()
        {
            Assert.Equal(RegistrationStatus.RegistrationSuccess, _loadBalancerManager.Register("providerIdentifier1"));
            Assert.Equal(RegistrationStatus.RegistrationSuccess, _loadBalancerManager.Register("providerIdentifier2"));
            Assert.Equal(RegistrationStatus.RegistrationSuccess, _loadBalancerManager.Register("providerIdentifier3"));
            Assert.Equal(RegistrationStatus.RegistrationSuccess, _loadBalancerManager.Register("providerIdentifier4"));
            Assert.Equal(RegistrationStatus.RegistrationSuccess, _loadBalancerManager.Register("providerIdentifier5"));
            Assert.Equal(RegistrationStatus.RegistrationSuccess, _loadBalancerManager.Register("providerIdentifier6"));
            Assert.Equal(RegistrationStatus.RegistrationSuccess, _loadBalancerManager.Register("providerIdentifier7"));
            Assert.Equal(RegistrationStatus.RegistrationSuccess, _loadBalancerManager.Register("providerIdentifier8"));
            Assert.Equal(RegistrationStatus.RegistrationSuccess, _loadBalancerManager.Register("providerIdentifier9"));
            Assert.Equal(RegistrationStatus.RegistrationSuccess, _loadBalancerManager.Register("providerIdentifier10"));

            Assert.Equal(RegistrationStatus.MaxNumberOfProviderExceeded, _loadBalancerManager.Register("providerIdentifier11"));
        }

        [Fact]
        public void Step2_RegisterProvider_Failure_ProviderAlreadyRegistered()
        {
            Assert.Equal(RegistrationStatus.RegistrationSuccess, _loadBalancerManager.Register("providerIdentifier1"));
            Assert.Equal(RegistrationStatus.ProviderAlreadyRegistered, _loadBalancerManager.Register("providerIdentifier1"));
        }

        [Fact]
        public void Step3_RandomLoadBalancerInvocationTest_Success()
        {
            _loadBalancerManager.SetLoadBalancerAlgorithm(_randomInvokationAlgorithm);

            var providers = new List<string>();

            for(var i = 1; i <= 10; i++)
            {
                var identifier = "providerIdentifier" + i;
                providers.Add(identifier);
                Assert.Equal(RegistrationStatus.RegistrationSuccess, _loadBalancerManager.Register(identifier));
            }

            var invokedProvider = _loadBalancerManager.Get("request");
            Assert.NotNull(invokedProvider);
            Assert.Contains(invokedProvider, providers);
        }

        [Fact]
        public void Step4_RoundRobinLoadBalancerInvocationTest_InvokeRequestsToProviders_Success()
        {
            _loadBalancerManager.SetLoadBalancerAlgorithm(_roundRobinAlgorithm);

            var providers = new List<string>();

            for (var i = 1; i <= 10; i++)
            {
                var identifier = "providerIdentifier" + i;
                providers.Add(identifier);
                Assert.Equal(RegistrationStatus.RegistrationSuccess, _loadBalancerManager.Register(identifier));
            }

            Assert.Equal(9, providers.IndexOf(_loadBalancerManager.Get("request1")));
            Assert.Equal(8, providers.IndexOf(_loadBalancerManager.Get("request2")));
            Assert.Equal(7, providers.IndexOf(_loadBalancerManager.Get("request3")));
            Assert.Equal(6, providers.IndexOf(_loadBalancerManager.Get("request4")));
            Assert.Equal(5, providers.IndexOf(_loadBalancerManager.Get("request5")));
            Assert.Equal(4, providers.IndexOf(_loadBalancerManager.Get("request6")));
            Assert.Equal(3, providers.IndexOf(_loadBalancerManager.Get("request7")));
            Assert.Equal(2, providers.IndexOf(_loadBalancerManager.Get("request8")));
            Assert.Equal(1, providers.IndexOf(_loadBalancerManager.Get("request9")));
            Assert.Equal(0, providers.IndexOf(_loadBalancerManager.Get("request10")));

            var registeredProviders = _loadBalancerManager.GetLoadBalancer().RegisteredProviders;

            var count = registeredProviders.Count;
            for (int i = count - 1, j = 1; i >= 0 && j <= count; i--, j++)
            {
                Assert.Single(registeredProviders[i].Requests);
                Assert.Equal("request" + j, registeredProviders[i].Requests.First());
            }

            Assert.Equal(9, providers.IndexOf(_loadBalancerManager.Get("request1")));
            Assert.Equal(8, providers.IndexOf(_loadBalancerManager.Get("request2")));
            Assert.Equal(7, providers.IndexOf(_loadBalancerManager.Get("request3")));
            Assert.Equal(6, providers.IndexOf(_loadBalancerManager.Get("request4")));
            Assert.Equal(5, providers.IndexOf(_loadBalancerManager.Get("request5")));
            Assert.Equal(4, providers.IndexOf(_loadBalancerManager.Get("request6")));
            Assert.Equal(3, providers.IndexOf(_loadBalancerManager.Get("request7")));
            Assert.Equal(2, providers.IndexOf(_loadBalancerManager.Get("request8")));
            Assert.Equal(1, providers.IndexOf(_loadBalancerManager.Get("request9")));
            Assert.Equal(0, providers.IndexOf(_loadBalancerManager.Get("request10")));
        }

        [Fact]
        public void Step5_DeregisterProvider_Success()
        {
            Assert.Equal(RegistrationStatus.RegistrationSuccess, _loadBalancerManager.Register("providerIdentifier1"));
            Assert.Equal(DeregistrationStatus.DeregistrationSuccess, _loadBalancerManager.Deregister("providerIdentifier1"));

            Assert.Empty(_loadBalancerManager.GetLoadBalancer().RegisteredProviders);
        }

        [Fact]
        public void Step5_DeregisterProvider_Failure()
        {
            Assert.Equal(RegistrationStatus.RegistrationSuccess, _loadBalancerManager.Register("providerIdentifier1"));
            Assert.Equal(DeregistrationStatus.DeregistrationFailure, _loadBalancerManager.Deregister("providerIdentifier11"));

            Assert.Equal(1, _loadBalancerManager.GetLoadBalancer().RegisteredProviders.Count);
        }

        [Fact]
        public void Step6_Check_IsAliveProviders()
        {
            _loadBalancerManager.SetLoadBalancerAlgorithm(_randomInvokationAlgorithm);

            for (var i = 1; i <= 10; i++)
            {
                var identifier = "providerIdentifier" + i;
                Assert.Equal(RegistrationStatus.RegistrationSuccess, _loadBalancerManager.Register(identifier));
            }

            _loadBalancerManager.Get("request");
            _loadBalancerManager.Check();

            Assert.Equal(10, _loadBalancerManager.GetLoadBalancer().RegisteredProviders.Count());
            Assert.Empty(_loadBalancerManager.GetLoadBalancer().DeregisteredProviders);
        }

        [Fact]
        public void Step6_PeriodicCheck_IsAliveProviders()
        {
            _loadBalancerManager.SetLoadBalancerAlgorithm(_randomInvokationAlgorithm);

            for (var i = 1; i <= 10; i++)
            {
                var identifier = "providerIdentifier" + i;
                Assert.Equal(RegistrationStatus.RegistrationSuccess, _loadBalancerManager.Register(identifier));
            }

            _loadBalancerManager.GetLoadBalancer().RegisteredProviders[0].IsActive = false;
            _loadBalancerManager.Check();

            Assert.Equal(9, _loadBalancerManager.GetLoadBalancer().RegisteredProviders.Count());
            Assert.Single(_loadBalancerManager.GetLoadBalancer().DeregisteredProviders);
        }
        [Fact]
        public void Step7_PeriodicCheck_HeartbeatCounter_ReRegister()
        {
            _loadBalancerManager.SetLoadBalancerAlgorithm(_randomInvokationAlgorithm);

            for (var i = 1; i <= 10; i++)
            {
                var identifier = "providerIdentifier" + i;
                Assert.Equal(RegistrationStatus.RegistrationSuccess, _loadBalancerManager.Register(identifier));
            }

            _loadBalancerManager.GetLoadBalancer().RegisteredProviders[0].IsActive = false;
            _loadBalancerManager.Check();

            Assert.Equal(9, _loadBalancerManager.GetLoadBalancer().RegisteredProviders.Count());
            Assert.Single(_loadBalancerManager.GetLoadBalancer().DeregisteredProviders);

            _loadBalancerManager.GetLoadBalancer().DeregisteredProviders[0].IsActive = true;
            _loadBalancerManager.Check();

            Assert.Equal(9, _loadBalancerManager.GetLoadBalancer().RegisteredProviders.Count());
            Assert.Single(_loadBalancerManager.GetLoadBalancer().DeregisteredProviders);

            _loadBalancerManager.GetLoadBalancer().DeregisteredProviders[0].IsActive = true;
            _loadBalancerManager.Check();

            Assert.Equal(10, _loadBalancerManager.GetLoadBalancer().RegisteredProviders.Count());
            Assert.Empty(_loadBalancerManager.GetLoadBalancer().DeregisteredProviders);
        }

        [Fact]
        public void Step8_ParallelRequetsProcessing()
        {
            Assert.Equal(RegistrationStatus.RegistrationSuccess, _loadBalancerManager.Register("provider1"));

            var lb = _loadBalancerManager.GetLoadBalancer();

            Assert.Empty(lb.RegisteredProviders[0].Requests);

        }

        [Fact]
        public void Step8()
        {
            _loadBalancerManager.SetLoadBalancerAlgorithm(_roundRobinAlgorithm);

            var providers = new List<string>();

            for (var i = 1; i <= 10; i++)
            {
                var identifier = "providerIdentifier" + i;
                providers.Add(identifier);
                Assert.Equal(RegistrationStatus.RegistrationSuccess, _loadBalancerManager.Register(identifier));
            }

            Assert.Equal(9, providers.IndexOf(_loadBalancerManager.Get("request0")));
            Assert.Equal(8, providers.IndexOf(_loadBalancerManager.Get("request1")));
            Assert.Equal(7, providers.IndexOf(_loadBalancerManager.Get("request2")));
            Assert.Equal(6, providers.IndexOf(_loadBalancerManager.Get("request3")));
            Assert.Equal(5, providers.IndexOf(_loadBalancerManager.Get("request4")));
            Assert.Equal(4, providers.IndexOf(_loadBalancerManager.Get("request5")));
            Assert.Equal(3, providers.IndexOf(_loadBalancerManager.Get("request6")));
            Assert.Equal(2, providers.IndexOf(_loadBalancerManager.Get("request7")));
            Assert.Equal(1, providers.IndexOf(_loadBalancerManager.Get("request8")));
            Assert.Equal(0, providers.IndexOf(_loadBalancerManager.Get("request9")));

            var registeredProviders = _loadBalancerManager.GetLoadBalancer().RegisteredProviders;

            var count = registeredProviders.Count;
            for (int i = count - 1, j = 0; i >= 0 && j < count; i--, j++)
            {
                Assert.Single(registeredProviders[i].Requests);
                Assert.Equal("request"+j, registeredProviders[i].Requests.First());
            }

            Assert.Equal(9, providers.IndexOf(_loadBalancerManager.Get("request1")));
            Assert.Equal(8, providers.IndexOf(_loadBalancerManager.Get("request2")));
            Assert.Equal(7, providers.IndexOf(_loadBalancerManager.Get("request3")));
            Assert.Equal(6, providers.IndexOf(_loadBalancerManager.Get("request4")));
            Assert.Equal(5, providers.IndexOf(_loadBalancerManager.Get("request5")));
            Assert.Equal(4, providers.IndexOf(_loadBalancerManager.Get("request6")));
            Assert.Equal(3, providers.IndexOf(_loadBalancerManager.Get("request7")));
            Assert.Equal(2, providers.IndexOf(_loadBalancerManager.Get("request8")));
            Assert.Equal(1, providers.IndexOf(_loadBalancerManager.Get("request9")));
            Assert.Equal(0, providers.IndexOf(_loadBalancerManager.Get("request10")));

            foreach (var p in registeredProviders)
            {
                Assert.Equal(2, p.Requests.Count);
            }

            Assert.Null(_loadBalancerManager.Get("request"));

            var lb = _loadBalancerManager.GetLoadBalancer();

            Assert.Contains(lb.RegisteredProviders, i => i.IsActive);

            _loadBalancerManager.Check();
            Assert.Single(lb.DeregisteredProviders);
            Assert.Equal(9, lb.RegisteredProviders.Count);
        }

    }
}
