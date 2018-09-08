using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;

namespace Stocqres.Core.Middlewares
{
    public static class Extensions
    {
        public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder builder)
            => builder.UseMiddleware<ErrorHandlerMiddleware>();
    }
}
