namespace API.DTOs;

public class MemberUpdateDto
{
    public string KnownAs { get; set; }
    
    public List<ApartmentDto> Apartments { get; set; }
    public List<ApartmentDto> RentedApartments { get; set; }
}