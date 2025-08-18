using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace Chatbot.Web
{
    public class ChatHub : Hub
    {

        // Lưu danh sách room cho mỗi connection ( Key: user, Value: danh sách room)
        private static readonly ConcurrentDictionary<string, HashSet<string>> _roomsByConnection = new();

        public ChatHub()
        {
        }

        public override async Task OnConnectedAsync()
        {
            _roomsByConnection[Context.ConnectionId] = new HashSet<string>();
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _roomsByConnection.TryRemove(Context.ConnectionId, out _);
            await base.OnDisconnectedAsync(exception);
        }

        // Tham gia vào 1 phòng
        public async Task JoinRoom(string room)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, room);

            _roomsByConnection[Context.ConnectionId].Add(room);

            await Clients.Caller.SendAsync("ReceiveMessage", "System", $"Bạn đã tham gia {room}");
        }

        // Rời khỏi 1 phòng
        public async Task LeaveRoom(string room)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, room);

            _roomsByConnection[Context.ConnectionId].Remove(room);

            await Clients.Caller.SendAsync("ReceiveMessage", "System", $"Bạn đã rời {room}");
        }

        // Gửi tin nhắn trong 1 phòng
        public async Task SendMessageToRoom(string room, string user, string message)
        {
            await Clients.Group(room).SendAsync("ReceiveMessage", room, user, message);
        }

        public Task<List<string>> GetMyRooms()
        {
            if (_roomsByConnection.TryGetValue(Context.ConnectionId, out var rooms))
            {
                return Task.FromResult(rooms.ToList());
            }
            return Task.FromResult(new List<string>());
        }
    }
}
