using System;
using System.Data.Entity;

namespace LoadBalancerProblem
{
    public class Provider
    {
        public Provider(string identifier)
        {
            this.identifier = identifier;
            isActive = true;
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
    }
}
