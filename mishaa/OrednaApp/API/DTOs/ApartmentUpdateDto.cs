namespace API.DTOs;

public class ApartmentUpdateDto
{
    public string Details { get; set; }
    public float Price { get; set; }
    public int MaxPeople { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
}