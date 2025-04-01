using Flurl;
using Flurl.Http;
using IHttpClientFactory = System.Net.Http.IHttpClientFactory;


namespace TrueStory.Infrastructure.Flurl
{
    public class FlurlClientFactory
    {
        private readonly IHttpClientFactory _factory;

        public FlurlClientFactory(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        protected IFlurlClient Create(Url url)
        {
            var client = _factory.CreateClient(url);

            return new FlurlClient(client)
            {
                BaseUrl = url
            };
        }
    }
}
