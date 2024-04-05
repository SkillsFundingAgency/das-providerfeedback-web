﻿using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ProviderFeedback.Web.AppStart;

[ExcludeFromCodeCoverage]
public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseContentSecurityPolicy(this IApplicationBuilder app)
    {
        const string dasCdn = "das-at-frnt-end.azureedge.net das-pp-frnt-end.azureedge.net das-mo-frnt-end.azureedge.net das-test-frnt-end.azureedge.net das-test2-frnt-end.azureedge.net das-prd-frnt-end.azureedge.net";
        app.Use(async (context, next) =>
        {
            context.Response.Headers["Content-Security-Policy"] =
                        $"script-src 'self' 'unsafe-inline' 'unsafe-eval' {dasCdn} https://www.googletagmanager.com https://tagmanager.google.com https://www.google-analytics.com https://ssl.google-analytics.com https://*.services.visualstudio.com https://*.rcrsv.io; " +
                        $"style-src 'self' 'unsafe-inline' {dasCdn} https://tagmanager.google.com https://fonts.googleapis.com https://*.rcrsv.io ; " +
                        $"img-src {dasCdn} www.googletagmanager.com https://ssl.gstatic.com https://www.gstatic.com https://www.google-analytics.com https://*.rcrsv.io ; " +
                        $"font-src {dasCdn} https://fonts.gstatic.com https://*.rcrsv.io ;" +
                        "connect-src https://www.google-analytics.com https://*.rcrsv.io;" +
                        "frame-src https://www.googletagmanager.com https://*.rcrsv.io";
            await next();

        });

        return app;
    }
}