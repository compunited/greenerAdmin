using System.Collections.Concurrent;
using GreenerConfigurator.ClientCore.Models;

namespace GreenerConfigurator.Web.Server.Repositories;

/// <summary>
/// Provides a simple in-memory persistence store for locations that mimics the backing API.
/// </summary>
public sealed class LocationRepository
{
    private readonly ConcurrentDictionary<Guid, LocationModel> _locations = new();

    public LocationRepository()
    {
        var headQuarterId = Guid.NewGuid();
        _locations[headQuarterId] = new LocationModel
        {
            Id = headQuarterId,
            Name = "Contoso HQ",
            City = "Zurich",
            Country = "Switzerland",
            Address = "Prime Tower 1",
            Details =
            {
                new LocationDetailModel
                {
                    Id = Guid.NewGuid(),
                    LocationId = headQuarterId,
                    Name = "Main Operations Floor",
                    FloorNumber = 17
                },
                new LocationDetailModel
                {
                    Id = Guid.NewGuid(),
                    LocationId = headQuarterId,
                    Name = "Laboratory",
                    FloorNumber = 3,
                    Description = "Device staging area"
                }
            }
        };
    }

    public IReadOnlyCollection<LocationModel> GetAll() => _locations.Values
        .OrderBy(location => location.Name, StringComparer.OrdinalIgnoreCase)
        .ToList();

    public LocationModel? Get(Guid id) => _locations.TryGetValue(id, out var location) ? Clone(location) : null;

    public LocationModel Upsert(LocationModel location)
    {
        if (location.Id == Guid.Empty)
        {
            location.Id = Guid.NewGuid();
        }

        var clone = Clone(location);
        _locations.AddOrUpdate(location.Id, clone, (_, _) => clone);
        return Clone(clone);
    }

    public void Delete(Guid id)
    {
        _locations.TryRemove(id, out _);
    }

    private static LocationModel Clone(LocationModel source) => new()
    {
        Id = source.Id,
        Name = source.Name,
        ExternalId = source.ExternalId,
        Address = source.Address,
        City = source.City,
        Country = source.Country,
        Details = source.Details.Select(detail => new LocationDetailModel
        {
            Id = detail.Id,
            LocationId = source.Id,
            Name = detail.Name,
            Description = detail.Description,
            FloorNumber = detail.FloorNumber
        }).ToList()
    };
}
