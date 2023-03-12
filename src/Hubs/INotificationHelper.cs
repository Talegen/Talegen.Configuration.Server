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
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Models.Notifications;

    /// <summary>
    /// This interface represents a minimal implementation of a notification helper class.
    /// </summary>
    public interface INotificationHelper
    {
        /// <summary>
        /// This method is used to send a notification message to the specific connected client.
        /// </summary>
        /// <typeparam name="TModel">Contains the model data type included in the package.</typeparam>
        /// <param name="notification">Contains the notification model to send.</param>
        /// <param name="cancellationToken">Contains an optional cancellation token.</param>
        /// <returns>Returns a task.</returns>
        Task NotifyClientAsync<TModel>(NotificationModel<TModel, string> notification, CancellationToken cancellationToken = default)
            where TModel : class, new();

        /// <summary>
        /// This method is used to send a notification message to all the connected clients.
        /// </summary>
        /// <typeparam name="TModel">Contains the model data type included in the package.</typeparam>
        /// <param name="notification">Contains the notification model to send.</param>
        /// <param name="excludeSubjectId">Contains a value indicating whether the current client is excluded from receiving the signal message.</param>
        /// <param name="cancellationToken">Contains an optional cancellation token.</param>
        /// <returns>Returns a task.</returns>
        Task NotifyAllAsync<TModel>(NotificationModel<TModel, string> notification, string excludeSubjectId = default!, CancellationToken cancellationToken = default)
            where TModel : class, new();
    }
}