using System.Collections.Generic;

namespace LoadBalancerProblem
{
    public class Provider
    {
        public Provider(string identifier)
        {
            this.identifier = identifier;
            isActive = true;
            requests = new Queue<string>();
            limit = 2; // for testing purposes, its initiazed with a hardcoded value
        }

        /// <summary>
        /// unique identifier of a provider
        /// </summary>
        private string identifier;

        public string Identifier
        {
            get
            {
                return identifier;
            }
        }

        /// <summary>
        /// Defines the status of the provider whether its active or inactive
        /// If the provider's queue has reached the limit, the isActive status will be false
        /// Otherwise true
        /// </summary>
        private bool isActive;

        public bool IsActive
        {
            get
            {
                return isActive;
            }
            set
            {
                isActive = value;
            }
        }

        /// <summary>
        /// Every provider has a queue to process the incoming requests
        /// </summary>
        private Queue<string> requests;

        public Queue<string> Requests
        {
            get
            {
                return requests;
            }
        }

        /// <summary>
        /// Max number of requests that a provider can handle. 
        /// </summary>
        private readonly int limit;

        public int Limit
        {
            get
            {
                return limit;
            }
        }
    }
}
