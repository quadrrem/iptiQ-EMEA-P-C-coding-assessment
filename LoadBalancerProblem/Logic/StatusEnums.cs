using System;
using System.Collections.Generic;
using System.Text;

namespace LoadBalancerProblem.Logic
{
    public enum RegistrationStatus
    {
        ProviderAlreadyRegistered = 1,
        MaxNumberOfProviderExceeded = 2,
        RegistrationSuccess = 3,
        RegistrationFailure = 4
    }

    public enum DeregistrationStatus
    {
        DeregistrationSuccess = 1,
        DeregistrationFailure = 2
    }
}
