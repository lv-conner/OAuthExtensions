using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace OAuthHandler.Pocket
{
    public static class PocketExtentions
    {
        public static AuthenticationBuilder AddPocket(this AuthenticationBuilder builder)
            => builder.AddPocket(PocketDefault.AuthenticationScheme, _ => { });

        public static AuthenticationBuilder AddPocket(this AuthenticationBuilder builder, Action<PocketOptions> configureOptions)
            => builder.AddPocket(PocketDefault.AuthenticationScheme, configureOptions);

        public static AuthenticationBuilder AddPocket(this AuthenticationBuilder builder, string authenticationScheme, Action<PocketOptions> configureOptions)
            => builder.AddPocket(authenticationScheme, PocketDefault.DisplayName, configureOptions);

        public static AuthenticationBuilder AddPocket(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<PocketOptions> configureOptions)
            => builder.AddOAuth<PocketOptions, PocketHandler>(authenticationScheme, displayName, configureOptions);
    }
}
