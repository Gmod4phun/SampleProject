using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("[controller]")]
public class PositionController : ControllerBase {
    private readonly IPositionService _service;

    public PositionController(IPositionService service) {
        _service = service;
    }

	[HttpGet("all")]
	public async Task<IActionResult> GetPositions()
	{
		var positions = await _service.GetPositions();
		return Ok(positions);
	}

	[HttpGet("{name}")]
	public async Task<IActionResult> GetPositionByName(string name)
	{
		var position = await _service.GetPositionByName(name);
		return position != null ? Ok(position) : NotFound();
	}

	[HttpPost("add/{name}")]
	public async Task<IActionResult> AddPosition(string name)
	{
		try {
			var newPosition = await _service.AddPosition(name);
			return Ok(newPosition);
		}
		catch (ResourceAlreadyExistsException ex) {
			return Conflict(ex.Message);
		}
        catch (ArgumentException ex) {
			return BadRequest(ex.Message);
		}
	}

	[HttpDelete("{name}")]
	public async Task<IActionResult> DeletePosition(string name)
	{
		try {
			await _service.DeletePosition(name);
			return NoContent();
		}
		catch (ResourceUnsafeToDeleteException ex) {
			return BadRequest(ex.Message);
		}
		catch (ResourceNotFoundException ex) {
			return NotFound(ex.Message);
		}
	}
}
