namespace API.Entities;

public class AppUser : BaseEntity
{
    public string UserName { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }

    public string KnownAs { get; set; }
    public string Gender { get; set; }

    public List<Apartment> Apartments { get; set; } = new List<Apartment>();
    public List<Apartment> RentedApartments { get; set; } = new List<Apartment>();
    // public List<RentedApartments> RentedApartments { get; set; }
}