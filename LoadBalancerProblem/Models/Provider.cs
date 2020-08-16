using System;
using System.Data.Entity;

namespace LoadBalancerProblem
{
    public class Provider
    {
        public Provider(string identifier)
        {
            this.identifier = identifier;
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
            set
            {
                identifier = value;
            }
        }
    }
}
