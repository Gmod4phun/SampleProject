using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public interface IEmployeeService {
    public Task<List<Employee>> GetAllEmployees(bool asNoTracking=true);
    public Task<List<Employee>> GetCurrentEmployees(bool asNoTracking=true);
    public Task<List<Employee>> GetPreviousEmployees(bool asNoTracking=true);
    public Task<Employee?> GetEmployeeById(int id, bool asNoTracking=true);
    public Task DeleteEmployeeById(int id);
    public Task ArchiveEmployeeById(int id);
    public Task<Employee> AddEmployee(Employee newEmployee);
    public Task<Employee> UpdateEmployee(Employee newEmployee);
}

public class EmployeeService : IEmployeeService {

    private readonly MyDbContext _context;

    public EmployeeService(MyDbContext context) {
        _context = context;
    }

    public Task<List<Employee>> GetAllEmployees(bool asNoTracking=true)
	{	
		var employees = _context.Employees
			.Include(p=>p.Position);

		return asNoTracking ? employees.AsNoTracking().ToListAsync() : employees.ToListAsync();
	}

	public Task<List<Employee>> GetCurrentEmployees(bool asNoTracking=true)
	{	
		var employees = _context.Employees
			.Include(p=>p.Position)
			.Where(p=>!p.IsArchived);

		return asNoTracking ? employees.AsNoTracking().ToListAsync() : employees.ToListAsync();
	}

	public Task<List<Employee>> GetPreviousEmployees(bool asNoTracking=true)
	{	
		var employees = _context.Employees
			.Include(p=>p.Position)
			.Where(p=>p.IsArchived);

		return asNoTracking ? employees.AsNoTracking().ToListAsync() : employees.ToListAsync();
	}

	public Task<Employee?> GetEmployeeById(int id, bool asNoTracking=true)
	{
		var employee = _context.Employees
			.Include(p=>p.Position)
			.Where(p=>p.Id == id);

		return asNoTracking ? employee.AsNoTracking().FirstOrDefaultAsync() : employee.FirstOrDefaultAsync();
	}

	public async Task DeleteEmployeeById(int id)
	{
		var single = await GetEmployeeById(id);
		if (single == null) {
            throw new ResourceNotFoundException("Employee does not exist");
		}

		_context.Remove(single);
		await _context.SaveChangesAsync();
	}

	public async Task ArchiveEmployeeById(int id)
	{
		var single = await GetEmployeeById(id, false);
		if (single == null) {
			throw new ResourceNotFoundException("Employee does not exist");
		}

		single.IsArchived = true;
		single.EndDate = DateOnly.FromDateTime(DateTime.Now);
		await _context.SaveChangesAsync();
	}

	public async Task<Employee> AddEmployee(Employee newEmployee)
	{
		var newPosition = newEmployee.Position;
		if (newPosition == null) {
			throw new ArgumentException("New position is null");
		}

		var existingPosition = await _context.Positions
			.Where(p => p.Name == newPosition.Name)
			.FirstOrDefaultAsync();

		if (existingPosition == null) {
			throw new ArgumentException("New position does not exist");
		}

		newEmployee.Position = existingPosition;

		var newContract = new Contract{Employee=newEmployee, Position=newEmployee.Position, StartDate=newEmployee.StartDate};
		_context.Employees.Add(newEmployee);
		_context.Contracts.Add(newContract);
		await _context.SaveChangesAsync();

		return newEmployee;
	}

	public async Task<Employee> UpdateEmployee(Employee newEmployee)
	{
		// check if employee with this id exists on server
		var existingEmployee = await GetEmployeeById(newEmployee.Id, false);

		if (existingEmployee == null) {
			throw new ResourceNotFoundException("Employee does not exist");
		}

		// check if new position exists on server
		var newPosition = newEmployee.Position;
		if (newPosition == null) {
			throw new ResourceNotFoundException("New position does not exist");
		}

		var existingPosition = await _context.Positions
			.Where(p => p.Name == newPosition.Name)
			.FirstOrDefaultAsync();
			
		if (existingPosition == null) {
			throw new ResourceNotFoundException("Position does not exist");
		}

		// change these attributes as necessary
		existingEmployee.FirstName = newEmployee.FirstName;
		existingEmployee.LastName = newEmployee.LastName;
		existingEmployee.Address = newEmployee.Address;
		existingEmployee.BirthDate = newEmployee.BirthDate;
		existingEmployee.Wage = newEmployee.Wage;
		
		// if we are changing positions, we need a new contract
		if (existingEmployee.Position?.Name != newEmployee.Position?.Name) {
			var newContract = new Contract{Employee=existingEmployee, Position=existingPosition, StartDate=newEmployee.StartDate};
			existingEmployee.Position = newContract.Position;
			existingEmployee.StartDate = newContract.StartDate;
			_context.Add(newContract);
		}
		
		await _context.SaveChangesAsync();

		return existingEmployee;
	}   
}
