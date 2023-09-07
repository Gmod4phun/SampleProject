using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Employee
{
    public int Id { get; set; }

    [Required]
    public string? FirstName { get; set; }

    [Required]
    public string? LastName { get; set; }

    public string? Address { get; set; }

    [Required, DateInPast]
    public DateOnly BirthDate { get; set; }

    [Required, DateInPresentOrFuture]
    public DateOnly StartDate { get; set; }

    [Required]
    public Position? Position { get; set; }

    [Required]
    public float Wage { get; set; }

    public bool IsArchived { get; set; }
    public DateOnly EndDate { get; set; }

    [NotMapped]
    public string FullName {
        get
        {
            return $"{FirstName} {LastName}";
        }
    }

    public override string ToString()
    {
        return $"Employee\nFullName: {FullName}\nAddress: {Address}\nBirthDate: {BirthDate}\nPosition: {Position?.Name}\nWage: {Wage}";
    }
}
