using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Axoom.MyApp.Infrastructure
{
    /// <summary>
    /// Provides global configuration.
    /// </summary>
    [Route("config")]
    [AllowAnonymous]
    public class ConfigController : Controller
    {
        private readonly IdentityServerAuthenticationOptions _identityOptions;

        public ConfigController(IdentityServerAuthenticationOptions identityOptions)
            => _identityOptions = identityOptions;

        /// <summary>
        /// Returns configuration for the frontend.
        /// </summary>
        [HttpGet, Route("app.js")]
        public IActionResult ReadOAuth() => Content(@"﻿var AXOOM_APP = AXOOM_APP || {};
AXOOM_APP.CONFIG = {
    OAUTH: {
        clientId: 'vendorname-appname',
        scope: 'openid profile email vendorname-appname.api',
        identityServerUri: '" + (_identityOptions.Authority ?? "") + @"'
    }
};", "application/javascript");
    }
}
