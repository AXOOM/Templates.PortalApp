using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyVendor.MyApp.Infrastructure
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
        {
            _identityOptions = identityOptions;
        }

        /// <summary>
        /// Returns configuration for the frontend.
        /// </summary>
        [HttpGet, Route("app.js")]
        public IActionResult ReadOAuth() => Content(@"﻿var AXOOM_APP = AXOOM_APP || {};
AXOOM_APP.CONFIG = {
    OAUTH: {
        clientId: 'myvendor-myapp',
        scope: 'openid profile email myvendor-myapp.api',
        identityServerUri: '" + (_identityOptions.Authority ?? "") + @"'
    }
};", "application/javascript");
    }
}
