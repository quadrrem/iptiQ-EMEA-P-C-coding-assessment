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
        public void Step2_RegisterProvider_Failure_RegistrationFailure_NullInput()
        {
            Assert.Equal(RegistrationStatus.RegistrationFailure, _loadBalancerManager.Register(null));;
        }

        [Fact]
        public void Step3_RandomInvocation_Success()
        {
            var providers = new List<string>();

            for(var i = 1; i <= 10; i++)
            {
                var identifier = "providerIdentifier" + i;
                providers.Add(identifier);
                Assert.Equal(RegistrationStatus.RegistrationSuccess, _loadBalancerManager.Register(identifier));
            }

            _randomInvokationAlgorithm.SetRegisteredProvidersCounter(10);
            _loadBalancerManager.SetLoadBalancerAlgorithm(_randomInvokationAlgorithm);

            var invokedProvider = _loadBalancerManager.Get("request");
            Assert.NotNull(invokedProvider);
            Assert.Contains(invokedProvider, providers);
        }

        [Fact]
        public void Step3_RandomInvocation_WhenRegistredProviderListIsEmpty_Failure()
        {
            _loadBalancerManager.SetLoadBalancerAlgorithm(_randomInvokationAlgorithm);

            var invokedProvider = _loadBalancerManager.Get("request");
            Assert.Null(invokedProvider);
        }

        [Fact]
        public void Step4_RoundRobinInvocation_Success()
        {
            var providers = new List<string>();

            for (var i = 1; i <= 5; i++)
            {
                var identifier = "providerIdentifier" + i;
                providers.Add(identifier);
                Assert.Equal(RegistrationStatus.RegistrationSuccess, _loadBalancerManager.Register(identifier));
            }

            var registeredProvidersCount = _loadBalancerManager.GetLoadBalancer().RegisteredProviders.Count;

            _roundRobinAlgorithm.SetRegisteredProvidersCounter(registeredProvidersCount);
            _loadBalancerManager.SetLoadBalancerAlgorithm(_roundRobinAlgorithm);

            Assert.Equal(4, providers.IndexOf(_loadBalancerManager.Get("request6")));
            Assert.Equal(3, providers.IndexOf(_loadBalancerManager.Get("request7")));
            Assert.Equal(2, providers.IndexOf(_loadBalancerManager.Get("request8")));
            Assert.Equal(1, providers.IndexOf(_loadBalancerManager.Get("request9")));
            Assert.Equal(0, providers.IndexOf(_loadBalancerManager.Get("request10")));

            Assert.Equal(4, providers.IndexOf(_loadBalancerManager.Get("request6")));
            Assert.Equal(3, providers.IndexOf(_loadBalancerManager.Get("request7")));
            Assert.Equal(2, providers.IndexOf(_loadBalancerManager.Get("request8")));
            Assert.Equal(1, providers.IndexOf(_loadBalancerManager.Get("request9")));
            Assert.Equal(0, providers.IndexOf(_loadBalancerManager.Get("request10")));
        }

        [Fact]
        public void Step4_RoundRobinInvocation_Failure_WhenRegistredProviderListIsEmpty()
        {
            _loadBalancerManager.SetLoadBalancerAlgorithm(_roundRobinAlgorithm);

            var invokedProvider = _loadBalancerManager.Get("request");
            Assert.Null(invokedProvider);
        }

        [Fact]
        public void Step5_DeregisterProvider_Success()
        {
            Assert.Equal(RegistrationStatus.RegistrationSuccess, _loadBalancerManager.Register("providerIdentifier1"));
            Assert.Equal(DeregistrationStatus.DeregistrationSuccess, _loadBalancerManager.Deregister("providerIdentifier1"));

            Assert.Empty(_loadBalancerManager.GetLoadBalancer().RegisteredProviders);
        }

        [Fact]
        public void Step5_DeregisterProvider_Failure_ProviderNotFoundInTheRegisteredProviderList()
        {
            Assert.Equal(RegistrationStatus.RegistrationSuccess, _loadBalancerManager.Register("providerIdentifier1"));
            Assert.Equal(DeregistrationStatus.DeregistrationFailure, _loadBalancerManager.Deregister("providerIdentifier11"));

            Assert.Equal(1, _loadBalancerManager.GetLoadBalancer().RegisteredProviders.Count);
        }

        [Fact]
        public void Step6_Check_IsAliveProviders_Success()
        {
            for (var i = 1; i <= 10; i++)
            {
                var identifier = "providerIdentifier" + i;
                Assert.Equal(RegistrationStatus.RegistrationSuccess, _loadBalancerManager.Register(identifier));
            }

            _randomInvokationAlgorithm.SetRegisteredProvidersCounter(10);
            _loadBalancerManager.SetLoadBalancerAlgorithm(_randomInvokationAlgorithm);

            _loadBalancerManager.Get("request");
            _loadBalancerManager.Check();

            Assert.Equal(10, _loadBalancerManager.GetLoadBalancer().RegisteredProviders.Count());
            Assert.Empty(_loadBalancerManager.GetLoadBalancer().DeregisteredProviders);
        }

        [Fact]
        public void Step7_HeartbeatCheacker_ReRegister_a_Provider_WhenProviderIsActiveTwiceInARow_Success()
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

            // heartbeat checked 1 when the provider is active
            _loadBalancerManager.GetLoadBalancer().DeregisteredProviders[0].IsActive = true;

            _loadBalancerManager.Check();

            // heartbeat checked 2 when the provider is active
            Assert.Equal(9, _loadBalancerManager.GetLoadBalancer().RegisteredProviders.Count());
            Assert.Single(_loadBalancerManager.GetLoadBalancer().DeregisteredProviders);

            _loadBalancerManager.Check();

            // provider re registered
            Assert.Equal(10, _loadBalancerManager.GetLoadBalancer().RegisteredProviders.Count());
            Assert.Empty(_loadBalancerManager.GetLoadBalancer().DeregisteredProviders);
        }

        [Fact]
        public void Step6_PeriodicCheck()
        {
            _loadBalancerManager.SetLoadBalancerAlgorithm(_randomInvokationAlgorithm);

            for (var i = 1; i <= 10; i++)
            {
                var identifier = "providerIdentifier" + i;
                Assert.Equal(RegistrationStatus.RegistrationSuccess, _loadBalancerManager.Register(identifier));
            }

            _loadBalancerManager.GetLoadBalancer().RegisteredProviders[0].IsActive = false;
            _loadBalancerManager.PeriodicCheck();

            Assert.Equal(9, _loadBalancerManager.GetLoadBalancer().RegisteredProviders.Count());
            Assert.Single(_loadBalancerManager.GetLoadBalancer().DeregisteredProviders);
        }

        [Fact]
        public void Step8_ClusterCapacityLimit_WithRoundRobin_Success()
        {
            var providers = new List<string>();

            for (var i = 1; i <= 10; i++)
            {
                var identifier = "providerIdentifier" + i;
                providers.Add(identifier);
                Assert.Equal(RegistrationStatus.RegistrationSuccess, _loadBalancerManager.Register(identifier));
            }

            var registeredProviders = _loadBalancerManager.GetLoadBalancer().RegisteredProviders;
            var count = registeredProviders.Count;
            _roundRobinAlgorithm.SetRegisteredProvidersCounter(count);
            _loadBalancerManager.SetLoadBalancerAlgorithm(_roundRobinAlgorithm);

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

            // all the providers has got one request to process
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

            // all the providers has got 2 requests to process
            foreach (var p in registeredProviders)
            {
                Assert.Equal(2, p.Requests.Count);
            }

            Assert.Null(_loadBalancerManager.Get("request"));

            var lb = _loadBalancerManager.GetLoadBalancer();

            Assert.False(lb.RegisteredProviders[9].IsActive);

            _loadBalancerManager.Check();

            Assert.Empty(lb.RegisteredProviders);
            Assert.Equal(10, lb.DeregisteredProviders.Count);
        }

        [Fact]
        public void Step8_ClusterCapacityLimit_RandomInvokation_Success()
        {
            var providers = new List<string>();

            Assert.Equal(RegistrationStatus.RegistrationSuccess, _loadBalancerManager.Register("provider1"));

            var registeredProviders = _loadBalancerManager.GetLoadBalancer().RegisteredProviders;

            var count = registeredProviders.Count;
            _randomInvokationAlgorithm.SetRegisteredProvidersCounter(count);
            _loadBalancerManager.SetLoadBalancerAlgorithm(_randomInvokationAlgorithm);

            Assert.Equal("provider1", _loadBalancerManager.Get("request1"));
            Assert.Single(registeredProviders.First(x => x.Identifier == "provider1").Requests);

            Assert.Equal("provider1", _loadBalancerManager.Get("request2"));
            Assert.Equal(2, registeredProviders.First(x => x.Identifier == "provider1").Requests.Count);

            Assert.Null(_loadBalancerManager.Get("request"));

            Assert.DoesNotContain(_loadBalancerManager.GetLoadBalancer().RegisteredProviders, x => x.IsActive);
        }
    }
}
