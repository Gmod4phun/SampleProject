using System;
using System.ComponentModel.DataAnnotations;

public class Contract {
    public int Id { get; set; }

    [Required]
    public Employee Employee { get; set; }

    [Required]
    public Position Position { get; set; }

    [Required]
    public DateOnly StartDate { get; set; }
}
