using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AzureMonitoring
{
    public class AzureApiAccess : BaseApi
    {
        public AzureApiAccess(AzureSubscription Subscription)
            : base(Subscription)
        { }

        public async Task<String> GetHostedServices()
        {
            var result = await this.HttpClientInstance.GetStringAsync(this.ServiceManagementUri);
            return result;
        }
    }
}
