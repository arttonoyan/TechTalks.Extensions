using System;
using System.Collections.Generic;
using System.Linq;
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
        public HttpStatusCode HttpStatusCode { get; set; } = HttpStatusCode.BadRequest;
        public string Content { get; set; }

        public Currency Currency { get; set; }

        //public bool IsSuccess =>
        //    HttpStatusCode == HttpStatusCode.OK &&
        //    Currency != null &&
        //    Currency.Success;
    }
}
