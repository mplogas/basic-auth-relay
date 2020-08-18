using Microsoft.AspNetCore.Mvc;

namespace AzurePoC.Library.Infrastructure.BasicAuth
{
    public class BasicAuthAttribute : TypeFilterAttribute
    {
        public BasicAuthAttribute(string realm = @"My Realm") : base(typeof(BasicAuthFilter))
        {
            Arguments = new object[] { realm };
        }
    }
}
