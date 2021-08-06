using Microsoft.AspNetCore.Http;

namespace Lapka.Pets.Application.Services
{
    public interface IAuthenticator
    {
        public void Authenticate(HttpContext ctx);
    }
}