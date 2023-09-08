using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Microsoft.EntityFrameworkCore;

using System.Linq;

var DbPath = "employees.db";

void PopulateDatabaseWithPositionsIfEmpty() {
	using var db = new MyDbContext();

	if (db.Positions?.Count() == 0) {
		db.Add(new Position {Name = "Tester"});
		db.Add(new Position {Name = "Programmer"});
		db.Add(new Position {Name = "Support"});
		db.Add(new Position {Name = "Analyst"});
		db.Add(new Position {Name = "Salesperson"});
		db.Add(new Position {Name = "Other"});

		db.SaveChanges();
	}
}

// PopulateDatabaseWithPositionsIfEmpty();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MyDbContext>(options => 
	options
	.UseSqlite($"Data Source={DbPath}")
);

// add services
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IPositionService, PositionService>();
builder.Services.AddScoped<IContractService, ContractService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy  =>
                      {
                        //   policy.WithOrigins("http://localhost:4000", "http://localhost:8080").AllowAnyHeader().AllowAnyMethod();
						policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                      });
});


// BUILD APP

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();


// using (var scope = app.Services.CreateScope())
// {
//     var services = scope.ServiceProvider;

//     var context = services.GetRequiredService<MyDbContext>();
//     if (context.Database.GetPendingMigrations().Any())
//     {
//         context.Database.Migrate();
//     }
// }


app.Run();


/*

na frontended doplnit validaciu | done
presunut logiku requestov do spolocnej servisy | asi done
na backende rozne controllery podla typu entity, pre employee a position | done
presunut logiku z controllerov do aplikacnej vrstvy a infrastrukturovej vrstvy, db access je v infrastrukture | asi done
pouzit async methody | asi done


*/
