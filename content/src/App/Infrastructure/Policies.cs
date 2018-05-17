using System;
using System.Collections.Generic;
using System.Net.Sockets;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;

namespace MyVendorName.MyAppName.Infrastructure
{
    /// <summary>
    /// Provides error handling and retry policies.
    /// </summary>
    [UsedImplicitly]
    public class Policies
    {
        private readonly IOptions<PolicyOptions> _options;
        private readonly ILogger<Policies> _logger;

        public Policies(IOptions<PolicyOptions> options, ILogger<Policies> logger)
        {
            _options = options;
            _logger = logger;
        }

        /// <summary>
        /// Policy for handling connection problems with external services during startup.
        /// </summary>
        public void Startup(Action action)
        {
            try
            {
                Policy.Handle<SocketException>()
                      .WaitAndRetry(
                           sleepDurations: _options.Value.StartupRetries,
                           onRetry: (ex, timeSpan)
                               => _logger.LogWarning($"Problem connecting to external service; retrying in {timeSpan}.\n ({ex.GetType().Name}: {ex.Message})"))
                      .Execute(action);
            }
            catch (Exception ex)
            {
                // Print exception info in GELF instead of letting default handler take care of it
                _logger.LogCritical(ex, "Startup failed.");
                Environment.Exit(exitCode: 1);
            }
        }
    }

    /// <summary>
    /// Options for <see cref="Policies" />.
    /// </summary>
    public class PolicyOptions
    {
        /// <summary>
        /// The delays between subsequent retry attempts at startup.
        /// </summary>
        public ICollection<TimeSpan> StartupRetries { get; } = new List<TimeSpan>();
    }

    public static class PoliciesExtensions
    {
        public static IServiceCollection AddPolicies(this IServiceCollection services, IConfiguration config)
            => services.AddSingleton<Policies>()
                       .Configure<PolicyOptions>(config);
    }
}
