using GreenerConfigurator.ClientCore.Models;
using GreenerConfigurator.Web.Server.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GreenerConfigurator.Web.Server.Controllers;

[ApiController]
[Route("api/1.0/rule")]
public sealed class RulesController : ControllerBase
{
    private readonly RuleRepository _repository;

    public RulesController(RuleRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<RuleSummaryModel>> GetRules() => Ok(_repository.GetSummaries());

    [HttpGet("{ruleId:guid}")]
    public ActionResult<RuleEditModel> GetRule(Guid ruleId)
    {
        var rule = _repository.Get(ruleId);
        return rule is null ? NotFound() : Ok(rule);
    }

    [HttpPost]
    public ActionResult<RuleEditModel> CreateRule(RuleEditModel payload)
    {
        var created = _repository.Save(payload);
        return CreatedAtAction(nameof(GetRule), new { ruleId = created.Id }, created);
    }

    [HttpPut("{ruleId:guid}")]
    public ActionResult<RuleEditModel> UpdateRule(Guid ruleId, RuleEditModel payload)
    {
        if (ruleId != payload.Id)
        {
            return BadRequest("The route identifier and payload identifier must match.");
        }

        if (_repository.Get(ruleId) is null)
        {
            return NotFound();
        }

        var updated = _repository.Save(payload);
        return Ok(updated);
    }

    [HttpDelete("{ruleId:guid}")]
    public IActionResult DeleteRule(Guid ruleId)
    {
        if (_repository.Get(ruleId) is null)
        {
            return NotFound();
        }

        _repository.Delete(ruleId);
        return NoContent();
    }
}
