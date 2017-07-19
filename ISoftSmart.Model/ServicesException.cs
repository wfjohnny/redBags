using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISoftSmart.Model
{
    public class ServicesException : Exception
    {
        public ServicesException(string serviceName, string message) : base(message)
        {
            this.ServiceName = serviceName;
        }
        public string ServiceName { get; set; }
    }
}
