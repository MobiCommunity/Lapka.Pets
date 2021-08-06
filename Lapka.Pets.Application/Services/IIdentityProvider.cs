using Microsoft.AspNetCore.Http;
using System;

namespace Lapka.Pets.Application.Services
{
    public interface IIdValueProvider
    {
        Guid GetIdValue(HttpContext ctx);
        string GetIdValueAsString(HttpContext ctx);
    }
}