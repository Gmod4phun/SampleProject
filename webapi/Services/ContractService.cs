using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public interface IContractService {
    public Task<List<Contract>> GetContracts();
    public Task<Contract?> GetContractById(int id);
    public Task<List<Contract>> GetContractsByEmployeeId(int empid);
    public Task DeleteContractsByEmployeeId(int empid);
}

public class ContractService : IContractService {

    private readonly MyDbContext _context;

    public ContractService(MyDbContext context) {
        _context = context;
    }

    public async Task DeleteContractsByEmployeeId(int empid)
    {
		var contractsForEmployee = await _context.Contracts
			.Where(p=>p.Employee.Id == empid)
			.ToListAsync();

		if (contractsForEmployee.Any()) {
			_context.RemoveRange(contractsForEmployee);

			await _context.SaveChangesAsync();
			return;
		}

		throw new ResourceNotFoundException("Employee does not exist");
    }

    public Task<Contract?> GetContractById(int id)
    {
		return _context.Contracts
			.Include(p=>p.Position)
			.Include(p=>p.Employee)
			.Where(p=>p.Id == id)
			.AsNoTracking()
			.FirstOrDefaultAsync();
    }

    public Task<List<Contract>> GetContracts()
    {
		return _context.Contracts.Include(p=>p.Position).Include(p=>p.Employee).AsNoTracking().ToListAsync();
    }

    public Task<List<Contract>> GetContractsByEmployeeId(int empid)
    {
		return _context.Contracts
			.Include(p=>p.Position)
			.Include(p=>p.Employee)
			.Where(p=>p.Employee.Id == empid)
			.OrderBy(p=>p.StartDate)
			.AsNoTracking()
			.ToListAsync();
    }
}
