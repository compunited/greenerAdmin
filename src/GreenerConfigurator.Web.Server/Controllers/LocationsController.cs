using GreenerConfigurator.ClientCore.Models;
using GreenerConfigurator.Web.Server.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GreenerConfigurator.Web.Server.Controllers;

[ApiController]
[Route("api/1.0/location")]
public sealed class LocationsController : ControllerBase
{
    private readonly LocationRepository _repository;

    public LocationsController(LocationRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<LocationModel>> GetLocations() => Ok(_repository.GetAll());

    [HttpGet("{locationId:guid}")]
    public ActionResult<LocationModel> GetLocation(Guid locationId)
    {
        var location = _repository.Get(locationId);
        return location is null ? NotFound() : Ok(location);
    }

    [HttpPost]
    public ActionResult<LocationModel> CreateLocation(LocationModel payload)
    {
        var created = _repository.Upsert(payload);
        return CreatedAtAction(nameof(GetLocation), new { locationId = created.Id }, created);
    }

    [HttpPut("{locationId:guid}")]
    public ActionResult<LocationModel> UpdateLocation(Guid locationId, LocationModel payload)
    {
        if (locationId != payload.Id)
        {
            return BadRequest("The route identifier and payload identifier must match.");
        }

        if (_repository.Get(locationId) is null)
        {
            return NotFound();
        }

        var updated = _repository.Upsert(payload);
        return Ok(updated);
    }

    [HttpDelete("{locationId:guid}")]
    public IActionResult DeleteLocation(Guid locationId)
    {
        if (_repository.Get(locationId) is null)
        {
            return NotFound();
        }

        _repository.Delete(locationId);
        return NoContent();
    }
}
