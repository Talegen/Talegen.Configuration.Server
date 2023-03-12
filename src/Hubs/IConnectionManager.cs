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
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// This interface implements a connection manager for tracking current users in notifications system.
    /// </summary>
    public interface IConnectionManager
    {
        /// <summary>
        /// Gets an enumerated list of clients currently online.
        /// </summary>
        IEnumerable<string> ConnectedClients { get; }

        /// <summary>
        /// This method is used to return a list of tenant connection identities.
        /// </summary>
        /// <param name="tenantId">Contains the tenant identity.</param>
        /// <param name="excludeSubjectId">Contains a client identity to exclude from the connections returned.</param>
        /// <returns>Returns a list of connection client identities associated with the tenant.</returns>
        List<string> FindTenantConnections(string tenantId, string excludeSubjectId = "");

        /// <summary>
        /// This method is used to return a list of tenant connection client identities.
        /// </summary>
        /// <param name="tenantId">Contains the tenant identity.</param>
        /// <param name="excludeSubjectId">Contains a client identity to exclude from the connections returned.</param>
        /// <param name="cancellationToken">Contains a cancellation token.</param>
        /// <returns>Returns a list of connection identities associated with the tenant.</returns>
        Task<List<string>> FindTenantConnectionsAsync(string tenantId, string excludeSubjectId = "", CancellationToken cancellationToken = default);

        /// <summary>
        /// This method is used to add a client's connection to the connection manager.
        /// </summary>
        /// <param name="tenantId">Contains the application instance tenant identity.</param>
        /// <param name="subjectId">Contains the client's identity.</param>
        /// <param name="connectionId">Contains the SignalR connection identity.</param>
        void AddConnection(string tenantId, string subjectId, string connectionId);

        /// <summary>
        /// This method is used to add a user's connection to the connection manager.
        /// </summary>
        /// <param name="tenantId">Contains the application instance tenant identity.</param>
        /// <param name="subjectId">Contains the client's identity.</param>
        /// <param name="connectionId">Contains the SignalR connection identity.</param>
        /// <param name="cancellationToken">Contains a cancellation token.</param>
        /// <returns>Returns a task object.</returns>
        Task AddConnectionAsync(string tenantId, string subjectId, string connectionId, CancellationToken cancellationToken = default);

        /// <summary>
        /// This method is used to remove a connection from the manager.
        /// </summary>
        /// <param name="connectionId">Contains the connection identity to remove.</param>
        /// <param name="tenantId">Contains the application instance tenant identity.</param>
        /// <param name="subjectId">Contains an optional client identity to speed up lookup.</param>
        void RemoveConnection(string connectionId, string tenantId = "", string subjectId = "");

        /// <summary>
        /// This method is used to remove a connection from the manager.
        /// </summary>
        /// <param name="connectionId">Contains the connection identity to remove.</param>
        /// <param name="tenantId">Contains the application instance tenant identity.</param>
        /// <param name="subjectId">Contains an optional client identity to speed up lookup.</param>
        /// <param name="cancellationToken">Contains a cancellation token.</param>
        /// <returns>Returns a task object.</returns>
        Task RemoveConnectionAsync(string connectionId, string tenantId = "", string subjectId = "", CancellationToken cancellationToken = default);

        /// <summary>
        /// This method is used to get the connections of the specified client.
        /// </summary>
        /// <param name="tenantId">Contains the application instance tenant identity.</param>
        /// <param name="subjectId">Contains the client identity to find.</param>
        /// <returns>Returns a hash set of connection identities for the specified client.</returns>
        IEnumerable<string> GetConnections(string tenantId, string subjectId);

        /// <summary>
        /// This method is used to get the connections of the specified client.
        /// </summary>
        /// <param name="tenantId">Contains the application instance tenant identity.</param>
        /// <param name="subjectId">Contains the client identity to find.</param>
        /// <param name="cancellationToken">Contains a cancellation token.</param>
        /// <returns>Returns a hash set of connection identities for the specified client.</returns>
        Task<IEnumerable<string>> GetConnectionsAsync(string tenantId, string subjectId, CancellationToken cancellationToken = default);
    }
}