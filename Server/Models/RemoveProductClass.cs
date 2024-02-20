namespace homework22;
using System;
using System.ComponentModel.DataAnnotations;

public class RemoveProductClass
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; }

    public RemoveProductClass(string name)
    {
        Name = name;
    }

}
