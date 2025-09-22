using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Web.Common.ApplicationBuilder;
using Umbraco.Cms.Web.Common.Routing;

namespace Dyfort.Umbraco.RobotsTxt
{
    public class RobotsTxtComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.Services.Configure<RobotsTxtSettings>(builder.Config.GetSection("RobotsTxt"));

            builder.Services.Configure<UmbracoRequestOptions>(options =>
            {
                var allowList = new[] { "/robots.txt" };

                options.HandleAsServerSideRequest = httpRequest =>
                {
                    foreach (string route in allowList)
                    {
                        if (httpRequest.Path.StartsWithSegments(route))
                        {
                            return true;
                        }
                    }

                    return false;
                };
            });

            builder.Services.Configure<UmbracoPipelineOptions>(options =>
            {
                options.AddFilter(new UmbracoPipelineFilter(
                    "robots_txt",
                    applicationBuilder => { },
                    applicationBuilder => { },
                    applicationBuilder =>
                    {
                        applicationBuilder.UseRobotsTxt(applicationBuilder.ApplicationServices.GetRequiredService<IWebHostEnvironment>());
                    }
                    ));
            });
        }
    }
}
