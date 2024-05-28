namespace API.DTOs;

public class ApartmentDto : BaseDto
{
    public string PhotoUrl { get; set; }
    
    public string Title { get; set; }
    public string Details { get; set; }

    public string City { get; set; }
    public string Country { get; set; }
    
    public float Price { get; set; }
        
    public int MaxPeople { get; set; }
        
    public bool HasTV { get; set; }
    public bool HasWifi { get; set; }
    
    public bool IsAvaliable { get; set; }

    public List<PhotoDto> Photos { get; set; }
}