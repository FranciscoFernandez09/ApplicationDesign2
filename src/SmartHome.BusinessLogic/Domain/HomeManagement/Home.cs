using System.Text.RegularExpressions;
using SmartHome.BusinessLogic.Models.Arguments.DomainArguments;

namespace SmartHome.BusinessLogic.Domain.HomeManagement;

public sealed class Home()
{
    public Home(CreateHomeArgs createHomeArgs)
        : this()
    {
        if (!AreValidParameters(createHomeArgs.AddressStreet, createHomeArgs.AddressNumber, createHomeArgs.MaxMembers,
                createHomeArgs.Latitude, createHomeArgs.Longitude, out var errorMessage))
        {
            throw new ArgumentException(errorMessage);
        }

        Id = Guid.NewGuid();
        AddressStreet = createHomeArgs.AddressStreet;
        AddressNumber = createHomeArgs.AddressNumber;
        Latitude = createHomeArgs.Latitude;
        Longitude = createHomeArgs.Longitude;
        MembersCount = 1;
        Name = createHomeArgs.Name;
        MaxMembers = createHomeArgs.MaxMembers;
        Owner = createHomeArgs.Owner;
        OwnerId = Owner.Id;
        Members = [];
        Devices = [];
        Rooms = [];
    }

    public Guid Id { get; init; }
    public string AddressStreet { get; init; } = null!;
    public int AddressNumber { get; init; }
    public int Latitude { get; init; }
    public int Longitude { get; init; }
    public int MaxMembers { get; init; }
    public User Owner { get; init; } = null!;
    public Guid OwnerId { get; init; }
    public int MembersCount { get; set; }
    public string Name { get; set; } = null!;
    public List<HomeMember> Members { get; set; } = null!;
    public List<HomeDevice> Devices { get; set; } = null!;
    public List<Room> Rooms { get; set; } = null!;

    public List<HomeMember> GetMembersToNotify()
    {
        return Members.Where(m => m.ShouldNotify).ToList();
    }

    public bool IsFull()
    {
        return MembersCount >= MaxMembers;
    }

    public bool IsOwner(User user)
    {
        return Owner.Id == user.Id;
    }

    public bool IsMember(User user)
    {
        return IsOwner(user) || Members.Any(m => m.UserId == user.Id);
    }

    public void AddMember(HomeMember homeMember)
    {
        Members.Add(homeMember);
        MembersCount++;
    }

    public void AddDevice(HomeDevice homeDevice)
    {
        Devices.Add(homeDevice);
    }

    private static bool AreValidParameters(string addressStreet, int addressNr, int maxMembers, int latitude,
        int longitude,
        out string errorMessage)
    {
        return IsValidAddressNumber(addressNr, out errorMessage) &&
               IsValidAddressStreet(addressStreet, out errorMessage) &&
               IsValidMaxMembers(maxMembers, out errorMessage) &&
               IsValidLatitude(latitude, out errorMessage) &&
               IsValidLongitude(longitude, out errorMessage);
    }

    private static bool IsValidAddressStreet(string addressStreet, out string errorMessage)
    {
        if (!Regex.IsMatch(addressStreet, @"^[a-zA-Z\s]+$"))
        {
            errorMessage = "Invalid street: Only letters and spaces are allowed.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    private static bool IsValidAddressNumber(int addressNumber, out string errorMessage)
    {
        if (addressNumber <= 0)
        {
            errorMessage = "Invalid address number: Must be greater than 0.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    private static bool IsValidMaxMembers(int maxMembers, out string errorMessage)
    {
        if (maxMembers <= 0)
        {
            errorMessage = "Invalid max members: Must be greater than 0.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    private static bool IsValidLatitude(int latitude, out string errorMessage)
    {
        if (latitude <= 0)
        {
            errorMessage = "Invalid latitude: Must be greater than 0.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    private static bool IsValidLongitude(int longitude, out string errorMessage)
    {
        if (longitude <= 0)
        {
            errorMessage = "Invalid longitude: Must be greater than 0.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }
}
