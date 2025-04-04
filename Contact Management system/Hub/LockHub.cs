using EntityModels.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

public class LockHub : Hub
{
    private readonly AppDbContext _context;

    public LockHub(AppDbContext context)
    {
        _context = context;
    }

    // Lock a record when a user starts editing
    public async Task LockRecord(int contactId, string userId)
    {
        var existingLock = await _context.EditLocks.FirstOrDefaultAsync(e => e.ContactId == (contactId));
        Guid UserId = Guid.Parse(userId);

        if (existingLock == null)
        {
            var newLock = new EditLock
            {
                ContactId =(contactId),
                UserId = UserId,
                LockedAt = DateTime.Now,
                ConnectionId = Context.ConnectionId
            };
            _context.EditLocks.Add(newLock);
            await _context.SaveChangesAsync();

            // Notify other users
            await Clients.Others.SendAsync("LockReceived", contactId, userId);
        }
        else
        {
            // Notify the current user that the record is locked by another user
            await Clients.Caller.SendAsync("LockDenied", contactId, existingLock.UserId);
        }
    }

    // Unlock a record when the user leaves the page or disconnects
    public async Task UnlockRecord(string contactId, string userId)
    {
        Guid UserId = Guid.Parse(userId);

        var existingLock = await _context.EditLocks.FirstOrDefaultAsync(e => e.ContactId == int.Parse(contactId) && e.UserId == UserId);

        if (existingLock != null)
        {
            _context.EditLocks.Remove(existingLock);
            await _context.SaveChangesAsync();

            // Notify all users that the record is now unlocked
            await Clients.All.SendAsync("UnlockReceived",int.Parse(contactId));
        }
    }

    // Auto-unlock records when a user disconnects
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
            var locks = _context.EditLocks.Where(e => e.ConnectionId == Context.ConnectionId);
        _context.EditLocks.RemoveRange(locks);
        await _context.SaveChangesAsync();

        await base.OnDisconnectedAsync(exception);
    }
}
