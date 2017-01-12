﻿namespace Ng2Aa_demo.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Owin;

    namespace Microsoft.AspNetCore.Builder
    {
        using global::Microsoft.AspNetCore.Builder;
        using global::Microsoft.AspNetCore.DataProtection;
        using global::Microsoft.Extensions.DependencyInjection;
        using global::Microsoft.Owin.Builder;
        using AppFunc = Func<IDictionary<string, object>, Task>;
        using DataProtectionProviderDelegate = Func<string[], Tuple<Func<byte[], byte[]>, Func<byte[], byte[]>>>;
        using DataProtectionTuple = Tuple<Func<byte[], byte[]>, Func<byte[], byte[]>>;

        public static class KatanaIApplicationBuilderExtensions
        {
            public static IApplicationBuilder UseAppBuilder(this IApplicationBuilder app, Action<IAppBuilder> configure)
            {
                app.UseOwin(addToPipeline =>
                {
                    addToPipeline(next =>
                    {
                        var appBuilder = new AppBuilder();
                        appBuilder.Properties["builder.DefaultApp"] = next;

                        configure(appBuilder);

                        return appBuilder.Build<AppFunc>();
                    });
                });
                return app;
            }

            public static IAppBuilder SetDataProtectionProvider(this IAppBuilder appBuilder, IApplicationBuilder app)
            {
                var provider = app.ApplicationServices.GetRequiredService<IDataProtectionProvider>();
                appBuilder.Properties["security.DataProtectionProvider"] = new DataProtectionProviderDelegate(purposes =>
                {
                    var dataProtection = provider.CreateProtector(string.Join(",", purposes));
                    return new DataProtectionTuple(dataProtection.Protect, dataProtection.Unprotect);
                });
                return appBuilder;
            }
        }
    }
}
