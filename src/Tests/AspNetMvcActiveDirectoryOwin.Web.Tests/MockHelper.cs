using System.IO;
using System.Reflection;
using System.Web;
using System.Web.SessionState;

namespace AspNetMvcActiveDirectoryOwin.Web.Tests
{
    public static class MockHelper
    {
        public static HttpContextBase FakeHttpContext()
        {
            var httpRequest = new HttpRequest("", "http://www.sample.org/", "");
            var stringWriter = new StringWriter();
            var httpResponse = new HttpResponse(stringWriter);
            var httpContext = new HttpContext(httpRequest, httpResponse);

            var sessionContainer = new HttpSessionStateContainer("id", new SessionStateItemCollection(),
                new HttpStaticObjectsCollection(), 10, true,
                HttpCookieMode.AutoDetect,
                SessionStateMode.InProc, false);

            httpContext.Items["AspSession"] = typeof(HttpSessionState).GetConstructor(
                    BindingFlags.NonPublic | BindingFlags.Instance,
                    null, CallingConventions.Standard,
                    new[] {typeof(HttpSessionStateContainer)},
                    null)
                .Invoke(new object[] {sessionContainer});

            return new HttpContextWrapper(httpContext);
        }
    }
}
