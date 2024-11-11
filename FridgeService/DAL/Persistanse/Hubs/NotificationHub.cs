using DAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace DAL.Persistanse.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        private readonly ILogger<NotificationHub> _logger;
        private readonly IFridgeRepository _fridgeRepository;

        public NotificationHub(ILogger<NotificationHub> logger,IFridgeRepository fridgeRepository)
        {
            _logger = logger;
            _fridgeRepository = fridgeRepository;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            _logger.LogInformation($"User {userId} connected to fridge groups");
            
            if (userId is not null)
            {
                var fridgeIds = await GetFridgesByUserIdAsync(userId);

                _logger.LogInformation($"User {userId} connected to fridge groups {fridgeIds.ToString()}");

                foreach (var fridgeId in fridgeIds)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, fridgeId.ToString());
                
                    _logger.LogInformation($"User {userId} added to fridge group {fridgeId}");
                }
            }
            else
            {
                _logger.LogInformation("User is not authorized");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId is not null)
            {
                var fridgeIds = await GetFridgesByUserIdAsync((userId));

                foreach (var fridgeId in fridgeIds)
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, fridgeId.ToString());
                    _logger.LogInformation($"User {userId} removed from fridge group {fridgeId}");
                }
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task<List<int>> GetFridgesByUserIdAsync(string userId)
        {
            return (await _fridgeRepository.GetFridgeByUserId(int.Parse(userId)))
                .Select(f=>f.id).ToList();
        }
    }
}
