using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace DAL.Persistanse.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly ILogger<NotificationHub> _logger;

        public NotificationHub(ILogger<NotificationHub> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation($"Connection ID: {Context.ConnectionId}");
            _logger.LogInformation($"User Claims: {string.Join(", ", Context.User.Claims.Select(c => $"{c.Type}: {c.Value}"))}");

            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _logger.LogInformation($"User ID: {userId}");

            if (userId != null)
            {
                _logger.LogInformation($"User connected: {userId} (Connection ID: {Context.ConnectionId})");
            }
            else
            {
                _logger.LogInformation($"Anonymous user connected (Connection ID: {Context.ConnectionId})");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _logger.LogInformation($"User disconnected: {Context.ConnectionId}");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
