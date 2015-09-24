using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;

namespace BeerCellier.Tests.Fakes
{
    class FakeControllerContext : ControllerContext
    {
        HttpContextBase _context = new FakeHttpContext();

        public override HttpContextBase HttpContext
        {
            get
            {
                return _context;
            }

            set
            {
                _context = value;
            }
        }
    }

    class FakeHttpContext : HttpContextBase
    {
        HttpRequestBase _request = new FakeRequest();

        public override HttpRequestBase Request
        {
            get
            {
                return _request;
            }
        }
    }

    class FakeRequest : HttpRequestBase
    {
        public override string this[string key]
        {
            get
            {
                return null;
            }
        }

        public override NameValueCollection Headers
        {
            get
            {
                return new NameValueCollection();
            }
        }
    }
}
