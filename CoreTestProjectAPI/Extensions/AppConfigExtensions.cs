using Microsoft.AspNetCore.Builder;
using Utilities.Common;

namespace CoreDepositAPI.Extensions
{
    public static class AppConfigExtensions
    {
        public static void ConfigureExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandler>();
        }
    }
}
