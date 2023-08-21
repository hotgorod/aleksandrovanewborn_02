using System.ComponentModel.DataAnnotations;

namespace AleksandrovaNewborn.Models;

public class PhotoshootModel
{
    [Required]
    public int ClientId { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public DateTime PhotoshootDate { get; set; }
    
    public DateTime? ExpirationDate { get; set; }
}