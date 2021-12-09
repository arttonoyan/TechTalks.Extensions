using System;
using System.Net;

namespace TechTalks.FixerIo
{
    public interface IFixerResponse
    {
        HttpStatusCode HttpStatusCode { get; }

        public string Content { get; }

        public Currency Currency { get; }
    }

    public class FixerResponse : IFixerResponse
    {
        public FixerResponse(Func<Currency> getCurrencyFunc = null)
        {
            if (getCurrencyFunc != null)
                _currencyFactory = new Lazy<Currency>(getCurrencyFunc);
        }

        private Lazy<Currency> _currencyFactory;
        public HttpStatusCode HttpStatusCode { get; set; }
        public string Content { get; set; }
        public Currency Currency => _currencyFactory?.Value;
    }
}
