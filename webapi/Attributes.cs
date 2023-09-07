using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class DateInPastAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        DateOnly date = (DateOnly) value;
        DateOnly dateNow = DateOnly.FromDateTime(DateTime.Now);

        return date < dateNow;
    }
}

public class DateInPresentOrFutureAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        DateOnly date = (DateOnly) value;
        DateOnly dateNow = DateOnly.FromDateTime(DateTime.Now);

        return date >= dateNow;
    }
}
