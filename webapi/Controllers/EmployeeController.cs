using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase {
    private readonly IEmployeeService _service;

    public EmployeeController(IEmployeeService service) {
        _service = service;
    }

	[HttpGet("all")]
	public async Task<IActionResult> GetAllEmployees()
	{
		var employees = await _service.GetAllEmployees();
		return Ok(employees);
	}

	[HttpGet("current")]
	public async Task<IActionResult> GetCurrentEmployees()
	{	
		var employees = await _service.GetCurrentEmployees();
		return Ok(employees);
	}

	[HttpGet("previous")]
	public async Task<IActionResult> GetPreviousEmployees()
	{	
		var employees = await _service.GetPreviousEmployees();
		return Ok(employees);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetEmployeeById(int id)
	{	
		var employee = await _service.GetEmployeeById(id);
		return employee != null ? Ok(employee) : NotFound();
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteEmployeeById(int id)
	{
		try {
			await _service.DeleteEmployeeById(id);
			return NoContent();
		}
		catch (ResourceNotFoundException ex) {
			return NotFound(ex.Message);
		}
	}

	[HttpPost("archive/{id}")]
	public async Task<IActionResult> ArchiveEmployeeById(int id)
	{
		try {
			await _service.ArchiveEmployeeById(id);
			return NoContent();
		}
		catch (ResourceNotFoundException ex) {
			return NotFound(ex.Message);
		}
	}

	[HttpPost("add")]
	public async Task<IActionResult> AddEmployee(Employee newEmployee)
	{
		try {
			await _service.AddEmployee(newEmployee);
			return Ok(newEmployee);
		}
		catch (ArgumentException ex) {
			return BadRequest(ex.Message);
		}
	}

	[HttpPatch("{id}")]
	public async Task<IActionResult> UpdateEmployee(Employee newEmployee)
	{
		try {
			var updatedEmployee = await _service.UpdateEmployee(newEmployee);
			return Ok(updatedEmployee);
		}
		catch (ResourceNotFoundException ex) {
			return NotFound(ex.Message);
		}
	}
}
