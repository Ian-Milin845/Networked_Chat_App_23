// --------------------------------------------
//  Project: Network Chat App
//  Engineer: Ian Milin
//  Date: March 11 2026
//  Description: Defines the ChatHub class for real-time communication using SignalR.
//  This hub allows clients to send messages to all connected clients or to specific groups,
//  and manage group memberships.
// --------------------------------------------

using Microsoft.AspNetCore.SignalR;

namespace Backend.API.Hubs
{
    public class ChatHub : Hub
    {
        /*
         * At this time, any method in this class uses this named parameter the same way, so we will just explain it once here:
         * param name="user": The name of the user sending the message. This is used to identify the sender to the recipients.
         */

        public async Task SendMessageToAll(string user, string message)
            => await Clients.All.SendAsync("ReceiveMessage", user, message);

        public async Task SendMessageToGroup(string groupName, string user, string message)
            => await Clients.Group(groupName).SendAsync("ReceiveMessage", user, message);

        // Need to study functionality of groups more to implement this properly, but here are the basic methods to add/remove from groups
        //public async Task AddToGroup(string groupName)
        //    => await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        //public async Task RemoveFromGroup(string groupName)
        //    => await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            Console.WriteLine($"Client connected: {Context.ConnectionId}");
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
            Console.WriteLine($"Client disconnected: {Context.ConnectionId}");
        }

        // Sends a private message to a specific client connection.
        // This method targets a single client based on the specified connection identifier. The
        // param name="connectionId": The unique identifier of the client connection to which the message will be sent. Cannot be null or empty.
        // param name="user": The name of the user sending the message. Used to identify the sender to the recipient.
        public async Task SendPrivateMessage(string connectionId, string user, string message)
            => await Clients.Client(connectionId).SendAsync("ReceiveMessage", user, message);

        // This method sends a message back to the caller only, which can be useful for acknowledgments or private responses.
        public async Task SendMessageToCaller(string user, string message)
            => await Clients.Caller.SendAsync("ReceiveMessage", user, message);
    }
}
