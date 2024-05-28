namespace API.DTOs;

public class MemberDto : BaseDto
{
    public string UserName { get; set; }
    
    public string KnownAs { get; set; }
    public string Gender { get; set; }

    public List<ApartmentDto> Apartments { get; set; }
    public List<ApartmentDto> RentedApartments { get; set; }
}