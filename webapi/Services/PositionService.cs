using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public interface IPositionService {
    public Task<List<Position>> GetPositions();
    public Task<Position?> GetPositionByName(string name);
    public Task<Position> AddPosition(string name);
    public Task DeletePosition(string name);
}

public class PositionService : IPositionService {

    private readonly MyDbContext _context;

    public PositionService(MyDbContext context) {
        _context = context;
    }

    public async Task<Position> AddPosition(string name)
    {
		if (name.Length == 0) {
            throw new ArgumentException("Name cannot be an empty string", name);
		}

		var single = await _context.Positions
			.Where(p=>p.Name == name)
			.FirstOrDefaultAsync();

		if (single != null) {
			throw new ResourceAlreadyExistsException("Position already exists");
		}

		var newPosition = new Position {Name = name};

		_context.Add(newPosition);
		await _context.SaveChangesAsync();
		
		return newPosition;
    }

    public async Task DeletePosition(string name)
    {
		var single = await _context.Positions
			.Where(p=>p.Name == name)
			.FirstOrDefaultAsync();

		if (single == null) {
			throw new ResourceNotFoundException("Position does not exist");
		}

		var contracts = await _context.Contracts
			.Where(p=>p.Position.Name == name)
			.ToListAsync();

		// if we have any contracts with this position, its not safe to delete
		if (contracts.Count > 0) {
			throw new ResourceUnsafeToDeleteException("Position is referenced in contracts.");
		}

		_context.Remove(single);
		await _context.SaveChangesAsync();

		return;
    }

    public Task<Position?> GetPositionByName(string name)
    {
		return _context.Positions
			.Where(p=>p.Name == name)
			.AsNoTracking()
			.FirstOrDefaultAsync();
    }

    public Task<List<Position>> GetPositions()
    {
		return _context.Positions
			.AsNoTracking()
			.ToListAsync();
    }
}
