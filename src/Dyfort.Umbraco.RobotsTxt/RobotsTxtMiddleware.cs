using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;

namespace Dyfort.Umbraco.RobotsTxt
{
    public class RobotsTxtMiddleware
    {
        const string Default =
            @"User-Agent: *
Disallow: /";

        private readonly RequestDelegate next;
        private readonly string environmentName;
        private readonly string rootPath;
        private readonly RobotsTxtSettings settings;
        private readonly IUmbracoContextAccessor umbracoContextAccessor;

        public RobotsTxtMiddleware(
            RequestDelegate next,
            string environmentName,
            string rootPath,
            IOptions<RobotsTxtSettings> settings,
            IUmbracoContextAccessor umbracoContextAccessor
        )
        {
            this.next = next;
            this.environmentName = environmentName;
            this.rootPath = rootPath;
            this.settings = settings.Value;
            this.umbracoContextAccessor = umbracoContextAccessor;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/robots.txt"))
            {
                var generalRobotsTxt = Path.Combine(rootPath, "robots.txt");
                var environmentRobotsTxt = Path.Combine(rootPath, $"robots.{environmentName}.txt");
                string output;

                if (settings.ContentKey.HasValue && !string.IsNullOrWhiteSpace(settings.FieldName))
                {
                    var umbracoContext = umbracoContextAccessor.GetRequiredUmbracoContext();

                    if (umbracoContext.Content == null)
                    {
                        throw new Exception("Published content cache is null.");
                    }

                    var content = umbracoContext.Content.GetById(settings.ContentKey.Value);

                    if (content == null)
                    {
                        throw new Exception($"Content by '{settings.ContentKey.Value}' not found.");
                    }

                    if (!content.HasProperty(settings.FieldName))
                    {
                        throw new Exception($"Property by '{settings.FieldName}' not found on content with key '{content.Key}'.");
                    }

                    output = content.Value<string>(settings.FieldName);
                }
                // try environment first
                else if (File.Exists(environmentRobotsTxt))
                {
                    output = await File.ReadAllTextAsync(environmentRobotsTxt);
                }
                // then robots.txt
                else if (File.Exists(generalRobotsTxt))
                {
                    output = await File.ReadAllTextAsync(generalRobotsTxt);
                }
                // then just a general default
                else
                {
                    output = Default;
                }

                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync(output);
            }
            else
            {
                await next(context);
            }
        }
    }
}
