using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace LoadBalancerProblem
{
    public class Provider
    {
        public Provider(string identifier)
        {
            this.identifier = identifier;
            isActive = true;
            requests = new Queue<string>();
            limit = 2;
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

        private Queue<string> requests;

        public Queue<string> Requests
        {
            get
            {
                return requests;
            }
            set
            {
                requests = value;
            }
        }

        /// <summary>
        /// Max number of requests that a provider can handle. 
        /// We assume any provider can handle maximum 10 requests
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
