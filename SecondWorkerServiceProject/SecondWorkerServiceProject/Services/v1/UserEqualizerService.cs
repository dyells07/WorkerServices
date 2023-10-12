using SecondWorkerServiceProject.Data;
using SecondWorkerServiceProject.Data.Api;
using SecondWorkerServiceProject.Models;
using SecondWorkerServiceProject.PlaceHolder.Models;

namespace SecondWorkerServiceProject.Services.v1;

public class UserEqualizerService
{
    private readonly ILogger<UserEqualizerService> _logger;
    private readonly PlaceHolderClient _client;
    private readonly UserDbContext _context;

    public UserEqualizerService(ILogger<UserEqualizerService> logger, PlaceHolderClient client, UserDbContext context)
    {
        _logger = logger;
        _client = client;
        _context = context;
    }

    public virtual async Task<bool> ExecuteService()
    {
        _logger.LogInformation("Starting process");

        var placeHolderUsers = await _client.GetPlaceHolderUsers();

        var result = await EqualizeUsers(placeHolderUsers);

        _logger.LogInformation("Ending process");

        return result;
    }

    public virtual async Task<bool> EqualizeUsers(List<PlaceHolderUser> phUsers)
    {
        var users = _context.Users.ToList();

        var newUsers = phUsers.Where(x => !users.Any(x1 => x1.Username != x.Username && x1.Email != x.Email)).ToList();

        if (!newUsers.Any())
        {
            _logger.LogInformation("No new users to add");
            return true;
        }

        _logger.LogInformation($"Found {newUsers.Count} new users");

        return await SaveNewUsers(newUsers);
    }

    public virtual async Task<bool> SaveNewUsers(List<PlaceHolderUser> newUsers)
    {
        try
        {
            _logger.LogInformation("Saving new users");

            var newUsersEntity = newUsers.Select(x => new User
            {
                Id = x.Id,
                Name = x.Name,
                Username = x.Username,
                Email = x.Email,
                Address = new Models.Address
                {
                    Id = x.Id,
                    City = x.Address?.City,
                    Geo = new Models.Geo { Id = x.Id, Lat = x.Address?.Geo?.Lat, Lng = x.Address?.Geo?.Lng },
                    Street = x.Address?.Street,
                    Suite = x.Address?.Suite,
                    Zipcode = x.Address?.Zipcode,
                },
                Phone = x.Phone,
                Website = x.Website,
                Company = new Models.Company { Id = x.Id, Name = x.Company?.Name, CatchPhrase = x.Company?.CatchPhrase, Bs = x.Company?.Bs }
            }).ToList();

            await _context.Users.AddRangeAsync(newUsersEntity);
            await _context.Address.AddRangeAsync(newUsersEntity.Select(x => x.Address).ToList());
            await _context.Geo.AddRangeAsync(newUsersEntity.Select(x => x.Address.Geo).ToList());
            await _context.Company.AddRangeAsync(newUsersEntity.Select(x => x.Company).ToList());
            await _context.SaveChangesAsync();

            _logger.LogInformation("New users successfully saved");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error saving new users - {ex.Message}");
            return false;
        }
    }
}