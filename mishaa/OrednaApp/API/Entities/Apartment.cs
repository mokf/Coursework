using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("Apartments")]
public class Apartment : BaseEntity
{
    public string? PhotoUrl { get; set; }
    
    public string Title { get; set; }
    public string? Details { get; set; }

    public string City { get; set; }
    public string Country { get; set; }
    
    public float Price { get; set; }
        
    public int MaxPeople { get; set; }
        
    public bool HasTV { get; set; }
    public bool HasWifi { get; set; }
    
    public bool IsAvaliable { get; set; }

    public AppUser? RentedByUser { get; set; }
    public int? RentedByUserId { get; set; }

    public int AppUserId { get; set; }
    public AppUser AppUser { get; set; }
    
    public List<Photo> Photos { get; set; } = new List<Photo>();
}