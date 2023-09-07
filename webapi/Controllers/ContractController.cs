using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ContractController : ControllerBase {
    private readonly IContractService _service;

    public ContractController(IContractService service) {
        _service = service;
    }

	[HttpGet("all")]
	public async Task<IActionResult> GetContracts()
	{
		var contracts = await _service.GetContracts();
		return Ok(contracts);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetContractById(int id)
	{
		var contract = await _service.GetContractById(id);
		return contract != null ? Ok(contract) : NotFound();	
	}

	[HttpGet("foremployee/{empid}")]
	public async Task<IActionResult> GetContractsByEmployeeId(int empid)
	{
		var contracts = await _service.GetContractsByEmployeeId(empid);
		return Ok(contracts);
	}

	[HttpDelete("foremployee/{empid}")]
	public async Task<IActionResult> DeleteContractsByEmployeeId(int empid)
	{
		try {
			await _service.DeleteContractsByEmployeeId(empid);
			return NoContent();
		}
		catch (ResourceNotFoundException ex) {
			return NotFound(ex.Message);
		}
	}
}
