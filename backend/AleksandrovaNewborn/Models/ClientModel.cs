using System.ComponentModel.DataAnnotations;

namespace AleksandrovaNewborn.Models;

public class ClientModel
{
    [Required]
    public string ChildName { get; set; }
    
    [EmailAddress]
    public string? EmailAddress { get; set; }    
    
    [Phone]
    public string? PhoneNumber { get; set; }
}