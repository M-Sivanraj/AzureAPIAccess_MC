using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace AzureMonitoring
{
    public class BaseApi
    {
        private AzureSubscription azureSubscription = null;

        public BaseApi(AzureSubscription Subscription)
        {
            if (Subscription == null)
            {
                throw new ArgumentNullException(
                    "Subscription", "Subscription parameter cannot be null.");
            }

            this.azureSubscription = Subscription;
        }

        public virtual Uri ServiceManagementUri
        {
            get
            {
                return new Uri(string.Format("{0}/{1}", azureSubscription.Subscription.ServiceUrl, azureSubscription.Subscription.Id));
            }
        }

        public Uri GetUri(string serviceURI)
        {
            return new Uri(string.Format("{0}/{1}/{2}", azureSubscription.Subscription.ServiceUrl, azureSubscription.Subscription.Id, serviceURI));
        }

        public WebRequestHandler RequestHandler
        {
            get
            {
                var handler = new WebRequestHandler();
                handler.ClientCertificates.Add(
                    this.azureSubscription.Subscription.Certificate);
                return handler;
            }
        }

        public HttpClient HttpClientInstance
        {
            get
            {
                var httpClient = new HttpClient(this.RequestHandler);
                httpClient.BaseAddress = ServiceManagementUri;
                httpClient.DefaultRequestHeaders.Add("x-ms-version", "2013-03-01");
                httpClient.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/xml"));

                return httpClient;
            }
        }
    }
}
