namespace API.DTOs;

public class CreateApartmentDto
{
    public string Title { get; set; }
    
    public string City { get; set; }
    public string Country { get; set; }
    
    public float Price { get; set; }
        
    public int MaxPeople { get; set; }
        
    public bool HasTV { get; set; }
    public bool HasWifi { get; set; }
}