// --------------------------------------------
//  Project: Network Chat App
//  Engineer: Ian Milin
//  Date: March 11 2026
//  Description: Defines the ChatHub class for real-time communication using SignalR.
//  This hub allows clients to send messages to all connected clients or to specific groups,
//  and manage group memberships.
// --------------------------------------------

using Microsoft.AspNetCore.SignalR;
using Backend.API.src.Core.Logging;

namespace Backend.API.Hubs
{
    public class ChatHub : Hub
    {


        //----------------------------------
        //----------- Methods --------------
        //----------------------------------


        public override async Task OnConnectedAsync()
        {
            // TODO: Instantiate chat event DTO for user connection (Feature #34)

            // Implement authentication middleware to replace this with actual user information (eg. Context.User?.Identity?.Name)
            await Clients.All.SendAsync("UserConnected", Context.ConnectionId);

            await base.OnConnectedAsync();

            AppLogger.ConnectionEvent(Context.ConnectionId, "UserConnected");
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // TODO: Instantiate chat event DTO for user disconnection (Feature #34)

            await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);

            await base.OnDisconnectedAsync(exception);

            AppLogger.ConnectionEvent(Context.ConnectionId, "UserDisconnected");
        }

        /*
         * At this time, every method in this class uses the following parameter the same way, so we will just explain it once here:
         * param name="user": The name of the user sending the message. This is used to identify the sender to the recipients.
         * 
         * To target a user across all their sessions, we could use a ClaimsPrincipal-based approach with Context.UserIdentifier and 
         * user-specific groups, but that would require additional setup in the authentication middleware to populate the 
         * UserIdentifier based on the authenticated user's ID.
         */

        public async Task SendMessageToAll(string user, string message)
        {
            // TODO: Instantiate message DTO (Feature #34)

            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendMessageToGroup(string groupName, string user, string message)
        {
            // TODO: Instantiate message DTO (Feature #34)

            await Clients.Group(groupName).SendAsync("ReceiveMessage", user, message);
        }

        // Need to study functionality of groups more to implement this properly, but here are the basic methods to add/remove from groups
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            // TODO: Instantiate chat event DTO (Feature #34)

            await Clients.Group(groupName).SendAsync("UserJoined", Context.ConnectionId, groupName);
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            // TODO: Instantiate chat event DTO (Feature #34)

            await Clients.Group(groupName).SendAsync("UserLeft", Context.ConnectionId, groupName);
        }

        // Sends a private message to a specific client connection.
        // This method targets a single client based on the specified connection identifier. The
        // param name="connectionId": The unique identifier of the client connection to which the message will be sent. Cannot be null or empty.
        //
        // Note that a user could be connected with multiple devices or browser tabs, each having a different connection ID, so this method is
        // for targeting a specific session rather than a user as a whole. See my comment above SendMessageToAll() in the "user" parameter
        // description for more on how we could target users across sessions with additional setup.
        public async Task SendDirectMessage(string connectionId, string user, string message)
        {
            // TODO: Instantiate message DTO (Feature #34)

            await Clients.Client(connectionId).SendAsync("ReceiveMessage", user, message);
        }

        // This method sends a message back to the caller only, which can be useful for acknowledgments or private responses.
        public async Task SendMessageToCaller(string user, string message)
        {
            // TODO: Instantiate message DTO (Feature #34)

            await Clients.Caller.SendAsync("ReceiveMessage", user, message);
        }
    }
}
