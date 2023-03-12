/*
 * Talegen Configuration Server
 * (c) Copyright Talegen, LLC.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * http://www.apache.org/licenses/LICENSE-2.0
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
*/

namespace Talegen.Configuration.Server.Hubs
{
    using System.Threading.Tasks;
    using IdentityModel;
    using Microsoft.AspNetCore.SignalR;
    using Serilog;

    /// <summary>
    /// This class defines the Hub used for SignalR service configuration change notification.
    /// </summary>
    public class ConfigurationSyncHub : Hub<IConfiguration>
    {
        /// <summary>
        /// Contains an instance of the application settings.
        /// </summary>
        //private readonly ApplicationSettings settings;

        /// <summary>
        /// Contains the connection manager instance.
        /// </summary>
        private readonly IConnectionManager connectionManager;

        /// <summary>
        /// Contains the hub context.
        /// </summary>
        private readonly IHubContext<ConfigurationSyncHub> hubContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationSyncHub" /> class.
        /// </summary>
        /// <param name="hubContext">Contains the hub context to use.</param>
        /// <param name="connectionManager">Contains the connection manager.</param>
        public ConfigurationSyncHub(IHubContext<ConfigurationSyncHub> hubContext, IConnectionManager connectionManager)
        {
            this.hubContext = hubContext;
            this.connectionManager = connectionManager;
        }

        /// <summary>
        /// This method is called when a connection is established.
        /// </summary>
        /// <returns>Returns the task.</returns>
        public override async Task OnConnectedAsync()
        {
            if (this.Context.User is { Identity.IsAuthenticated: true })
            {
                string emailId = this.Context.User.Claims.FirstOrDefault(c => c.Type.Equals(JwtClaimTypes.Email, StringComparison.InvariantCultureIgnoreCase))?.Value ?? string.Empty;
                string subjectId = this.Context.User.Claims.FirstOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject, StringComparison.InvariantCultureIgnoreCase))?.Value ?? string.Empty;
                string tenantId = this.Context.User.Claims.FirstOrDefault(c => c.Type.Equals("oid", StringComparison.InvariantCultureIgnoreCase))?.Value ?? string.Empty; //this.RefineHostToTenantKey(this.Context.GetHttpContext()!.Request.Host.Host);

                if (!string.IsNullOrWhiteSpace(subjectId))
                {
                    Log.Information("Adding {0} user Id {1}:{2} to connection {3}.", tenantId, emailId, subjectId, this.Context.ConnectionId);

                    if (!string.IsNullOrWhiteSpace(tenantId))
                    {
                        // add connection to tenant group
                        await this.Groups.AddToGroupAsync(this.Context.ConnectionId, tenantId);
                        await this.connectionManager.AddConnectionAsync(tenantId, subjectId, this.Context.ConnectionId);
                    }
                    else
                    {
                        Log.Warning("No tenant host value was found in HTTP context during OnConnected hub call. The user connection cannot be tracked and notifications may not work.");
                    }
                }
                else
                {
                    Log.Warning("No external User identity value was found in JWT Claim during OnConnected hub call. The user connection cannot be tracked and notifications may not work.");
                }
            }

            await base.OnConnectedAsync();
        }

        /// <summary>
        /// This method is called with the connection is closed.
        /// </summary>
        /// <param name="exception">Contains an exception.</param>
        /// <returns>Returns the task.</returns>
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (this.Context.User is { Identity.IsAuthenticated: true })
            {
                string emailId = this.Context.User.Claims.FirstOrDefault(c => c.Type.Equals(JwtClaimTypes.Email, StringComparison.InvariantCultureIgnoreCase))?.Value ?? string.Empty;
                string subjectId = this.Context.User.Claims.FirstOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject, StringComparison.InvariantCultureIgnoreCase))?.Value ?? string.Empty;
                string tenantId = this.Context.User.Claims.FirstOrDefault(c => c.Type.Equals("oid", StringComparison.InvariantCultureIgnoreCase))?.Value ?? string.Empty; //this.RefineHostToTenantKey(this.Context.GetHttpContext()!.Request.Host.Host);

                if (!string.IsNullOrWhiteSpace(subjectId))
                {
                    Log.Information("Removing {0} user Id {1}:{2} connection {3}.", tenantId, emailId, subjectId, this.Context.ConnectionId);
                    await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, tenantId);
                    await this.connectionManager.RemoveConnectionAsync(this.Context.ConnectionId, tenantId, subjectId);
                }
                else
                {
                    Log.Warning("No external User identity value was found in JWT Claim during OnDisconnected hub call. The user connection cannot be tracked and notifications may not work.");
                }
            }

            if (exception != null)
            {
                Log.Warning(exception, "An exception was reported to the OnDisconnectedAsync method of Notification Hub.");
            }

            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// This method sends configuration data to all connected subscribers.
        /// </summary>
        /// <param name="configurationData">Contains the configuration data to send.</param>
        /// <returns>Returns the async task object.</returns>
        public async Task SendConfigurationToClients(string configurationData)
        {
            await this.Clients.All.ConfigurationChange(configurationData);
        }
    }
}